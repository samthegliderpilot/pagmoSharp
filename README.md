# Pagmo.NET

**Pagmo.NET** is a C# wrapper for [pagmo2](https://esa.github.io/pagmo2/), a C++ library
providing high-quality metaheuristic and gradient-based optimization routines with multi-island
parallel evolution support.

The wrapper is built with [SWIG 4.4](https://www.swig.org/) and supports Windows x64 and Linux x64.

## Installation

```
dotnet add package Pagmo.NET --version 1.0.0-beta.1
```

The NuGet package is self-contained — it includes the native runtime libraries for Windows x64
and Linux x64 in the `runtimes/` directory. No additional installation or DLL copying is
required; just add the package and run.

Source archives and individual native bundles are also available at
`https://github.com/samthegliderpilot/Pagmo.NET/releases` if you need them for custom
deployment scenarios.

## Build requirements (contributors / from source)

### Windows x64

- .NET 10 SDK
- pagmo2 (headers and binaries) — install via `vcpkg install pagmo2:x64-windows`
- SWIG 4.4.x (for wrapper regeneration only — pre-generated wrappers are checked in)
- Visual Studio 2022 / Build Tools 2022 (C++ toolchain)

To regenerate SWIG wrappers after editing the interface file, run
`pwsh createSwigWrappersAndPlaceThem.ps1` from the repo root (SWIG resolution via `SWIG_EXE`,
`SWIG_HOME`, or `PATH`). The legacy `createSwigWrappersAndPlaceThem.bat` is also preserved for
environments without PowerShell Core.

### Linux x64

The native library is built with pagmo2, Boost.Serialization, and TBB statically linked
(via vcpkg's `x64-linux-static-pic` triplet), so `libPagmoWrapper.so` has **no system
runtime dependencies** beyond the standard C++ runtime.

**One-time setup:**

```bash
# Build tools, SWIG, and .NET
sudo apt install -y cmake build-essential swig

# .NET 10 SDK (required for full test discovery on Linux — see note below)
sudo tee /etc/apt/preferences.d/dotnet-ubuntu << 'EOF'
Package: dotnet* aspnetcore*
Pin: release o=Ubuntu
Pin-Priority: 1001
EOF
sudo apt update && sudo apt install -y dotnet-sdk-10.0

# PowerShell Core (for build scripts)
wget -q https://packages.microsoft.com/config/ubuntu/24.04/packages-microsoft-prod.deb -O ms-prod.deb
sudo dpkg -i ms-prod.deb && sudo apt update && sudo apt install -y powershell

# vcpkg (builds pagmo2 + dependencies as static PIC libraries)
git clone https://github.com/microsoft/vcpkg ~/vcpkg
~/vcpkg/bootstrap-vcpkg.sh
export VCPKG_ROOT=~/vcpkg   # add to ~/.bashrc or ~/.profile
```

**Build and test:**

```bash
# 1. Build the native shared library (vcpkg install runs automatically on first build)
pwsh scripts/build-native.ps1 -Configuration Debug

# 2. Run the full test suite (593 tests)
dotnet test Tests/Tests.Pagmo.NET/Tests.Pagmo.NET.csproj -p:Platform=x64

# 3. Run the examples
dotnet run --project Examples/Examples.Pagmo.NET/Examples.Pagmo.NET.csproj -- all
```

`build-native.ps1` runs `vcpkg install pagmo2:x64-linux-static-pic` (using the custom
triplet in `triplets/`) then invokes CMake. vcpkg is idempotent — subsequent builds skip
the install step. The first build takes several minutes while vcpkg compiles pagmo2, Boost,
and TBB from source.

SWIG wrapper regeneration (`pwsh scripts/regen-swig.ps1`) is only needed when editing `.i`
files — the generated wrappers are checked in.

> **Note on .NET SDK:** The library itself targets `net8.0`. The test and examples projects
> target `net10.0` because the .NET 8 VsTest runner on Linux only discovers ~198 of 593
> tests due to a known discovery issue. Consumers can reference the package from any .NET 8+
> project.

## FAQ

### Why .Net and C#?
I think that the .Net ecosystem and languages are a bit under appreciated for scientific computing.  Although raw C/C++ code written by an expert will be faster, C# can get pretty close.  And with Microsoft open-sourcing so much of .Net with .Net Core... it has a lot going for it.

Yes - [pygmo](https://esa.github.io/pygmo2/) is the official Python binding. Pagmo.NET is an
independent .NET binding for teams that want pagmo's optimization power in C# / F# / VB.NET
environments without a Python runtime dependency.

### Why SWIG and not C++/CLI?
SWIG takes care of all of the repetitive wrapping that a library like this needed.  Once I realized it exists, I just couldn't not use it.
Also, if someone wants to make wrappers for another language, the SWIG .i file will be a great start to that endeavor.  

### What's covered in v1.0?
All major pagmo algorithms, built-in benchmark problems, multi-island archipelago,
batch fitness evaluators, and migration policies are wrapped. See the feature matrix below.

### Your automated tests are not really testing meaningful optimization problems.
True, but they don't have to.  These tests need to only test the wrappers; they do not need to test that the algorithms in pagmo work as well as they do.

### Where's IPOPT?

IPOPT is included in both the Windows and Linux released builds — statically linked via vcpkg.
`OptionalSolverAvailability.IsIpoptAvailable` returns `true` out of the box on both platforms.

### Where's SNOPT7?
SNOPT7 (Stanford's nonlinear programming solver) is proprietary and cannot be bundled with this project.  Users who hold a SNOPT7 license can build Pagmo.NET from source with SNOPT7 support enabled — the `snopt7` class will then be available in the managed assembly. This is currently untested.

**To enable SNOPT7:**
1. Obtain a SNOPT7 license from Stanford University.  Your distribution includes the SNOPT7 C-interface headers (`snopt_cwrap.h` etc.) and a compiled shared library (e.g. `snopt7.dll`).
2. Build pagmo2 with `-DPAGMO_WITH_SNOPT7=ON` and ensure your SNOPT7 headers are on the include path.  This produces a `pagmo.lib` that contains the SNOPT7 dispatch layer.
3. Add `#define PAGMO_WITH_SNOPT7` to `swig/pagmo/config.hpp`.
4. Copy `pagmo/algorithms/snopt7.hpp` from the pagmo2 source tree into `swig/pagmo/algorithms/`.
5. Run `scripts/regen-swig.ps1` then `scripts/build-native.ps1`.
6. Build `Pagmo.NET.csproj` — MSBuild detects the generated `snopt7.cs` automatically and activates the C# extension.

**Runtime usage** — pagmo's `snopt7` loads the solver DLL dynamically, so you pass the path to your `snopt7.dll` at construction time and no additional DLL deployment is needed on your end:

```csharp
using var solver = new snopt7(screenOutput: false, snopt7LibPath: "path/to/snopt7.dll", minorVersion: 6u);
```

Set `SNOPT7_LIB` to the DLL path to also enable the live execution test in the optional solver test suite.

Also, this is made completely independently of the base pagmo and the team that makes and maintains it.  This is independent of ESA and the original developers of pagmo.

## VS Code workflow

Repo includes VS Code tasks and launch configs in `.vscode/`:

- `Pagmo.NET: regenerate SWIG wrappers`
- `Pagmo.NET: build native (Debug x64)`
- `Pagmo.NET: build tests (Debug x64)`
- `Pagmo.NET: build examples (Debug x64)`
- `Pagmo.NET: test (Debug x64)`

All tasks use `pwsh` (PowerShell Core), which works on both Windows and Linux. The native
build task calls `scripts/build-native.ps1`, which uses MSBuild on Windows and CMake on Linux.

**Launch configs:**

- `Pagmo.NET: Run Tests` — runs the full test suite
- `Pagmo.NET: Debug Examples` — launches the examples project with full C# breakpoint support

### Requirements — Windows

- Visual Studio Build Tools 2022 (or VS 2022) with MSBuild + C++ toolchain
- .NET 10 SDK
- PowerShell Core (`pwsh`)
- `pagmo2` headers/libs via vcpkg (`vcpkg install pagmo2:x64-windows`)
- VS Code extensions: `ms-dotnettools.csharp`, `ms-dotnettools.csdevkit`, `ms-vscode.cpptools`, `ms-vscode.powershell`

### Requirements — Linux

- cmake, build-essential, swig, vcpkg (see Linux build section above)
- .NET 10 SDK
- PowerShell Core (`pwsh`)
- VS Code extensions: `ms-dotnettools.csharp`, `ms-dotnettools.csdevkit`, `ms-vscode.cpptools`, `ms-vscode.powershell`

Build the native library first (`pwsh scripts/build-native.ps1`) before using VS Code tasks.

### Configurable tool/include paths

- SWIG resolution for `createSwigWrappersAndPlaceThem.bat`:
  - `SWIG_EXE` env var (preferred),
  - or `SWIG_HOME` (expects `%SWIG_HOME%\swig.exe`),
  - or `swig.exe` from `PATH`.
- C++ include path resolution for `pagmoWrapper.vcxproj`:
  - `PagmoVcpkgInstalledDir` MSBuild property (preferred),
  - or `VCPKG_INSTALLED_DIR` environment variable,
  - or default repo-relative fallback: `$(SolutionDir)..\\vcpkg\\installed`.
- Triplet override:
  - `PagmoVcpkgTriplet` MSBuild property (default: `x64-windows`).

## Managed problem architecture (C# UDP support)

The core C# problem pipeline is:

1. User implements `IProblem` / `ManagedProblemBase` in C#
2. A SWIG director adapter (`problem_callback`) forwards calls to managed code
3. Native bridge wraps callback into `managed_problem` (`std::shared_ptr` owned)
4. A real `pagmo::problem` is built from `managed_problem`
5. `population`, `archipelago`, and BFE operator helpers consume that `pagmo::problem`

This keeps ownership on the native side with `shared_ptr`, avoiding raw-pointer lifetime bugs for managed UDPs.

### Managed UDP authoring defaults

- Minimal managed UDPs can implement just:
  - `fitness(DoubleVector x)`
  - `get_bounds()`
- Optional capabilities (`batch_fitness`, gradients, hessians, seed hooks, metadata, thread safety) have defaults on `IProblem` / `ManagedProblemBase`.
- `ManagedProblemBase` includes helper methods for concise authoring:
  - `Vec(...)`
  - `Bounds(lower, upper)`

### Threading policy for managed UDPs

- `thread_bfe.Operator(IProblem, ...)` and `archipelago.push_back_island(..., IProblem, ...)` require explicit parallel-safety opt-in.
- Managed UDPs declaring `ThreadSafety.None` are rejected on those threaded entrypoints.
- Declare `ThreadSafety.Basic` or `ThreadSafety.Constant` when your managed UDP is safe for concurrent evaluation.

## C# quickstart

```csharp
using pagmo;

public sealed class SphereProblem : ManagedProblemBase
{
    public override PairOfDoubleVectors get_bounds() => Bounds(
        new[] { -5.0, -5.0 },
        new[] { 5.0, 5.0 });

    public override DoubleVector fitness(DoubleVector x)
    {
        return Vec(x[0] * x[0] + x[1] * x[1]);
    }

    public override ThreadSafety get_thread_safety() => ThreadSafety.Basic;
}

using IAlgorithm algo = new de(100u);
using var udp = new SphereProblem();
using var isl = island.Create(algo, udp, popSize: 64u, seed: 2u);
isl.evolve(1u);
isl.wait_check();

using var result = isl.get_population();
using var bestX = result.champion_x();
using var bestF = result.champion_f();
```

Notes:
- Use `island.Create(...)` for single-island runs and `archipelago.push_back_island(...)` for multi-island orchestration.
- When using threaded paths (`thread_bfe`, `archipelago` with managed UDPs), managed problems must report `ThreadSafety.Basic` or `ThreadSafety.Constant`.

## Runnable examples

A dedicated non-test examples project is available at `Examples/Examples.Pagmo.NET`.

Run all scenarios (works on Windows and Linux):

```bash
dotnet run --project Examples/Examples.Pagmo.NET/Examples.Pagmo.NET.csproj -- all
```

Run individual scenarios:

```bash
dotnet run --project Examples/Examples.Pagmo.NET/Examples.Pagmo.NET.csproj -- single
dotnet run --project Examples/Examples.Pagmo.NET/Examples.Pagmo.NET.csproj -- archipelago
dotnet run --project Examples/Examples.Pagmo.NET/Examples.Pagmo.NET.csproj -- policies
```

These examples are intentionally half API walkthrough and half optimization-structure teaching:
- `single`: one-island baseline.
- `archipelago`: topology connectivity intuition (`ring` vs `unconnected`) and single-island vs archipelago multi-start comparison.
- `policies`: default policy wiring vs explicit `fair_replace` + `select_best` wiring through archipelago APIs.

## Docs (Executable-First)

Concept-first walkthroughs live in `docs/` and link directly to runnable source:
- `docs/getting-started.md`
- `docs/archipelago-topology-policies.md`

Smoke-check all documented scenarios:

```bash
# Windows
powershell -ExecutionPolicy Bypass -File scripts/docs-smoke.ps1
# Linux
pwsh scripts/docs-smoke.ps1
```

This verifies docs-backed scenarios (`single`, `archipelago`, `policies`) against `Examples/Examples.Pagmo.NET`.

## Release gates (Sprint 5)

Run the consolidated release-readiness gate script:

```bash
# Windows
powershell -ExecutionPolicy Bypass -File scripts/release-gates.ps1
# Linux
pwsh scripts/release-gates.ps1
```

This performs:
- SWIG regen reproducibility check (two consecutive regens with hash comparison of generated outputs),
- native rebuild (`Debug x64` + `Release x64`),
- full managed test suite,
- optional solver availability tests.

Build beta/release artifacts (NuGet + native runtime bundle + source archive + checksums):

```bash
# Windows
powershell -ExecutionPolicy Bypass -File scripts/build-release-artifacts.ps1 -Version 1.0.0-beta.1
# Linux
pwsh scripts/build-release-artifacts.ps1 -Version 1.0.0-beta.1
```
## Supported feature matrix (current state)

| Area | Status | Notes |
|---|---|---|
| Managed C# UDP (`IProblem` / `ManagedProblemBase`) | Supported | Core path for v1.0; callback lifetime and exception bubbling are covered by tests. |
| Type-erased `IAlgorithm` interop in island/archipelago paths | Supported | Bridged via `AlgorithmInterop` for wrapped algorithms and managed `IAlgorithm` callback bridge (for example `grid_search`). |
| Core runtime orchestration (`population`, `island`, `archipelago`) | Supported | Managed-problem, policy, and topology runtime paths are covered by tests. |
| Managed policy extensibility (`RPolicyCallback`, `SPolicyCallback`) | Supported | Direct managed-policy entrypoints are available on island/archipelago helpers. |
| Topology wrappers (`ring`, `fully_connected`, `unconnected`, `free_form`) | Supported | Managed projection helpers are provided and tested. |
| Optional solver wrapper (`ipopt`) | Feature-gated | Build-dependent; availability/runtime behavior (construct/evolve/type-erasure/log projection) is validated when IPOPT is present. |
| Optional solver wrapper (`nlopt`) | Feature-gated | Build-dependent; availability is asserted by test and runtime wrapper behavior is validated when present. |
| Linux/CMake build flow | Supported | Verified on Ubuntu 24.04 / Linux Mint 22.1. pagmo2, Boost, TBB, NLopt, and IPOPT statically linked via vcpkg `x64-linux-static-pic` triplet — no system runtime deps. All 593 tests pass with .NET 10 SDK. |

## Code style preferences

- Keep code lean and readable; avoid defensive scaffolding unless it provides clear operational value.
- Prefer direct checks near callsites over indirection-heavy policy layers when there are only a few callsites.
- Throw exceptions only when:
  - the message adds useful, novel context beyond the default exception/debugger signal, or
  - there is a strong boundary contract that benefits from explicit validation.
- Favor tight exception usage and concise error messages.

### API naming

- Managed extension helpers prefer C#-style PascalCase where practical.
- Existing snake_case runtime entrypoints are retained for pagmo parity/compatibility.
- See .ai/API_NAMING_POLICY.md for the detailed policy and compatibility approach.


