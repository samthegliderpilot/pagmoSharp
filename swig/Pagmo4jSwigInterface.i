/* Pagmo4jSwigInterface.i — SWIG module for the Java/Kotlin pagmo4j bindings.
 *
 * Key differences from PagmoNETSwigInterface.i:
 *   - Uses -java SWIG backend instead of -csharp
 *   - cs* typemaps replaced with java* / jtype / jstype equivalents
 *   - No partial-class modifiers (Java has no partial classes)
 *   - Log injection and IProblem/IAlgorithm wiring done via %typemap(javacode) and
 *     %typemap(javainterfaces) instead of partial-class extension files
 *   - archipelago and island managed-problem injection done via %typemap(javacode)
 */

// The generated JNI class will be named "pagmo4jJNI" (module name + "JNI").
%module(naturalvar=1, directors="1") pagmo4j

// Inject the native library loader into pagmo4jJNI so the library is loaded
// before any JNI method is called.
%pragma(java) jniclasscode=%{
  static {
    io.github.samthegliderpilot.pagmo4j.NativeLoader.load();
  }
%}

// ── Java typemap for PairOfDoubleVectors (std::pair) ─────────────────────────
// Must appear before shared_core.i because %template(PairOfDoubleVectors) is
// there and SWIG applies javainterfaces/javacode at template instantiation time.
// Note: std::vector wrappers (DoubleVector etc.) use SWIG_STD_VECTOR_MINIMUM_INTERNAL
// which overrides javainterfaces, so those types do NOT implement AutoCloseable and
// cannot be used in try-with-resources — use try/finally or explicit .delete() instead.
%typemap(javainterfaces) std::pair<std::vector<double>, std::vector<double>> "AutoCloseable"
%typemap(javacode) std::pair<std::vector<double>, std::vector<double>> %{
    @Override public void close() { delete(); }
%}

// ── Shared core (exception handlers, renames, directors, templates) ───────────
%include "shared_core.i"

// ── Java-only rename: avoid conflict with java.lang.Object.wait() (final) ────
%rename(waitFor) pagmo::island::wait;
%rename(waitFor) pagmo::archipelago::wait;

// ── Pagmo enum and type-alias forward declarations ────────────────────────────
// Declaring these here tells SWIG they live in pagmo::, matching pagmo/types.hpp.
// Required before any sub-file %include that uses them without the namespace qualifier.
namespace pagmo {
    enum class thread_safety   { none, basic, constant };
    enum class evolve_status   { idle = 0, busy = 1, idle_error = 2, busy_error = 3 };
    enum class migration_type  { p2p, broadcast };
    enum class migrant_handling { preserve, evict };
    typedef std::vector<double> vector_double;
    typedef std::vector<std::pair<vector_double::size_type, vector_double::size_type>> sparsity_pattern;
}
// Make these accessible at global scope in the generated C++ JNI glue.
%{
    using sparsity_pattern = pagmo::sparsity_pattern;
    using vector_double    = pagmo::vector_double;
%}

// ── Extern-C bridge functions (pagmonet_*) ────────────────────────────────────
// Declared here so SWIG generates JNI wrappers callable as static methods on the
// pagmo4j module class.  In the Java backend, void* maps to long (jlong) automatically.
%typemap(jtype)   void * "long"
%typemap(jstype)  void * "long"
%typemap(javain)  void * "$javainput"
%typemap(javaout) void * { return $jnicall; }
extern void* pagmonet_problem_from_callback(void* callbackPtr);
extern void* pagmonet_algorithm_from_callback(void* callbackPtr);
extern void* pagmonet_algorithm_from_callback_java(void* callbackPtr);
extern void  pagmonet_problem_delete(void* problemPtr);
extern const char* pagmonet_get_last_error();
extern void* pagmonet_default_bfe_evaluate(void* problemPtr, void* batchXPtr);
extern void* pagmonet_population_new(void* problemPtr, long popSize, unsigned int seed);
extern void* pagmonet_estimate_gradient_problem(void* problemPtr, void* xPtr, double dx);
extern void* pagmonet_estimate_gradient_h_problem(void* problemPtr, void* xPtr, double dx);
extern void* pagmonet_estimate_sparsity_problem(void* problemPtr, void* xPtr, double dx);

// ── Java typemaps for unsigned integer types ──────────────────────────────────
// unsigned int → long (fits in signed long; used for seeds and population sizes)
%typemap(jtype)   unsigned int         "long"
%typemap(jstype)  unsigned int         "long"
%typemap(jtype)   const unsigned int & "long"
%typemap(jstype)  const unsigned int & "long"
%typemap(javain)  unsigned int         "$javainput"
%typemap(javaout) unsigned int         { return $jnicall; }

// unsigned long long is left as BigInteger (SWIG default).
// ULongLongVector.get()/set() require AbstractList<BigInteger> — mapping to primitive
// long causes compile-time type mismatches in the vector template.
// Injected javacode uses .longValue() where a primitive long is needed.

// ── AutoCloseable for resource-owning types ───────────────────────────────────
%typemap(javainterfaces) pagmo::archipelago        "AutoCloseable"
%typemap(javainterfaces) pagmo::island             "AutoCloseable"
%typemap(javainterfaces) pagmo::problem            "AutoCloseable"
%typemap(javainterfaces) pagmo::population         "AutoCloseable"
%typemap(javainterfaces) pagmo::default_bfe        "AutoCloseable"
%typemap(javainterfaces) pagmo::thread_bfe         "AutoCloseable"
%typemap(javainterfaces) pagmo::member_bfe         "AutoCloseable"
%typemap(javainterfaces) pagmo::algorithm          "AutoCloseable"
%typemap(javainterfaces) pagmo::hypervolume        "AutoCloseable"
%typemap(javainterfaces) pagmo::topology           "AutoCloseable"
%typemap(javainterfaces) pagmo::fully_connected    "AutoCloseable"
%typemap(javainterfaces) pagmo::ring               "AutoCloseable"
%typemap(javainterfaces) pagmo::free_form          "AutoCloseable"

