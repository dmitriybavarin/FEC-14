import argparse
import json
import re
import subprocess
import sys
from collections import Counter
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
ENGINE = ROOT / "RobustToolbox"
STASH_MSG = "fec-engine-local"
REPORT = ROOT / "bin" / "engine_build_report.txt"


def out(msg=""):
    print(msg, flush=True)


def fail(msg):
    out(f"ОШИБКА: {msg}")
    sys.exit(1)


def git(args, cwd=ENGINE, check=True):
    r = subprocess.run(["git", *args], cwd=cwd, text=True, encoding="utf-8", errors="replace", capture_output=True)
    if check and r.returncode != 0:
        fail(f"git {' '.join(args)}\n{(r.stderr or r.stdout).strip()}")
    return r.stdout.strip()


def git_ok(args, cwd=ENGINE):
    return subprocess.run(["git", *args], cwd=cwd, capture_output=True).returncode == 0


def read_tag(path, tag):
    if not path.exists():
        return "?"
    m = re.search(rf"<{tag}>([^<]+)</{tag}>", path.read_text(encoding="utf-8-sig"))
    return m.group(1) if m else "?"


def engine_version():
    return read_tag(ENGINE / "MSBuild" / "Robust.Engine.Version.props", "Version")


def engine_framework():
    return read_tag(ENGINE / "MSBuild" / "Robust.Engine.props", "TargetFramework")


def sdk_pin():
    path = ROOT / "global.json"
    if not path.exists():
        return "?"
    return json.loads(path.read_text(encoding="utf-8-sig")).get("sdk", {}).get("version", "?")


def gitlink(ref):
    line = git(["ls-tree", ref, "RobustToolbox"], cwd=ROOT, check=False)
    parts = line.split()
    return parts[2] if len(parts) >= 3 else None


def index_gitlink():
    line = git(["ls-files", "-s", "RobustToolbox"], cwd=ROOT, check=False)
    parts = line.split()
    return parts[1] if len(parts) >= 2 else None


def head():
    return git(["rev-parse", "HEAD"])


def branch():
    name = git(["branch", "--show-current"], check=False)
    return name or None


def dirty():
    return [l for l in git(["status", "--porcelain"]).splitlines() if l.strip()]


def unmerged():
    return [l for l in git(["diff", "--name-only", "--diff-filter=U"]).splitlines() if l.strip()]


def merging():
    return git_ok(["rev-parse", "-q", "--verify", "MERGE_HEAD"])


def describe(sha):
    if not sha:
        return "-"
    tag = git(["describe", "--tags", "--exact-match", sha], check=False)
    return f"{sha[:10]} ({tag})" if tag else sha[:10]


def resolve_ref(ref):
    for cand in (ref, f"v{ref}", f"origin/{ref}", f"fec/{ref}"):
        sha = git(["rev-parse", "-q", "--verify", f"{cand}^{{commit}}"], check=False)
        if sha:
            return sha
    fail(f"не найдена версия или коммит '{ref}'. Сначала выполните: engine.py fetch")


def stash_local():
    if not dirty():
        return False
    git(["stash", "push", "-m", STASH_MSG])
    out("Локальные правки движка временно убраны в stash.")
    return True


def pop_local():
    stashes = git(["stash", "list"]).splitlines()
    if not stashes or STASH_MSG not in stashes[0]:
        return
    r = subprocess.run(["git", "stash", "pop"], cwd=ENGINE, capture_output=True, text=True)
    if r.returncode == 0:
        out("Локальные правки движка возвращены.")
        return
    out("Локальные правки движка конфликтуют с новой версией:")
    for f in unmerged():
        out(f"  {f}")
    out("Решите конфликт в файлах выше (или engine.py resolve ours|theirs <файл>), затем: git -C RobustToolbox stash drop")


def update_engine_submodules():
    git(["submodule", "update", "--init", "--recursive"])


def stage_gitlink():
    git(["add", "RobustToolbox"], cwd=ROOT)
    out("Новая версия движка записана в индекс репозитория (git add RobustToolbox), сборка ее не откатит.")


