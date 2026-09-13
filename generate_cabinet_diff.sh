#!/usr/bin/env bash
set -euo pipefail

ORIG_BRANCH="$(git branch --show-current)"
REF_BRANCH="master"

if [[ -n "$(git status --porcelain)" ]]; then
    echo "Error: Please commit or stash all changes first."
    exit 1
fi

if [[ $ORIG_BRANCH == $REF_BRANCH ]]; then
    echo "Error: You are on the reference branch."
    exit 1
fi

TEMP_DIR="$(mktemp -d)"

cleanup() {
    echo "[4/5] Checking out '$ORIG_BRANCH'..."
    git checkout "$ORIG_BRANCH" >/dev/null 2>&1 || true
    echo "[5/5] Cleaning up..."
    rm -rf "$TEMP_DIR"
}

trap cleanup EXIT

echo "[1/5] Publishing '$ORIG_BRANCH' branch..."
dotnet publish --ucr -p:PublishDir="$TEMP_DIR/local" >/dev/null 2>&1

echo "[2/5] Checking out '$REF_BRANCH'..."
git checkout "$REF_BRANCH" >/dev/null 2>&1
echo "[3/5] Publishing '$REF_BRANCH' branch..."
dotnet publish --ucr -p:PublishDir="$TEMP_DIR/ref" >/dev/null 2>&1

echo
echo
git --no-pager diff --no-index "$TEMP_DIR/ref/cabinet.h" "$TEMP_DIR/local/cabinet.h" || true
echo
echo