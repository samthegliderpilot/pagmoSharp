# PagmoSharp Roadmap

Last updated: 2026-03-29

## PagmoSharp Roadmap Reset (v1.0 with Explicit Breadth Sprint)

### Summary
- [x] Preserve reset from Sprint 0 with completed items marked complete.
- [x] Keep Windows-first v1.0 release before Linux/CMake.
- [x] v1.0 extensibility targets: `Problem`, `Algorithm`, `BFE`, `Policy`, `Topology`.
- [x] Defer `Island` and custom threading extensibility.
- [x] Make breadth explicit: **Sprint 3A is the "dozens of algorithms/problems" wrapping sprint**.

### Sprint Structure
1. **Sprint 0 (Completed): Tooling Baseline**
- [x] SWIG workflow, build/test path, hello-world SWIG validation.

2. **Sprint 1 (Completed): Fundamentals**
- [x] `Program` and C# `Problem` baseline, partial types, foundational tests.

3. **Sprint 2 (Completed): Critical Runtime + Patterns**
- [x] Finish runtime-critical orchestration (`island/archipelago`, migration controls, policies/topologies needed for runtime correctness).
- [x] Establish extensibility implementation patterns for `Algorithm`, `BFE`, `Policy`, `Topology`.
- [x] Acceptance: core runtime scenarios and pattern examples are stable.
- Active backlog additions:
  - [x] Serialize SWIG regeneration and native build execution to prevent concurrent wrapper-copy races.
  - [x] Replace `Assert.Ignore()` with `Assert.Pass()` where algorithm-test non-applicability is expected behavior.
  - [x] Prefer descriptive parameter names in handwritten managed/test code (avoid `arg0` / `arg1`).
  - [x] Add topology runtime coverage for `fully_connected`, `ring`, and `free_form` with smoke tests.
  - [x] Add archipelago migration/topology control wrappers and validation tests.
  - [x] Add explicit `thread_island` creation paths for `island` and cover with runtime tests.
  - [x] Strengthen `ring` topology tests to validate constructor-argument effects, not just type construction.
  - [x] Add thread-island managed-policy constructor coverage tests (`CreateWithThreadIslandAndPolicies`).
  - [x] Rework `bfe` interop model for safe concrete-BFE to type-erased-BFE conversion (remove invalid inheritance/upcasts).
  - [x] Re-enable `island + bfe` constructor surfaces using explicit safe conversion paths.
  - [x] Reintroduce explicit `thread_island + bfe` constructor surfaces with parity runtime tests.
  - [x] Strengthen island/thread-island constructor tests to assert configuration wiring (algorithm identity, population size, vector dimensions), not only no-throw.
  - [x] Add first concrete extensibility boundary patterns for `Algorithm` and `BFE` (internal normalization/dispatch, no new public factory surface).
  - [x] Add managed educational algorithm `grid_search` with constrained-feasible selection and runtime tests on 2D unconstrained/constrained problems.
  - [x] Add `archipelago.push_back_island` parity wrappers for thread-island/BFE/policy combinations with typed policy surfaces (`fair_replace`/`select_best`, managed policy wrappers).
  - [x] Add `archipelago` island snapshot access (`GetIslandCopy`) and strengthen archipelago runtime tests to assert configuration wiring.
  - [x] Harden managed `Policy` wrapper ownership semantics (`r_policyPagmoWrapper` / `s_policyPagmoWrapper`) to avoid raw-pointer shallow-copy lifetime risk.

