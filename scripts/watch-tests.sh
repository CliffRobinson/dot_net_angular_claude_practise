#!/usr/bin/env bash
# scripts/watch-tests.sh
cd "$(dirname "$0")/../DotNetApi.Tests"
while true; do
  find .. \( -name '*.cs' -o -name '*.csproj' \) -not -path '*/bin/*' -not -path '*/obj/*' \
    | entr -cd dotnet test
done
