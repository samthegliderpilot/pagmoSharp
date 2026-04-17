# pagmoSharp Code Review

**Reviewer:** Claude (Sonnet 4.6)  
**Started:** 2026-04-15  
**Status:** Complete — Phases 2–10 finished + R1 compliance; 589/589 tests passing  
**Scope:** Full audit before v1.0-beta.1 release

---

## How to Read This File

- `[ ]` = open finding, not yet fixed  
- `[x]` = fixed / resolved  
- `[~]` = noted / deferred / intentional  
- `[?]` = needs further investigation  

Each finding has an ID (e.g. **P2.1**), a severity tag, the file(s) involved, and a description.

Severity tags:
- `[BUG]` = incorrect or unsafe behavior
- `[LEAK]` = resource leak
- `[SAFETY]` = could cause crash, AV, or UB if triggered
- `[ODR]` = C++ One Definition Rule / linker issue
- `[SWIG]` = SWIG-specific problem
- `[DEAD]` = unreachable / dead code
- `[STALE]` = comment or code that no longer matches reality
- `[STYLE]` = naming, formatting, or polish
- `[DOCS]` = missing or wrong documentation
- `[COMPAT]` = build / runtime compatibility concern
- `[LEGACY]` = pre-AI era artifact that should be cleaned up

---

## Standing Rules

These rules apply to all phases of the review. Any violation found should be flagged and fixed.

### R1 — C# types use PascalCase

All C# type names (classes, structs, interfaces, enums) must use PascalCase — including
SWIG-generated bridge and director types. Use `%rename` directives in the SWIG interface file
to map C++ snake_case class names to their PascalCase C# equivalents. Pagmo-compatible
snake_case is acceptable only for *method* names that intentionally mirror the pagmo C++ API
(e.g. `get_bounds()`, `fitness()`, `push_back_island()`).

### R2 — No exceptions escaping P/Invoke boundaries

Managed exceptions must not propagate through C++/P/Invoke callback boundaries. Director
callbacks (`SwigDirectorMethod*` stubs) must catch all exceptions and route them through
`pagmoPINVOKE.SWIGPendingException.Set()`. The `%typemap(csdirectorout)` blocks in the SWIG
interface file implement this for all return types; any new director class added must be covered.

### R3 — No unreachable null guards at internal call sites

`ArgumentNullException` and similar guards are appropriate only at real system boundaries
(user-visible API entry points, external callbacks). Internal wiring that is always non-null
by construction must not be guarded — the guard is dead code and adds noise.

---

## Phase 2 — C++ Pointer & Memory Safety ✓

### [x] P2.1 — `pagmosharp_algorithm_delete` investigation `[LEAK]`

**File:** `pagmoWrapper/managed_bridge.cpp`, `pagmoSharp/pagmoExtensions/NativeInterop.cs`  
`pagmosharp_algorithm_from_callback` allocates `new pagmo::algorithm(...)` and returns a raw pointer.
Investigated: confirmed the pointer is immediately consumed by `algorithm(IntPtr, true)` (SWIG's
ownership-taking constructor), so deletion goes through SWIG's normal generated path — not a leak.
`pagmosharp_algorithm_delete` was added for symmetry and self-documentation.

### [x] P2.2 — Redundant null check in `managed_problem` raw-pointer constructor `[DEAD]`

**File:** `pagmoWrapper/problem.h`  
The delegating constructor already throws on null `m_cb`; the extra `if (!cb)` guard was unreachable.
**Fix:** Removed.

### [x] P2.3 — `multi_objective.h` ODR violation `[ODR]`

**File:** `pagmoWrapper/multi_objective.h`  
`DecompositionWeights::METHOD_GRID/RANDOM/LOW_DISCREPANCY` were non-inline `const std::string` static
members defined in a header — ODR violation if included in >1 TU.  
**Fix:** Changed to `static inline const std::string` (C++17).

### [x] P2.4 — `r_policy.h` wall of commented-out legacy code + missing null guard `[LEGACY][SAFETY]`

