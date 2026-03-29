# `std::size_t` Strategy (Sprint 3B Design Pass)

Last updated: 2026-03-29

## Goal
- Eliminate unsafe/opaque `size_t` exposure in public managed APIs without destabilizing SWIG generation, ABI, or runtime behavior.

## Current State Snapshot
- Managed typed wrappers already exist for several `size_t` containers and are currently generated as `uint`:
  - `SizeTVector`, `SizeTPair`, `TopologyConnections`, `SparsityPattern`, `VectorOfSparsityPattern`
- Remaining `size_t` leakage is concentrated in opaque SWIG pointer types, primarily:
  - Algorithm log tuple vectors (`gaco`, `ihs`, `mbh`, `sea`, `compass_search`, `cstrs_self_adaptive`)
  - Problem sparsity pointer bridges (`SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t` and nested variants)
- `MigrationEntry` now uses a stable wrapper-boundary conversion to `ulong` (no `SWIGTYPE_p_std__size_t` file present).

## Findings From Failed Attempts
- Global `std::size_t -> nuint` typemap produced mixed generated output (`nuint` + `uint` + `SWIGTYPE`), causing compile failures.
- Alias-targeted typemap (`pagmoWrap::MigrationId`) was canonicalized by SWIG and did not reliably map all call paths.
- Regenerating managed wrappers without rebuilding native can cause hard crashes due to constructor signature drift.

## Compatibility Matrix
1. Stable now (keep for v1):
- `std::vector<std::size_t>` -> `SizeTVector` (`uint`)
- `std::pair<std::size_t, std::size_t>` -> `SizeTPair` (`uint`,`uint`)
- Topology connection payloads using size vectors (`TopologyConnections`)

2. Opaque now (target for managed projection wrappers in 3B):
- `std::vector<std::tuple<... size_t ...>>` algorithm logs
- raw sparsity pointer return paths from generated classes/directors

3. High risk if changed globally:
- Any broad typemap rewrite for `std::size_t` / `size_t` across entire interface
- Director and STL wrapper generation consistency

## Recommended v1 Policy
- Keep native signatures as-is (`std::size_t` in C++).
- Keep generated container mappings as currently stable (`uint` for typed `SizeT*` wrappers).
- Remove user-facing `SWIGTYPE_*size_t*` from touched APIs by adding explicit managed projection layers, not by global typemap flips.

## Sprint 3B Execution Plan
1. Add projection DTOs + helpers for each affected algorithm log:
- Example: `GacoLogEntry`, `IhsLogEntry`, `MbhLogEntry`, etc.
- Convert opaque SWIG log pointers into typed managed records in `pagmoExtensions`.

2. Add projection helpers for sparsity where opaque pointers still appear:
- Standardize on `SparsityPattern` / `VectorOfSparsityPattern` in public extensions.

3. Add regression tests for each projection surface:
- Validate shape, counts, and index-value transfer (not algorithm correctness).

4. Gate rule:
- No new public API may expose `SWIGTYPE_*size_t*` after touch.

5. Post-v1 (v2 exploration):
- Re-evaluate a true pointer-width managed mapping (`nuint`) only with a dedicated SWIG typemap isolation spike.

## Acceptance Criteria
- No touched v1 public managed surface returns or requires `SWIGTYPE_*size_t*`.
- Full build + test suite stable, with no teardown/access-violation regressions.
- Native + managed generation order remains enforced (`regen-swig` then native build).
