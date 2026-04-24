# Linux Build & Test Guide

**Target:** Ubuntu 24.04 / Linux Mint 22.1, x64.
**This session's goals:**
1. Verify 593/593 tests still pass with the new vcpkg static-PIC build (first time testing this approach)
2. Confirm `libPagmoWrapper.so` has no system runtime dependencies (`ldd` check)
3. Run `build-release-artifacts.ps1` and copy the artifacts back to Windows for the beta release

---

## How the native library is built

`libPagmoWrapper.so` is built using the custom vcpkg triplet `triplets/x64-linux-static-pic.cmake`.
This triplet builds pagmo2, Boost.Serialization, and Intel TBB as **static, position-independent**
libraries (`-fPIC`). CMake links them all into the shared library at build time, so the resulting
`.so` requires no system packages at runtime — only `libstdc++` and `libgcc`, which are present
on every Linux system.

This is different from the earlier apt-based approach (used during initial Linux verification)
where `libpagmo.so.9` was a runtime system dependency. The static-PIC approach eliminates that
requirement entirely.

---

## Prerequisites

### 1 — Build tools and SWIG

```bash
sudo apt install -y cmake build-essential swig
```

Verified with cmake 3.31, SWIG 4.2.

### 2 — vcpkg

pagmo2 and its dependencies are built from source by vcpkg. This takes several minutes on
first run; subsequent builds use the cached installed packages.

```bash
git clone https://github.com/microsoft/vcpkg ~/vcpkg
~/vcpkg/bootstrap-vcpkg.sh
export VCPKG_ROOT=~/vcpkg    # add to ~/.bashrc or ~/.profile
```

### 3 — .NET 10 SDK

The test and examples projects target `net10.0`. The library itself targets `net8.0`.
On Ubuntu 24.04, pin .NET packages to Ubuntu's repo to avoid conflicts with Microsoft's feed:

```bash
sudo tee /etc/apt/preferences.d/dotnet-ubuntu << 'EOF'
Package: dotnet* aspnetcore*
Pin: release o=Ubuntu
Pin-Priority: 1001
EOF
sudo apt update && sudo apt install -y dotnet-sdk-10.0
```

Verify: `dotnet --list-sdks` should show `10.0.x`.

### 4 — PowerShell Core

```bash
wget -q https://packages.microsoft.com/config/ubuntu/24.04/packages-microsoft-prod.deb -O ms-prod.deb
sudo dpkg -i ms-prod.deb && sudo apt update && sudo apt install -y powershell
```

---

## Build steps

Run all commands from the repo root with `VCPKG_ROOT` set.

### Step 1 — Build the native shared library

```bash
pwsh scripts/build-native.ps1 -Configuration Debug
```

This does two things automatically:
1. `vcpkg install pagmo2:x64-linux-static-pic --overlay-triplets=triplets/`
   (idempotent — skips if already installed; first run takes ~10 min)
2. `cmake -B pagmoWrapper/build -S pagmoWrapper ... && cmake --build pagmoWrapper/build`

Expected output: `pagmoWrapper/build/libPagmoWrapper.so`

To build Release:
```bash
pwsh scripts/build-native.ps1 -Configuration Release
```

### Step 1b — Verify no system deps

```bash
ldd pagmoWrapper/build/libPagmoWrapper.so | grep -E "pagmo|boost|tbb"
# Expected: no output — those should all be statically linked in
```

If you see `libpagmo.so.9` or `libboost` in the output, the static triplet didn't take effect.
Delete `pagmoWrapper/build/` and re-run `build-native.ps1` (vcpkg may have used a cached
dynamic build from a previous apt-based session).

### Step 2 — SWIG wrapper regeneration (only if you edit `.i` files)

The pre-generated wrappers are committed; skip this unless you changed a SWIG interface file.

```bash
pwsh scripts/regen-swig.ps1
```

### Step 3 — Run the full test suite

```bash
dotnet test Tests/Tests.Pagmo.NET/Tests.Pagmo.NET.csproj -p:Platform=x64
```

Expected: **Passed: 593, Failed: 0**

### Step 4 — Build release artifacts

Once tests pass, build the Linux release artifacts:

```bash
pwsh scripts/build-release-artifacts.ps1 -Version 1.0.0-beta.1
```

Then verify the NuGet package contains the Linux runtime:

```bash
unzip -l artifacts/release/1.0.0-beta.1/nuget/Pagmo.NET.1.0.0-beta.1.nupkg | grep linux
# Expected: runtimes/linux-x64/native/libPagmoWrapper.so
```

### Step 5 — Copy artifacts to Windows

You need two files on your Windows machine:
- `artifacts/release/1.0.0-beta.1/Pagmo.NET-native-linux-x64-1.0.0-beta.1.zip`
- `artifacts/release/1.0.0-beta.1/nuget/Pagmo.NET.1.0.0-beta.1.nupkg` *(the linux one, for the merge step)*