**File:** `pagmoWrapper/r_policy.h`  
Large block of commented-out template exploration code from early development. `setBasePolicy()`
also lacked a null guard.  
**Fix:** Removed all dead code. Added null guard. Same treatment applied to `s_policy.h`.

### [~] P2.5 — `algorithm_callback` `has_set_seed`/`has_set_verbosity` default true `[STYLE]`

**File:** `pagmoWrapper/algorithm_callback.h`  
Defaults return `true` while implementations are no-ops. Valid pagmo semantics (a no-op seed is still
a "supported" seed call). **Deferred** — no change needed.

### [~] P2.6 — BFE operator functions are near-identical `[STYLE]`

**File:** `pagmoWrapper/managed_bridge.cpp`  
Three near-identical functions differ only in the cast. A template refactor would require exposing
C++ templates through the C bridge. **Deferred.**

---

## Phase 3 — SWIG Interface Health ✓

### [x] P3.1 — Namespace ordering inconsistency `[SWIG]`

**File:** `swigInterfaceFileAndPagmoHeaders/pagmoSharpSwigInterface.i`  
Several algorithm `.i` files are included outside any `namespace pagmo {}` block in the root `.i` file.
Investigation shows this is actually valid SWIG — the individual `.i` files use fully-qualified class
names (`class pagmo::nsga2`, `class pagmo::cstrs_self_adaptive`, etc.) which SWIG resolves correctly
without a surrounding namespace block. The inconsistency is stylistic but not harmful.  
The `bee_colony.i` and `algorithm.i` files at the bottom of the root file are also correctly structured
using qualified names. **No change required; finding downgraded to style note.**

### [x] P3.2 — Stale `%feature("director") pagmoWrap::multi_objective` `[STALE]`

**File:** `swigInterfaceFileAndPagmoHeaders/pagmoSharpSwigInterface.i`  
`pagmoWrap::multi_objective` class does not exist — `multi_objective.h` defines free functions and
helper classes in `pagmo` and global namespaces.  
**Fix:** Removed the stale director directive.

### [x] P3.3 — `wrapped_exception` struct and `test.throw_native` typemap `[LEGACY][BUG]`

**File:** `swigInterfaceFileAndPagmoHeaders/pagmoSharpSwigInterface.i`  
Bottom of file contained an unused `wrapped_exception` struct and a `%typemap(csdirectorout) void`
block calling `test.throw_native(e.ToString())`. The `test` class is test-only and does not exist in
the production assembly — this typemap would generate non-compiling code.  
**Fix:** Removed both blocks entirely.

### [x] P3.4 — `%apply void *VOID_INT_PTR { void * }` placement and purpose undocumented `[SWIG]`

**File:** `swigInterfaceFileAndPagmoHeaders/pagmoSharpSwigInterface.i`  
This typemap is load-bearing: it maps all `void*` to `IntPtr` in generated C# P/Invoke signatures,
which is required for the extern-C bridge functions in `managed_bridge.cpp` that return
heap-allocated native objects. Without it they would become `SWIGTYPE_p_void` (unusable opaque type).
The duplicate `%include "pagmoWrapper/tuple_adapters.h"` that followed it was redundant (already
included at line 77).  
**Fix:** Added explanatory comment to `%apply` directive; removed duplicate include.

### [x] P3.5 — Commented-out BFE `%extend` blocks `[LEGACY]`

**File:** `swigInterfaceFileAndPagmoHeaders/pagmoSharpSwigInterface.i`  
Three large commented-out `%extend default_bfe/member_bfe/thread_bfe` blocks from the old approach
before the managed bridge was built.  
**Fix:** Removed.

### [x] P3.6 — Commented-out `%include swigInterfaceFiles\exceptions.i` `[STALE]`

**File:** `swigInterfaceFileAndPagmoHeaders/pagmoSharpSwigInterface.i`  
The comment read: "causing errors, not sure why, and not really implemented anyway." The file is a stub
covered by the global exception handler already in place.  
**Fix:** Removed the commented line.

