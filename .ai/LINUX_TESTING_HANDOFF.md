# Linux Build & Test Guide

**Status:** Verified working on Linux Mint 22.1 (Ubuntu 24.04 base), x64.  
**Test result:** 593/593 tests pass.

---

## Prerequisites

### 1 — Build tools

```bash
sudo apt install -y cmake build-essential swig
```

Verified with: cmake 3.31, SWIG 4.2 (the pre-generated wrappers are committed — SWIG version
only matters if you regenerate; 4.2 works fine).

### 2 — pagmo2 and its dependencies

The apt repos on Ubuntu 24.04 ship pagmo 2.19.0 with all needed dependencies:

```bash
sudo apt install -y libpagmo-dev
```

This pulls in `libboost-serialization-dev`, `libeigen3-dev`, and `libtbb-dev` automatically.
**Do not use vcpkg** — it is not needed and mixing sources causes package conflicts.

### 3 — .NET 10 SDK

The test project targets `net10.0`. The .NET 10 SDK is needed for full test discovery
(with .NET 8 only ~198 of 593 tests are enumerated by the VsTest runner due to a known
discovery limitation on Linux — .NET 10 fixes this).

On Ubuntu 24.04 / Linux Mint 22.1 you may get a package conflict when mixing Ubuntu's
and Microsoft's .NET repos. To avoid it, pin .NET packages to Ubuntu's repo:

```bash
# Remove any partially-installed conflicting packages
sudo apt remove --purge dotnet-host-10.0 aspnetcore-runtime-10.0 dotnet-runtime-10.0 2>/dev/null
sudo apt autoremove

# Pin .NET packages to Ubuntu's self-consistent set
sudo tee /etc/apt/preferences.d/dotnet-ubuntu << 'EOF'
Package: dotnet* aspnetcore*
Pin: release o=Ubuntu
Pin-Priority: 1001
EOF

sudo apt update
sudo apt install -y dotnet-sdk-10.0
```

Verify: `dotnet --list-sdks` should show `10.0.x`.

### 4 — PowerShell Core

Required by VS Code tasks and the build scripts.

```bash
# Microsoft's package feed is used — add it first if not already present
wget -q https://packages.microsoft.com/config/ubuntu/24.04/packages-microsoft-prod.deb -O ms-prod.deb
sudo dpkg -i ms-prod.deb
sudo apt update
sudo apt install -y powershell
```

Verify: `pwsh --version`

### 5 — Optional solvers (NLopt / IPOPT)

The apt-packaged `libpagmo.so` was NOT compiled with NLopt or IPOPT support, so installing
these only makes sense if you build pagmo from source with those features enabled. For the
standard apt-based setup, the optional solver tests correctly self-exclude using
`OptionalSolverAvailability.IsNloptAvailable` / `IsIpoptAvailable`.

---

## Build steps

Run all commands from the repo root.

### Step 1 — Build the native shared library

```bash
cmake -B pagmoWrapper/build -S pagmoWrapper -DCMAKE_BUILD_TYPE=Debug
cmake --build pagmoWrapper/build
```

Expected output: `pagmoWrapper/build/libPagmoWrapper.so`

For a Release build:
```bash
cmake -B pagmoWrapper/build -S pagmoWrapper -DCMAKE_BUILD_TYPE=Release
cmake --build pagmoWrapper/build
```

**Do NOT pass `-DPAGMO_WITH_NLOPT=ON` or `-DPAGMO_WITH_IPOPT=ON`** unless you have built
pagmo from source with those solvers enabled. The apt pagmo library does not contain
`pagmo::nlopt` or `pagmo::ipopt` symbols, and enabling these flags causes a runtime
symbol-lookup crash. The `CMakeLists.txt` uses `PAGMONET_WITH_NLOPT` / `PAGMONET_WITH_IPOPT`
as its own options to avoid collision with pagmo's exported cmake variables; both default to
`OFF`.

### Step 2 — SWIG wrapper regeneration (only if you edit `.i` files)

The pre-generated wrappers (`pagmoWrapper/GeneratedWrappers.cxx`, `Pagmo.NET/pygmoWrappers/*.cs`)
are committed. Skip this step unless you changed a SWIG interface file.

```bash
pwsh scripts/regen-swig.ps1
```

SWIG 4.2 produces slightly different comment/whitespace output than the 4.4-generated
committed wrappers. The logic is identical; commit the diff if you regenerate.

### Step 3 — Build and run all tests

```bash
dotnet test Tests/Tests.Pagmo.NET/Tests.Pagmo.NET.csproj -p:Platform=x64
```

Expected: **Passed: 593, Failed: 0**

---

## Key Linux-specific changes in the codebase

These changes were made to support Linux and are documented here for future reference:

