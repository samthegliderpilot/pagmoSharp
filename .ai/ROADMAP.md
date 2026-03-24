# PagmoSharp Roadmap

Last updated: 2026-03-24

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
- [ ] Include only minimal smoke validation per added type (construct/use basic APIs/evolve where applicable).
- [ ] Output: large usable surface, not final ergonomics.
- Active progress:
  - [x] Fully wrap `null_algorithm` with managed extension polish (`IAlgorithm` compatibility helpers) and dedicated regression tests.

5. **Sprint 3B: Hardening + Extensibility Completion (Depth)**
- [ ] Apply/complete C# extensibility surfaces where in v1 scope.
- [ ] Remove/contain `SWIGTYPE_*` leakage on touched public APIs.
- [ ] Audit and eliminate shallow raw-pointer ownership semantics across wrapper facades (copy/assign/destructor ownership rules), replacing with robust lifetime-safe patterns.
- [ ] Normalize naming/signatures and add deeper behavior/regression tests.
- [ ] Standardize C++→C# exception bubbling and mapping (constructor/evolve/wait paths, including async/runtime wrapper paths).
- [ ] Complete multi-objective support end-to-end (problem/algorithm flows, champion and population semantics, and static helper functions in `utils/multi_objective`).
- [ ] Anything from 3A is not considered production-ready until 3B gates pass.

6. **Sprint 4: Documentation + Samples**
- [ ] C#-first docs, quickstart, and canonical runnable examples.

7. **Sprint 5: Release Readiness**
- [ ] Packaging/versioning/changelog/release checklist and ship gates.

8. **Sprint 6: v1.0 Release**
- [ ] Publish artifacts and release notes.

9. **Sprint 7 (v1.x/2.0): Linux/CMake**
- [ ] Cross-platform build track after v1.0.

### API and Quality Rules
- During 3A breadth, prioritize coverage velocity with smoke tests.
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

