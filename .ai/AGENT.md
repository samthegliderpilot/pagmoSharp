# AGENT NOTES - pagmoSharp

## Project Summary
- `pagmoSharp` is a C# wrapper/binding layer over pagmo2 using SWIG + native C++ bridge code.
- Main goal: robust C# interop for pagmo runtime + extensibility points.
- Current roadmap is tracked in `.ai/ROADMAP.md`.

## Repository Structure
- `pagmoSharp/`
  - Generated SWIG C# wrappers: `pygmoWrappers/`
  - Handwritten C# extensions/adapters: `pagmoExtensions/`
- `pagmoWrapper/`
  - Generated SWIG C++ wrapper: `GeneratedWrappers.cxx`
  - Native bridge/shim code (interop glue)
- `swigInterfaceFileAndPagmoHeaders/`
  - SWIG `.i` interface files + vendored headers used for wrapper generation
- `Tests/Tests.PagmoSharp/`
  - NUnit tests for wrapper behavior and runtime wiring
- `scripts/`
  - Build/regeneration helpers (`regen-swig.ps1`, `build-native.ps1`)

## Working Model
- Regenerate SWIG first, then native build. Do not run those in parallel.
- Avoid editing auto-generated SWIG outputs directly; edit `.i` and handwritten extension files.
- Prefer boundary-focused managed APIs in `pagmoExtensions` over leaking raw SWIG internals.

## Sprint Strategy (High-Level)
- Sprint 2 focus: runtime-critical orchestration + stable interop patterns.
- Breadth (`dozens` of algorithms/problems) is Sprint 3A.
- Hardening/consistency (API quality, exception mapping, SWIGTYPE cleanup) is Sprint 3B.

## User Coding Preferences (Persistent)
- Exception usage: avoid noisy guard clauses unless one of these applies:
  1. Safety-critical corruption/risk prevention.
  2. Meaningful higher-signal error than natural downstream exception.
  3. Intentional boundary validation (config-to-execution threshold crossing).
- Testing philosophy:
  - Tests should validate wrapper wiring/configuration, not algorithm optimality correctness.
  - For wrapper constructor/factory paths, assert configured inputs are reflected in observable state.
  - "No throw" is insufficient by itself.
  - Include negative-path contract tests for unsupported/invalid usage.
- Style preference: pragmatic, minimal noise, fail-fast with useful messages at true boundaries.

## Technical Notes / Pitfalls
- `bfe` interop is sensitive; fake inheritance/upcasts between concrete BFE types and `bfe` can cause access violations.
- Prefer explicit conversion to type-erased `bfe` (`to_bfe()`) over relying on unsafe casts.
- Keep algorithm normalization centralized at the interop boundary (`IAlgorithm` -> `algorithm`).

## Collaboration Expectations
- If design seems wrong or fragile, explicitly push back and explain why.
- Do not preserve legacy patterns just because they exist; prioritize robust/idiomatic architecture.
- If scope pressure exists, comment out/defer unstable features rather than ship brittle hacks.
