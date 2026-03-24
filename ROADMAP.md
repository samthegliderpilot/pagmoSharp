# PagmoSharp Roadmap

Last updated: 2026-03-24

## PagmoSharp Roadmap Reset (v1.0 with Explicit Breadth Sprint)

### Summary
- Preserve reset from Sprint 0 with completed items marked complete.
- Keep Windows-first v1.0 release before Linux/CMake.
- v1.0 extensibility targets: `Problem`, `Algorithm`, `BFE`, `Policy`, `Topology`.
- Defer `Island` and custom threading extensibility.
- Make breadth explicit: **Sprint 3A is the "dozens of algorithms/problems" wrapping sprint**.

### Sprint Structure
1. **Sprint 0 (Completed): Tooling Baseline**
- SWIG workflow, build/test path, hello-world SWIG validation.

2. **Sprint 1 (Completed): Fundamentals**
- `Program` and C# `Problem` baseline, partial types, foundational tests.

3. **Sprint 2 (In Progress): Critical Runtime + Patterns**
- Finish runtime-critical orchestration (`island/archipelago`, migration controls, policies/topologies needed for runtime correctness).
- Establish extensibility implementation patterns for `Algorithm`, `BFE`, `Policy`, `Topology`.
- Acceptance: core runtime scenarios and pattern examples are stable.

4. **Sprint 3A: Broad Coverage Pass (Breadth)**
- Primary goal: add `.i` + wrapper coverage for targeted v1 algorithm/problem catalog (the "dozens" sprint).
- Include only minimal smoke validation per added type (construct/use basic APIs/evolve where applicable).
- Output: large usable surface, not final ergonomics.

5. **Sprint 3B: Hardening + Extensibility Completion (Depth)**
- Apply/complete C# extensibility surfaces where in v1 scope.
- Remove/contain `SWIGTYPE_*` leakage on touched public APIs.
- Normalize naming/signatures and add deeper behavior/regression tests.
- Anything from 3A is not considered production-ready until 3B gates pass.

6. **Sprint 4: Documentation + Samples**
- C#-first docs, quickstart, and canonical runnable examples.

7. **Sprint 5: Release Readiness**
- Packaging/versioning/changelog/release checklist and ship gates.

8. **Sprint 6: v1.0 Release**
- Publish artifacts and release notes.

9. **Sprint 7 (v1.x/2.0): Linux/CMake**
- Cross-platform build track after v1.0.

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
