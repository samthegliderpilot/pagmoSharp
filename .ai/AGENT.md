# AGENT NOTES - Pagmo.NET

## Project Summary
- `Pagmo.NET` is a C# wrapper/binding layer over pagmo2 using SWIG + native C++ bridge code.
- Main goal: robust C# interop for pagmo runtime + extensibility points.
- Current roadmap is tracked in `.ai/ROADMAP.md`.

## Repository Structure
- `Pagmo.NET/`
  - Generated SWIG C# wrappers: `pygmoWrappers/`
  - Handwritten C# extensions/adapters: `pagmoExtensions/`
- `pagmoWrapper/`
  - Generated SWIG C++ wrapper: `GeneratedWrappers.cxx`
  - Native bridge/shim code (interop glue)
- `swig/`
  - SWIG `.i` interface files + vendored headers used for wrapper generation
- `Tests/Tests.Pagmo.NET/`
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
- Logging API preference: avoid per-algorithm `IHas...` marker interfaces; expose a universal `IAlgorithm.GetLogLines()` (default empty) and add typed per-algorithm log helpers where useful.

## Technical Notes / Pitfalls
- `bfe` interop is sensitive; fake inheritance/upcasts between concrete BFE types and `bfe` can cause access violations.
- Prefer explicit conversion to type-erased `bfe` (`to_bfe()`) over relying on unsafe casts.
- Keep algorithm normalization centralized at the interop boundary (`IAlgorithm` -> `algorithm`).
- `std::size_t` policy for v1: keep native `size_t` signatures and avoid global typemap flips; use managed projection wrappers to remove `SWIGTYPE_*size_t*` from touched public APIs (details: `.ai/SIZE_T_STRATEGY.md`).

## Cross-Platform (Linux) Notes
- **`pop_size_t` is `size_t`, not `unsigned long long` on Linux.** On Linux x64, `size_t` is
  `unsigned long` (32-bit less); on Windows x64 both happen to be 64-bit. Do NOT typedef
  `pop_size_t` as `unsigned long long` in the pagmo namespace — it conflicts. Instead, use
  `unsigned long long` directly at the SWIG boundary and convert to `pagmo::pop_size_t` inside
  implementations. This affects `multi_objective.h` (`FNDSResult` fields, `RekSum` parameters)
  and the corresponding `GeneratedWrappers.cxx` SWIG accessor methods.
- **pagmo INTERFACE cmake definitions leak.** `Pagmo::pagmo` exports `PAGMO_WITH_NLOPT=ON`
  and `PAGMO_WITH_IPOPT=ON` as INTERFACE compile definitions when upstream pagmo was built
  with those features (e.g. the Ubuntu apt package). This causes our native library to try to
  compile in NLopt/IPOPT code paths even when we don't want them. Fix: add
  `target_compile_options(PagmoWrapper PRIVATE -UPAGMO_WITH_NLOPT -UPAGMO_WITH_IPOPT)` to
  `CMakeLists.txt`. Our own optional-solver options are named `PAGMONET_WITH_*` to avoid collision.
- **`GeneratedWrappers.cxx` has manual `#if PAGMO_WITH_NLOPT/IPOPT` guards.** The NLopt and
  IPOPT code zones in the generated wrapper are guarded with preprocessor blocks so the file
  compiles cleanly when those solvers aren't present. These guards must be preserved when
  regenerating wrappers (re-apply them to the fresh SWIG output).
- **`OptionalSolverAvailability`** (`Pagmo.NET/pagmoExtensions/OptionalSolverAvailability.cs`)
  provides `IsNloptAvailable` / `IsIpoptAvailable` via `NativeLibrary.TryGetExport`. Use these
  in tests instead of try-catching `EntryPointNotFoundException`.
- **Linux requires .NET 10 SDK for full test discovery.** .NET 8 on Linux only enumerates ~198
  of 593 tests via VsTest. Tests and examples target `net10.0`; the library itself targets `net8.0`.
- **moead_gen `get_name()` changed between pagmo versions.** pagmo 2.19 (Ubuntu 24.04 apt)
  returns a different string than older versions. Test uses `Does.Contain("MOEAD")` not exact match.
- **Linux native build:** Use `pwsh scripts/build-native.ps1` with `$VCPKG_ROOT` set. The
  script runs `vcpkg install pagmo2:x64-linux-static-pic --overlay-triplets=triplets/` then
  invokes cmake with the vcpkg toolchain. pagmo2, Boost.Serialization, and TBB are linked
  statically into `libPagmoWrapper.so` — no system runtime deps. See `.ai/LINUX_TESTING_HANDOFF.md`.

## Collaboration Expectations
- If design seems wrong or fragile, explicitly push back and explain why.
- Do not preserve legacy patterns just because they exist; prioritize robust/idiomatic architecture.
- If scope pressure exists, comment out/defer unstable features rather than ship brittle hacks.