// ── IAlgorithm implementations ────────────────────────────────────────────────
%typemap(javainterfaces) pagmo::de                 "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::de1220             "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::pso                "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::pso_gen            "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::sade               "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::bee_colony         "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::cmaes              "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::compass_search     "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::cstrs_self_adaptive "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::gaco               "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::gwo                "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::ihs                "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::maco               "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::mbh                "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::moead              "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::moead_gen          "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::nsga2              "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::nspso              "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::null_algorithm     "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::sea                "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::sga                "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::simulated_annealing "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::xnes               "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::nlopt              "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"
%typemap(javainterfaces) pagmo::ipopt              "io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm, AutoCloseable"

// ── javaimports: sub-package imports for generated classes ────────────────────
%typemap(javaimports) pagmo::archipelago      "import io.github.samthegliderpilot.pagmo4j.problems.*; import io.github.samthegliderpilot.pagmo4j.algorithms.*; import io.github.samthegliderpilot.pagmo4j.batchevaluators.*; import io.github.samthegliderpilot.pagmo4j.migration.*;"
%typemap(javaimports) pagmo::island           "import io.github.samthegliderpilot.pagmo4j.problems.*; import io.github.samthegliderpilot.pagmo4j.algorithms.*; import io.github.samthegliderpilot.pagmo4j.batchevaluators.*;"
%typemap(javaimports) pagmo::algorithm        "import io.github.samthegliderpilot.pagmo4j.problems.*; import io.github.samthegliderpilot.pagmo4j.algorithms.*; import io.github.samthegliderpilot.pagmo4j.batchevaluators.*;"
%typemap(javaimports) pagmo::problem          "import io.github.samthegliderpilot.pagmo4j.problems.*;"
%typemap(javaimports) pagmo::population       "import io.github.samthegliderpilot.pagmo4j.problems.*;"
%typemap(javaimports) pagmo::default_bfe      "import io.github.samthegliderpilot.pagmo4j.batchevaluators.*; import io.github.samthegliderpilot.pagmo4j.problems.*;"
%typemap(javaimports) pagmo::thread_bfe       "import io.github.samthegliderpilot.pagmo4j.batchevaluators.*; import io.github.samthegliderpilot.pagmo4j.problems.*;"
%typemap(javaimports) pagmo::member_bfe       "import io.github.samthegliderpilot.pagmo4j.batchevaluators.*; import io.github.samthegliderpilot.pagmo4j.problems.*;"
%typemap(javaimports) pagmo::de               "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::pso              "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::cmaes            "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::simulated_annealing "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::compass_search   "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::ihs              "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::mbh              "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::sade             "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::bee_colony       "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::cstrs_self_adaptive "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::de1220           "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::gaco             "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::gwo              "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::maco             "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::moead            "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::moead_gen        "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::nsga2            "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::nspso            "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::null_algorithm   "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::pso_gen          "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::sea              "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::sga              "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::xnes             "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::nlopt            "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"
%typemap(javaimports) pagmo::ipopt            "import io.github.samthegliderpilot.pagmo4j.algorithms.*;"

// ── Log projection injection ──────────────────────────────────────────────────
%typemap(javacode) pagmo::bee_colony %{
    public java.util.List<BeeColonyLine> getTypedLogLines() {
        BeeColonyLogLineVector raw = get_log_lines();
        try {
            java.util.List<BeeColonyLine> out = new java.util.ArrayList<>((int) raw.size());
            for (int i = 0; i < (int) raw.size(); i++) {
                BeeColonyLogLine e = raw.get(i);
                try { out.add(new BeeColonyLine(e.getGen(), e.getFevals().longValue(),
                              e.getBest(), e.getCur_best())); }
                finally { e.delete(); }
            }
            return out;
        } finally { raw.delete(); }
    }
    @Override public java.util.List<IAlgorithmLogLine> getLogLines() {
        return new java.util.ArrayList<>(getTypedLogLines());
    }
    @Override public void close() { delete(); }

    public record BeeColonyLine(long generation, long functionEvaluations,
                                double bestFitness, double currentBest)
            implements IAlgorithmLogLine {
        @Override public String getAlgorithmName() { return "bee_colony"; }
        @Override public java.util.Map<String, Object> getRawFields() {
            return java.util.Map.of("generation", generation, "function_evaluations", functionEvaluations,
                "best_fitness", bestFitness, "current_best", currentBest);
        }
        @Override public String toDisplayString() {
            return "gen=" + generation + ", fevals=" + functionEvaluations +
                   ", best=" + bestFitness + ", cur_best=" + currentBest;
        }
    }
%}

%typemap(javacode) pagmo::de %{
    public java.util.List<DeLogLine> getTypedLogLines() {
        DeLogEntryVector raw = get_log_entries();
        try {
            java.util.List<DeLogLine> out = new java.util.ArrayList<>((int) raw.size());
            for (int i = 0; i < (int) raw.size(); i++) {
                DeLogEntry e = raw.get(i);
                try { out.add(new DeLogLine(e.getGen(), e.getFevals().longValue(), e.getBest(), e.getFeval_difference(), e.getDx())); }
                finally { e.delete(); }
            }
            return out;
        } finally { raw.delete(); }
    }
    @Override public java.util.List<IAlgorithmLogLine> getLogLines() {
        return new java.util.ArrayList<>(getTypedLogLines());
    }
    @Override public void close() { delete(); }

    public record DeLogLine(long generation, long functionEvaluations, double bestFitness,
                            double functionEvaluationDifference, double dx)
            implements IAlgorithmLogLine {
        @Override public String getAlgorithmName() { return "de"; }
        @Override public java.util.Map<String, Object> getRawFields() {
            return java.util.Map.of(
                "generation", generation, "function_evaluations", functionEvaluations,
                "best_fitness", bestFitness, "dx", dx);
        }
        @Override public String toDisplayString() {
            return "gen=" + generation + ", fevals=" + functionEvaluations +
                   ", best=" + bestFitness + ", df=" + functionEvaluationDifference + ", dx=" + dx;
        }
    }
%}