def check_sdk():
    fw = engine_framework()
    sdk = sdk_pin()
    m_fw = re.match(r"net(\d+)", fw)
    m_sdk = re.match(r"(\d+)\.", sdk)
    if m_fw and m_sdk and int(m_fw.group(1)) > int(m_sdk.group(1)):
        out(f"ВНИМАНИЕ: движок требует {fw}, а global.json закрепляет SDK {sdk}. Поменяйте sdk.version в global.json на {m_fw.group(1)}.0.100.")


def cmd_status(_):
    out(f"Репозиторий:        {ROOT}")
    out(f"Движок:             {engine_version()} ({engine_framework()}), SDK в global.json: {sdk_pin()}")
    out(f"Коммит движка:      {describe(head())}")
    out(f"Ветка движка:       {branch() or 'нет (detached HEAD)'}")
    out(f"В коммите FEC-14:   {describe(gitlink('HEAD'))}")
    out(f"В индексе FEC-14:   {describe(index_gitlink())}")
    rmc = gitlink("upstream/master")
    out(f"У RMC-14 (upstream): {describe(rmc) if rmc else 'неизвестно (нет remote upstream или не было fetch)'}")
    latest = git(["tag", "--list", "v*", "--sort=-v:refname"], check=False).splitlines()
    out(f"Последний офиц. тег: {latest[0] if latest else 'неизвестно (engine.py fetch)'}")
    remotes = sorted(set(l.split()[0] + " " + l.split()[1] for l in git(["remote", "-v"]).splitlines()))
    out("Remotes движка:     " + "; ".join(remotes))
    d = dirty()
    out(f"Локальные правки:   {len(d)}")
    for l in d:
        out(f"  {l}")
    if merging():
        out("Идет слияние. Конфликты:")
        for f in unmerged():
            out(f"  {f}")


def cmd_fetch(_):
    out("Скачиваю версии движка...")
    git(["fetch", "origin", "--tags", "--prune"])
    if "fec" in git(["remote"]).split():
        git(["fetch", "fec", "--tags", "--prune"])
    if "upstream" in git(["remote"], cwd=ROOT).split():
        out("Скачиваю RMC-14 (upstream)...")
        git(["fetch", "upstream"], cwd=ROOT)
    out("Готово.")


def cmd_versions(a):
    rows = git(["for-each-ref", "refs/tags/v*", "--sort=-v:refname", f"--count={a.count}",
                "--format=%(refname:short)  %(creatordate:short)"])
    out(rows or "Тегов нет, выполните: engine.py fetch")


def switch_to(sha_or_branch, is_branch=False):
    if merging():
        fail("идет слияние. Завершите его (engine.py abort или commit) перед сменой версии.")
    stashed = stash_local()
    if is_branch:
        git(["switch", sha_or_branch])
    else:
        git(["checkout", "--detach", sha_or_branch])
    update_engine_submodules()
    if stashed:
        pop_local()
    stage_gitlink()
    out(f"Движок теперь: {engine_version()} ({engine_framework()}), {describe(head())}")
    check_sdk()
    out("Дальше: engine.py build")


def cmd_use(a):
    is_branch = git_ok(["show-ref", "--verify", "--quiet", f"refs/heads/{a.ref}"])
    switch_to(a.ref if is_branch else resolve_ref(a.ref), is_branch)


def cmd_rmc(_):
    if "upstream" not in git(["remote"], cwd=ROOT).split():
        fail("нет remote upstream (RMC-14) в репозитории FEC-14.")
    git(["fetch", "upstream"], cwd=ROOT)
    sha = gitlink("upstream/master")
    if not sha:
        fail("не удалось узнать версию движка у RMC-14.")
    if not git_ok(["cat-file", "-e", f"{sha}^{{commit}}"]):
        git(["fetch", "origin", sha])
    out(f"RMC-14 использует движок {describe(sha)}")
    switch_to(sha)


