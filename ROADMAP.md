# PagmoSharp Roadmap

Last updated: 2026-03-16

## Intent
- Build a robust C#-first wrapper over pagmo with safe lifetimes and predictable behavior.
- Keep SWIG generation mechanical and minimize handwritten native glue.
- Prefer composition/adapters over inheritance where inheritance creates false abstractions.

## Explicit Product Decisions
- `problem` inheritance-heavy design from earlier versions is not sacred.
- It is acceptable (preferred) to remove/replace bad inheritance patterns, especially for wrapped problems (`ackley`, etc.).
- If C# utility code exists only because C++ was hard to call, replace it with direct C++ calls once bindings are reliable.
  - Candidate: `pagmoSharp/pagmoExtensions/Utils/GradientsAndHessians.cs` can be removed if C++ `utils/gradients_and_hessians` path is exposed cleanly.

## Current Architecture Snapshot
- Managed UDP flow:
  - C# `IProblem`/`ManagedProblemBase` -> `ProblemCallbackAdapter` (director) -> native `managed_problem` -> `pagmo::problem`.
- Native bridge exists for managed callback construction and selected helper calls:
  - `pagmoWrapper/managed_bridge.cpp`
- First-class SWIG `problem` wrapper added:
  - `swigInterfaceFileAndPagmoHeaders/swigInterfaceFiles/problem.i`
- `population` now has direct constructor from `problem` in SWIG interface.

## What Was Completed Recently
- VS Code workflow improved (tasks/launch/extensions/scripts).
- SWIG batch script hardened to remove intermittent `Access is denied` noise.
- Managed problem contract expanded (batch/gradient/hessians/sparsity/seed/extra-info/thread-safety).
- End-to-end DE managed pipeline test added:
  - `Tests/Tests.PagmoSharp/Algorithms/Test_de_managed_problem_pipeline.cs`
- Island wrapping completed for managed and native workflows:
  - generated wrapper: `pagmoSharp/pygmoWrappers/island.cs`
  - extension factories: `pagmoSharp/pagmoExtensions/island.cs`
  - tests: `Tests/Tests.PagmoSharp/Test_island.cs`
- Managed sparsity contract matured:
  - `IProblem` / `ManagedProblemBase` now use concrete `SparsityPattern` / `VectorOfSparsityPattern`
  - `ProblemCallbackAdapter` converts to director raw pointer wrappers internally
  - `problem` partial helpers added: `GradientSparsity()` / `HessiansSparsity()`
- Inheritance-heavy `ProblemWrapper` removed.
- Native DLL post-build copy paths corrected to `pagmoWrapper/pagmoWrapper/bin` in both C# projects.

## Known Issues / Gaps
- SWIG currently emits sparsity callback signatures as opaque `SWIGTYPE_*` in some director methods (`problem_callback`), even though concrete wrappers (`SparsityPattern`, `VectorOfSparsityPattern`) are generated.
  - This is functional but not ergonomic.
- Local environment/sandbox lock contention on `pagmoSharp/bin` and `pagmoSharp/obj` can block managed builds/tests.
- Repo includes generated files in working tree; keep generation deterministic and avoid manual edits in generated wrappers.
- Full generic `r_policy` / `s_policy` surface is not complete yet (temporary island narrowing still in place).

## Immediate Next Steps (Priority Order)
0. Complete generic `r_policy` / `s_policy` support end-to-end
- Remove temporary island policy narrowing to concrete `fair_replace` / `select_best`.
- Expose and use generic `r_policy` / `s_policy` in island APIs (constructors/factories/getters) for C# clients.
- Ensure C# surface uses real wrapped policy types, not `SWIGTYPE_*` pointers.
- Keep concrete-policy overloads only as convenience helpers.

1. Normalize problem/sparsity API ergonomics
- Make director-facing sparsity methods return concrete wrapper types instead of `SWIGTYPE_*` where possible.
- Add C# partial helpers to convert between ergonomic and raw forms only if typemap cleanup is not feasible.

2. Make `problem` the primary managed boundary
- Prefer `problem` wrapper + composition over `ProblemWrapper` inheritance-heavy usage.
- Reduce dependence on `IProblem` for wrapped native problem types where unnecessary.

3. Replace handwritten gradients/hessians utility if feasible
- Attempt wrapping pagmo C++ `utils/gradients_and_hessians`.
- If successful: deprecate/remove C# translation utility.