%typemap(javacode) pagmo::pso %{
    public java.util.List<PsoLogLine> getTypedLogLines() {
        PsoLogEntryVector raw = get_log_entries();
        try {
            java.util.List<PsoLogLine> out = new java.util.ArrayList<>((int) raw.size());
            for (int i = 0; i < (int) raw.size(); i++) {
                PsoLogEntry e = raw.get(i);
                try { out.add(new PsoLogLine(e.getGen(), e.getFevals().longValue(), e.getBest(), e.getInertia(), e.getCognitive(), e.getSocial())); }
                finally { e.delete(); }
            }
            return out;
        } finally { raw.delete(); }
    }
    @Override public java.util.List<IAlgorithmLogLine> getLogLines() {
        return new java.util.ArrayList<>(getTypedLogLines());
    }
    @Override public void close() { delete(); }

    public record PsoLogLine(long generation, long functionEvaluations,
                             double bestFitness, double inertia, double cognitive, double social)
            implements IAlgorithmLogLine {
        @Override public String getAlgorithmName() { return "pso"; }
        @Override public java.util.Map<String, Object> getRawFields() {
            return java.util.Map.of(
                "generation", generation, "function_evaluations", functionEvaluations,
                "best_fitness", bestFitness, "inertia", inertia);
        }
        @Override public String toDisplayString() {
            return "gen=" + generation + ", fevals=" + functionEvaluations +
                   ", best=" + bestFitness + ", inertia=" + inertia;
        }
    }
%}

%define PAGMO4J_SIMPLE_ALGO_CODE(ALGO)
%typemap(javacode) pagmo::ALGO %{
    @Override public java.util.List<IAlgorithmLogLine> getLogLines() {
        return java.util.Collections.emptyList();
    }
    @Override public void close() { delete(); }
%}
%enddef

%typemap(javacode) pagmo::cmaes %{
    public java.util.List<CmaesLogLine> getTypedLogLines() {
        CmaesLogEntryVector raw = get_log_entries();
        try {
            java.util.List<CmaesLogLine> out = new java.util.ArrayList<>((int) raw.size());
            for (int i = 0; i < (int) raw.size(); i++) {
                CmaesLogEntry e = raw.get(i);
                try { out.add(new CmaesLogLine(e.getGen(), e.getFevals().longValue(), e.getBest(), e.getSigma(), e.getMin_variance(), e.getMax_variance())); }
                finally { e.delete(); }
            }
            return out;
        } finally { raw.delete(); }
    }
    @Override public java.util.List<IAlgorithmLogLine> getLogLines() {
        return new java.util.ArrayList<>(getTypedLogLines());
    }
    @Override public void close() { delete(); }

    public record CmaesLogLine(long generation, long functionEvaluations, double bestFitness,
                               double sigma, double minVariance, double maxVariance)
            implements IAlgorithmLogLine {
        @Override public String getAlgorithmName() { return "cmaes"; }
        @Override public java.util.Map<String, Object> getRawFields() {
            return java.util.Map.of("generation", generation, "function_evaluations", functionEvaluations,
                "best_fitness", bestFitness, "sigma", sigma);
        }
        @Override public String toDisplayString() {
            return "gen=" + generation + ", fevals=" + functionEvaluations +
                   ", best=" + bestFitness + ", sigma=" + sigma;
        }
    }
%}

%typemap(javacode) pagmo::simulated_annealing %{
    public java.util.List<SALogLine> getTypedLogLines() {
        SimulatedAnnealingLogEntryVector raw = get_log_entries();
        try {
            java.util.List<SALogLine> out = new java.util.ArrayList<>((int) raw.size());
            for (int i = 0; i < (int) raw.size(); i++) {
                SimulatedAnnealingLogEntry e = raw.get(i);
                try { out.add(new SALogLine(e.getFevals().longValue(), e.getBest(), e.getCurrent(), e.getTemperature())); }
                finally { e.delete(); }
            }
            return out;
        } finally { raw.delete(); }
    }
    @Override public java.util.List<IAlgorithmLogLine> getLogLines() {
        return new java.util.ArrayList<>(getTypedLogLines());
    }
    @Override public void close() { delete(); }

    public record SALogLine(long functionEvaluations, double bestFitness,
                            double currentFitness, double temperature)
            implements IAlgorithmLogLine {
        @Override public String getAlgorithmName() { return "simulated_annealing"; }
        @Override public java.util.Map<String, Object> getRawFields() {
            return java.util.Map.of("function_evaluations", functionEvaluations,
                "best_fitness", bestFitness, "temperature", temperature);
        }
        @Override public String toDisplayString() {
            return "fevals=" + functionEvaluations + ", best=" + bestFitness + ", T=" + temperature;
        }
    }
%}

%typemap(javacode) pagmo::compass_search %{
    public java.util.List<CSLogLine> getTypedLogLines() {
        CompassSearchLogEntryVector raw = get_log_entries();
        try {
            java.util.List<CSLogLine> out = new java.util.ArrayList<>((int) raw.size());
            for (int i = 0; i < (int) raw.size(); i++) {
                CompassSearchLogEntry e = raw.get(i);
                try { out.add(new CSLogLine(e.getFevals().longValue(), e.getBest(), e.getRange())); }
                finally { e.delete(); }
            }
            return out;
        } finally { raw.delete(); }
    }
    @Override public java.util.List<IAlgorithmLogLine> getLogLines() {
        return new java.util.ArrayList<>(getTypedLogLines());
    }
    @Override public void close() { delete(); }

    public record CSLogLine(long functionEvaluations, double bestFitness, double range)
            implements IAlgorithmLogLine {
        @Override public String getAlgorithmName() { return "compass_search"; }
        @Override public java.util.Map<String, Object> getRawFields() {
            return java.util.Map.of("function_evaluations", functionEvaluations,
                "best_fitness", bestFitness, "range", range);
        }
        @Override public String toDisplayString() {
            return "fevals=" + functionEvaluations + ", best=" + bestFitness + ", range=" + range;
        }
    }
%}

