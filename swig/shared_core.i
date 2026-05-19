/* shared_core.i — Boilerplate shared between PagmoNETSwigInterface.i and Pagmo4jSwigInterface.i.
 *
 * Include this AFTER the language-specific %module declaration.  Language-specific
 * pragma, typemaps, and code-injection directives remain in the per-language file.
 *
 * Placement-sensitive typemaps that must precede content here (e.g. csclassmodifiers
 * before %template, csdirectorout before %feature("director")) must be placed in the
 * language file BEFORE the %include of this file.
 */

// ── Preamble ──────────────────────────────────────────────────────────────────
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

// ── Per-method exception enrichment ──────────────────────────────────────────
// Wraps key runtime operations with a labelled context string so failures are
// actionable when they surface through type-erased algorithm/island machinery.
%define PAGMO_EXEC_EXCEPTION(METHOD, LABEL)
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

PAGMO_EXEC_EXCEPTION(pagmo::algorithm::evolve,         "algorithm.evolve failed")
PAGMO_EXEC_EXCEPTION(pagmo::island::evolve,            "island.evolve failed")
PAGMO_EXEC_EXCEPTION(pagmo::island::wait,              "island.wait failed")
PAGMO_EXEC_EXCEPTION(pagmo::island::wait_check,        "island.wait_check failed")
PAGMO_EXEC_EXCEPTION(pagmo::archipelago::evolve,       "archipelago.evolve failed")
PAGMO_EXEC_EXCEPTION(pagmo::archipelago::wait,         "archipelago.wait failed")
PAGMO_EXEC_EXCEPTION(pagmo::archipelago::wait_check,   "archipelago.wait_check failed")
PAGMO_EXEC_EXCEPTION(pagmo::thread_island::run_evolve, "thread_island.run_evolve failed")

// ── C++ header includes ───────────────────────────────────────────────────────
%{
    #include "pagmo/problem.hpp"
    #include "pagmo/algorithm.hpp"
    #include "pagmo/island.hpp"
    #include "pagmo/archipelago.hpp"
    #include "pagmo/bfe.hpp"
    #include "pagmo/exceptions.hpp"
    #include "pagmo/population.hpp"
    #include "pagmo/rng.hpp"
    #include "pagmo/s11n.hpp"
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
#ifdef SWIGJAVA
    // BeeColonyLogLine must be defined here (before SWIG emits the BeeColonyLogLineVector
    // template helpers) to avoid forward-reference errors.  The guard prevents the
    // duplicate definition in bee_colony.i's %inline block.
    #include "pagmo/algorithms/bee_colony.hpp"
    #include <vector>
    #include <tuple>
    #ifndef PAGMOWRAP_BEE_COLONY_LOG_LINE_DEFINED
    #define PAGMOWRAP_BEE_COLONY_LOG_LINE_DEFINED
    namespace pagmoWrap {
        struct BeeColonyLogLine {
            unsigned gen;
            unsigned long long fevals;
            double best;
            double cur_best;
            BeeColonyLogLine() : gen(0), fevals(0), best(0.0), cur_best(0.0) {}
            BeeColonyLogLine(unsigned g, unsigned long long f, double b, double cb)
                : gen(g), fevals(f), best(b), cur_best(cb) {}
        };
    }
    #endif
    // Extern-C bridge functions exposed to Java via SWIG declarations below.
    extern "C" void* pagmonet_problem_from_callback(void* callbackPtr);
    extern "C" void* pagmonet_algorithm_from_callback(void* callbackPtr);
    extern "C" void* pagmonet_algorithm_from_callback_java(void* callbackPtr);
    extern "C" void  pagmonet_problem_delete(void* problemPtr);
    extern "C" const char* pagmonet_get_last_error();
    extern "C" void* pagmonet_default_bfe_evaluate(void* problemPtr, void* batchXPtr);
    extern "C" void* pagmonet_population_new(void* problemPtr, long popSize, unsigned int seed);
    extern "C" void* pagmonet_estimate_gradient_problem(void* problemPtr, void* xPtr, double dx);
    extern "C" void* pagmonet_estimate_gradient_h_problem(void* problemPtr, void* xPtr, double dx);
    extern "C" void* pagmonet_estimate_sparsity_problem(void* problemPtr, void* xPtr, double dx);
#endif
%}

// ── Log-projection struct includes ───────────────────────────────────────────
%include "pagmoWrapper/tuple_adapters.h"
%include "pagmoWrapper/algorithm_log_projections_more.h"
%include "pagmoWrapper/cmaes_log_projection.h"
%include "pagmoWrapper/cstrs_log_projection.h"
%include "pagmoWrapper/de_log_projection.h"
%include "pagmoWrapper/gaco_log_projection.h"
%include "pagmoWrapper/ihs_log_projection.h"
%include "pagmoWrapper/mbh_log_projection.h"

