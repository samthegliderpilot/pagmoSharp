/* Pagmo4jSwigInterface.i — SWIG module for pagmo4j (Java/Kotlin bindings).
 *
 * Derived from pagmoSharp's PagmoNETSwigInterface.i. Key differences:
 *   - Uses -java SWIG backend instead of -csharp
 *   - cs* typemaps replaced with java* / jtype / jstype equivalents
 *   - No partial-class modifiers (Java does not have partial classes)
 *   - Log injection and IProblem/IAlgorithm wiring done via %typemap(javacode) and
 *     %typemap(javainterfaces) instead of partial-class extension files
 *   - archipelago and island managed-problem injection done via %typemap(javacode)
 *
 * Sub-file includes reference the shared swig/ directory at the monorepo root
 * (../../swig/ relative to this file's location in java/swig/).
 */

#define SUPPORT_VARIDEC FALSE
%include "exception.i"
%include "pagmo/config.hpp"
%{
#include <string>
#include <pagmo/exceptions.hpp>
%}

// ── Global exception handler ──────────────────────────────────────────────────
%exception {
    try {
        $action
    } catch (const std::exception &e) {
        SWIG_exception(SWIG_RuntimeError, e.what());
    } catch (...) {
        SWIG_exception(SWIG_RuntimeError, "Unknown C++ exception");
    }
}

%define PAGMO4J_EXEC_EXCEPTION(METHOD, LABEL)
%exception METHOD {
    try {
        $action
    } catch (const std::exception &e) {
        std::string msg = std::string(LABEL) + ": " + e.what();
        SWIG_exception(SWIG_RuntimeError, msg.c_str());
    } catch (...) {
        std::string msg = std::string(LABEL) + ": Unknown C++ exception";
        SWIG_exception(SWIG_RuntimeError, msg.c_str());
    }
}
%enddef

PAGMO4J_EXEC_EXCEPTION(pagmo::algorithm::evolve,       "algorithm.evolve failed")
PAGMO4J_EXEC_EXCEPTION(pagmo::island::evolve,          "island.evolve failed")
PAGMO4J_EXEC_EXCEPTION(pagmo::island::wait,            "island.wait failed")
PAGMO4J_EXEC_EXCEPTION(pagmo::island::wait_check,      "island.wait_check failed")
PAGMO4J_EXEC_EXCEPTION(pagmo::archipelago::evolve,     "archipelago.evolve failed")
PAGMO4J_EXEC_EXCEPTION(pagmo::archipelago::wait,       "archipelago.wait failed")
PAGMO4J_EXEC_EXCEPTION(pagmo::archipelago::wait_check, "archipelago.wait_check failed")
PAGMO4J_EXEC_EXCEPTION(pagmo::thread_island::run_evolve, "thread_island.run_evolve failed")

// ── Module declaration ────────────────────────────────────────────────────────
// The generated JNI class will be named "pagmo4jJNI" by SWIG convention (module name + "JNI").
%module(naturalvar=1, directors="1") pagmo4j

%{
    #include "pagmo/problem.hpp"
    #include "pagmo/algorithm.hpp"
    #include "pagmo/island.hpp"
    #include "pagmo/archipelago.hpp"
    #include "pagmo/bfe.hpp"
    #include "pagmo/exceptions.hpp"
    #include "pagmo/population.hpp"
    #include "pagmo/rng.hpp"
    #include "pagmo/threading.hpp"
    #include "pagmo/topology.hpp"
    #include "pagmo/type_traits.hpp"
    #include "pagmo/types.hpp"
    #include "pagmo/utils/hv_algos/hv_algorithm.hpp"

    #include "problem.h"
    #include "algorithm_callback.h"
    #include "tuple_adapters.h"
    #include "algorithm_log_projections_more.h"
    #include "cmaes_log_projection.h"
    #include "cstrs_log_projection.h"
    #include "de_log_projection.h"
    #include "gaco_log_projection.h"
    #include "ihs_log_projection.h"
    #include "mbh_log_projection.h"
    #include "r_policy.h"
    #include "s_policy.h"
    // Extern-C bridge functions — exposed to Java via SWIG declarations below.
    extern "C" void* pagmonet_problem_from_callback(void* callbackPtr);
    extern "C" void* pagmonet_algorithm_from_callback(void* callbackPtr);
    extern "C" void  pagmonet_problem_delete(void* problemPtr);
    extern "C" void* pagmonet_default_bfe_evaluate(void* problemPtr, void* batchXPtr);
%}