### [x] P3.7 — Narrative "journal entry" developer comments `[LEGACY]`

**File:** `swigInterfaceFileAndPagmoHeaders/pagmoSharpSwigInterface.i`  
"The whole problem vs. problemBase question is a little confusing. To make it better (or worse)..."
and stray `// need other languages?` removed. Replaced with factual comments explaining what each
section does.

### [~] P3.8 — Redundant `%ignore` directives for `shared_ptr` constructors `[STYLE]`

**File:** `swigInterfaceFileAndPagmoHeaders/pagmoSharpSwigInterface.i`  
Four `%ignore` directives for `shared_ptr<problem_callback>` constructor variants (with/without
namespace prefix, with/without spaces). SWIG is very literal about name matching and different SWIG
versions may generate slightly different forms. Keeping all four is the safe choice. **Deferred.**

### [x] P8.3 — `VectorOfVectorIndexes` rename `[STYLE]`

**File:** `swigInterfaceFileAndPagmoHeaders/pagmoSharpSwigInterface.i`, generated wrappers, `docs/api-reference.md`  
`VectorOfVectorIndexes` → `VectorOfVectorOfULongs` for consistency with `VectorOfVectorOfDoubles`.
No hand-written C# used the old name; only SWIG-generated files and docs were affected.  
**Fix:** Renamed in `.i` file; regenerated SWIG; updated `docs/api-reference.md`.

---

## Pattern Assessment — "Was a simpler SWIG approach missed?"

The core challenge is that pagmo uses **duck-typed type-erasure** for its UDA/UDP concepts:
`pagmo::problem` and `pagmo::algorithm` wrap any type via templated constructors — not virtual
inheritance. SWIG cannot expose C++ templates, so there is no way to do
`new pagmo::problem(MyCSharpProblem{})` directly from C#.

The `managed_problem / problem_callback` director pattern addresses this correctly:

1. `problem_callback` is the director-enabled virtual base that C# subclasses.
2. `managed_problem` is a copy-safe UDT (holds `shared_ptr<problem_callback>`) that satisfies
   pagmo's by-value type-erasure model.
3. The `extern "C"` bridge (`pagmosharp_problem_from_callback`) provides a non-template entry
   point to create `pagmo::problem(managed_problem(cb))` from a raw pointer.
4. GCHandle pinning in `NativeInterop.cs` keeps the C# director alive as long as the owning
   `IProblem` is alive.

**Could `%extend` replace the extern-C bridge?**  
`%extend pagmoWrap::managed_problem { pagmo::problem to_problem() const { ... } }` would work and
is arguably cleaner for the SWIG surface — the same pattern used for all built-in UDAs
(`bee_colony.to_algorithm()`, etc.). The lifetime/ownership contract is the same either way. This
is a viable simplification to consider for v2, but the current pattern is correct and not fragile.

**`ProblemInterop` (reflection) vs. `AlgorithmInterop` (hardcoded switch):**  
`ProblemInterop.cs` uses `Expression.Lambda` to dynamically discover `to_problem()` on wrapped
native UDP types; `AlgorithmInterop.cs` uses a hardcoded switch. The reflection approach scales
automatically but has a small runtime cost and is harder to trace through a debugger. The
inconsistency is intentional (problems are a larger, more open-ended set) but worth a comment.

**Verdict:** No obviously better SWIG approach was missed. The pattern complexity is inherent in
bridging pagmo's duck-typed C++ concepts with SWIG's class-based binding model.

---

## Clever / Non-Obvious Code That Needs Comments

The following items need clarifying comments added for anyone reading the code cold:

### [x] NC.1 — `%ignore` on `managed_r_policy::replace` / `managed_s_policy::select`

**File:** `swigInterfaceFileAndPagmoHeaders/pagmoSharpSwigInterface.i`  
These ignore the `replace`/`select` methods on the *wrapper* class (not the director class).
The reason: those methods take `pagmo::individuals_group_t` (a `std::tuple<...>`), which SWIG cannot
wrap. C# code only ever calls through `r_policy_callback::replace` / `s_policy_callback::select`, which
take the `pagmoWrap::IndividualsGroup` struct adapter. Comments added explaining why during P8.1.