%typemap(javacode) pagmo::ihs %{
    public java.util.List<IhsLogLine> getTypedLogLines() {
        IhsLogEntryVector raw = get_log_entries();
        try {
            java.util.List<IhsLogLine> out = new java.util.ArrayList<>((int) raw.size());
            for (int i = 0; i < (int) raw.size(); i++) {
                IhsLogEntry e = raw.get(i);
                try { out.add(new IhsLogLine(e.getFevals().longValue(), e.getPpar(), e.getBw(), e.getDx(), e.getDf())); }
                finally { e.delete(); }
            }
            return out;
        } finally { raw.delete(); }
    }
    @Override public java.util.List<IAlgorithmLogLine> getLogLines() {
        return new java.util.ArrayList<>(getTypedLogLines());
    }
    @Override public void close() { delete(); }

    public record IhsLogLine(long functionEvaluations, double pitchAdjustmentRate,
                             double bandwidth, double dx, double df)
            implements IAlgorithmLogLine {
        @Override public String getAlgorithmName() { return "ihs"; }
        @Override public java.util.Map<String, Object> getRawFields() {
            return java.util.Map.of("function_evaluations", functionEvaluations,
                "pitch_adjustment_rate", pitchAdjustmentRate, "bandwidth", bandwidth, "dx", dx, "df", df);
        }
        @Override public String toDisplayString() {
            return "fevals=" + functionEvaluations + ", ppar=" + pitchAdjustmentRate + ", bw=" + bandwidth;
        }
    }
%}

%typemap(javacode) pagmo::mbh %{
    public java.util.List<MbhLogLine> getTypedLogLines() {
        MbhLogEntryVector raw = get_log_entries();
        try {
            java.util.List<MbhLogLine> out = new java.util.ArrayList<>((int) raw.size());
            for (int i = 0; i < (int) raw.size(); i++) {
                MbhLogEntry e = raw.get(i);
                try { out.add(new MbhLogLine(e.getFevals().longValue(), e.getBest(), e.getTrial())); }
                finally { e.delete(); }
            }
            return out;
        } finally { raw.delete(); }
    }
    @Override public java.util.List<IAlgorithmLogLine> getLogLines() {
        return new java.util.ArrayList<>(getTypedLogLines());
    }
    @Override public void close() { delete(); }

    public record MbhLogLine(long functionEvaluations, double bestFitness, long trials)
            implements IAlgorithmLogLine {
        @Override public String getAlgorithmName() { return "mbh"; }
        @Override public java.util.Map<String, Object> getRawFields() {
            return java.util.Map.of("function_evaluations", functionEvaluations,
                "best_fitness", bestFitness, "trials", trials);
        }
        @Override public String toDisplayString() {
            return "fevals=" + functionEvaluations + ", best=" + bestFitness + ", trials=" + trials;
        }
    }
%}

// Algorithms with no structured log output
PAGMO4J_SIMPLE_ALGO_CODE(cstrs_self_adaptive)
PAGMO4J_SIMPLE_ALGO_CODE(de1220)
PAGMO4J_SIMPLE_ALGO_CODE(gaco)
PAGMO4J_SIMPLE_ALGO_CODE(gwo)
PAGMO4J_SIMPLE_ALGO_CODE(maco)
PAGMO4J_SIMPLE_ALGO_CODE(moead)
PAGMO4J_SIMPLE_ALGO_CODE(moead_gen)
PAGMO4J_SIMPLE_ALGO_CODE(nsga2)
PAGMO4J_SIMPLE_ALGO_CODE(nspso)
PAGMO4J_SIMPLE_ALGO_CODE(null_algorithm)
PAGMO4J_SIMPLE_ALGO_CODE(pso_gen)
PAGMO4J_SIMPLE_ALGO_CODE(xnes)
PAGMO4J_SIMPLE_ALGO_CODE(nlopt)
PAGMO4J_SIMPLE_ALGO_CODE(ipopt)

// ── sade/sea/sga typed log projections ───────────────────────────────────────
// Indexed accessors avoid the full std::vector<> JNI wrapper infrastructure.
// The corresponding JNI functions are hand-written in pagmo4j_wrap.cxx.
%extend pagmo::sade {
    int get_log_entry_count() const { return (int)$self->get_log().size(); }
    pagmoWrap::SadeLogEntry get_log_entry(int idx) const {
        const auto& line = $self->get_log().at((std::size_t)idx);
        return {std::get<0>(line), std::get<1>(line), std::get<2>(line),
                std::get<3>(line), std::get<4>(line), std::get<5>(line), std::get<6>(line)};
    }
}
%extend pagmo::sea {
    int get_log_entry_count() const { return (int)$self->get_log().size(); }
    pagmoWrap::SeaLogEntry get_log_entry(int idx) const {
        const auto& line = $self->get_log().at((std::size_t)idx);
        return {std::get<0>(line), std::get<1>(line), std::get<2>(line),
                std::get<3>(line), static_cast<unsigned long long>(std::get<4>(line))};
    }
}
%extend pagmo::sga {
    int get_log_entry_count() const { return (int)$self->get_log().size(); }
    pagmoWrap::SgaLogEntry get_log_entry(int idx) const {
        const auto& line = $self->get_log().at((std::size_t)idx);
        return {std::get<0>(line), std::get<1>(line), std::get<2>(line), std::get<3>(line)};
    }
}