def cmd_merge(a):
    if not branch():
        fail("движок не на ветке. Сначала создайте ветку форка: engine.py branch <имя>")
    if merging():
        fail("слияние уже идет: engine.py conflicts / resolve / abort")
    sha = resolve_ref(a.ref)
    stashed = stash_local()
    r = subprocess.run(["git", "merge", "--no-ff", "--no-commit", sha], cwd=ENGINE, capture_output=True, text=True)
    update_engine_submodules()
    if r.returncode != 0 and unmerged():
        out(f"Конфликты при слиянии {a.ref}:")
        cmd_conflicts(a)
        if stashed:
            out("Локальные правки лежат в stash, после слияния: git -C RobustToolbox stash pop")
        return
    if r.returncode != 0:
        fail((r.stderr or r.stdout).strip())
    if stashed:
        pop_local()
    out(f"Слияние {a.ref} подготовлено без конфликтов, коммит не создан.")
    out("Проверьте сборку (engine.py build), затем закоммитьте в RobustToolbox, запушьте ветку и выполните git add RobustToolbox.")


def cmd_conflicts(_):
    files = unmerged()
    if not files:
        out("Конфликтов нет.")
        return
    for f in files:
        text = (ENGINE / f).read_text(encoding="utf-8", errors="replace") if (ENGINE / f).exists() else ""
        out(f"  {f}  (блоков: {text.count('<<<<<<<')})")
    out("Варианты: открыть файл и поправить вручную, затем git -C RobustToolbox add <файл>;")
    out("  engine.py resolve ours <файл>   оставить версию ветки FEC;")
    out("  engine.py resolve theirs <файл> взять официальную версию;")
    out("  engine.py abort                 отменить слияние.")


def cmd_resolve(a):
    files = a.files or unmerged()
    if not files:
        out("Конфликтов нет.")
        return
    for f in files:
        git(["checkout", f"--{a.side}", "--", f])
        git(["add", "--", f])
        out(f"  {f}: взята версия {'FEC' if a.side == 'ours' else 'официальная'}")
    left = unmerged()
    out("Все конфликты решены." if not left else f"Осталось конфликтов: {len(left)}")


def cmd_abort(_):
    if merging():
        git(["merge", "--abort"])
        update_engine_submodules()
        out("Слияние отменено.")
    pop_local()


def cmd_restore(_):
    sha = gitlink("HEAD")
    if not sha:
        fail("в коммите FEC-14 нет записи о движке.")
    if merging():
        fail("идет слияние, сначала engine.py abort")
    git(["reset", "-q", "--", "RobustToolbox"], cwd=ROOT)
    stashed = stash_local()
    git(["checkout", "--detach", sha])
    update_engine_submodules()
    if stashed:
        pop_local()
    out(f"Движок возвращен к версии из коммита FEC-14: {describe(sha)}")


def cmd_branch(a):
    if a.remote:
        if "fec" in git(["remote"]).split():
            git(["remote", "set-url", "fec", a.remote])
        else:
            git(["remote", "add", "fec", a.remote])
        out(f"Remote fec -> {a.remote}")
    if git_ok(["show-ref", "--verify", "--quiet", f"refs/heads/{a.name}"]):
        git(["switch", a.name])
    else:
        git(["switch", "-c", a.name])
    out(f"Движок на ветке {a.name} ({describe(head())}). Локальные правки остались в рабочей копии.")
    out("Дальше вручную (коммит и пуш делает владелец репозитория):")
    out(f"  git -C RobustToolbox add -A && git -C RobustToolbox commit -m \"FEC: правки движка\"")
    out(f"  git -C RobustToolbox push -u fec {a.name}")
    out(f"  git submodule set-url RobustToolbox <URL форка движка>")
    out(f"  git submodule set-branch --branch {a.name} RobustToolbox")
    out(f"  git add .gitmodules RobustToolbox && git commit -m \"FEC: движок из форка\"")


def summarize(logs):
    errors = set()
    for log in logs:
        for line in log.splitlines():
            if re.search(r": error [A-Z]+\d+", line) or re.search(r": error :", line):
                errors.add(line.strip())
    codes = Counter(m.group(1) for e in errors if (m := re.search(r"error ([A-Z]+\d+)", e)))
    names = Counter(m.group(1) for e in errors if (m := re.search(r"error CS02(?:46|34)[^\"'«]*[\"'«]([^\"'»]+)[\"'»]", e)))
    files = Counter(m.group(1) for e in errors if (m := re.match(r"(.+?)\(\d+,\d+\)", e)))
    projects = Counter(m.group(1) for e in errors if (m := re.search(r"\[.*?([^\\/]+)\.csproj\]", e)))
    lines = [f"Ошибок: {len(errors)}", "", "По проектам:"]
    lines += [f"  {n:5}  {k}" for k, n in projects.most_common()]
    lines += ["", "По кодам:"] + [f"  {n:5}  {k}" for k, n in codes.most_common(20)]
    lines += ["", "Не найденные типы и пространства имен:"] + [f"  {n:5}  {k}" for k, n in names.most_common(40)]
    lines += ["", "Файлы с наибольшим числом ошибок:"]
    lines += [f"  {n:5}  {Path(k).relative_to(ROOT) if k.startswith(str(ROOT)) else k}" for k, n in files.most_common(30)]
    lines += ["", "Все ошибки:"] + sorted(errors)
    return len(errors), "\n".join(lines)