### [x] NC.2 — `typedef unsigned long long pop_size_t` inside `namespace pagmo {}`

**File:** `pagmoWrapper/multi_objective.h`  
Comment added explaining this is load-bearing for SWIG type mapping: without it `pagmo::pop_size_t`
becomes an opaque `SWIGTYPE_p_pagmo__pop_size_t` in generated C# instead of `ulong`.

### [x] NC.3 — `%apply void *VOID_INT_PTR { void * }` in the SWIG file

**File:** `swigInterfaceFileAndPagmoHeaders/pagmoSharpSwigInterface.i`  
Addressed in P3.4 — comment added.

### [x] NC.4 — `%pragma(csharp) moduleclassmodifiers = "public partial class"`

**File:** `swigInterfaceFileAndPagmoHeaders/pagmoSharpSwigInterface.i`  
Comment added explaining the `partial` modifier allows hand-written C# to extend the SWIG-generated
class. Covered by the comment block above the `%pragma` line.

### [x] NC.4b — GCHandle pinning pattern in `NativeInterop.cs`

**File:** `pagmoSharp/pagmoExtensions/NativeInterop.cs`  
Comment added explaining why `ConditionalWeakTable` is used instead of a plain dictionary
(plain dictionary would pin the owner forever via strong key reference).

### [x] NC.5 — Four-variant `%ignore` for `shared_ptr` constructors

**File:** `swigInterfaceFileAndPagmoHeaders/pagmoSharpSwigInterface.i`  
Comment added explaining all four spelling variants are needed because SWIG matches by the exact
string it sees after resolving includes (with/without namespace prefix, with/without spaces).

---

## Phase 4 — Director Pattern Health ✓

### [x] P4.1 — `ProblemCallbackAdapter` unprotected callbacks `[SAFETY]`

**File:** `pagmoSharp/pagmoExtensions/Problems/ProblemCallbackAdapter.cs`  
Complete rewrite: all callbacks now have try/catch with deferred exception capture via
`ExceptionDispatchInfo`. Safe fallback values returned on exception path (`[0,1]` bounds,
`""` string, `false` bool, `0` uint, empty collections).

Also fixed a related bug: `NativeInterop.CreateProblemPointer` never checked
`ConsumeDeferredManagedException()` after `problem_from_callback` returned, so exceptions
deferred during problem construction were silently lost. Added check immediately after the call;
deferred exceptions now rethrow with the original stack trace preserved via
`ExceptionDispatchInfo.Capture(...).Throw()`. Passes 589/589 tests.

### [~] P4.2 — `gradient_sparsity()` / `hessians_sparsity()` return raw `SWIGTYPE_*` `[SWIG]`

**File:** `pagmoSharp/pagmoExtensions/Problems/ProblemCallbackAdapter.cs`  
These methods are `override`s whose return type is dictated by the SWIG-generated base class
(`problem_callback`). They cannot return a cleaner type without SWIG changes. The methods already
correctly use `swigRelease()` for ownership transfer. Finding downgraded to note — not actionable
without a SWIG interface change.

### [x] P4.3 — r_policy/s_policy director chain has no exception deferral `[SAFETY]`

**File:** `swigInterfaceFileAndPagmoHeaders/pagmoSharpSwigInterface.i`  
Added `%typemap(csdirectorout)` blocks for `void`, `std::string`, `bool`, `unsigned int`, and
`pagmoWrap::IndividualsGroup` return types. These are the standard SWIG mechanism for wrapping
the generated `SwigDirectorMethod*` stubs with try/catch; any managed exception is captured via
`pagmoPINVOKE.SWIGPendingException.Set()` so the C++ director can re-throw it as a C++
exception rather than letting it escape through P/Invoke (which aborts in .NET Core).

