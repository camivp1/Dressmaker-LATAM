#!/usr/bin/env python3
"""Validate Dressmaker es-419 translation CSV files."""

from __future__ import annotations

import argparse
import csv
import json
import re
from collections import Counter, defaultdict
from pathlib import Path


PLACEHOLDER_RE = re.compile(r"\{[^{}\r\n]+\}")
TAG_RE = re.compile(r"</?[^<>\r\n]+?>")
VARIABLE_RE = re.compile(r"(?<!\w)(?:\$[A-Za-z_][\w.]*|%[A-Za-z_][\w.]*)")


def project_root() -> Path:
    return Path(__file__).resolve().parents[1]


def read_csv(path: Path) -> list[dict[str, str]]:
    with path.open("r", encoding="utf-8-sig", newline="") as handle:
        return list(csv.DictReader(handle))


def multiset_diff(source: list[str], target: list[str]) -> tuple[list[str], list[str]]:
    source_counter = Counter(source)
    target_counter = Counter(target)
    removed = list((source_counter - target_counter).elements())
    added = list((target_counter - source_counter).elements())
    return removed, added


def validate_file(path: Path) -> list[dict[str, str]]:
    rows = read_csv(path)
    issues: list[dict[str, str]] = []
    ids_by_table: defaultdict[str, Counter[str]] = defaultdict(Counter)
    keys_by_table: defaultdict[str, Counter[str]] = defaultdict(Counter)

    for index, row in enumerate(rows, start=2):
        table = row.get("table", "")
        row_id = row.get("id", "")
        key = row.get("key", "")
        source = row.get("source_text", "") or ""
        translated = row.get("es_419", row.get("translation", "")) or ""
        ids_by_table[table][row_id] += 1
        if key:
            keys_by_table[table][key] += 1
        if not source:
            issues.append({"file": str(path), "line": str(index), "severity": "error", "type": "missing_source", "detail": "Row has no source_text."})
        if not translated:
            continue

        for label, pattern in (
            ("placeholder", PLACEHOLDER_RE),
            ("tag", TAG_RE),
            ("variable", VARIABLE_RE),
        ):
            removed, added = multiset_diff(pattern.findall(source), pattern.findall(translated))
            if removed:
                issues.append(
                    {
                        "file": str(path),
                        "line": str(index),
                        "severity": "error",
                        "type": f"removed_{label}",
                        "detail": json.dumps(removed, ensure_ascii=False),
                    }
                )
            if added:
                issues.append(
                    {
                        "file": str(path),
                        "line": str(index),
                        "severity": "warning",
                        "type": f"added_{label}",
                        "detail": json.dumps(added, ensure_ascii=False),
                    }
                )

    for table, counter in ids_by_table.items():
        for row_id, count in counter.items():
            if row_id and count > 1:
                issues.append({"file": str(path), "line": "", "severity": "error", "type": "duplicate_id", "detail": f"{table}:{row_id} appears {count} times."})
    for table, counter in keys_by_table.items():
        for key, count in counter.items():
            if key and count > 1:
                issues.append({"file": str(path), "line": "", "severity": "error", "type": "duplicate_key", "detail": f"{table}:{key} appears {count} times."})
    return issues


def main() -> int:
    parser = argparse.ArgumentParser(description="Validate es-419 translation CSV files.")
    parser.add_argument("paths", nargs="*", type=Path, help="CSV files to validate. Defaults to translation/*.csv")
    parser.add_argument("--json", action="store_true", help="Print JSON report.")
    args = parser.parse_args()

    root = project_root()
    paths = args.paths or sorted((root / "translation").glob("*.csv"))
    issues: list[dict[str, str]] = []
    for path in paths:
        issues.extend(validate_file(path))

    report = {
        "files_checked": [str(path) for path in paths],
        "issue_count": len(issues),
        "issues": issues,
    }
    report_path = root / "docs" / "validation-report.json"
    report_path.parent.mkdir(parents=True, exist_ok=True)
    with report_path.open("w", encoding="utf-8") as handle:
        json.dump(report, handle, ensure_ascii=False, indent=2)

    if args.json:
        print(json.dumps(report, ensure_ascii=False, indent=2))
    else:
        print(f"files_checked={len(paths)}")
        print(f"issues={len(issues)}")
        print(f"report={report_path}")
    return 1 if any(issue["severity"] == "error" for issue in issues) else 0


if __name__ == "__main__":
    raise SystemExit(main())
