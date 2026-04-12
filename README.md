[Pagmo](https://esa.github.io/pagmo2/) is a powerful library providing many high quality optimization routines in C++.  In an effort to learn more about C++ and to bring this ability into the .Net world, I'm creating this wrapper around pagmo for C# and other .Net languages.

There is a growing (aka. incomplete) [SWIG](https://www.swig.org/) interface file in the swigInterfaceFileAndPagmoHeaders folder.  When edits are made to that interface file, run the createSwigWrappersAndPlaceThem.bat file one directory up to regenerate the wrapers.  Note that the path of swig.exe is hard-coded in that bat file.  After that, build and run the Visual Studio solution normally.  The C++ project runs that bat file as a pre-build step.

Although pagmo has embraced a type-erasure style of coding for their problems, I'm embracing the OOP nature of C# and C++ to create a problem class that will have all the possible functions that a pagmo problem might implement.  This is going to be a hybrid of manually written code on both sides of the [un]managed line, but with SWIG director feature to assist with implementing UDP's in C#.  Right now, the problem class is the only one with custom C++ code, the rest of pagmo (so far) is working well with the swig .i file.

Not every function in every type in pagmo's hpp file's are going to get ported over.  Note that swig does not currently support varidec templates, so things like std::template<...> are not handled well. Pagmo does use these features and currently I'm mostly ignoring them until I can't.

This is still a work in progress with only a handful of types wrapped.  There are still some oddities that I am not sure are things I need to live with or if there are better ways to deal with.  I want to take my time before going nuts putting everything in the .i file.

As for requirements, pagmo 2.18, C++ 17, swig 4.0.2, .Net Core 6.0 (however nothing I'm doing should really require it and I want to look into downgrading it at some point), nUnit for unit testing.

Note that I am developing this on Windows 10, and used vcpkg to setup pagmo.  Although I hope to make the wrapper library cross-platform (hence choosing pinvoke instead of something like C++ CLI) this project isn't there yet.

## FAQ

### Why .Net and C#?
I think that the .Net ecosystem and languages are a bit under appreciated for scientific computing.  Although raw C/C++ code written by an expert will be faster, C# can get pretty close.  And with Microsoft open-sourcing so much of .Net with .Net Core... it has a lot going for it.

### Aren't you just making a wrapper of a wrapper?
Pagmo is more than just a wrapper.  Pygmo adds a consistent interface that wraps several other optimizers, as well as multithreading and multi-process support when available and appropriate.  That makes it more than just a wrapper.

### Why SWIG and not C++/CLI?
Several related reasons.  First, I wanted a P-Invoke wrapper to allow for the possibility of cross-platform support.  Also, SWIG takes care of all of the repetitive wrapping that a library like this needed.  Once I realized it exists, I just couldn't not use it.
Also, if someone wants to make wrappers for another language, the SWIG .i file will be a great start to that endeavor.  

### This hasn't implemented most of pagmo, why release it in such an incomplete state?
It's a work in progress.  Getting eyes on it sooner than later is worthwhile.

### Your automated tests are not really testing meaningful optimization problems.
True, but they don't have to.  These tests need to only test the wrappers; they do not need to test that the algorithms in pygmo work as well as they do.

### Where's IPOPT?
IPOPT is supported as an optional, feature-gated solver when pagmo is built with IPOPT enabled (for example via vcpkg feature configuration).

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
- .NET SDK (net6-targeting project; newer SDKs also work)
- NuGet connectivity
- `pagmo2` headers/libs available at paths configured in `pagmoWrapper/pagmoWrapper.vcxproj`
- VS Code extensions:
  - `ms-dotnettools.csharp`
  - `ms-dotnettools.csdevkit`
  - `ms-vscode.cpptools`
  - `ms-vscode.powershell`

### Configurable tool/include paths

- SWIG resolution for `createSwigWrappersAndPlaceThem.bat`:
  - `SWIG_EXE` env var (preferred), otherwise `swig.exe` from `PATH`.
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
- Managed UDPs declaring `thread_safety.none` are rejected on those threaded entrypoints.
- Declare `thread_safety.basic` or `thread_safety.constant` when your managed UDP is safe for concurrent evaluation.

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

    public override thread_safety get_thread_safety() => thread_safety.basic;
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
- When using threaded paths (`thread_bfe`, `archipelago` with managed UDPs), managed problems must report `thread_safety.basic` or `thread_safety.constant`.

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
## Supported feature matrix (current state)

| Area | Status | Notes |
|---|---|---|
| Managed C# UDP (`IProblem` / `ManagedProblemBase`) | Supported | Core path for v1.0; callback lifetime and exception bubbling are covered by tests. |
| Type-erased `IAlgorithm` interop in island/archipelago paths | Supported | Bridged via `AlgorithmInterop` for wrapped algorithms in scope. |
| Core runtime orchestration (`population`, `island`, `archipelago`) | Supported (with known topology caveat) | Managed-problem and policy runtime paths are covered; archipelago `set_topology_*` runtime mutation has a tracked issue in Sprint 4. |
| Managed policy extensibility (`r_policyBase`, `s_policyBase`) | Supported | Direct managed-policy entrypoints are available on island/archipelago helpers. |
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