Key subtlety: `csdirectorout` typemaps must appear BEFORE the first `%feature("director")`
declaration. A previous placement after the director includes had no effect. SWIG wraps
non-void typemap content with `return ...;` so `try/catch` blocks (statements, not expressions)
must be packaged as immediately-invoked lambda expressions: `((Func<T>)(() => { ... }))()`.

For `problem_callback` and `algorithm_callback`, `ProblemCallbackAdapter` /
`AlgorithmCallbackAdapter` are the primary safety net; these typemaps are a second line of
defence. For `r_policy_callback` and `s_policy_callback` (which have no adapter layer), they are the
primary safety net.

### [x] P4.4 — `r_policy_callback::is_valid()` default returns `false` `[BUG]`

**Files:** `pagmoWrapper/r_policy.h`, `pagmoWrapper/s_policy.h`  
pagmo uses `is_valid()` to check a policy is properly constructed. Default `false` would cause pagmo
to reject any managed policy even when fully implemented.  
**Fix:** Changed default to `true` in both `r_policy_callback` and `s_policy_callback` (done in Phase 2).

### [x] P4.5 — `r_policy_callback`/`s_policy_callback` `get_name()` returns empty string `[STYLE]`

**Fix:** Changed defaults to `"C# r_policy"` / `"C# s_policy"` (done in Phase 2).

---

## Phase 5 — ROADMAP Completeness Audit ✓

### [x] P5.1 — AlgorithmInterop coverage check

**File:** `pagmoSharp/pagmoExtensions/Algorithms/AlgorithmInterop.cs`  
Found one gap: `ipopt` implements `IAlgorithm` and has `to_algorithm()` in its `.i` file but was
missing from the switch in `NormalizeToTypeErased`. Without this arm, `ipopt` fell through to the
managed-callback fallback path (`new algorithm(source)`), which is functionally correct but incurs
unnecessary overhead.  
**Fix:** Added `ipopt` arm to the switch.  
`grid_search` and `not_population_based` are correctly handled by the `_ =>` fallback and mixin base
respectively — no switch arm needed.

### [x] P5.2 — Log projection coverage check

**Files:** `pagmoWrapper/*_log_projection.h`, `pagmoSharp/pagmoExtensions/Algorithms/*.cs`  
All 27 algorithm `.cs` files that wrap pagmo algorithms with logs have `GetTypedLogLines()`.
Complete coverage confirmed.

### [x] P5.3 — Problem `to_problem()` coverage check

**File:** `swigInterfaceFileAndPagmoHeaders/pagmoSharpSwigInterface.i`  
All 22 wrapped native problem types have `PAGMOSHARP_PROBLEM_TO_PROBLEM(TYPE_NAME)` macros.
CS extension files match exactly. Complete coverage confirmed.

### [~] P5.4 — RELEASE_CHECKLIST code items

**File:** `.ai/RELEASE_CHECKLIST.md`  
Most items are release-process work (build gates, artifacts, changelog, publish). Code items:
- "Freeze API surface" — deferred until code review phases complete
- "SWIG regen reproducibility" — confirmed working via this review
- "README quickstart accuracy" — addressed in Phase 6 (P6.5)

---

## Phase 6 — Managed C# Documentation ✓

### [x] P6.1 — Copy-paste `docs/api-reference.md` doc comments

**Files:** `ProblemHandle.cs`, `NativeInterop.cs`  
`docs/api-reference.md` is auto-generated from XML docs and does exist. The boilerplate is
harmless on thin forwarding methods where the method name is self-describing. Fixed the two
genuinely misleading instances:
- `ProblemHandle.IsInvalid`: replaced with accurate description
- `NativeInterop.CallbackRootBucket.Add`: replaced with description of GC-rooting purpose
The volume of boilerplate in algorithm/problem extension files (~55 files) was reviewed and
left as-is; those files are thin wrappers where the class-level summary is sufficient.

### [~] P6.2 — Algorithm extension files doc audit

All algorithm files have class-level summaries and method-level docs on non-trivial adaptations
(`GetTypedLogLines`, `GetLastOptimizationResultCode`, etc.). Thin forwarding methods have the
boilerplate comment (harmless). **No action required.**