**Via VirtualBox shared folder:**
```bash
cp artifacts/release/1.0.0-beta.1/Pagmo.NET-native-linux-x64-1.0.0-beta.1.zip /path/to/shared/folder/
cp artifacts/release/1.0.0-beta.1/nuget/Pagmo.NET.1.0.0-beta.1.nupkg /path/to/shared/folder/Pagmo.NET-linux.nupkg
```

Or extract just the `.so` if you prefer to inject it manually on Windows:
```bash
cp pagmoWrapper/build/libPagmoWrapper.so /path/to/shared/folder/
```

---

## Likely issues and fixes

### `VCPKG_ROOT` not set

```
Timed out waiting for build/SWIG lock ... / VCPKG_ROOT must be set
```
Set `export VCPKG_ROOT=~/vcpkg` and ensure `~/vcpkg/vcpkg` exists (run bootstrap first).

### vcpkg build fails for pagmo2

First run issues are usually missing build tools:
```bash
sudo apt install -y curl zip unzip tar pkg-config
```
vcpkg will print the specific missing tool.

### cmake can't find Pagmo

```
CMake Error: Could not find a package configuration file provided by "Pagmo"
```
The `-DCMAKE_TOOLCHAIN_FILE` wasn't passed, or vcpkg install didn't complete. Run
`build-native.ps1` rather than invoking cmake directly — the script wires up the toolchain.

### `libPagmoWrapper.so` not found at test runtime

The `PostBuildLinux` target in `Tests.Pagmo.NET.csproj` copies the `.so` from
`pagmoWrapper/build/` to the test output directory. If the copy didn't fire:

```bash
ls pagmoWrapper/build/libPagmoWrapper.so               # confirm .so was built
ls Tests/Tests.Pagmo.NET/bin/x64/Debug/net10.0/libPagmoWrapper.so  # confirm copy
```

If the copy is missing, the `$(OS)` MSBuild property detection may need adjusting.
Try changing the `PostBuildLinux` condition from `'$(OS)' != 'Windows_NT'` to
`!$([MSBuild]::IsOSPlatform('Windows'))` in both `Tests.Pagmo.NET.csproj` and
`Examples.Pagmo.NET.csproj`.

### Only 198 tests discovered instead of 593

.NET 8 VsTest on Linux has a discovery limitation. Install .NET 10 SDK and ensure the test
project's `TargetFramework` is `net10.0` (it is in the committed csproj).

### Optional solver tests

With the vcpkg pagmo2 built without `[nlopt,ipopt]` features (the default), `PAGMO_WITH_NLOPT`
and `PAGMO_WITH_IPOPT` are not defined. The managed `OptionalSolverAvailability` class detects
this via `NativeLibrary.TryGetExport` and the relevant tests call `Assert.Pass` to self-exclude.
Expected: 0 failures, a few skipped/passed-early for the optional solver tests.

---

## What changed from the initial apt-based verification

The initial Linux verification (commit `2b640f6`) used `libpagmo-dev` from apt as a system
dependency. The current approach replaces that with vcpkg static linking:

| | apt approach (initial) | vcpkg static-PIC (current) |
|---|---|---|
| Build time (first) | fast (apt install) | slow (~10 min, compiling from source) |
| Build time (subsequent) | fast | fast (vcpkg cache) |
| Runtime system deps | `libpagmo9t64`, `libtbb12`, `libboost-serialization` | none beyond `libstdc++` |
| Release artifact | requires user to install system packages | self-contained `.so` |

The apt approach still works for quick local development if you don't care about system deps.
To use it: `sudo apt install libpagmo-dev`, then invoke cmake directly without the vcpkg
toolchain: `cmake -B pagmoWrapper/build -S pagmoWrapper && cmake --build pagmoWrapper/build`.

---

## Key Linux-specific files

| File | Purpose |
|------|---------|
| `triplets/x64-linux-static-pic.cmake` | Custom vcpkg triplet enabling static + PIC build |
| `pagmoWrapper/CMakeLists.txt` | CMake build; links static pagmo/Boost/TBB into the `.so` |
| `pagmoWrapper/multi_objective.h` | Uses `unsigned long long` at SWIG boundary (not `pop_size_t`, which is `size_t` = `unsigned long` on Linux) |
| `pagmoWrapper/GeneratedWrappers.cxx` | Manual `#if PAGMO_WITH_NLOPT/IPOPT` guards on optional-solver code zones |
| `Pagmo.NET/pagmoExtensions/OptionalSolverAvailability.cs` | Runtime detection of optional solver symbols |
| `Tests/Tests.Pagmo.NET/Algorithms/Test_moead_gen.cs` | Uses `Does.Contain("MOEAD")` — pagmo 2.19 changed the `get_name()` return value |