%typemap(javacode) pagmo::sade %{
    public java.util.List<SadeLogLine> getTypedLogLines() {
        int count = get_log_entry_count();
        java.util.List<SadeLogLine> out = new java.util.ArrayList<>(count);
        for (int i = 0; i < count; i++) {
            SadeLogEntry e = get_log_entry(i);
            try { out.add(new SadeLogLine(e.getGen(), e.getFevals().longValue(),
                          e.getBest(), e.getF(), e.getCr(), e.getDx(), e.getDf())); }
            finally { e.delete(); }
        }
        return out;
    }
    @Override public java.util.List<IAlgorithmLogLine> getLogLines() {
        return new java.util.ArrayList<>(getTypedLogLines());
    }
    @Override public void close() { delete(); }

    public record SadeLogLine(long generation, long functionEvaluations, double bestFitness,
                              double f, double cr, double dx, double df)
            implements IAlgorithmLogLine {
        @Override public String getAlgorithmName() { return "sade"; }
        @Override public java.util.Map<String, Object> getRawFields() {
            return java.util.Map.of("generation", generation, "function_evaluations", functionEvaluations,
                "best_fitness", bestFitness, "f", f, "cr", cr, "dx", dx, "df", df);
        }
        @Override public String toDisplayString() {
            return "gen=" + generation + ", fevals=" + functionEvaluations +
                   ", best=" + bestFitness + ", f=" + f + ", cr=" + cr +
                   ", dx=" + dx + ", df=" + df;
        }
    }
%}

%typemap(javacode) pagmo::sea %{
    public java.util.List<SeaLogLine> getTypedLogLines() {
        int count = get_log_entry_count();
        java.util.List<SeaLogLine> out = new java.util.ArrayList<>(count);
        for (int i = 0; i < count; i++) {
            SeaLogEntry e = get_log_entry(i);
            try { out.add(new SeaLogLine(e.getGen(), e.getFevals().longValue(),
                          e.getBest(), e.getImprovement(), e.getOffspring_evals().longValue())); }
            finally { e.delete(); }
        }
        return out;
    }
    @Override public java.util.List<IAlgorithmLogLine> getLogLines() {
        return new java.util.ArrayList<>(getTypedLogLines());
    }
    @Override public void close() { delete(); }

    public record SeaLogLine(long generation, long functionEvaluations, double bestFitness,
                             double improvement, long offspringEvaluations)
            implements IAlgorithmLogLine {
        @Override public String getAlgorithmName() { return "sea"; }
        @Override public java.util.Map<String, Object> getRawFields() {
            return java.util.Map.of("generation", generation, "function_evaluations", functionEvaluations,
                "best_fitness", bestFitness, "improvement", improvement,
                "offspring_evaluations", offspringEvaluations);
        }
        @Override public String toDisplayString() {
            return "gen=" + generation + ", fevals=" + functionEvaluations +
                   ", best=" + bestFitness + ", improvement=" + improvement +
                   ", offspring_evals=" + offspringEvaluations;
        }
    }
%}

%typemap(javacode) pagmo::sga %{
    public java.util.List<SgaLogLine> getTypedLogLines() {
        int count = get_log_entry_count();
        java.util.List<SgaLogLine> out = new java.util.ArrayList<>(count);
        for (int i = 0; i < count; i++) {
            SgaLogEntry e = get_log_entry(i);
            try { out.add(new SgaLogLine(e.getGen(), e.getFevals().longValue(),
                          e.getBest(), e.getImprovement())); }
            finally { e.delete(); }
        }
        return out;
    }
    @Override public java.util.List<IAlgorithmLogLine> getLogLines() {
        return new java.util.ArrayList<>(getTypedLogLines());
    }
    @Override public void close() { delete(); }

    public record SgaLogLine(long generation, long functionEvaluations,
                             double bestFitness, double improvement)
            implements IAlgorithmLogLine {
        @Override public String getAlgorithmName() { return "sga"; }
        @Override public java.util.Map<String, Object> getRawFields() {
            return java.util.Map.of("generation", generation, "function_evaluations", functionEvaluations,
                "best_fitness", bestFitness, "improvement", improvement);
        }
        @Override public String toDisplayString() {
            return "gen=" + generation + ", fevals=" + functionEvaluations +
                   ", best=" + bestFitness + ", improvement=" + improvement;
        }
    }
%}

%typemap(javacode) pagmo::algorithm %{
    public algorithm(IAlgorithm managed) {
        this(NativeInterop.createAlgorithmPointer(managed), true);
    }
    @Override public void close() { delete(); }
%}

// ── problem: IProblem constructor + gradient/sparsity helpers ─────────────────
%typemap(javacode) pagmo::problem %{
    public problem(IProblem managed) {
        this(NativeInterop.createProblemPointer(managed), true);
    }

    /** Estimates gradient sparsity by finite differencing. */
    public SparsityPattern estimateSparsity(DoubleVector x, double dx) {
        long ptr = pagmo4j.pagmonet_estimate_sparsity_problem(
            problem.getCPtr(this), DoubleVector.getCPtr(x), dx);
        if (ptr == 0) throw new RuntimeException("Native estimate_sparsity() failed.");
        return new SparsityPattern(ptr, true);
    }

    /** Estimates gradient by forward finite differencing. */
    public DoubleVector estimateGradient(DoubleVector x, double dx) {
        long ptr = pagmo4j.pagmonet_estimate_gradient_problem(
            problem.getCPtr(this), DoubleVector.getCPtr(x), dx);
        if (ptr == 0) throw new RuntimeException("Native estimate_gradient() failed.");
        return new DoubleVector(ptr, true);
    }

    /** Estimates gradient by higher-order finite differencing. */
    public DoubleVector estimateGradientHighOrder(DoubleVector x, double dx) {
        long ptr = pagmo4j.pagmonet_estimate_gradient_h_problem(
            problem.getCPtr(this), DoubleVector.getCPtr(x), dx);
        if (ptr == 0) throw new RuntimeException("Native estimate_gradient_h() failed.");
        return new DoubleVector(ptr, true);
    }

    @Override public void close() { delete(); }
%}

