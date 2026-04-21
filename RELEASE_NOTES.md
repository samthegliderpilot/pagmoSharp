# Release Notes

## v1.0.0-beta.1

### Overview

First public beta of Pagmo.NET - a .NET 8 C# wrapper for [pagmo2](https://esa.github.io/pagmo2/).
Targets Windows x64. Linux/CMake support is planned for a later release.

### Highlights

**Managed C# problem and algorithm extensibility**
- Implement `IProblem` / `ManagedProblemBase` to define custom optimization problems in pure C#.
  Full lifecycle (fitness, bounds, gradients, hessians, batch evaluation, seed, thread safety)
  is supported, with exception-safe callback boundaries.
- Implement `IAlgorithm` to define custom optimization algorithms in pure C#. Managed algorithms
  participate in `island` and `archipelago` type-erased execution paths (evolve, wait, migrate).

**Broad algorithm and problem coverage**
All major pagmo algorithms are wrapped with typed extension helpers and log projections:
`bee_colony`, `cmaes`, `compass_search`, `cstrs_self_adaptive`, `de`, `de1220`, `gaco`,
`gwo`, `ihs`, `maco`, `mbh`, `moead`, `moead_gen`, `nsga2`, `nspso`, `pso`, `pso_gen`,
`sade`, `sea`, `sga`, `simulated_annealing`, `xnes`.

Optional/feature-gated solvers: `ipopt`, `nlopt` (build-dependent; availability is
asserted by tests).  `snopt7` is supported for users who build from source with their
own SNOPT7 license; see README for build instructions.

All built-in benchmark problems are wrapped: `ackley`, `cec2006/2009/2013/2014`, `decompose`,
`dtlz`, `golomb_ruler`, `griewank`, `hock_schittkowski_71`, `inventory`, `lennard_jones`,
`luksan_vlcek1`, `minlp_rastrigin`, `null_problem`, `rastrigin`, `rosenbrock`, `schwefel`,
`translate`, `unconstrain`, `wfg`, `zdt`.

**Island and archipelago orchestration**
- Full `island` and `archipelago` managed API with topology wrappers (`ring`, `fully_connected`,
  `unconnected`, `free_form`), migration type/handling controls, and island snapshot access.
- Managed replacement and selection policies: `RPolicyCallback` / `SPolicyCallback` base classes
  for custom policies, plus built-in `fair_replace` / `select_best`.

**Batch fitness evaluators**
- `default_bfe`, `thread_bfe`, `member_bfe` with safe managed-problem dispatch.

**Multi-objective utilities**
- `non_dominated_front_2d`, `crowding_distance`, `sort_population_mo`, `select_best_N_mo`,
  `ideal`, `nadir`, `decompose_objectives`, `hypervolume` and related APIs.

**Runnable examples and docs**
- `Examples/Examples.Pagmo.NET`: three teaching scenarios (single-island, archipelago topology,
  migration policies).
- `docs/`: concept-first walkthroughs linked to runnable example code.

### Breaking / Behavior Notes

This is a first beta. No public stable API surface has been previously released.

**C# naming conventions**: bridge and enum types use PascalCase as idiomatic C#.
Pagmo-compatible snake_case is preserved for method names that mirror the pagmo C++ API
(e.g. `get_bounds()`, `fitness()`, `evolve()`).

Notable type renames from internal pre-release names:
- Enum types: `ThreadSafety`, `EvolveStatus`, `MigrationType`, `MigrantHandling`,
  `SgaCrossover`, `SgaMutation`, `SgaSelection`
- Bridge types: `ProblemCallback`, `ManagedProblem`, `AlgorithmCallback`, `ManagedAlgorithm`,
  `RPolicyCallback`, `ManagedRPolicy`, `SPolicyCallback`, `ManagedSPolicy`, `NullProblemCallback`

### Known Limitations

- **Thread-clone strategy** - Managed problems reporting `ThreadSafety.None` are rejected on
  threaded execution paths (`thread_bfe`, `archipelago` with managed UDPs). A per-thread clone
  strategy (`IThreadCloneableProblem`) is being evaluated for a later release.

### Supported Environment Matrix

| Environment | Status |
|---|---|
| Windows x64, .NET 8 | Supported |
| Windows x64, IPOPT enabled | Feature-gated (vcpkg `coin-or-ipopt:x64-windows`) |
| Windows x64, NLopt enabled | Feature-gated (vcpkg `nlopt:x64-windows`) |
| Linux x64, .NET 8 | Supported — CMake build via `pagmoWrapper/CMakeLists.txt` |
| Linux x64, IPOPT enabled | Feature-gated (vcpkg `coin-or-ipopt:x64-linux`, CMake `-DPAGMO_WITH_IPOPT=ON`) |
| Linux x64, NLopt enabled | Feature-gated (vcpkg `nlopt:x64-linux`, CMake `-DPAGMO_WITH_NLOPT=ON`) |
| .NET Framework | Not supported |
| x86 / ARM | Not supported in v1 |
| macOS | Not supported in v1 |

Repo note:
- The shipped library/package target is `.NET 8`.
- Tests, examples, and documentation tooling may use a newer SDK internally during development.



