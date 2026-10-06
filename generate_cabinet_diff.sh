#!/usr/bin/env bash
set -euo pipefail

ORIG_BRANCH="$(git branch --show-current)"
ORIG_BRANCH="${ORIG_BRANCH:-$(git rev-parse HEAD)}"
REF_BRANCH="${REF_BRANCH:-master}"

case "$(uname -s)" in
    Linux) LIBRARY="osu.Native.so" ;;
    Darwin) LIBRARY="osu.Native.dylib" ;;
    *) LIBRARY="osu.Native.dll" ;;
esac

if [[ -n "$(git status --porcelain)" ]]; then
    echo "Error: Please commit or stash all changes first."
    exit 1
fi

if [[ $ORIG_BRANCH == $REF_BRANCH ]]; then
    echo "Error: You are on the reference branch."
    exit 1
fi

TEMP_ROOT="$(cd "${TMPDIR:-/tmp}" && pwd -P)"
TEMP_DIR="$(mktemp -d "$TEMP_ROOT/osu-native-diff.XXXXXX")"
BUILD_LOG_DIR="${BUILD_LOG_DIR:-$TEMP_DIR}"

cleanup() {
    echo "[4/5] Checking out '$ORIG_BRANCH'..."
    git checkout "$ORIG_BRANCH" >/dev/null 2>&1 || true
    echo "[5/5] Cleaning up..."
    [[ "$TEMP_DIR" == "$TEMP_ROOT"/osu-native-diff.* ]] && rm -rf "$TEMP_DIR"
}

trap cleanup EXIT

echo "[1/5] Publishing '$ORIG_BRANCH' branch..."
dotnet publish osu.Native -c Release --ucr -p:PublishDir="$TEMP_DIR/local" > "$BUILD_LOG_DIR/local.log" 2>&1

echo "[2/5] Checking out '$REF_BRANCH'..."
git checkout "$REF_BRANCH" >/dev/null 2>&1
echo "[3/5] Publishing '$REF_BRANCH' branch..."
dotnet publish osu.Native -c Release --ucr -p:PublishDir="$TEMP_DIR/ref" > "$BUILD_LOG_DIR/ref.log" 2>&1

echo
echo
REF_SIZE="$(wc -c < "$TEMP_DIR/ref/$LIBRARY")"
LOCAL_SIZE="$(wc -c < "$TEMP_DIR/local/$LIBRARY")"
printf '%s size: %s -> %s bytes (%+d)\n' "$LIBRARY" "$REF_SIZE" "$LOCAL_SIZE" "$((LOCAL_SIZE - REF_SIZE))"
echo
sed -i '/^\/\/ *Date:/d' "$TEMP_DIR/ref/cabinet.h" "$TEMP_DIR/local/cabinet.h"
git --no-pager diff --no-index "$TEMP_DIR/ref/cabinet.h" "$TEMP_DIR/local/cabinet.h" || [[ $? -eq 1 ]]
echo
echo