// ── archipelago: managed-problem wiring ──────────────────────────────────────
%typemap(javacode) pagmo::archipelago %{
    private final java.util.List<IProblem> managedProblemCloneRoots = new java.util.ArrayList<>();

    private long withManagedProblem(IProblem prob, java.util.function.Function<problem, Long> action) {
        if (prob == null) throw new NullPointerException("problem");
        if (prob.get_thread_safety() == ThreadSafety.None && prob instanceof IThreadCloneableProblem) {
            IProblem clone = ((IThreadCloneableProblem) prob).clone();
            if (clone != null) {
                if (clone == prob)
                    throw new IllegalStateException("'" + prob.get_name() +
                        ".clone()' returned the same instance.");
                ExclusiveCloneAdapter adapter = new ExclusiveCloneAdapter(clone);
                managedProblemCloneRoots.add(adapter);
                try (problem wrapped = new problem(adapter)) {
                    return action.apply(wrapped);
                }
            }
        }
        prob.throwIfNotThreadSafe();
        try (problem wrapped = new problem(prob)) {
            return action.apply(wrapped);
        }
    }

    public long pushBackIsland(algorithm algo, IProblem problem, long popSize, long seed) {
        return withManagedProblem(problem, wrapped -> {
            long nativePop = SizeTInterop.toNativeUInt32(popSize, "popSize");
            return push_back_island(algo, wrapped, nativePop, seed);
        });
    }

    public long pushBackIsland(IAlgorithm algo, IProblem problem, long popSize, long seed) {
        try (algorithm normalized = AlgorithmInterop.normalizeToTypeErased(algo)) {
            return pushBackIsland(normalized, problem, popSize, seed);
        }
    }

    public long pushBackIsland(algorithm algo, IProblem problem,
            IRPolicy rPolicy, ISPolicy sPolicy, long popSize, long seed) {
        if (rPolicy == null) throw new NullPointerException("rPolicy");
        if (sPolicy == null) throw new NullPointerException("sPolicy");
        return withManagedProblem(problem, wrapped -> {
            long nativePop = SizeTInterop.toNativeUInt32(popSize, "popSize");
            RPolicyCallbackAdapter rAdapter = wrapRPolicy(rPolicy);
            SPolicyCallbackAdapter sAdapter = wrapSPolicy(sPolicy);
            policyCallbackRoots.add(rAdapter);
            policyCallbackRoots.add(sAdapter);
            return push_back_island(algo, wrapped,
                new ManagedRPolicy(rAdapter), new ManagedSPolicy(sAdapter), nativePop, seed);
        });
    }

    public long pushBackIsland(IAlgorithm algo, IProblem problem,
            IRPolicy rPolicy, ISPolicy sPolicy, long popSize, long seed) {
        try (algorithm normalized = AlgorithmInterop.normalizeToTypeErased(algo)) {
            return pushBackIsland(normalized, problem, rPolicy, sPolicy, popSize, seed);
        }
    }

    public long pushBackIsland(algorithm algo, IProblem problem, bfe b, long popSize, long seed) {
        return withManagedProblem(problem, wrapped -> {
            long nativePop = SizeTInterop.toNativeUInt32(popSize, "popSize");
            return push_back_island(algo, wrapped, b, nativePop, seed);
        });
    }

    public long pushBackIsland(IAlgorithm algo, IProblem problem, bfe b, long popSize, long seed) {
        try (algorithm normalized = AlgorithmInterop.normalizeToTypeErased(algo)) {
            return pushBackIsland(normalized, problem, b, popSize, seed);
        }
    }

    private final java.util.List<Object> policyCallbackRoots = new java.util.ArrayList<>();

    private static RPolicyCallbackAdapter wrapRPolicy(IRPolicy rPolicy) {
        RPolicyCallbackAdapter adapter = new RPolicyCallbackAdapter() {
            @Override public IndividualsGroup replace(
                    IndividualsGroup incoming, long n_f, long n_ec, long n_ic, long n_obj,
                    long pop_size, DoubleVector tol, IndividualsGroup current) {
                return rPolicy.replace(incoming, n_f, n_ec, n_ic, n_obj, pop_size, tol, current);
            }
            @Override public String get_name() { return rPolicy.get_name(); }
        };
        adapter.swigReleaseOwnership();
        return adapter;
    }

    private static SPolicyCallbackAdapter wrapSPolicy(ISPolicy sPolicy) {
        SPolicyCallbackAdapter adapter = new SPolicyCallbackAdapter() {
            @Override public IndividualsGroup select(
                    IndividualsGroup population, long n_f, long n_ec, long n_ic, long n_obj,
                    long pop_size, DoubleVector tol) {
                return sPolicy.select(population, n_f, n_ec, n_ic, n_obj, pop_size, tol);
            }
            @Override public String get_name() { return sPolicy.get_name(); }
        };
        adapter.swigReleaseOwnership();
        return adapter;
    }

    public void waitCheck() { wait_check(); }

    @Override public void close() { delete(); }
%}

// ── island: managed-problem factory ──────────────────────────────────────────
%typemap(javacode) pagmo::island %{
    private static final java.util.concurrent.ConcurrentHashMap<Long, java.util.List<Object>>
        constructionRoots = new java.util.concurrent.ConcurrentHashMap<>();

    private static void attachRoot(island owner, Object root) {
        if (owner == null || root == null) return;
        constructionRoots.computeIfAbsent(island.getCPtr(owner),
            k -> new java.util.ArrayList<>()).add(root);
    }

    public static island create(algorithm algo, IProblem problem, long popSize) {
        return create(algo, problem, popSize, new random_device().next());
    }

    public static island create(algorithm algo, IProblem problem, long popSize, long seed) {
        problem wrapped = new problem(problem);
        long nativePop = SizeTInterop.toNativeUInt32(popSize, "popSize");
        island isl = island.Create(algo, wrapped, nativePop, seed);
        attachRoot(isl, wrapped);
        return isl;
    }

    public static island create(IAlgorithm algo, IProblem problem, long popSize) {
        return create(algo, problem, popSize, new random_device().next());
    }

    public static island create(IAlgorithm algo, IProblem problem, long popSize, long seed) {
        try (algorithm normalized = AlgorithmInterop.normalizeToTypeErased(algo)) {
            return create(normalized, problem, popSize, seed);
        }
    }

    public static island create(algorithm algo, IProblem problem, bfe b, long popSize, long seed) {
        problem wrapped = new problem(problem);
        long nativePop = SizeTInterop.toNativeUInt32(popSize, "popSize");
        island isl = island.Create(algo, wrapped, b, nativePop, seed);
        attachRoot(isl, wrapped);
        return isl;
    }

    public static island create(IAlgorithm algo, IProblem problem, bfe b, long popSize, long seed) {
        try (algorithm normalized = AlgorithmInterop.normalizeToTypeErased(algo)) {
            return create(normalized, problem, b, popSize, seed);
        }
    }

    public void waitCheck() { wait_check(); }

    @Override public void close() { delete(); }
%}