### [~] P6.3 — Problem extension files doc audit

Same as P6.2. **No action required.**

### [x] P6.4 — `ProblemManualFunctions` rename and doc

**File:** `pagmoSharp/problemManualFunctions.cs` → `pagmoSharp/ProblemExtensions.cs`  
Class renamed to `ProblemExtensions`; XML summary added to the class and both overloads.
Old file deleted. No external references to the old class name.

### [x] P6.5 — README review and update

**File:** `README.md`  
- Replaced the "learning project" intro with a production-grade description
- Updated requirements (SWIG 4.4, .NET 8+, removed stale versions)
- Updated FAQ to remove "only a handful of types" and fix the pygmo question
- Kept the detailed architecture, quickstart, examples, and feature matrix sections

---

## Phase 7 — Exception Policy Audit ✓

### [x] P7.1 — `BfeBridge` null check on `op` delegate

**File:** `pagmoSharp/pagmoExtensions/BatchEvaluators/bfe.cs`  
`op` is always a static P/Invoke method reference passed as a method group — it can never be
null. **Fix:** Removed the unreachable `if (op == null)` guard.

### [x] P7.2 — Internal null guards audit

**Files:** `NativeInterop.cs`, `ProblemInterop.cs`, `archipelago.cs`, `island.cs`  
All remaining `ArgumentNullException` throws guard user-facing parameters that flow into native
code. These are at real system boundaries and are appropriate. No changes needed.

---

## Phase 8 — Naming & Idioms ✓

### [x] P8.1 — C++ r_policy/s_policy naming + full PascalCase rename `[STYLE]`

**Phase 1 (C++ rename):**  
C++ class names unified with `*_callback` / `managed_*` convention:
- `r_policyBase` → `r_policy_callback`, `r_policyPagmoWrapper` → `managed_r_policy`
- `s_policyBase` → `s_policy_callback`, `s_policyPagmoWrapper` → `managed_s_policy`

Updated: `r_policy.h`, `s_policy.h`, `archipelago_swig.h`, `island_swig.h`, `archipelago.i`, `island.i`,
`pagmoSharpSwigInterface.i`, `r_policy.cs`, `s_policy.cs`, `archipelago.cs`, `island.cs`, test files.