def cmd_build(a):
    check_sdk()
    logs = []
    for proj in ("Content.Server", "Content.Client"):
        out(f"Собираю {proj} ({a.configuration})...")
        r = subprocess.run(["dotnet", "build", f"{proj}/{proj}.csproj", "-c", a.configuration, "-m", "-v", "q", "-nologo"],
                           cwd=ROOT, capture_output=True, text=True, encoding="utf-8", errors="replace")
        logs.append(r.stdout + r.stderr)
    count, report = summarize(logs)
    REPORT.parent.mkdir(parents=True, exist_ok=True)
    REPORT.write_text(report, encoding="utf-8")
    out("\n".join(report.split("\nВсе ошибки:")[0].splitlines()[:60]))
    out(f"\nПолный отчет: {REPORT}")
    if any("MSB3030" in l for l in logs):
        out("Есть MSB3030: путь к папке слишком длинный для Windows. Перенесите репозиторий ближе к корню диска или используйте subst.")
    sys.exit(1 if count else 0)


def main():
    if hasattr(sys.stdout, "reconfigure"):
        sys.stdout.reconfigure(encoding="utf-8")
    if not (ENGINE / ".git").exists():
        fail("сабмодуль RobustToolbox не инициализирован, запустите python RUN_THIS.py")
    p = argparse.ArgumentParser(prog="engine.py", description="Обновление движка RobustToolbox для FEC-14")
    sub = p.add_subparsers(dest="cmd", required=True)
    sub.add_parser("status", help="текущая версия движка, ветка, правки, версия у RMC-14").set_defaults(fn=cmd_status)
    sub.add_parser("fetch", help="скачать версии движка и RMC-14").set_defaults(fn=cmd_fetch)
    s = sub.add_parser("versions", help="список официальных версий")
    s.add_argument("-n", "--count", type=int, default=15)
    s.set_defaults(fn=cmd_versions)
    s = sub.add_parser("use", help="переключить движок на версию, коммит или ветку (292.0.0, v292.0.0, sha, имя ветки)")
    s.add_argument("ref")
    s.set_defaults(fn=cmd_use)
    sub.add_parser("rmc", help="поставить ту же версию движка, что у RMC-14").set_defaults(fn=cmd_rmc)
    s = sub.add_parser("merge", help="влить официальную версию в ветку форка движка (без коммита)")
    s.add_argument("ref")
    s.set_defaults(fn=cmd_merge)
    sub.add_parser("conflicts", help="показать конфликты слияния").set_defaults(fn=cmd_conflicts)
    s = sub.add_parser("resolve", help="решить конфликт: ours = версия FEC, theirs = официальная")
    s.add_argument("side", choices=["ours", "theirs"])
    s.add_argument("files", nargs="*")
    s.set_defaults(fn=cmd_resolve)
    sub.add_parser("abort", help="отменить слияние").set_defaults(fn=cmd_abort)
    sub.add_parser("restore", help="вернуть движок к версии из коммита FEC-14").set_defaults(fn=cmd_restore)
    s = sub.add_parser("branch", help="создать или выбрать ветку форка движка")
    s.add_argument("name")
    s.add_argument("--remote", help="URL форка движка на GitHub, станет remote fec")
    s.set_defaults(fn=cmd_branch)
    s = sub.add_parser("build", help="собрать сервер и клиент и сгруппировать ошибки")
    s.add_argument("-c", "--configuration", default="DebugOpt")
    s.set_defaults(fn=cmd_build)
    a = p.parse_args()
    a.fn(a)


if __name__ == "__main__":
    main()