%include "pagmoWrapper/tuple_adapters.h"
%include "pagmoWrapper/algorithm_log_projections_more.h"
%include "pagmoWrapper/cmaes_log_projection.h"
%include "pagmoWrapper/cstrs_log_projection.h"
%include "pagmoWrapper/de_log_projection.h"
%include "pagmoWrapper/gaco_log_projection.h"
%include "pagmoWrapper/ihs_log_projection.h"
%include "pagmoWrapper/mbh_log_projection.h"

%include <std_shared_ptr.i>
%include "std_string.i"
%include "std_vector.i"
%include "std_pair.i"
%include "std_map.i"

// ── Note on AutoCloseable for SWIG vector types ───────────────────────────────
// SWIG's std_vector.i uses SWIG_STD_VECTOR_MINIMUM_INTERNAL macro (non-redefinable)
// to set javainterfaces="java.util.RandomAccess" at template instantiation time.
// This overrides any specific javainterfaces we set earlier. As a result, SWIG
// vector types (DoubleVector, PairOfDoubleVectors etc.) do NOT implement AutoCloseable
// and cannot be used in try-with-resources. Production code uses try/finally;
// tests use explicit .delete() calls or delegate to SWIGTestUtils.close(v).

// Add AutoCloseable to pair<DoubleVector,DoubleVector> (PairOfDoubleVectors) —
// pairs use std_pair.i which does not have the same macro lock-out.
%typemap(javainterfaces) std::pair<std::vector<double>, std::vector<double>>
    "AutoCloseable"
%typemap(javacode) std::pair<std::vector<double>, std::vector<double>> %{
    @Override public void close() { delete(); }
%}

// ── Rename snake_case C++ bridge types to PascalCase Java ────────────────────
%rename(ProblemCallback)     pagmoWrap::problem_callback;
%rename(ManagedProblem)      pagmoWrap::managed_problem;
%rename(AlgorithmCallback)   pagmoWrap::algorithm_callback;
%rename(ManagedAlgorithm)    pagmoWrap::managed_algorithm;
%rename(RPolicyCallback)     pagmoWrap::r_policy_callback;
%rename(ManagedRPolicy)      pagmoWrap::managed_r_policy;
%rename(SPolicyCallback)     pagmoWrap::s_policy_callback;
%rename(ManagedSPolicy)      pagmoWrap::managed_s_policy;
%rename(NullProblemCallback) pagmoWrap::null_problem_callback;

// Pagmo enum types — PascalCase names, matching C# equivalents.
%rename(ThreadSafety) pagmo::thread_safety;
%rename(None)         pagmo::thread_safety::none;
%rename(Basic)        pagmo::thread_safety::basic;
%rename(Constant)     pagmo::thread_safety::constant;
%rename(EvolveStatus) pagmo::evolve_status;
%rename(Idle)         pagmo::evolve_status::idle;
%rename(Busy)         pagmo::evolve_status::busy;
%rename(IdleError)    pagmo::evolve_status::idle_error;
%rename(BusyError)    pagmo::evolve_status::busy_error;
%rename(MigrationType)   pagmo::migration_type;
%rename(P2P)             pagmo::migration_type::p2p;
%rename(Broadcast)       pagmo::migration_type::broadcast;
%rename(MigrantHandling) pagmo::migrant_handling;
%rename(Preserve)        pagmo::migrant_handling::preserve;
%rename(Evict)           pagmo::migrant_handling::evict;

// Rename wait() to avoid conflict with java.lang.Object.wait() which is final.
%rename(waitFor)      pagmo::island::wait;
%rename(waitFor)      pagmo::archipelago::wait;

// ── Declare pagmo enums so SWIG generates proper Java classes ─────────────────
// These must be declared before any headers that use pagmo::thread_safety etc.
namespace pagmo {
    enum class thread_safety { none, basic, constant };
    enum class evolve_status { idle = 0, busy = 1, idle_error = 2, busy_error = 3 };
    enum class migration_type { p2p, broadcast };
    enum class migrant_handling { preserve, evict };
}

// ── Director declarations ─────────────────────────────────────────────────────
%feature("director") pagmoWrap::problem_callback;
%feature("director") pagmoWrap::algorithm_callback;