4. **Sprint 3A: Broad Coverage Pass (Breadth)**
- [ ] Primary goal: add `.i` + wrapper coverage for targeted v1 algorithm/problem catalog (the "dozens" sprint).
- [ ] For each newly wrapped type, complete pragmatic finish work in the same slice (meaningful assertions, API polish, and lifecycle sanity), not smoke-only.
- [ ] Output: large usable surface with per-type baseline quality, while deeper cross-cutting hardening remains in 3B.
- Active progress:
  - [x] Fully wrap `null_algorithm` with managed extension polish (`IAlgorithm` compatibility helpers) and dedicated regression tests.
  - [x] Fully wrap `null_problem` with managed extension polish and dedicated regression tests (metadata + fitness behavior assertions).
  - [x] Fully wrap `rosenbrock` with managed extension polish and dedicated regression tests (metadata, bounds, optimum fitness, and evolve-path assertions).
  - [x] Fully wrap `rastrigin` with managed extension polish and dedicated regression tests (metadata, differential-info APIs, optimum fitness, and evolve-path assertions).
  - [x] Fully wrap `griewank` with managed extension polish and dedicated regression tests (metadata, bounds, optimum fitness, and evolve-path assertions).
  - [x] Fully wrap `schwefel` with managed extension polish and dedicated regression tests (metadata, bounds, near-optimum fitness, and evolve-path assertions).
  - [x] Fully wrap `lennard_jones` with managed extension polish and dedicated regression tests (metadata, safe fitness evaluation, and evolve-path assertions).
  - [x] Fully wrap `hock_schittkowski_71` with managed extension polish and dedicated regression tests (constrained outputs, differential-info surfaces, and evolve-path assertions).
  - [x] Fully wrap `luksan_vlcek1` with managed extension polish and dedicated regression tests (constraint-vector outputs, gradient contract checks, and evolve-path assertions).
  - [x] Fully wrap `cec2009` with managed extension polish and dedicated regression tests (multi-objective metadata, bounded evolve-path assertions, and safe known-input fitness validation).
  - [x] Fully wrap `cec2013` with managed extension polish and dedicated regression tests (metadata, bounds, evolve-path assertions, and guarded safe-input fitness validation for unstable midpoint probes).
  - [x] Fully wrap `cec2014` with managed extension polish and dedicated regression tests (metadata, bounds, evolve-path assertions, and regression baseline checks).
  - [x] Fully wrap `dtlz` with managed extension polish and dedicated regression tests (multi-objective metadata, bounded evolve-path assertions, Pareto-distance API checks, and safe known-input fitness validation).
  - [x] Fully wrap `wfg` with managed extension polish and dedicated regression tests (multi-objective metadata, bounded evolve-path assertions, and safe known-input fitness validation).
  - [x] Fully wrap `unconstrain` with managed extension polish and dedicated regression tests (death-penalty semantics, constrained-input adaptation, and safe known-input validation).
  - [x] Reach full parity with current pagmo `problems/*.hpp` catalog in Sprint 3A wrapper coverage (no remaining unwrapped problem headers).
  - [x] Fully wrap `translate` with managed extension polish and dedicated regression tests (translation vector surfaces, shifted-optimum behavior checks, and safe known-input validation).
  - [x] Fully wrap `decompose` with managed extension polish and dedicated regression tests (meta-problem construction from managed `IProblem`, decomposed/original fitness assertions, and safe known-input validation).
  - [x] Extend midpoint-probe hardening to constrained/singular-probe problems by adding explicit safe-input fitness-size checks in per-type tests.
  - [x] Harden shared problem test base with per-problem midpoint-probe opt-out to avoid singular fitness crashes while preserving explicit safe-input contract checks.
  - [x] Harden legacy `cec2006` tests with explicit safe-input fitness-length checks and midpoint-probe opt-out to eliminate host-crash instability during grouped runs.
  - [x] Standardize problem test architecture with shared `TestProblemBase` runtime checks and instance-based regression data providers (no static-only regression plumbing).
  - [x] Fully wrap `ihs` with managed extension polish and dedicated regression tests (name/seed/verbosity/log access, evolve-path assertions, and compatibility gating in shared algorithm tests).
  - [x] Fully wrap `nsga2` with managed extension polish and dedicated regression tests (multi-objective evolve-path assertions and compatibility gating in shared algorithm tests).
  - [x] Fully wrap `moead` with managed extension polish and dedicated regression tests (multi-objective evolve-path assertions and compatibility gating in shared algorithm tests).
  - [x] Fully wrap `moead_gen` with managed extension polish and dedicated regression tests (multi-objective evolve-path assertions and compatibility gating in shared algorithm tests).
  - [x] Fully wrap `maco` with managed extension polish and dedicated regression tests (multi-objective evolve-path assertions and compatibility gating in shared algorithm tests).
  - [x] Fully wrap `cstrs_self_adaptive` with managed extension polish and dedicated regression tests (constrained evolve-path assertions, seed/verbosity coverage, and runtime API checks).
  - [x] Fully wrap `mbh` with managed extension polish and dedicated regression tests (name/seed/verbosity coverage and perturbation configuration assertions).
  - [x] Fully wrap `not_population_based` helper surface with managed extension polish and dedicated regression tests (selection/replacement policy configuration coverage).
  - [x] Strengthen `not_population_based` regression tests to assert invalid selection/replacement inputs throw expected wrapper exceptions (not just happy-path no-throw).
  - [x] Reach full parity with current pagmo `algorithms/*.hpp` catalog in Sprint 3A wrapper coverage (no remaining unwrapped algorithm headers).
  - [x] Add type-erasure bridges (`to_algorithm`) for newly wrapped v1 algorithms (`ihs`, `nsga2`, `moead`, `moead_gen`, `maco`, `mbh`, `cstrs_self_adaptive`) and wire them in `AlgorithmInterop`.
  - [x] Add runtime validation that new bridged algorithms flow through `archipelago`/`island` managed paths without unsupported-type failures (including constrained and unconstrained managed-problem cases).
  - [x] Refactor shared `TestAlgorithmBase` multi-objective coverage to assert correct population/objective-shape behavior (instead of single-objective champion assumptions), and re-enable `MultiObjective=true` on wrapped MO algorithms.
  - [x] Add managed multi-objective `IProblem` test wrapper so multi-objective algorithm bridges can be validated through managed `archipelago` overloads end-to-end.
  - [x] Phase 1: remove SWIG `%include`-level `namespace pagmo` wrapping for touched algorithm surfaces (`cstrs_self_adaptive`, `ihs`, `maco`, `mbh`, `moead`, `moead_gen`, `nsga2`) using fully-qualified `.i` declarations.
  - [x] Phase 2 slice: remove SWIG `%include`-level `namespace pagmo` wrapping for `batch_evaluators` + `r/s_policies` + `topologies` by converting touched `.i` declarations to fully qualified `pagmo::` forms and moving includes outside namespace scope.
  - [x] Phase 2 slice: remove `%include`-level namespace wrapping for `utils/hv_algos/hv_algorithm` and `utils/hypervolume` (fully-qualified `pagmo::` declarations + includes moved outside namespace scope).
  - [x] Phase 2+: continue rolling namespace-wrapper removal across remaining wrapper groups (`problems/*` and residual `utils/*`, especially `utils/multi_objective`) with per-slice regen/build/test gates.
  - [x] Remove topology connection `SWIGTYPE_*` exposure by instantiating typed SWIG templates (`SizeTVector`, `TopologyConnections`), add managed `GetConnectionsData` helpers, and strengthen topology tests to assert neighbor/weight wiring.
  - [x] Remove `de1220` constructor `SWIGTYPE_*` leakage by instantiating typed `UIntVector` and add dedicated constructor regression coverage for `allowed_variants`.

