# PagmoSharp Roadmap

Last updated: 2026-03-28

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

5. **Sprint 3B: Hardening + Extensibility Completion (Depth)**
- [ ] Apply/complete C# extensibility surfaces where in v1 scope.
- [ ] Remove/contain `SWIGTYPE_*` leakage on touched public APIs.
- [ ] Audit and eliminate shallow raw-pointer ownership semantics across wrapper facades (copy/assign/destructor ownership rules), replacing with robust lifetime-safe patterns.
- [ ] Normalize naming/signatures and add deeper behavior/regression tests.
- [ ] Standardize C++?C# exception bubbling and mapping (constructor/evolve/wait paths, including async/runtime wrapper paths).
- [ ] Replace null-return `catch (...)` patterns in native bridge with explicit error propagation so managed exceptions preserve actionable native context.
- [ ] Correct built-in problem `thread_safety` metadata in wrappers (remove placeholder `none` defaults where inaccurate) and add threaded runtime verification coverage.
- [x] Investigate and eliminate full-suite post-run test-host crashes (all tests pass but host process aborts during teardown), with explicit native-lifetime root cause and regression guard (resolved by switching midpoint probe vector construction to array-based initialization in `TestProblemBase`).
- [x] Deduplicate SWIG director/include declarations in root interface and keep one canonical registration path for `problem`, `r_policy`, and `s_policy` bridges.
- [x] Normalize SWIG fragment hygiene by removing per-file `%module` directives from included `.i` fragments and keeping module definition at the root interface only.
- [ ] Complete multi-objective support end-to-end (problem/algorithm flows, champion and population semantics, and static helper functions in `utils/multi_objective`).
- [ ] Anything from 3A is not considered production-ready until 3B gates pass.

6. **Sprint 4: Documentation + Samples**
- [ ] C#-first docs, quickstart, and canonical runnable examples.
- [ ] Publish a supported-feature matrix by build/environment (for example optional algorithm availability such as IPOPT/NLopt).

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








