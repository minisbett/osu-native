#!/usr/bin/env bash
set -euo pipefail

REF_BRANCH="${REF_BRANCH:-master}"

case "$(uname -s)" in
    Linux) LIBRARY="osu.Native.so" ;;
    Darwin) LIBRARY="osu.Native.dylib" ;;
    *) LIBRARY="osu.Native.dll" ;;
esac

TEMP_DIR="$(mktemp -d)"
BUILD_LOG_DIR="${BUILD_LOG_DIR:-$TEMP_DIR}"

mkdir -p "$TEMP_DIR/ref-src"

echo "[1/4] Publishing working tree..."
dotnet publish osu.Native -c Release --ucr -p:PublishDir="$TEMP_DIR/local" > "$BUILD_LOG_DIR/local.log" 2>&1

echo "[2/4] Archiving '$REF_BRANCH'..."
git archive "$REF_BRANCH" | tar -x -C "$TEMP_DIR/ref-src"

echo "[3/4] Publishing '$REF_BRANCH'..."
dotnet publish "$TEMP_DIR/ref-src/osu.Native" -c Release --ucr -p:PublishDir="$TEMP_DIR/ref" > "$BUILD_LOG_DIR/ref.log" 2>&1

echo "[4/4] Comparing..."
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