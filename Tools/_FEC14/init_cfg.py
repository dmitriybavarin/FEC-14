import argparse
import json
import re
import shutil
import subprocess
import sys
import tomllib
from datetime import datetime
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
TEMPLATE = Path(__file__).resolve().parent / "CONFIG.template"
TARGET = ROOT / ".CONFIG"

SECRET_PATTERNS = {
    "токен Discord-бота": re.compile(r"[MNO][A-Za-z\d_-]{23,25}\.[\w-]{6}\.[\w-]{27,}"),
    "вебхук Discord": re.compile(r"https://(?:ptb\.|canary\.)?discord(?:app)?\.com/api/webhooks/\d+/[\w-]+"),
    "пароль в строке подключения": re.compile(r"(?i)(?:Host|Server|Username|User Id)\s*=[^\n]*;\s*Password\s*=\s*[^;\s\"'{]{3,}"),
    "приватный ключ": re.compile(r"-----BEGIN (?:RSA |EC |OPENSSH |)PRIVATE KEY-----"),
    "ключ AWS": re.compile(r"\bAKIA[0-9A-Z]{16}\b"),
}
SCAN_SKIP = ("Resources/", "RobustToolbox/", "Content.Packaging/", "BuildFiles/", ".github/")
SECRET_KEYS = {
    "discord": ("ahelp_webhook", "on_call_webhook", "round_update_webhook", "vote_webhook",
                "votekick_webhook", "watchlist_connection_webhook", "news_webhook"),
    "database": ("pg_password",),
}


class Report:
    def __init__(self):
        self.failed = 0
        self.warned = 0

    def ok(self, msg):
        print(f"  [ OK ] {msg}")

    def warn(self, msg):
        self.warned += 1
        print(f"  [WARN] {msg}")

    def fail(self, msg):
        self.failed += 1
        print(f"  [FAIL] {msg}")


def copy_template():
    TARGET.mkdir(exist_ok=True)
    created = kept = 0
    for src in sorted(TEMPLATE.rglob("*")):
        if not src.is_file():
            continue
        dst = TARGET / src.relative_to(TEMPLATE)
        if dst.exists():
            kept += 1
            continue
        dst.parent.mkdir(parents=True, exist_ok=True)
        shutil.copy2(src, dst)
        created += 1
        print(f"создан: {dst.relative_to(ROOT)}")
    print(f".CONFIG: создано {created}, оставлено как есть {kept}")


def reset():
    if TARGET.exists():
        backup = ROOT / f".CONFIG.backup-{datetime.now():%Y%m%d-%H%M%S}"
        TARGET.rename(backup)
        print(f"старый .CONFIG сохранен: {backup.name} (в .gitignore, удалить можно вручную)")
    copy_template()


def git(*args):
    return subprocess.run(["git", *args], cwd=ROOT, capture_output=True, text=True, encoding="utf-8")


def check_files(r):
    print("Файлы .CONFIG")
    for src in sorted(TEMPLATE.rglob("*")):
        if src.is_file():
            rel = src.relative_to(TEMPLATE)
            (r.ok if (TARGET / rel).exists() else r.fail)(f"{rel}")
    for name in ("ooc_colors.json", "admin_weights.json"):
        path = TARGET / name
        if not path.exists():
            continue
        try:
            data = json.loads(path.read_text(encoding="utf-8-sig"))
            expected = dict if name == "ooc_colors.json" else list
            if isinstance(data, expected):
                r.ok(f"{name}: корректный JSON")
            else:
                r.fail(f"{name}: ожидается {expected.__name__}")
        except ValueError as e:
            r.fail(f"{name}: не JSON ({e})")


