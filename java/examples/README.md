# pagmo4j Examples

Runnable examples that teach both how to use pagmo4j APIs and why optimization structures
(islands, archipelagos, topology, policies) matter in practice.

## Run

From the `java/` directory:

```powershell
$env:PAGMO4J_NATIVE_DIR = "pagmoWrapper/win-build"
.\gradlew :examples:run --args="all"
```

Add `--verbose` (or `-v`) to print algorithm logs after each scenario:

```powershell
.\gradlew :examples:run --args="all --verbose"
```

## Scenarios

| Scenario | Description |
|----------|-------------|
| `single` | Baseline: one optimizer on one island. |
| `archipelago` | Topology intuition + multi-island vs single-island comparison. |
| `policies` | Default policy wiring vs managed `IRPolicy`/`ISPolicy` callbacks. |
| `cloning` | Non-thread-safe problem with `clone()` for parallel island search. |
| `kotlin` | Kotlin-idiomatic version using the `kotlin-ext` DSL extensions. |
| `all` | All scenarios in sequence (default). |

## What each scenario demonstrates

- **single**: `island.create(algo, problem, popSize, seed)` — the simplest path from problem to result.
- **archipelago**: `buildArchipelago {}` with `withTopology(ring())` — how topology affects exploration.
- **policies**: `IRPolicy` / `ISPolicy` callbacks — observe and customize migration behaviour.
- **cloning**: `IThreadCloneableProblem.clone()` — safely run a non-thread-safe problem in parallel.
- **kotlin**: The same scenarios using the Kotlin extension DSL (`buildArchipelago`, `islandOf`,
  `withTopology`, `pushBackIslandWithPolicies`, `bestChampionF`, etc.).
