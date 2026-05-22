# pagmo4j — Java/Kotlin bindings for pagmo2

pagmo4j wraps the [pagmo2](https://esa.github.io/pagmo2/) C++ optimization library for
use from Java and Kotlin.  It exposes pagmo's full algorithm/problem/island/archipelago
API through JNI, with director-backed managed callbacks so you can define custom problems
and algorithms entirely in Java.

---

## Module layout

| Module | Description |
|--------|-------------|
| `core` | SWIG-generated JNI wrappers + handwritten Java glue (`IProblem`, `IAlgorithm`, `ManagedProblemBase`, migration policies, BFE utilities) |
| `kotlin-ext` | Kotlin extension functions and DSL helpers (`buildArchipelago`, `islandOf`, `batchFitness`, topology/policy sugar) |
| `examples` | Runnable Java and Kotlin usage examples |

---

## Quick start

### Define a custom problem

```java
import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.problems.ManagedProblemBase;

public class Sphere extends ManagedProblemBase {
    @Override
    public DoubleVector fitness(DoubleVector x) {
        double sum = 0;
        for (int i = 0; i < x.size(); i++) sum += x.get(i) * x.get(i);
        return vec(sum);
    }

    @Override
    public PairOfDoubleVectors get_bounds() {
        return bounds(new double[]{-5, -5}, new double[]{5, 5});
    }
}
```

### Optimize with a single island

```java
try (de algo = new de(100L);
     Sphere prob = new Sphere()) {

    island isl = island.create(algo, prob, 64);
    isl.evolve(1);
    isl.waitFor();
    System.out.println("Champion: " + isl.get_population().champion_f().get(0));
}
```

### Archipelago with topology (Kotlin DSL)

```kotlin
import io.github.samthegliderpilot.pagmo4j.ext.*

val archi = buildArchipelago {
    withTopology(ring())
    repeat(4) { pushBackIsland(de(100L), MyProblem(), popSize = 64) }
}
archi.run(rounds = 5)
println("Best: " + archi.bestChampionF().min())
```

### Custom migration policies

Use managed policies via `IRPolicy` / `ISPolicy`, or use the built-in pagmo policies
through the provided adapters (requires DLL rebuild to activate native delegation):

```java
import io.github.samthegliderpilot.pagmo4j.migration.*;

try (archipelago archi = new archipelago()) {
    archi.set_topology_ring(new ring());
    archi.pushBackIsland(new de(50L), new Sphere(),
        new FairReplaceAdapter(), new SelectBestAdapter(), 32, 42L);
    archi.pushBackIsland(new de(50L), new Sphere(),
        new FairReplaceAdapter(), new SelectBestAdapter(), 32, 43L);
    archi.evolve(3L);
    archi.waitFor();
}
```

---

## Running the tests

Tests require the native `pagmo4j` library to be built.  Point the
`PAGMO4J_NATIVE_DIR` environment variable at the directory containing `pagmo4j.dll`
(Windows) or `libpagmo4j.so` / `libpagmo4j.dylib` (Linux / macOS):

```powershell
$env:PAGMO4J_NATIVE_DIR = "C:\src\pagmoSharp\java\pagmoWrapper\win-build"
.\gradlew :core:test :kotlin-ext:test
```

## Running the examples

```powershell
$env:PAGMO4J_NATIVE_DIR = "C:\src\pagmoSharp\java\pagmoWrapper\win-build"
.\gradlew :examples:run
```

---

## Building the native library

```powershell
.\scripts\build-native.ps1
```

This compiles `pagmoWrapper/` with CMake and produces `pagmo4j.dll` /
`libpagmo4j.so` / `libpagmo4j.dylib` under `pagmoWrapper/win-build` (or the
platform equivalent).  Requires MSVC on Windows, GCC/Clang on Linux/macOS, and
`VCPKG_ROOT` pointing to a vcpkg installation with `pagmo2` installed.

---

## Package structure (core)

```
io.github.samthegliderpilot.pagmo4j
├── problems/
│   ├── IProblem.java            — user-defined problem interface
│   ├── ManagedProblemBase.java  — convenience base class
│   ├── IThreadCloneableProblem  — opt-in per-thread cloning
│   └── ExclusiveCloneAdapter    — internal thread-isolation wrapper
├── algorithms/
│   ├── IAlgorithm.java          — user-defined algorithm interface
│   ├── IAlgorithmLogLine.java   — typed log line contract
│   └── AlgorithmInterop.java    — normalises concrete algo → type-erased algorithm
├── migration/
│   ├── IRPolicy.java            — replacement policy interface
│   ├── ISPolicy.java            — selection policy interface
│   ├── RPolicyCallbackAdapter   — base for managed replacement policies
│   ├── SPolicyCallbackAdapter   — base for managed selection policies
│   ├── FairReplaceAdapter       — wraps pagmo's native fair_replace
│   └── SelectBestAdapter        — wraps pagmo's native select_best
├── batchevaluators/
│   ├── BfeBridge.java           — routes batch evaluation to the right backend
│   └── ManagedThreadBfe.java    — parallel BFE for cloneable problems
└── utils/
    └── GradientsAndHessians.java — finite-difference gradient/sparsity helpers
```