%typemap(javacode) pagmo::population %{
    /** Creates a population for a managed problem using a random seed. */
    public population(IProblem problem, long popSize) {
        this(problem, popSize, new random_device().next());
    }

    /** Creates a population for a managed problem with an explicit seed. */
    public population(IProblem problem, long popSize, long seed) {
        this(createFromManagedProblem(problem, popSize, seed), true);
    }

    private static long createFromManagedProblem(IProblem problem, long popSize, long seed) {
        if (problem == null) throw new NullPointerException("problem");
        long nativePop = SizeTInterop.toNativeUInt32(popSize, "popSize");
        long problemPtr = NativeInterop.createProblemPointer(problem);
        try {
            // nativePop: SWIG maps C long → Java int on Windows (32-bit C long)
            long popPtr = pagmo4j.pagmonet_population_new(problemPtr, (int) nativePop, seed);
            if (popPtr == 0) throw new RuntimeException("Failed to create native pagmo::population.");
            return popPtr;
        } finally {
            pagmo4j.pagmonet_problem_delete(problemPtr);
        }
    }

    @Override public void close() { delete(); }
%}

// ── BFE operator helpers ──────────────────────────────────────────────────────
%typemap(javacode) pagmo::default_bfe %{
    public DoubleVector operator(IProblem problem, DoubleVector batchX) {
        return BfeBridge.batchEvaluate(problem, batchX, false);
    }
    @Override public void close() { delete(); }
%}

%typemap(javacode) pagmo::thread_bfe %{
    public DoubleVector operator(IProblem problem, DoubleVector batchX) {
        return BfeBridge.batchEvaluate(problem, batchX, true);
    }
    @Override public void close() { delete(); }
%}

%typemap(javacode) pagmo::member_bfe %{
    public DoubleVector operator(IProblem problem, DoubleVector batchX) {
        return BfeBridge.batchEvaluate(problem, batchX, false);
    }
    @Override public void close() { delete(); }
%}

%typemap(javacode) pagmo::topology %{ @Override public void close() { delete(); } %}
%typemap(javacode) pagmo::hypervolume %{ @Override public void close() { delete(); } %}
%typemap(javacode) pagmo::fully_connected %{ @Override public void close() { delete(); } %}
%typemap(javacode) pagmo::ring %{ @Override public void close() { delete(); } %}
%typemap(javacode) pagmo::free_form %{ @Override public void close() { delete(); } %}

// ── Sub-module includes ───────────────────────────────────────────────────────
%include "swigInterfaceFiles/problem.i"
%include "swigInterfaceFiles/algorithm.i"
namespace pagmo { %include "swigInterfaceFiles/topology.i" }
namespace pagmo {
    %include "swigInterfaceFiles/population.i"
}
// Now that pagmo::population is known, include algorithm_callback.h so the director
// generates evolve(population) instead of evolve(SWIGTYPE_p_pagmo__population).
%include "pagmoWrapper/algorithm_callback.h"
%include "swigInterfaceFiles/island.i"
%include "swigInterfaceFiles/archipelago.i"
namespace pagmo {
    %include "swigInterfaceFiles/bfe.i"
}
namespace pagmo {
    %include "swigInterfaceFiles/rng.i"
    %include "swigInterfaceFiles/io.i"
}
%include "swigInterfaceFiles/utils/hypervolume.i"
%include "swigInterfaceFiles/utils/multi_objective.i"
// gradients_and_hessians.i excluded — pagmo C++ functions take a callback function
// type SWIG can't map to Java.  GradientsAndHessians.java uses the pagmonet_estimate_*
// bridge functions instead.

%include "swigInterfaceFiles/algorithms/bee_colony.i"
%include "swigInterfaceFiles/algorithms/cstrs_self_adaptive.i"
%include "swigInterfaceFiles/algorithms/ihs.i"
%include "swigInterfaceFiles/algorithms/maco.i"
%include "swigInterfaceFiles/algorithms/mbh.i"
%include "swigInterfaceFiles/algorithms/moead.i"
%include "swigInterfaceFiles/algorithms/moead_gen.i"
%include "swigInterfaceFiles/algorithms/nsga2.i"

namespace pagmo {
    %include "swigInterfaceFiles/algorithms/cmaes.i"
    %include "swigInterfaceFiles/algorithms/compass_search.i"
    %include "swigInterfaceFiles/algorithms/de.i"
    %include "swigInterfaceFiles/algorithms/de1220.i"
    %include "swigInterfaceFiles/algorithms/gaco.i"
    %include "swigInterfaceFiles/algorithms/gwo.i"
    %include "swigInterfaceFiles/algorithms/ipopt.i"
    %include "swigInterfaceFiles/algorithms/nlopt.i"
    %include "swigInterfaceFiles/algorithms/not_population_based.i"
    %include "swigInterfaceFiles/algorithms/nspso.i"
    %include "swigInterfaceFiles/algorithms/null_algorithm.i"
    %include "swigInterfaceFiles/algorithms/pso.i"
    %include "swigInterfaceFiles/algorithms/pso_gen.i"
    %include "swigInterfaceFiles/algorithms/sade.i"
    %include "swigInterfaceFiles/algorithms/sea.i"
    %include "swigInterfaceFiles/algorithms/sga.i"
    %include "swigInterfaceFiles/algorithms/simulated_annealing.i"
    %include "swigInterfaceFiles/algorithms/xnes.i"
}