4. Harden threading behavior for managed UDPs
- Add tests for `thread_bfe` + managed callbacks under declared thread-safety levels.
- Verify callback lifetime and concurrency behavior under archipelago/island evolution.

5. Validation pass
- Run full test suite locally in VS Code task pipeline.
- Fix any regressions introduced by `problem` exposure changes.

## Medium-Term Plan
- Complete `problem.hpp` parity (publicly useful members only).
- Audit C# public API surface for consistency and naming clarity.
- Build one canonical sample app/test that exercises:
  - managed UDP + `problem`
  - `population`
  - `de` evolve
  - constraints + tolerance + feasibility checks
  - eval counters

## Do Not Expose Publicly (unless strong reason emerges)
- Raw pointer APIs (`get_ptr`, void* casts).
- Template extraction APIs (`extract<T>`, `is<T>`).
- Internal serialization hooks.

## Session Resume Checklist
- Regenerate wrappers:
  - `createSwigWrappersAndPlaceThem.bat`
- Native build:
  - `scripts/build-native.ps1`
- Managed build/test in VS Code:
  - `pagmoSharp: test (Debug x64)`
- If build fails due lock:
  - ensure no running testhost/dotnet process is holding `pagmoSharp/bin` or `obj`.

## Laptop Travel Handoff (Do This First)
1. Prerequisites to verify
- Visual Studio 2022 Build Tools (MSBuild + MSVC C++ toolchain).
- .NET SDK (net6-targeting works with newer SDKs).
- SWIG available at the path used by `createSwigWrappersAndPlaceThem.bat`.
- vcpkg dependencies installed so `pagmoWrapper.vcxproj` include/lib paths resolve.

2. First-run command sequence (repo root)
- `powershell -ExecutionPolicy Bypass -File scripts/regen-swig.ps1`
- `powershell -ExecutionPolicy Bypass -File scripts/build-native.ps1 -Configuration Debug -Platform x64`
- `powershell -ExecutionPolicy Bypass -File scripts/test.ps1 -Configuration Debug -Stage build`
- `powershell -ExecutionPolicy Bypass -File scripts/test.ps1 -Configuration Debug -Stage test -NoRestore`

3. Important build-path note
- Native build output expected by managed projects is `pagmoWrapper/pagmoWrapper/bin` (not `pagmoWrapper/bin`).
- Both `.csproj` files were updated to copy native DLLs from that path.

4. If lock/contention appears on `bin/obj`
- Stop stale processes: `dotnet`, `vstest.console`, `testhost`.
- Use artifact paths to avoid locked default output folders:
  - `dotnet build ... -p:BaseOutputPath=artifacts/out/ -p:IntermediateOutputPath=artifacts/obj/x64/Debug/net6.0/`
- In constrained environments, adding `-p:GenerateDependencyFile=false` can avoid `.deps.json` write contention during build-only validation.

5. If NuGet is unavailable (travel/offline network)
- Prefer `--no-restore` after one successful restore on that machine.
- If restore is required and offline, expect `NU1301` failures until connectivity is restored.

6. Current verified test baseline (2026-03-16)
- Targeted maturation set passes:
  - `Test_de_managed_problem_pipeline`
  - `Test_bfe`
  - `Test_archipelago`
  - `Test_island`
  - Result: `9 passed, 0 failed`
- Full suite currently has pre-existing failures unrelated to this handoff work:
  - `TestAlgorithmBase.TestGetLog` (`bee_colony.get_log` missing)
  - `TestAlgorithmBase.TestIntegerProgrammingWithUnConstraints` (threshold/behavior drift)
  - `TestAlgorithmBase.TestMultiobjectiveUnconstrained` (champion extraction on multi-objective population)
  - `Test_multi_objective.TestDecompositionWeights` (invalid method string in test input)

7. Next sprint priority on return
- Complete generic `r_policy` / `s_policy` support end-to-end (remove temporary island narrowing).

## Notes for Future GPT Sessions
- Respect user preference to discard flawed inheritance decisions.
- Favor architectural simplification over preserving legacy wrapper patterns.
- Keep SWIG interface files as source of truth; avoid hand-editing generated C#.
- When changing SWIG typemaps, re-check director signatures (`problem_callback.cs`) before assuming type substitutions took effect.