5. **Sprint 3B: Hardening + Extensibility Completion (Depth)**
- [ ] Apply/complete C# extensibility surfaces where in v1 scope.
- [ ] Remove/contain `SWIGTYPE_*` leakage on touched public APIs.
- [ ] Audit and eliminate shallow raw-pointer ownership semantics across wrapper facades (copy/assign/destructor ownership rules), replacing with robust lifetime-safe patterns.
- [x] Hardened managed policy ownership transfer paths (`r_policy`/`s_policy`) to be exception-safe after ownership release, with regression coverage for null/disposed inputs and validity checks.
- [x] Fixed managed problem callback lifetime in native director flows by rooting `ProblemCallbackAdapter` instances via `ConditionalWeakTable`, preventing GC callback crashes during long-running native calls.
- [ ] Normalize naming/signatures and add deeper behavior/regression tests.
- [ ] Standardize C++?C# exception bubbling and mapping (constructor/evolve/wait paths, including async/runtime wrapper paths).
- [x] Replace null-return `catch (...)` patterns in native bridge with explicit error propagation so managed exceptions preserve actionable native context.
- [x] Verified non-generated native bridge surface (`managed_bridge.cpp`) consistently records contextual thread-local errors for both `std::exception` and `catch (...)` paths before null returns, and managed interop consumes that channel.
- [x] Add thread-local native bridge error channel for managed_bridge exports (pagmosharp_get_last_error / pagmosharp_clear_last_error) and consume it in managed interop (problem creation, population creation, gradient/sparsity helpers, BFE operators) so null-return paths surface actionable failure messages.
- [x] Add execute-path SWIG exception context for runtime orchestration methods (`algorithm.evolve`, `island.evolve`, `island.wait`, `island.wait_check`, `archipelago.evolve`, `archipelago.wait`, `archipelago.wait_check`, `thread_island.run_evolve`) and lock behavior with regression tests.
- [ ] Correct built-in problem `thread_safety` metadata in wrappers (remove placeholder `none` defaults where inaccurate) and add threaded runtime verification coverage.
- [x] Partial pass: corrected placeholder thread-safety metadata from `none` to `basic` for `ackley`, `cec2006`, `golomb_ruler`, `inventory`, `minlp_rastrigin`, and `zdt`, with explicit assertions added in corresponding problem tests.
- [x] Investigate and eliminate full-suite post-run test-host crashes (all tests pass but host process aborts during teardown), with explicit native-lifetime root cause and regression guard (resolved by switching midpoint probe vector construction to array-based initialization in `TestProblemBase`).
- [x] Deduplicate SWIG director/include declarations in root interface and keep one canonical registration path for `problem`, `r_policy`, and `s_policy` bridges.
- [x] Normalize SWIG fragment hygiene by removing per-file `%module` directives from included `.i` fragments and keeping module definition at the root interface only.
- [x] Complete multi-objective support end-to-end (problem/algorithm flows, champion and population semantics, and static helper functions in `utils/multi_objective`).
- [ ] Define and implement a project-wide std::size_t managed mapping strategy (beyond current migration-entry adapter conversion) without breaking SWIG STL wrappers/ABI contracts.
- [x] Documented Sprint 3B size_t compatibility matrix and phased plan in `.ai/SIZE_T_STRATEGY.md`.
- [ ] Implement managed projection wrappers for remaining size_t-heavy opaque log/sparsity surfaces so touched public APIs avoid `SWIGTYPE_*size_t*`.
- [ ] Add regression tests for size_t projection wrappers (shape/count/index transfer assertions).
- [x] First size_t projection slice completed: gaco log projection (get_log_entries + managed GetLogLines()) with behavior assertions in Test_gaco.
- [x] Added shared C# log abstraction (IAlgorithmLogLine + universal IAlgorithm.GetLogLines() default-empty contract, no per-algorithm `IHas...` marker interface requirement) and implemented it for gaco with generic raw-field access assertions.
- [x] Added second size_t projection slice: mbh log projection (get_log_entries + typed/generic managed logs) with field-parity assertions in Test_mbh.
- [x] Added third size_t projection slice: ihs log projection (including ideal-point vector payload) with field-parity assertions in Test_ihs.
- [x] Added fourth size_t projection slice: cstrs_self_adaptive log projection with typed/generic managed logs and field-parity assertions in Test_cstrs_self_adaptive.
- [x] Added fifth size_t projection slice: de log projection (get_log_entries + typed/generic managed logs) with field-parity assertions in Test_de.
- [x] Added sixth size_t projection slice: cmaes log projection (get_log_entries + typed/generic managed logs) with field-parity assertions in Test_cmaes.
- [x] Completed algorithm-log projection sweep for all active wrapped algorithms that expose logs in v1 surface (`bee_colony`, `compass_search`, `cmaes`, `cstrs_self_adaptive`, `de`, `de1220`, `gaco`, `gwo`, `ihs`, `maco`, `mbh`, `moead`, `moead_gen`, `nsga2`, `nspso`, `pso`, `pso_gen`, `sade`, `sea`, `sga`, `simulated_annealing`, `xnes`), with universal `IAlgorithm.GetLogLines()` plus typed `GetTypedLogLines()` surfaces and shared evolve-path log assertions.
- [x] Removed raw tuple-based algorithm log leakage from generated APIs by ignoring direct `get_log()` on active algorithm wrappers and retaining only typed log projection surfaces (`get_log_entries` / `GetTypedLogLines` / `GetLogLines`).
- [x] Removed legacy bee-colony tuple bridge (`FromBeeColonyLogTuple`) so tuple `SWIGTYPE_*` artifacts no longer leak into generated managed surfaces.
- [x] Added a concrete end-to-end constrained optimization regression using a simple managed two-parabola problem (`objective = parabola1 + parabola2`, one inequality constraint) and asserted real feasible-objective improvement after `cstrs_self_adaptive` evolve.
- [x] Strengthened managed multi-objective runtime regression in `archipelago` (`nsga2` + managed MO problem): assert ideal-point improvement after evolve and verify non-trivial non-dominated front extraction via `non_dominated_front_2d`.
- [x] Expanded type-erasure bridge coverage to all active algorithm UDAs with explicit `to_algorithm()` regressions (`Test_algorithm_type_erasure_bridges`) so `AlgorithmInterop` mappings are locked by tests.
- [x] Added end-to-end managed runtime interop coverage for all active bridged algorithms via `island.Create(IAlgorithm, IProblem, ...)` (single-objective, constrained, and multi-objective cases), including evolve/wait_check and population-shape assertions.
- [x] Added matching `archipelago.push_back_island(IAlgorithm, IProblem, ...)` interop matrix coverage for all active bridged algorithms with evolve/wait_check and objective/champion-shape assertions.
- [x] Add managed wrapper surface for core `utils/multi_objective` static helpers (`pareto_dominance`, `non_dominated_front_2d`, `crowding_distance`, `sort_population_mo`, `select_best_N_mo`, `ideal`, `nadir`, `decompose_objectives`) and assert concrete behavior in `Test_multi_objective`.
- [x] Assert and lock multi-objective population semantics in shared algorithm tests: champion_x/champion_f must throw on multi-objective populations while get_x/get_f remain the supported data path.
- [x] Reduce wrapper layering for multi-objective helpers by removing intermediate MultiObjectiveUtils C++/C# helper classes and exposing direct pagmo.pagmo.* static bindings via SWIG namespace declarations.
- [x] Remove hypervolume/hv_algorithm shared-pointer SWIGTYPE_* exposure by instantiating typed HvAlgorithmSharedPtr and add runtime selector coverage in Test_hypervolume.
- [x] Remove `std::ostream` SWIGTYPE leakage from managed surface by pruning unused ostream-only declarations in `io.i` (no managed API usage, no runtime behavior change).
- [x] Remove `double*` SWIGTYPE leakage from `hv_algorithm` by suppressing the raw-pointer `volume_between(double*, double*, size_t)` overload and keeping vector-based managed overloads.
- [x] Remove policy/raw-pointer SWIGTYPE leakage from generated surface by suppressing low-level `r_policyPagmoWrapper::replace` / `s_policyPagmoWrapper::select` exports; managed policy APIs remain unchanged.
- [x] Remove managed-problem shared_ptr-constructor SWIGTYPE leakage by suppressing the shared_ptr overload and retaining callback-based managed constructor paths.
- [ ] Remaining SWIGTYPE leakage is now limited to sparsity-pattern pointer surfaces (`problem` / `managed_problem` / `problem_callback` / `minlp_rastrigin`) and is tracked as a focused follow-up slice.
- [x] Remove MigrationEntry ID SWIGTYPE_* exposure by mapping adapter IDs (migration_id, immigrant_id) to ulong and add dedicated regression coverage in Test_migration_entry.
- [ ] Anything from 3A is not considered production-ready until 3B gates pass.