%ignore pagmoWrap::managed_problem::managed_problem(std::shared_ptr<pagmoWrap::problem_callback>);
%ignore pagmoWrap::managed_problem::managed_problem(std::shared_ptr<problem_callback>);
%ignore pagmoWrap::managed_problem::managed_problem(std::shared_ptr< problem_callback >);
%ignore pagmoWrap::managed_problem::managed_problem(std::shared_ptr< pagmoWrap::problem_callback >);
%include "pagmoWrapper/problem.h"

// algorithm_callback.h is included AFTER population.i (below) so that SWIG knows
// pagmo::population when generating the director's evolve() method signature.
%ignore pagmoWrap::managed_algorithm::managed_algorithm(std::shared_ptr<pagmoWrap::algorithm_callback>);
%ignore pagmoWrap::managed_algorithm::managed_algorithm(std::shared_ptr<algorithm_callback>);
%ignore pagmoWrap::managed_algorithm::managed_algorithm(std::shared_ptr< algorithm_callback >);
%ignore pagmoWrap::managed_algorithm::managed_algorithm(std::shared_ptr< pagmoWrap::algorithm_callback >);
// Suppress the inner null_algorithm_callback class — SWIG generates Java code that
// directly accesses the private swigCMemOwn field of the director base class.
%ignore pagmoWrap::managed_algorithm::null_algorithm_callback;
%shared_ptr(pagmoWrap::problem_callback);
%shared_ptr(pagmoWrap::algorithm_callback);

%feature("director") pagmoWrap::r_policy_callback;
%ignore pagmoWrap::managed_r_policy::replace;
%ignore pagmoWrap::managed_r_policy::operator=;
%include "pagmoWrapper/r_policy.h"

%feature("director") pagmoWrap::s_policy_callback;
%ignore pagmoWrap::managed_s_policy::select;
%ignore pagmoWrap::managed_s_policy::operator=;
%include "pagmoWrapper/s_policy.h"

// ── Extern-C bridge functions (pagmonet_*) ────────────────────────────────────
// Declared here so SWIG generates JNI wrappers callable as static methods on
// the pagmo4j module class. In the Java backend, void* automatically maps to
// long (jlong) — no explicit %apply needed.
%typemap(jtype)  void * "long"
%typemap(jstype) void * "long"
%typemap(javain) void * "$javainput"
%typemap(javaout) void * { return $jnicall; }
extern void* pagmonet_problem_from_callback(void* callbackPtr);
extern void* pagmonet_algorithm_from_callback(void* callbackPtr);
extern void  pagmonet_problem_delete(void* problemPtr);
extern void* pagmonet_default_bfe_evaluate(void* problemPtr, void* batchXPtr);
extern void* pagmonet_population_new(void* problemPtr, long popSize, unsigned int seed);
extern void* pagmonet_estimate_gradient_problem(void* problemPtr, void* xPtr, double dx);
extern void* pagmonet_estimate_gradient_h_problem(void* problemPtr, void* xPtr, double dx);
extern void* pagmonet_estimate_sparsity_problem(void* problemPtr, void* xPtr, double dx);

// ── Java typemaps for unsigned integer types ──────────────────────────────────
// unsigned int → long (fits in signed long; used for seeds and population sizes)
%typemap(jtype)  unsigned int "long"
%typemap(jstype) unsigned int "long"
%typemap(jtype)  const unsigned int & "long"
%typemap(jstype) const unsigned int & "long"
%typemap(javain) unsigned int "$javainput"
%typemap(javaout) unsigned int { return $jnicall; }

// unsigned long long intentionally left as BigInteger (SWIG default).
// ULongLongVector.get()/set()/etc. require AbstractList<BigInteger> — mapping to
// primitive long causes compile-time type mismatches in the vector template.
// Our injected javacode uses .longValue() where a long is needed from BigInteger.

// ── AutoCloseable for types used with try-with-resources ─────────────────────
// SWIG-generated classes have a delete() method for native memory; we expose
// it as close() so Java try-with-resources (and kotlin .use {}) work.
// Note: std::vector wrappers extend AbstractList and SWIG's template logic
// overrides our javainterfaces typemap. We add close() without @Override so that
// callers can call close() directly; use try/finally in javacode instead of
// try-with-resources for these types.

