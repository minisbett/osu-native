#!/usr/bin/env bash

set -euo pipefail

trap 'echo "Failed at line $LINENO: $BASH_COMMAND (exit $?)" >&2' ERR

REF_BRANCH="${REF_BRANCH:-master}"

case "$(uname -s)" in
    Linux) LIBRARY="osu.Native.so" ;;
    Darwin) LIBRARY="osu.Native.dylib" ;;
    *) LIBRARY="osu.Native.dll" ;;
esac

TEMP_DIR="$(mktemp -d)"

cleanup() {
    rm -rf "$TEMP_DIR"
}

trap cleanup EXIT

dotnet publish osu.Native -c Release --ucr -p:PublishDir="$TEMP_DIR/local"

mkdir "$TEMP_DIR/ref"
git archive "$REF_BRANCH" | tar -x -C "$TEMP_DIR/ref"
dotnet publish "$TEMP_DIR/ref/osu.Native" -c Release --ucr -p:PublishDir="$TEMP_DIR/ref"

LOCAL_SIZE="$(wc -c < "$TEMP_DIR/local/$LIBRARY")"
REF_SIZE="$(wc -c < "$TEMP_DIR/ref/$LIBRARY")"
SIZE_DIFF=$((LOCAL_SIZE - REF_SIZE))

sed -Ei '/^\/\/ *(Date|Assembly):/d' "$TEMP_DIR/ref/cabinet.h" "$TEMP_DIR/local/cabinet.h"
DIFF="$(git --no-pager diff --no-index "$TEMP_DIR/ref/cabinet.h" "$TEMP_DIR/local/cabinet.h")"

printf '%s size: %s -> %s bytes (%+d)\n' "$LIBRARY" "$REF_SIZE" "$LOCAL_SIZE" "$SIZE_DIFF"
printf '%s\n' "$DIFF"

if [[ -n "${CI:-}" ]]; then
    {
        printf 'ref_size=%s\n' "$REF_SIZE"
        printf 'local_size=%s\n' "$LOCAL_SIZE"
        printf 'size_diff=%s\n' "$SIZE_DIFF"
        printf 'diff<<EOF\n%s\nEOF\n' "$DIFF"
    } >> "$GITHUB_OUTPUT"
fi