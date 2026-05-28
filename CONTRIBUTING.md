# Contributing to pagmo.NET

## Prerequisites

| Tool | Version | Notes |
|------|---------|-------|
| .NET SDK | 10.x | |
| Visual Studio 2022 / Build Tools | 2022+ | C++ toolchain required |
| vcpkg | latest | Set `VCPKG_ROOT` env var |
| SWIG | 4.4.x | Only needed to regenerate wrappers |
| PowerShell | 7+ | `pwsh` must be on PATH |

## Cloning

The native bridge lives in the `pagmoNet` submodule. Always clone recursively:

```powershell
git clone --recurse-submodules https://github.com/samthegliderpilot/pagmo.NET
```

If you already cloned without submodules:

```powershell
git submodule update --init --recursive
```

## Building the native layer

```powershell
# Debug (fast, no IPOPT)
.\scripts\build-native.ps1

# Release with all optimizers
.\scripts\build-native.ps1 -Configuration Release
```

The script detects whether `pagmoNet\native\CMakeLists.txt` exists (submodule present) and uses it; otherwise falls back to the legacy `pagmoWrapper\` directory. The DLL lands in `pagmoNet\native\win-build\PagmoWrapper.dll` (or `pagmoWrapper\win-build\` on the legacy path).

## Running tests

```powershell
$env:PATH = "$(Resolve-Path pagmoNet\native\win-build);$env:PATH"
dotnet test Tests/Tests.Pagmo.NET/Tests.Pagmo.NET.csproj -p:Platform=x64 --logger "console;verbosity=normal"
```

Expected: 103 tests green.

## Running examples

```powershell
dotnet run --project Examples/Examples.Pagmo.NET -- all
```

## Regenerating SWIG wrappers

Only needed after editing `.i` interface files in `pagmoNet/swig/`:

```powershell
.\createSwigWrappersAndPlaceThem.ps1
```

SWIG is resolved via `SWIG_EXE` env var, `SWIG_HOME`, or `PATH`. Pre-generated wrappers are checked in — most contributions do not require regeneration.

## Repo layout

```
pagmo.NET/
  Pagmo.NET/          C# library (pagmoExtensions/, generated wrappers)
  Tests/              xUnit test suite
  Examples/           Runnable examples (single, archipelago, cloning, etc.)
  docs/               Markdown documentation
  scripts/            build-native.ps1, build-release-artifacts.ps1, etc.
  pagmoNet/           Submodule: shared SWIG interface + native C++ bridge
  pagmoWrapper/       Legacy MSBuild project (pre-submodule fallback)
  ports/              vcpkg overlay ports (coin-or-ipopt fix)
  triplets/           vcpkg custom triplets
```

## Pull requests

- Keep PRs focused — one concern per PR
- Run `dotnet test` before opening
- If you change the SWIG interface, regenerate wrappers and include them in the PR
- The `pagmoNet` submodule is a separate repo; changes to the native bridge go there first
