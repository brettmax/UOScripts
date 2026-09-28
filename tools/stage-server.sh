#!/usr/bin/env bash
# Stages a runnable T2A server from this package set.
#
#   tools/stage-server.sh [Debug|Release] [output-dir]
#
# Builds AxmolUO.Server.slnx, then assembles <output-dir> (default: Staging/)
# from ModernUO's Distribution, the ModernSpawner module and the script
# assemblies, and overlays this repository's Distribution/ (T2A expansion
# preset and assemblies.json). Saves, Logs and existing Configuration in
# <output-dir> are left alone, so re-staging does not wipe a shard.
set -euo pipefail

REPO_ROOT="$(cd "$(dirname "$0")/.." && pwd)"
CONFIG="${1:-Release}"
OUT="${2:-$REPO_ROOT/Staging}"

git -C "$REPO_ROOT" submodule update --init

dotnet build "$REPO_ROOT/AxmolUO.Server.slnx" -c "$CONFIG"

# ModernSpawner is a library; publish it so YamlDotNet travels with it.
SPAWNER_PUB="$(mktemp -d)"
trap 'rm -rf "$SPAWNER_PUB"' EXIT
dotnet publish "$REPO_ROOT/Modules/ModernSpawner/Projects/ModernSpawner/ModernSpawner.csproj" \
  -c "$CONFIG" --no-build -o "$SPAWNER_PUB"

mkdir -p "$OUT/Assemblies"
tar -C "$REPO_ROOT/ModernUO/Distribution" \
  --exclude './Saves' --exclude './Logs' --exclude './Configuration' -cf - . | tar -C "$OUT" -xf -
cp "$SPAWNER_PUB/ModernSpawner.dll" "$SPAWNER_PUB/YamlDotNet.dll" "$OUT/Assemblies/"

# Package overlay. The expansion preset is only written on first stage, so a
# shard that later edits its Configuration keeps its choice.
cp "$REPO_ROOT/Distribution/Data/assemblies.json" "$OUT/Data/assemblies.json"
mkdir -p "$OUT/Configuration"
if [ ! -f "$OUT/Configuration/expansion.json" ]; then
  cp "$REPO_ROOT/Distribution/Configuration/expansion.json" "$OUT/Configuration/expansion.json"
fi

echo "Staged T2A server in $OUT (run: dotnet $OUT/ModernUO.dll)"