// ── STL template instantiations ───────────────────────────────────────────────
namespace std {
  %template(DoubleVector)             std::vector<double>;
  %template(UIntVector)               std::vector<unsigned int>;
  %template(SizeTVector)              std::vector<std::size_t>;
  %template(SizeTPair)                std::pair<std::size_t, std::size_t>;
  %template(TopologyConnections)      std::pair<std::vector<std::size_t>, std::vector<double>>;
  %template(SparsityPattern)          std::vector<std::pair<std::size_t, std::size_t>>;
  %template(VectorOfSparsityPattern)  std::vector<std::vector<std::pair<std::size_t, std::size_t>>>;
  %template(ULongLongVector)          std::vector<unsigned long long>;
  %template(VectorOfVectorOfIndices)  std::vector<std::vector<unsigned long long>>;
  %template(VectorOfVectorOfDoubles)  std::vector<std::vector<double>>;
  %template(PairOfDoubleVectors)      std::pair<std::vector<double>, std::vector<double>>;
  %template(HvAlgorithmSharedPtr)     std::shared_ptr<pagmo::hv_algorithm>;

  %template(IndividualsGroupVector)           std::vector<pagmoWrap::IndividualsGroup>;
  %template(MigrationEntryVector)             std::vector<pagmoWrap::MigrationEntry>;
  %template(PsoLogEntryVector)                std::vector<pagmoWrap::PsoLogEntry>;
  %template(XnesLogEntryVector)               std::vector<pagmoWrap::XnesLogEntry>;
  %template(MoVectorLogEntryVector)           std::vector<pagmoWrap::MoVectorLogEntry>;
  %template(MoeadLogEntryVector)              std::vector<pagmoWrap::MoeadLogEntry>;
  %template(GwoLogEntryVector)                std::vector<pagmoWrap::GwoLogEntry>;
  %template(De1220LogEntryVector)             std::vector<pagmoWrap::De1220LogEntry>;
  %template(CompassSearchLogEntryVector)      std::vector<pagmoWrap::CompassSearchLogEntry>;
  %template(NloptLogEntryVector)              std::vector<pagmoWrap::NloptLogEntry>;
  %template(IpoptLogEntryVector)              std::vector<pagmoWrap::IpoptLogEntry>;
  %template(Snopt7LogEntryVector)             std::vector<pagmoWrap::Snopt7LogEntry>;
  %template(SimulatedAnnealingLogEntryVector) std::vector<pagmoWrap::SimulatedAnnealingLogEntry>;
  %template(BeeColonyLogLineVector)           std::vector<pagmoWrap::BeeColonyLogLine>;
  %template(DeLogEntryVector)                 std::vector<pagmoWrap::DeLogEntry>;
  %template(CmaesLogEntryVector)              std::vector<pagmoWrap::CmaesLogEntry>;
  %template(CstrsLogEntryVector)              std::vector<pagmoWrap::CstrsLogEntry>;
  %template(GacoLogEntryVector)               std::vector<pagmoWrap::GacoLogEntry>;
  %template(IhsLogEntryVector)                std::vector<pagmoWrap::IhsLogEntry>;
  %template(MbhLogEntryVector)                std::vector<pagmoWrap::MbhLogEntry>;
}

%include "pagmoWrapper/multi_objective.h"

// ── javaimports: add sub-package imports to generated classes ─────────────────
// Every generated class that uses IProblem, IAlgorithm, IThreadCloneableProblem,
// ExclusiveCloneAdapter, BfeBridge, or IAlgorithmLogLine needs these imports.
// javaimports strings must be inlined — SWIG does not expand %define macros inside them.
%typemap(javaimports) pagmo::archipelago      "import io.github.samthegliderpilot.pagmo4j.problems.*; import io.github.samthegliderpilot.pagmo4j.algorithms.*; import io.github.samthegliderpilot.pagmo4j.batchevaluators.*;"
%typemap(javaimports) pagmo::island           "import io.github.samthegliderpilot.pagmo4j.problems.*; import io.github.samthegliderpilot.pagmo4j.algorithms.*; import io.github.samthegliderpilot.pagmo4j.batchevaluators.*;"
%typemap(javaimports) pagmo::algorithm        "import io.github.samthegliderpilot.pagmo4j.problems.*; import io.github.samthegliderpilot.pagmo4j.algorithms.*; import io.github.samthegliderpilot.pagmo4j.batchevaluators.*;"
%typemap(javaimports) pagmo::problem          "import io.github.samthegliderpilot.pagmo4j.problems.*;"
%typemap(javaimports) pagmo::default_bfe      "import io.github.samthegliderpilot.pagmo4j.batchevaluators.*; import io.github.samthegliderpilot.pagmo4j.problems.*;"
%typemap(javaimports) pagmo::thread_bfe       "import io.github.samthegliderpilot.pagmo4j.batchevaluators.*; import io.github.samthegliderpilot.pagmo4j.problems.*;"
%typemap(javaimports) pagmo::member_bfe       "import io.github.samthegliderpilot.pagmo4j.batchevaluators.*; import io.github.samthegliderpilot.pagmo4j.problems.*;"

