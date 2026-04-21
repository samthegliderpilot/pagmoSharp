# Linux Testing Handoff

This document is a briefing for a Claude AI session picking up Linux cross-platform
testing of Pagmo.NET. The Windows side of this work is complete and passing (593/593
tests). You are on a Linux machine trying to get the same tests passing.

---

## What Was Done on Windows

Pagmo.NET was originally Windows-only. The following changes were made to add Linux support:

| File | What changed |
|------|-------------|
| `pagmoWrapper/managed_bridge.cpp` | `PAGMONET_EXPORT` macro now uses `__attribute__((visibility("default")))` on non-Windows |
| `swig/PagmoNETSwigInterface.i` | All `%include` backslash path separators → forward slashes |
| `pagmoWrapper/CMakeLists.txt` | **NEW** — CMake build for the native shared library |
| `createSwigWrappersAndPlaceThem.ps1` | **NEW** — cross-platform PowerShell Core SWIG regen script |
| `scripts/regen-swig.ps1` | Calls `.ps1` on all platforms; `.bat` fallback on Windows only |
| `scripts/build-native.ps1` | Linux branch: CMake configure + build; Windows branch: MSBuild |
| `Pagmo.NET/Pagmo.NET.csproj` | PostBuild Windows-conditional; Linux target copies `.so` from CMake build dir |

Key architectural point: `[DllImport("PagmoWrapper")]` in C# resolves to
`libPagmoWrapper.so` on Linux automatically — no C# changes were needed.

---

## Prerequisites to Install

```bash
# .NET 8 SDK
# (follow https://learn.microsoft.com/en-us/dotnet/core/install/linux)

# PowerShell Core (for build scripts)
# (follow https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-linux)

# Build tools
sudo apt-get install -y cmake ninja-build build-essential git curl zip unzip tar

# SWIG 4.4.x — check distro version first
swig -version
# If older than 4.4, build from source or use a PPA:
# sudo apt-get install -y swig   (may be 4.0 on older Ubuntu — test if it works)

# vcpkg
git clone https://github.com/microsoft/vcpkg ~/vcpkg
~/vcpkg/bootstrap-vcpkg.sh
export VCPKG_ROOT=~/vcpkg

# pagmo2 and its dependencies via vcpkg
~/vcpkg/vcpkg install pagmo2:x64-linux

# Optional solvers (skip if you just want the baseline working first):
# ~/vcpkg/vcpkg install nlopt:x64-linux
# ~/vcpkg/vcpkg install coin-or-ipopt:x64-linux
```

---

## Build Steps

All scripts use PowerShell Core (`pwsh`). Run from the repo root.

### Step 1 — Regenerate SWIG wrappers (optional if you trust the checked-in ones)

The pre-generated wrappers (`pagmoWrapper/GeneratedWrappers.cxx`,
`Pagmo.NET/pygmoWrappers/*.cs`) are already committed and should be identical on Linux
because SWIG output is deterministic. You can skip this step unless you changed a `.i` file.

```bash
pwsh scripts/regen-swig.ps1
```

If SWIG is not on PATH, set `SWIG_EXE` or `SWIG_HOME` first.

### Step 2 — Build the native shared library

```bash
pwsh scripts/build-native.ps1 -Configuration Release
```

This runs:
```
cmake -B pagmoWrapper/build -S pagmoWrapper \
  -DCMAKE_BUILD_TYPE=Release \
  -DCMAKE_TOOLCHAIN_FILE=$VCPKG_ROOT/scripts/buildsystems/vcpkg.cmake
cmake --build pagmoWrapper/build --config Release
```

Expected output: `pagmoWrapper/build/libPagmoWrapper.so`

### Step 3 — Build and test the managed assembly

```bash
dotnet build Pagmo.NET/Pagmo.NET.csproj
dotnet test Tests/Tests.Pagmo.NET/Tests.Pagmo.NET.csproj
```

Expected: **593 tests passed, 0 failed**.

---

## Likely Issues and How to Fix Them

### CMake can't find pagmo2