| File | What changed |
|------|-------------|
| `pagmoWrapper/CMakeLists.txt` | CMake build for `libPagmoWrapper.so`; IPOPT detection via `pkg_check_modules` (not `find_package`); optional-solver options renamed to `PAGMONET_WITH_*` to avoid collision with pagmo's exported cmake variables; explicit `-UPAGMO_WITH_NLOPT -UPAGMO_WITH_IPOPT` compile options to prevent pagmo's INTERFACE definitions from leaking into our build |
| `pagmoWrapper/multi_objective.h` | Removed conflicting `typedef unsigned long long pop_size_t` (on Linux `size_t` is `unsigned long`, not `unsigned long long`); `FNDSResult` fields and `RekSum::reksum` parameters use `unsigned long long` directly at the SWIG boundary with conversion to `pagmo::pop_size_t` internally |
| `pagmoWrapper/GeneratedWrappers.cxx` | `FNDSResult` setter/getter types changed from `pagmo::pop_size_t` to `unsigned long long` to match the C# `ULongLongVector` types; six IPOPT code zones wrapped in `#if defined(PAGMO_WITH_IPOPT)` guards; six NLopt code zones wrapped in `#if defined(PAGMO_WITH_NLOPT)` guards (both sets of guards use zone 6 ending before `new_not_population_based`/`new_nlopt` respectively, not at end-of-file) |
| `Pagmo.NET/pagmoExtensions/OptionalSolverAvailability.cs` | NEW — `OptionalSolverAvailability.IsNloptAvailable` / `IsIpoptAvailable` static properties using `NativeLibrary.TryGetExport` to check native symbol presence without catching `EntryPointNotFoundException` in user code |
| `Tests/Tests.Pagmo.NET/Tests.Pagmo.NET.csproj` | Target `net10.0` (required for full VsTest discovery on Linux); added `PostBuildLinux` target to copy `libPagmoWrapper.so` |
| `Examples/Examples.Pagmo.NET/Examples.Pagmo.NET.csproj` | Target `net10.0`; added `PostBuildLinux` target |
| `Tests/Tests.Pagmo.NET/Algorithms/Test_optional_solver_availability.cs` | Uses `OptionalSolverAvailability.IsNloptAvailable` / `IsIpoptAvailable` instead of try-catching `EntryPointNotFoundException` |
| `Tests/Tests.Pagmo.NET/Algorithms/Test_moead_gen.cs` | `TestNameIsCorrect` uses `Does.Contain("MOEAD")` instead of exact string — pagmo 2.19.0 changed the algorithm's `get_name()` return value |
| `.vscode/tasks.json` | `"command": "powershell"` → `"command": "pwsh"`; added "build examples" task |
| `.vscode/launch.json` | Fixed to use `pwsh`; added "Debug Examples" `coreclr` launch config targeting `net10.0` output path |

---

## VS Code debugging

After building (steps 1–2 above), you can use these VS Code launch configs:

- **Pagmo.NET: Run Tests** — runs the full test suite via `pwsh scripts/test.ps1`
- **Pagmo.NET: Debug Examples** — launches the examples project with full C# breakpoint debugging

The C# extension (`ms-dotnettools.csharp`) must be installed; it downloads `vsdbg` on first use.

---

## Troubleshooting

### cmake: cannot find pagmo

```
CMake Error: Could not find a package configuration file provided by "Pagmo"
```

Make sure `libpagmo-dev` is installed: `dpkg -l libpagmo-dev`. If you previously used vcpkg
and have `$VCPKG_ROOT` set, that toolchain file may interfere — unset it or don't pass it.

### Runtime crash: `undefined symbol: _ZN5pagmo5nloptC1Ev` (or similar nlopt/ipopt)

This means `libPagmoWrapper.so` was built with NLopt/IPOPT enabled but the apt pagmo library
does not have those symbols. Delete the cmake cache and rebuild without those options:

```bash
rm -rf pagmoWrapper/build
cmake -B pagmoWrapper/build -S pagmoWrapper -DCMAKE_BUILD_TYPE=Debug
cmake --build pagmoWrapper/build
```

### Only 198 tests discovered (instead of 593)

This happens with .NET 8 SDK on Linux — VsTest's discovery mechanism only enumerates one
namespace group. Install .NET 10 SDK (see Prerequisites § 3) and ensure the test project
targets `net10.0`.

### Package conflict when installing dotnet-sdk-10.0

```
trying to overwrite '/usr/bin/dnx', which is also in package dotnet-host-10.0 ...
```

Ubuntu's and Microsoft's .NET packages conflict. Apply the apt pinning fix in Prerequisites § 3.

### `libPagmoWrapper.so` not found at test runtime

The `PostBuildLinux` MSBuild target copies the `.so` automatically. If it didn't fire:

```bash
ls pagmoWrapper/build/libPagmoWrapper.so          # should exist
ls Tests/Tests.Pagmo.NET/bin/x64/Debug/net10.0/libPagmoWrapper.so  # should be copied here
```

If missing, copy manually and investigate why the PostBuildLinux target didn't run:
```bash
cp pagmoWrapper/build/libPagmoWrapper.so Tests/Tests.Pagmo.NET/bin/x64/Debug/net10.0/
```
