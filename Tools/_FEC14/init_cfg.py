import shutil
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
TEMPLATE = Path(__file__).resolve().parent / "CONFIG.template"
TARGET = ROOT / ".CONFIG"


def main():
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
    return 0


if __name__ == "__main__":
    sys.exit(main())