// All algorithm classes need the IAlgorithmLogLine import for getLogLines()
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

// ── AutoCloseable for resource-owning types ──────────────────────────────────
%typemap(javainterfaces) pagmo::archipelago        "AutoCloseable"
%typemap(javainterfaces) pagmo::island             "AutoCloseable"
%typemap(javainterfaces) pagmo::problem            "AutoCloseable"
%typemap(javainterfaces) pagmo::population         "AutoCloseable"
%typemap(javainterfaces) pagmo::default_bfe        "AutoCloseable"
%typemap(javainterfaces) pagmo::thread_bfe         "AutoCloseable"
%typemap(javainterfaces) pagmo::member_bfe         "AutoCloseable"

// ── IAlgorithm implementations ───────────────────────────────────────────────
%typemap(javainterfaces) pagmo::algorithm          "AutoCloseable"
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

// ── Log projection injection ──────────────────────────────────────────────────
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
PAGMO4J_SIMPLE_ALGO_CODE(sade)
PAGMO4J_SIMPLE_ALGO_CODE(bee_colony)
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
PAGMO4J_SIMPLE_ALGO_CODE(sea)
PAGMO4J_SIMPLE_ALGO_CODE(sga)
PAGMO4J_SIMPLE_ALGO_CODE(xnes)
PAGMO4J_SIMPLE_ALGO_CODE(nlopt)
PAGMO4J_SIMPLE_ALGO_CODE(ipopt)

%typemap(javacode) pagmo::algorithm %{
    public algorithm(IAlgorithm managed) {
        this(NativeInterop.createAlgorithmPointer(managed), true);
    }
    @Override public void close() { delete(); }
%}

// ── problem: IProblem constructor + sparsity/gradient helpers ────────────────
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

    private long withManagedProblem(IProblem prob, java.util.function.LongSupplier action) {
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
                    return action.getAsLong();
                }
            }
        }
        prob.throwIfNotThreadSafe();
        try (problem wrapped = new problem(prob)) {
            return action.getAsLong();
        }
    }

    public long pushBackIsland(algorithm algo, IProblem problem, long popSize, long seed) {
        return withManagedProblem(problem, () -> {
            long nativePop = SizeTInterop.toNativeUInt32(popSize, "popSize");
            return push_back_island(algo, new problem(problem), nativePop, seed);
        });
    }

    public long pushBackIsland(IAlgorithm algo, IProblem problem, long popSize, long seed) {
        try (algorithm normalized = AlgorithmInterop.normalizeToTypeErased(algo)) {
            return pushBackIsland(normalized, problem, popSize, seed);
        }
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

    public void waitCheck() { wait_check(); }

    @Override public void close() { delete(); }
%}

%typemap(javaimports) pagmo::population "import io.github.samthegliderpilot.pagmo4j.problems.*;"
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