```
CMake Error: Could not find a package configuration file provided by "Pagmo"
```

Fix: make sure `$VCPKG_ROOT` is set and `pagmo2:x64-linux` is installed:
```bash
$VCPKG_ROOT/vcpkg list | grep pagmo
```
Then ensure the toolchain file is being passed. Check `scripts/build-native.ps1` — it reads
`$env:VCPKG_ROOT`.

### Boost not found

pagmo2 depends on Boost serialization. vcpkg should pull it in automatically as a dependency
of `pagmo2:x64-linux`. If cmake complains:
```bash
$VCPKG_ROOT/vcpkg install boost-serialization:x64-linux
```

### SWIG version mismatch

If SWIG regen produces different output than what's committed (or errors), check the version:
```bash
swig -version   # needs 4.4.x
```
SWIG 4.0 (common on Ubuntu 22.04) may work but could produce minor differences. If it does,
update the committed generated files by running `pwsh scripts/regen-swig.ps1` and committing
the diff. The C# and C++ logic are unchanged; only formatting/comments differ between versions.

### `libPagmoWrapper.so` not found at test runtime

The `PostBuildLinux` MSBuild target in `Pagmo.NET.csproj` copies the `.so` from
`pagmoWrapper/build/` to the test output directory. If the copy didn't happen:

```bash
# Check if the .so exists
ls pagmoWrapper/build/libPagmoWrapper.so

# Check if it was copied next to the test DLL
ls Tests/Tests.Pagmo.NET/bin/x64/Debug/net*/libPagmoWrapper.so
```

If the copy target didn't fire (MSBuild property `OS` detection varies), copy manually:
```bash
cp pagmoWrapper/build/libPagmoWrapper.so \
   Tests/Tests.Pagmo.NET/bin/x64/Debug/net8.0/
```

Then investigate why the MSBuild target didn't fire and fix `Pagmo.NET.csproj` accordingly.
The condition is `Condition="'$(OS)' != 'Windows_NT'"` — on Linux `$(OS)` is typically empty
or `Unix`. You may need to change the condition to `Condition="!$([MSBuild]::IsOSPlatform('Windows'))"`.

### Linker errors in GeneratedWrappers.cxx

If the CMake build fails with undefined symbol errors in the SWIG-generated code, it is
likely a header path issue. Check `CMakeLists.txt` `target_include_directories` — it adds
`pagmoWrapper/` and `swig/pagmo/` to the include path. The generated code includes
`PagmoNETSwigInterface_wrap.h` and the hand-written bridge headers. Both should be found via
those paths.

### `__attribute__((visibility("default")))` errors

These would be compile errors in `managed_bridge.cpp` if the compiler doesn't support GCC
visibility attributes. GCC 4+ and Clang 3+ both support it. If you are using an unusual
compiler, check the macro guard in `managed_bridge.cpp` and adjust as needed.

---

## What to Report Back

Once you have the tests working (or have a specific error you can't resolve), record:

1. Exact Ubuntu / distro version
2. SWIG version (`swig -version`)
3. CMake version (`cmake --version`)
4. vcpkg pagmo2 version (`vcpkg list | grep pagmo`)
5. Any `CMakeLists.txt` changes needed (e.g. find_package names, Boost component names)
6. Any `Pagmo.NET.csproj` PostBuild target fixes needed
7. Final test result (`Passed: NNN, Failed: 0`)

That information can be used to update the build files and CI configuration.

---

## Files to Edit if Something Needs Fixing

| Problem | File to edit |
|---------|-------------|
| CMake configure fails | `pagmoWrapper/CMakeLists.txt` |
| Native build script logic | `scripts/build-native.ps1` |
| `.so` not copied to output | `Pagmo.NET/Pagmo.NET.csproj` (PostBuildLinux target) |
| SWIG regen fails on Linux | `createSwigWrappersAndPlaceThem.ps1` |
| SWIG `.i` include path issues | `swig/PagmoNETSwigInterface.i` |
| Export symbol not visible | `pagmoWrapper/managed_bridge.cpp` |
