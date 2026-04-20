# pagmoSharp

**pagmoSharp** is a .NET 8 C# wrapper for [pagmo2](https://esa.github.io/pagmo2/), a C++ library
providing high-quality metaheuristic and gradient-based optimization routines with multi-island
parallel evolution support.

The wrapper is built with [SWIG 4.4](https://www.swig.org/) and targets Windows x64 (v1.0).
Linux/CMake support is planned post-v1.

## Installation

Binary releases are distributed via:

- **NuGet** - `dotnet add package pagmoSharp --version 1.0.0-beta.1`
- **GitHub Releases** - tagged release bundles at
  `https://github.com/samthegliderpilot/pagmoSharp/releases` include the managed NuGet package,
  the native Windows x64 runtime bundle (`pagmoWrapper.dll` + dependencies), and a source archive.

The NuGet package contains only the managed assembly. The native runtime DLLs
(`pagmoWrapper.dll` and its dependencies) must be placed alongside your application at runtime.
Download the matching native bundle from the same GitHub release tag as the NuGet package version
you reference in your application.

## Build requirements (contributors / from source)

**Requirements for library consumers/building the package:**
- Windows x64
- .NET 8 SDK
- pagmo2 (headers and binaries) - install via `vcpkg install pagmo2:x64-windows`
- SWIG 4.4.x (for wrapper regeneration only - pre-generated wrappers are checked in)
- Visual Studio 2022 / Build Tools 2022 (C++ toolchain)

To regenerate SWIG wrappers after editing the interface file, run
`createSwigWrappersAndPlaceThem.bat` from the repo root (SWIG resolution via `SWIG_EXE`,
`SWIG_HOME`, or `PATH`). The C++ project runs this as a pre-build step automatically.

## FAQ

### Why .Net and C#?
I think that the .Net ecosystem and languages are a bit under appreciated for scientific computing.  Although raw C/C++ code written by an expert will be faster, C# can get pretty close.  And with Microsoft open-sourcing so much of .Net with .Net Core... it has a lot going for it.

Yes - [pygmo](https://esa.github.io/pygmo2/) is the official Python binding. pagmoSharp is an
independent .NET binding for teams that want pagmo's optimization power in C# / F# / VB.NET
environments without a Python runtime dependency.

### Why SWIG and not C++/CLI?
Several related reasons.  First, I wanted a P-Invoke wrapper to allow for the possibility of cross-platform support.  Also, SWIG takes care of all of the repetitive wrapping that a library like this needed.  Once I realized it exists, I just couldn't not use it.
Also, if someone wants to make wrappers for another language, the SWIG .i file will be a great start to that endeavor.  

### What's covered in v1.0?
All major pagmo algorithms, built-in benchmark problems, multi-island archipelago,
batch fitness evaluators, and migration policies are wrapped. See the feature matrix below.

### Your automated tests are not really testing meaningful optimization problems.
True, but they don't have to.  These tests need to only test the wrappers; they do not need to test that the algorithms in pygmo work as well as they do.

### Where's IPOPT?
IPOPT is supported as an optional, feature-gated solver when pagmo is built with IPOPT enabled (for example via vcpkg feature configuration).

Install IPOPT with vcpkg (Windows x64):

```powershell
& 'C:\src\vcpkg\vcpkg.exe' install coin-or-ipopt:x64-windows
```

If your `vcpkg.exe` is on `PATH`, you can use:

```powershell
vcpkg install coin-or-ipopt:x64-windows
```

Also, this is made completely independently of the base pagmo and the team that makes and maintains it.  This is independent of ESA and the original developers of pagmo.

## VS Code workflow

Repo now includes VS Code tasks/launch config in `.vscode/`:

- `pagmoSharp: regenerate SWIG wrappers`
- `pagmoSharp: build native (Debug x64)`
- `pagmoSharp: build tests (Debug x64)`
- `pagmoSharp: test (Debug x64)`

Native build task uses `scripts/build-native.ps1` and finds `MSBuild.exe` via `vswhere`.
SWIG regen task uses `scripts/regen-swig.ps1`.
Test build/run tasks use `scripts/test.ps1` with staged execution (`build` then `test`).

### Requirements for local VS Code test runs

- Visual Studio Build Tools 2022 (or VS 2022) with MSBuild + C++ toolchain
- .NET SDK
  Consumer/package target: `net8.0`
  Repo-internal test/examples/docs tooling may use a newer SDK during development
- NuGet connectivity
- `pagmo2` headers/libs available at paths configured in `pagmoWrapper/pagmoWrapper.vcxproj`
- VS Code extensions:
  - `ms-dotnettools.csharp`
  - `ms-dotnettools.csdevkit`
  - `ms-vscode.cpptools`
  - `ms-vscode.powershell`

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

A dedicated non-test examples project is available at `Examples/Examples.PagmoSharp`.

Run all scenarios:

```powershell
dotnet run --project Examples/Examples.PagmoSharp/Examples.PagmoSharp.csproj -- all
```

Run individual scenarios:

```powershell
dotnet run --project Examples/Examples.PagmoSharp/Examples.PagmoSharp.csproj -- single
dotnet run --project Examples/Examples.PagmoSharp/Examples.PagmoSharp.csproj -- archipelago
dotnet run --project Examples/Examples.PagmoSharp/Examples.PagmoSharp.csproj -- policies
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

```powershell
powershell -ExecutionPolicy Bypass -File scripts/docs-smoke.ps1
```

This verifies docs-backed scenarios (`single`, `archipelago`, `policies`) against `Examples/Examples.PagmoSharp`.

## Release gates (Sprint 5)

Run the consolidated release-readiness gate script:

```powershell
powershell -ExecutionPolicy Bypass -File scripts/release-gates.ps1
```

This performs:
- SWIG regen reproducibility check (two consecutive regens with hash comparison of generated outputs),
- native rebuild (`Debug x64` + `Release x64`),
- full managed test suite,
- optional solver availability tests.

Build beta/release artifacts (NuGet + native runtime bundle + source archive + checksums):

```powershell
powershell -ExecutionPolicy Bypass -File scripts/build-release-artifacts.ps1 -Version 1.0.0-beta.1
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
| Linux/CMake build flow | Planned post-v1 | Windows-first v1 release track; Linux/CMake is a later sprint. |

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