6. **Sprint 4: Documentation + Samples**
- [ ] C#-first docs, quickstart, and canonical runnable examples.
- [ ] Publish a supported-feature matrix by build/environment (for example optional algorithm availability such as IPOPT/NLopt).
- [ ] Perform an exception-usage audit across managed/native wrapper layers to verify existing code is surfacing actionable exceptions consistently and not silently swallowing failure context.

7. **Sprint 5: Release Readiness**
- [ ] Packaging/versioning/changelog/release checklist and ship gates.
- [ ] Remove machine-local hardcoded tool/include paths from build scripts/projects (`swig.exe`, include/lib paths) and replace with configurable/CI-friendly inputs.
- [ ] Purge dead/orphan native files and stale placeholders from the solution (unused headers/cpps and abandoned stubs) before release freeze.

8. **Sprint 6: v1.0 Release**
- [ ] Publish artifacts and release notes.

9. **Sprint 7 (v1.x/2.0): Linux/CMake**
- [ ] Cross-platform build track after v1.0.
- [ ] Evaluate optional managed thread-clone strategy for non-thread-safe managed problems (for example `IThreadCloneableProblem` with per-thread clone context) and integrate only if it fits pagmo execution semantics cleanly.

### API and Quality Rules
- During 3A breadth, prioritize coverage velocity with smoke tests.
- During 3A execution, do not add new `SWIGTYPE_*` leakage on touched public surfaces; either map to usable managed types now or keep the API out of scope until a later sprint.
- During 3B depth, enforce public API quality:
  - no unresolved `SWIGTYPE_*` exposure for in-scope v1 surfaces
  - consistent API shape and lifecycle behavior
  - deeper tests for callbacks/thread-safety/lifetimes where relevant
- Deferred items must be explicitly assigned to a later sprint; no silent carryover.

### Test Plan
- **Sprint 3A:** catalog smoke suite for each added algorithm/problem wrapper.
- **Sprint 3B:** focused deep tests by subsystem (`Algorithm`, `BFE`, `Policy`, `Topology`) plus regressions.
- Pre-release requires stable acceptance subset across fundamentals + runtime + selected breadth set.

### Assumptions
- Breadth-first then depth-hardening is intentional for large catalog onboarding.
- `Problem` remains core and already mature enough to build on.
- v1.0 stays Windows-first; Linux is explicitly post-release.

