// ── SWIG STL library includes ─────────────────────────────────────────────────
%include <std_shared_ptr.i>
%include "std_string.i"
%include "std_vector.i"
%include "std_pair.i"
%include "std_map.i"

// ── Rename snake_case C++ bridge types to PascalCase ─────────────────────────
// Names are identical in C# and Java; only the target language differs.
%rename(ProblemCallback)     pagmoWrap::problem_callback;
%rename(ManagedProblem)      pagmoWrap::managed_problem;
%rename(AlgorithmCallback)   pagmoWrap::algorithm_callback;
%rename(ManagedAlgorithm)    pagmoWrap::managed_algorithm;
%rename(RPolicyCallback)     pagmoWrap::r_policy_callback;
%rename(ManagedRPolicy)      pagmoWrap::managed_r_policy;
%rename(SPolicyCallback)     pagmoWrap::s_policy_callback;
%rename(ManagedSPolicy)      pagmoWrap::managed_s_policy;
%rename(NullProblemCallback) pagmoWrap::null_problem_callback;

// ── Pagmo enum renames (PascalCase in both C# and Java) ──────────────────────
%rename(ThreadSafety)    pagmo::thread_safety;
%rename(None)            pagmo::thread_safety::none;
%rename(Basic)           pagmo::thread_safety::basic;
%rename(Constant)        pagmo::thread_safety::constant;
%rename(EvolveStatus)    pagmo::evolve_status;
%rename(Idle)            pagmo::evolve_status::idle;
%rename(Busy)            pagmo::evolve_status::busy;
%rename(IdleError)       pagmo::evolve_status::idle_error;
%rename(BusyError)       pagmo::evolve_status::busy_error;
%rename(MigrationType)   pagmo::migration_type;
%rename(P2P)             pagmo::migration_type::p2p;
%rename(Broadcast)       pagmo::migration_type::broadcast;
%rename(MigrantHandling) pagmo::migrant_handling;
%rename(Preserve)        pagmo::migrant_handling::preserve;
%rename(Evict)           pagmo::migrant_handling::evict;

// ── Director declarations ─────────────────────────────────────────────────────
// csdirectorout / javadirectorin typemaps in the per-language file must precede
// these %feature("director") declarations to take effect.
%feature("director") pagmoWrap::problem_callback;
%feature("director") pagmoWrap::algorithm_callback;

// Hide the shared_ptr<> constructors — neither language constructs ManagedProblem /
// ManagedAlgorithm directly from a shared_ptr; ownership transfers via the extern-C
// bridge functions in managed_bridge.cpp.  All four spellings are listed because SWIG
// matches by the exact string it sees after resolving includes.
%ignore pagmoWrap::managed_problem::managed_problem(std::shared_ptr<pagmoWrap::problem_callback>);
%ignore pagmoWrap::managed_problem::managed_problem(std::shared_ptr<problem_callback>);
%ignore pagmoWrap::managed_problem::managed_problem(std::shared_ptr< problem_callback >);
%ignore pagmoWrap::managed_problem::managed_problem(std::shared_ptr< pagmoWrap::problem_callback >);
%include "pagmoWrapper/problem.h"

%ignore pagmoWrap::managed_algorithm::managed_algorithm(std::shared_ptr<pagmoWrap::algorithm_callback>);
%ignore pagmoWrap::managed_algorithm::managed_algorithm(std::shared_ptr<algorithm_callback>);
%ignore pagmoWrap::managed_algorithm::managed_algorithm(std::shared_ptr< algorithm_callback >);
%ignore pagmoWrap::managed_algorithm::managed_algorithm(std::shared_ptr< pagmoWrap::algorithm_callback >);

// ── SWIG shared_ptr / algorithm_callback.h ordering asymmetry ────────────────
// Java  : %shared_ptr BEFORE algorithm_callback.h  →  getCPtr() returns shared_ptr<algorithm_callback>*
//         pagmonet_algorithm_from_callback_java (Java-only bridge) handles this shared_ptr* path.
// .NET  : algorithm_callback.h BEFORE %shared_ptr  →  swigRelease() returns raw algorithm_callback*
//         pagmonet_algorithm_from_callback (original bridge) handles the raw pointer path.
#ifdef SWIGJAVA
    // Suppress the inner null_algorithm_callback class — generated Java accesses
    // the private swigCMemOwn field of the director base class.
    %ignore pagmoWrap::managed_algorithm::null_algorithm_callback;
    %shared_ptr(pagmoWrap::problem_callback);
    %shared_ptr(pagmoWrap::algorithm_callback);
    // algorithm_callback.h is included later (after population.i) so SWIG knows
    // pagmo::population when generating the director's evolve() signature.