def check_server_config(r):
    print("server_config.toml")
    path = TARGET / "server_config.toml"
    if not path.exists():
        return r.fail("файла нет")
    try:
        cfg = tomllib.loads(path.read_text(encoding="utf-8-sig"))
    except tomllib.TOMLDecodeError as e:
        return r.fail(f"невалидный TOML: {e}")
    r.ok("TOML читается")

    mode = cfg.get("auth", {}).get("mode")
    (r.ok if mode == 1 else r.fail)(f"auth.mode = {mode} (нужно 1: вход только через аккаунт SS14)")

    if cfg.get("console", {}).get("loginlocal", True):
        r.warn("console.loginlocal = true: ок для локальной разработки, на VPS поставить false")
    else:
        r.ok("console.loginlocal = false")

    for section, keys in SECRET_KEYS.items():
        for key in keys:
            value = cfg.get(section, {}).get(key, "")
            if value:
                r.fail(f"[{section}] {key} заполнен: секреты в пустой конфиг не кладем")
    r.ok("секретные поля пусты")

    if cfg.get("hub", {}).get("advertise"):
        r.warn("hub.advertise = true: сервер виден в публичном списке (нужно разрешение владельца по LICENSE.TXT)")
    if not cfg.get("whitelist", {}).get("enabled"):
        r.warn("whitelist.enabled = false: на закрытом тесте включить вайтлист")
    if not cfg.get("game", {}).get("role_whitelist", True):
        r.fail("game.role_whitelist = false: закрытые должности открыты всем")
    bind = str(cfg.get("net", {}).get("bindto", ""))
    r.ok(f"net.bindto = {bind}, port = {cfg.get('net', {}).get('port')}")


def check_repo(r):
    print("Репозиторий")
    for p in (".CONFIG/server_config.toml", ".BOT/config.json"):
        res = git("check-ignore", "-q", p)
        (r.ok if res.returncode == 0 else r.fail)(f"{p} в .gitignore" if res.returncode == 0 else f"{p} НЕ в .gitignore")

    tracked = [f for f in git("ls-files").stdout.splitlines() if not f.startswith(SCAN_SKIP)]
    hits = 0
    for rel in tracked:
        path = ROOT / rel
        try:
            if not path.is_file() or path.stat().st_size > 1_000_000:
                continue
            text = path.read_text(encoding="utf-8", errors="strict")
        except (UnicodeDecodeError, OSError):
            continue
        for label, rx in SECRET_PATTERNS.items():
            if rx.search(text):
                hits += 1
                r.fail(f"{rel}: похоже на {label}")
    if not hits:
        r.ok(f"в {len(tracked)} отслеживаемых файлах секретов не найдено")

    for rel in ("RobustToolbox",):
        res = git("diff", "--quiet", "--", rel)
        (r.ok if res.returncode == 0 else r.fail)(f"{rel} не изменен" if res.returncode == 0 else f"{rel} изменен (его меняет только человек)")


def check_external_scan(r):
    print("Сканер кода, текстур, аудио")
    scan = ROOT.parent / ".INSTRUCTIONS" / "I-FEC14" / "tools" / "sec" / "scan_all.py"
    python = ROOT.parent / ".INSTRUCTIONS" / "I-FEC14" / "tools" / ".venv" / "Scripts" / "python.exe"
    if not (scan.exists() and python.exists()):
        return r.warn("сканер не найден рядом с репозиторием (../.INSTRUCTIONS), пропущен")
    res = subprocess.run([str(python), str(scan), "--changed", "upstream/master"], cwd=ROOT,
                         env={**__import__("os").environ, "PYTHONIOENCODING": "utf-8", "FEC14_REPO": str(ROOT)})
    (r.ok if res.returncode == 0 else r.fail)(f"scan_all.py: код возврата {res.returncode}")


def check():
    r = Report()
    print("\n=== Проверки ===")
    check_files(r)
    check_server_config(r)
    check_repo(r)
    check_external_scan(r)
    print(f"\nИтого: ошибок {r.failed}, предупреждений {r.warned}")
    return 1 if r.failed else 0


def main():
    ap = argparse.ArgumentParser(description="Создать или проверить .CONFIG")
    ap.add_argument("--reset", action="store_true", help="пересоздать .CONFIG пустым из шаблона (старый переименуется в .CONFIG.backup-*)")
    ap.add_argument("--check", action="store_true", help="запустить проверки безопасности и целостности")
    args = ap.parse_args()
    if args.reset:
        reset()
    else:
        copy_template()
    return check() if args.check else 0


if __name__ == "__main__":
    sys.exit(main())