**Phase 2 (PascalCase C# surface — per user preference):**  
All SWIG bridge class names exposed to C# now use PascalCase via `%rename` directives:
- `problem_callback` → `ProblemCallback`
- `managed_problem` → `ManagedProblem`
- `algorithm_callback` → `AlgorithmCallback`
- `managed_algorithm` → `ManagedAlgorithm`
- `r_policy_callback` → `RPolicyCallback`
- `managed_r_policy` → `ManagedRPolicy`
- `s_policy_callback` → `SPolicyCallback`
- `managed_s_policy` → `ManagedSPolicy`
- `null_problem_callback` → `NullProblemCallback`

**Phase 3 (R1 — pagmo enum types):**  
All SWIG-generated enum types and their values now use PascalCase:
- `thread_safety` → `ThreadSafety` (values: `None`, `Basic`, `Constant`)
- `evolve_status` → `EvolveStatus` (values: `Idle`, `Busy`, `IdleError`, `BusyError`)
- `migration_type` → `MigrationType` (values: `P2P`, `Broadcast`)
- `migrant_handling` → `MigrantHandling` (values: `Preserve`, `Evict`)
- `sga_selection` → `SgaSelection` (values: `Tournament`, `Truncated`)
- `sga_crossover` → `SgaCrossover` (values: `Exponential`, `Binomial`, `Single`, `Sbx`)
- `sga_mutation` → `SgaMutation` (values: `Gaussian`, `Uniform`, `Polynomial`)

`%rename` directives added to `pagmoSharpSwigInterface.i` (pagmo enums) and `sga.i` (sga enums).
All handwritten C# extension, test, and example files updated. Method names (e.g. `get_thread_safety()`)
are pagmo API-mirror names and correctly remain snake_case.  
SWIG regenerated; 589/589 tests pass.

### [x] P8.2 — `VectorOfVectorIndexes` rename

See P8.3 above — done in Phase 3.

### [x] P8.3 — `ProblemManualFunctions` → `ProblemExtensions`

See P6.4.

---

## Phase 9 — Project Structure & Configuration ✓

### [x] P9.1 — `.gitignore` gaps `[STYLE]`

**File:** `.gitignore`  
**Fix:** Added `.idea/` (JetBrains Rider) and `.nuget/` (local package cache).

### [~] P9.2 — `createSwigWrappersAndPlaceThem.bat` vs. `scripts/regen-swig.ps1` `[LEGACY]`

**Files:** root `createSwigWrappersAndPlaceThem.bat`, `pagmoWrapper/pagmoWrapper.vcxproj`  
Investigation: the bat is NOT superseded — it is the actual SWIG runner. `regen-swig.ps1` is a
mutex-locking wrapper around the bat (called by VS Code tasks). The vcxproj calls the bat directly
from the Pre-Build event, which is correct. No change needed. **Deferred.**

### [x] P9.3 — Target framework downgrade to `net8.0` `[COMPAT]`

**File:** `pagmoSharp/pagmoSharp.csproj`  
Changed `net10.0` → `net8.0` (current LTS). Test project and Examples project intentionally remain
at `net10.0` — they are not shipped library binaries. 589/589 tests pass.

### [~] P9.4 — Root-level debris `[STYLE]`

`scratch/`, `artifacts/`, `.nuget/` are all gitignored and will be absent on a clean checkout.
`artifacts/` is populated by `build-release-artifacts.ps1`. **No action required.**

### [~] P9.5 — `Platforms` property is `x64` only `[COMPAT]`

Intentional for v1.0 (Windows x64 only). Noted. **No action required.**

### [~] P9.6 — Solution file `[STYLE]`

**File:** `pagmoSharp.sln`  
All projects present and correctly referenced: `pagmoSharp`, `pagmoWrapper`, `Tests.PagmoSharp`,
`Examples.PagmoSharp`. Solution item folders cover SWIG interface files, problems, algorithms,
utils, policies, topologies. No stale references. **No action required.**

### [~] P9.7 — vcxproj Pre-Build event runs SWIG on every build `[STYLE]`

**File:** `pagmoWrapper/pagmoWrapper.vcxproj`  
SWIG runs on every native build. This ensures generated wrappers are always in sync with the SWIG
interface file, at the cost of ~2s per build. Acceptable for active development; the generated
wrappers are checked in so CI/CD can skip SWIG by patching the pre-build step if needed.
**Deferred — not worth changing pre-beta.**

---

## Phase 10 — Legacy / Pivot Artifact Cleanup ✓

### [x] P10.1 — SWIG interface narrative comments

Addressed in Phase 3 (P3.7).

### [x] P10.2 — `problem.h` transition note

**File:** `pagmoWrapper/problem.h`  
`// NOTE: Keep everything in your existing namespace to minimize changes elsewhere.` removed in Phase 2.

### [x] P10.3 — `r_policy.h` commented-out code wall

Addressed in Phase 2 (P2.4).

---

## Git Staging Notes

| Phase | Files staged | Status |
|-------|-------------|--------|
| 2 | `pagmoWrapper/managed_bridge.cpp`, `pagmoWrapper/problem.h`, `pagmoWrapper/multi_objective.h`, `pagmoWrapper/r_policy.h`, `pagmoWrapper/s_policy.h`, generated SWIG outputs | Committed |
| 3 | `swigInterfaceFileAndPagmoHeaders/pagmoSharpSwigInterface.i`, regenerated SWIG outputs, `docs/api-reference.md` | Committed |
| 4–7 | Director safety, docs, exception policy, algorithm/problem coverage | Committed |
| 8–9 | Naming (P8.1 all phases), structure (P9.1, P9.3), NC comments, SWIG regen | **Ready to commit** |
| 10 | No new files — all legacy cleanup done in earlier phases | **Ready to commit** |