#else
    %include "pagmoWrapper/algorithm_callback.h"
    %shared_ptr(pagmoWrap::problem_callback);
    %shared_ptr(pagmoWrap::algorithm_callback);
#endif

%feature("director") pagmoWrap::r_policy_callback;
// Suppress replace() on ManagedRPolicy — it takes pagmo::individuals_group_t
// (a std::tuple<...>) which SWIG cannot wrap.  Language code calls through
// RPolicyCallback::replace() which takes the SWIG-friendly IndividualsGroup struct.
%ignore pagmoWrap::managed_r_policy::replace;
%ignore pagmoWrap::managed_r_policy::operator=;
%include "pagmoWrapper/r_policy.h"

%feature("director") pagmoWrap::s_policy_callback;
// Same rationale as ManagedRPolicy::replace above.
%ignore pagmoWrap::managed_s_policy::select;
%ignore pagmoWrap::managed_s_policy::operator=;
%include "pagmoWrapper/s_policy.h"

// ── STL template instantiations ───────────────────────────────────────────────
// Each template must be instantiated exactly once across all included files.
namespace std {
    %template(DoubleVector)                         std::vector<double>;
    %template(UIntVector)                           std::vector<unsigned int>;
    %template(SizeTVector)                          std::vector<std::size_t>;
    %template(SizeTPair)                            std::pair<std::size_t, std::size_t>;
    %template(TopologyConnections)                  std::pair<std::vector<std::size_t>, std::vector<double>>;
    %template(SparsityPattern)                      std::vector<std::pair<std::size_t, std::size_t>>;
    %template(VectorOfSparsityPattern)              std::vector<std::vector<std::pair<std::size_t, std::size_t>>>;
    %template(ULongLongVector)                      std::vector<unsigned long long>;
    %template(VectorOfVectorOfIndices)              std::vector<std::vector<unsigned long long>>;
    %template(VectorOfVectorOfDoubles)              std::vector<std::vector<double>>;
    %template(PairOfDoubleVectors)                  std::pair<std::vector<double>, std::vector<double>>;
    %template(HvAlgorithmSharedPtr)                 std::shared_ptr<pagmo::hv_algorithm>;

    %template(IndividualsGroupVector)               std::vector<pagmoWrap::IndividualsGroup>;
    %template(MigrationEntryVector)                 std::vector<pagmoWrap::MigrationEntry>;
    %template(PsoLogEntryVector)                    std::vector<pagmoWrap::PsoLogEntry>;
    %template(XnesLogEntryVector)                   std::vector<pagmoWrap::XnesLogEntry>;
    %template(MoVectorLogEntryVector)               std::vector<pagmoWrap::MoVectorLogEntry>;
    %template(MoeadLogEntryVector)                  std::vector<pagmoWrap::MoeadLogEntry>;
    %template(GwoLogEntryVector)                    std::vector<pagmoWrap::GwoLogEntry>;
    %template(De1220LogEntryVector)                 std::vector<pagmoWrap::De1220LogEntry>;
    %template(CompassSearchLogEntryVector)          std::vector<pagmoWrap::CompassSearchLogEntry>;
    %template(NloptLogEntryVector)                  std::vector<pagmoWrap::NloptLogEntry>;
    %template(IpoptLogEntryVector)                  std::vector<pagmoWrap::IpoptLogEntry>;
    %template(Snopt7LogEntryVector)                 std::vector<pagmoWrap::Snopt7LogEntry>;
    %template(SimulatedAnnealingLogEntryVector)     std::vector<pagmoWrap::SimulatedAnnealingLogEntry>;
    %template(SgaLogEntryVector)                    std::vector<pagmoWrap::SgaLogEntry>;
    %template(SadeLogEntryVector)                   std::vector<pagmoWrap::SadeLogEntry>;
    %template(SeaLogEntryVector)                    std::vector<pagmoWrap::SeaLogEntry>;
    %template(CmaesLogEntryVector)                  std::vector<pagmoWrap::CmaesLogEntry>;
    %template(CstrsLogEntryVector)                  std::vector<pagmoWrap::CstrsLogEntry>;
    %template(DeLogEntryVector)                     std::vector<pagmoWrap::DeLogEntry>;
    %template(GacoLogEntryVector)                   std::vector<pagmoWrap::GacoLogEntry>;
    %template(IhsLogEntryVector)                    std::vector<pagmoWrap::IhsLogEntry>;
    %template(MbhLogEntryVector)                    std::vector<pagmoWrap::MbhLogEntry>;
#ifdef SWIGJAVA
    %template(BeeColonyLogLineVector)               std::vector<pagmoWrap::BeeColonyLogLine>;
#endif
}