// ── Sub-module includes (shared swig/ at monorepo root) ───────────────────────
%include "swigInterfaceFiles/problem.i"
%include "swigInterfaceFiles/algorithm.i"
// Wrap topology.i in namespace pagmo so pagmo::topology is a known SWIG type
// (needed for ring/fully_connected/free_form to_topology() return types).
%typemap(javainterfaces) pagmo::topology "AutoCloseable"
%typemap(javacode) pagmo::topology %{ @Override public void close() { delete(); } %}
namespace pagmo { %include "swigInterfaceFiles/topology.i" }
// Wrap population.i in namespace pagmo so SWIG maps pagmo::population to population.
// This is critical: without it, algorithm directors and evolve() signatures use
// opaque SWIGTYPE_p_pagmo__population instead of the proper population wrapper.
namespace pagmo {
    %include "swigInterfaceFiles/population.i"
}
// Now that pagmo::population is known, include algorithm_callback.h so the director
// generates evolve(population) instead of evolve(SWIGTYPE_p_pagmo__population).
%include "pagmoWrapper/algorithm_callback.h"
%include "swigInterfaceFiles/island.i"
%include "swigInterfaceFiles/archipelago.i"
%include "swigInterfaceFiles/bfe.i"
// topology.i already included above in namespace pagmo {} block
// r_policy.i is a stub; s_policy.i does not exist — both are handled inline above
// via %feature("director"), %ignore, and %include of pagmoWrapper headers.
%include "swigInterfaceFiles/rng.i"
%include "swigInterfaceFiles/io.i"
%typemap(javainterfaces) pagmo::hypervolume "AutoCloseable"
%typemap(javacode) pagmo::hypervolume %{ @Override public void close() { delete(); } %}
%include "swigInterfaceFiles/utils/hypervolume.i"
%include "swigInterfaceFiles/utils/multi_objective.i"
%include "swigInterfaceFiles/utils/gradients_and_hessians.i"

// Algorithms using qualified names work without namespace wrapping.
%include "swigInterfaceFiles/algorithms/bee_colony.i"
%include "swigInterfaceFiles/algorithms/cstrs_self_adaptive.i"
%include "swigInterfaceFiles/algorithms/ihs.i"
%include "swigInterfaceFiles/algorithms/maco.i"
%include "swigInterfaceFiles/algorithms/mbh.i"
%include "swigInterfaceFiles/algorithms/moead.i"
%include "swigInterfaceFiles/algorithms/moead_gen.i"
%include "swigInterfaceFiles/algorithms/nsga2.i"

// Algorithms with unqualified class names need namespace pagmo {} wrapping
// so that %typemap(javainterfaces/javacode/javaimports) pagmo::X entries apply.
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

// ── Native problem classes implement IProblem (mirrors .NET partial class pattern) ──
// All native problem classes expose fitness() and get_bounds() and satisfy IProblem's
// contract. Other IProblem methods have defaults so they don't need to be in every .i file.
%define PAGMO4J_NATIVE_PROBLEM(TYPE)
%typemap(javaimports) pagmo::TYPE "import io.github.samthegliderpilot.pagmo4j.problems.*;"
%typemap(javainterfaces) pagmo::TYPE "io.github.samthegliderpilot.pagmo4j.problems.IProblem, AutoCloseable"
%typemap(javacode) pagmo::TYPE %{
    @Override public void close() { delete(); }
%}
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
// can't resolve to VectorOfSparsityPattern (typedef not expanded). Suppress; the
// IProblem default (throws UnsupportedOperationException) is used instead.
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
// The 2-arg constructor (unsigned, bool) maps to (long, boolean) which collides with
// SWIG's internal pointer constructor. Ignore the overload; use (prob_id, is_constrained, dim).
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
// AutoCloseable for topology UDT types — must be BEFORE their %include statements.
%typemap(javainterfaces) pagmo::fully_connected "AutoCloseable"
%typemap(javacode) pagmo::fully_connected %{ @Override public void close() { delete(); } %}
%typemap(javainterfaces) pagmo::ring "AutoCloseable"
%typemap(javacode) pagmo::ring %{ @Override public void close() { delete(); } %}
%typemap(javainterfaces) pagmo::free_form "AutoCloseable"
%typemap(javacode) pagmo::free_form %{ @Override public void close() { delete(); } %}
%include "swigInterfaceFiles/topologies/free_form.i"
%include "swigInterfaceFiles/topologies/fully_connected.i"
%include "swigInterfaceFiles/topologies/ring.i"

// Add to_topology() factory to topology UDTs (mirrors .NET to_algorithm() pattern).
%extend pagmo::fully_connected { pagmo::topology to_topology() const { return pagmo::topology(*self); } }
%extend pagmo::ring             { pagmo::topology to_topology() const { return pagmo::topology(*self); } }
%extend pagmo::free_form        { pagmo::topology to_topology() const { return pagmo::topology(*self); } }

%include "swigInterfaceFiles/batch_evaluators/default_bfe.i"
%include "swigInterfaceFiles/batch_evaluators/member_bfe.i"
%include "swigInterfaceFiles/batch_evaluators/thread_bfe.i"