// ── Native problem types implement IProblem ───────────────────────────────────
%define PAGMO4J_NATIVE_PROBLEM(TYPE)
%typemap(javaimports)   pagmo::TYPE "import io.github.samthegliderpilot.pagmo4j.problems.*;"
%typemap(javainterfaces) pagmo::TYPE "io.github.samthegliderpilot.pagmo4j.problems.IProblem, AutoCloseable"
%typemap(javacode) pagmo::TYPE %{ @Override public void close() { delete(); } %}
%enddef

PAGMO4J_NATIVE_PROBLEM(ackley)
PAGMO4J_NATIVE_PROBLEM(cec2006)
PAGMO4J_NATIVE_PROBLEM(cec2009)
PAGMO4J_NATIVE_PROBLEM(cec2013)
PAGMO4J_NATIVE_PROBLEM(cec2014)
PAGMO4J_NATIVE_PROBLEM(decompose)
PAGMO4J_NATIVE_PROBLEM(dtlz)
PAGMO4J_NATIVE_PROBLEM(golomb_ruler)
PAGMO4J_NATIVE_PROBLEM(griewank)
PAGMO4J_NATIVE_PROBLEM(hock_schittkowski_71)
PAGMO4J_NATIVE_PROBLEM(inventory)
PAGMO4J_NATIVE_PROBLEM(lennard_jones)
PAGMO4J_NATIVE_PROBLEM(luksan_vlcek1)
PAGMO4J_NATIVE_PROBLEM(minlp_rastrigin)
// minlp_rastrigin::hessians_sparsity() returns std::vector<sparsity_pattern> which SWIG
// can't resolve to VectorOfSparsityPattern.  Suppress; the IProblem default is used.
%ignore pagmo::minlp_rastrigin::hessians_sparsity;
PAGMO4J_NATIVE_PROBLEM(null_problem)
PAGMO4J_NATIVE_PROBLEM(rastrigin)
PAGMO4J_NATIVE_PROBLEM(rosenbrock)
PAGMO4J_NATIVE_PROBLEM(schwefel)
PAGMO4J_NATIVE_PROBLEM(translate)
PAGMO4J_NATIVE_PROBLEM(unconstrain)
PAGMO4J_NATIVE_PROBLEM(wfg)
PAGMO4J_NATIVE_PROBLEM(zdt)

%include "swigInterfaceFiles/problems/ackley.i"
%include "swigInterfaceFiles/problems/cec2006.i"
// The 2-arg constructor (unsigned, bool) collides with SWIG's internal pointer constructor.
%ignore pagmo::cec2009::cec2009(unsigned int, bool);
%include "swigInterfaceFiles/problems/cec2009.i"
%include "swigInterfaceFiles/problems/cec2013.i"
%include "swigInterfaceFiles/problems/cec2014.i"
%include "swigInterfaceFiles/problems/decompose.i"
%include "swigInterfaceFiles/problems/dtlz.i"
%include "swigInterfaceFiles/problems/golomb_ruler.i"
%include "swigInterfaceFiles/problems/griewank.i"
%include "swigInterfaceFiles/problems/hock_schittkowski_71.i"
%include "swigInterfaceFiles/problems/inventory.i"
%include "swigInterfaceFiles/problems/lennard_jones.i"
%include "swigInterfaceFiles/problems/luksan_vlcek1.i"
%include "swigInterfaceFiles/problems/minlp_rastrigin.i"
%include "swigInterfaceFiles/problems/null_problem.i"
%include "swigInterfaceFiles/problems/rastrigin.i"
%include "swigInterfaceFiles/problems/rosenbrock.i"
%include "swigInterfaceFiles/problems/schwefel.i"
%include "swigInterfaceFiles/problems/translate.i"
%include "swigInterfaceFiles/problems/unconstrain.i"
%include "swigInterfaceFiles/problems/wfg.i"
%include "swigInterfaceFiles/problems/zdt.i"

%include "swigInterfaceFiles/islands/thread_island.i"
%include "swigInterfaceFiles/topologies/free_form.i"
%include "swigInterfaceFiles/topologies/fully_connected.i"
%include "swigInterfaceFiles/topologies/ring.i"

// to_topology() factory mirroring the .NET to_algorithm() pattern.
%extend pagmo::fully_connected { pagmo::topology to_topology() const { return pagmo::topology(*self); } }
%extend pagmo::ring             { pagmo::topology to_topology() const { return pagmo::topology(*self); } }
%extend pagmo::free_form        { pagmo::topology to_topology() const { return pagmo::topology(*self); } }

// ── unconnected topology ──────────────────────────────────────────────────────
%typemap(javainterfaces) pagmo::unconnected "AutoCloseable"
%typemap(javacode) pagmo::unconnected %{ @Override public void close() { delete(); } %}
%extend pagmo::unconnected { pagmo::topology to_topology() const { return pagmo::topology(*self); } }
%include "swigInterfaceFiles/topologies/unconnected.i"

%include "swigInterfaceFiles/batch_evaluators/default_bfe.i"
%include "swigInterfaceFiles/batch_evaluators/member_bfe.i"
%include "swigInterfaceFiles/batch_evaluators/thread_bfe.i"

// ── Native migration policies ─────────────────────────────────────────────────
// fair_replace and select_best expose pagmo's built-in policies without the
// managed-callback overhead.  Use FairReplaceAdapter / SelectBestAdapter
// (in the migration package) to pass them to pushBackIsland.
%typemap(javainterfaces) pagmo::fair_replace "AutoCloseable"
%typemap(javacode) pagmo::fair_replace %{ @Override public void close() { delete(); } %}
%include "swigInterfaceFiles/r_policies/fair_replace.i"

%typemap(javainterfaces) pagmo::select_best "AutoCloseable"
%typemap(javacode) pagmo::select_best %{ @Override public void close() { delete(); } %}
%include "swigInterfaceFiles/s_policies/select_best.i"
