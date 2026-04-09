# API Naming Policy (Managed Surface)

## Goal
Provide a C#-friendly managed API without breaking parity with pagmo/pygmo naming where that parity is operationally useful.

## Rules
- Keep generated SWIG members unchanged.
  - Generated wrappers are an interop layer and are not hand-edited.
- Handwritten extension APIs should prefer PascalCase for C#-centric helpers.
  - Example: `GetConnectionsData`, `PushBackIsland`, `GetIsland`.
- Keep snake_case compatibility entrypoints where they are already established in public usage.
  - Especially for runtime orchestration paths that mirror pagmo naming (`push_back_island`, `get_island_copy`).
- Do not introduce duplicate naming families unless they provide a clear compatibility or readability benefit.
- New aliases must be thin forwards to canonical behavior (no semantic fork).

## Compatibility Plan
- Existing snake_case APIs remain supported for v1.x.
- PascalCase aliases are added selectively on high-traffic managed entrypoints.
- If deprecations are introduced later, they should be warning-only first, with at least one minor-version transition window.

## Test Guardrails
- Reflection tests should verify required aliases exist and intended snake_case compatibility paths remain available.
- Handwritten API audits continue to enforce no unintended low-level SWIGTYPE leakage on public extension surface.
