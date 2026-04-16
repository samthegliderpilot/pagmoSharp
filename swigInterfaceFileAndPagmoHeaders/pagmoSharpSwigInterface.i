/* File pagmoSharpSwigInterface.i */
#define SUPPORT_VARIDEC FALSE
%include "exception.i"
%include "pagmo/config.hpp"
%{
#include <string>
#include <pagmo/exceptions.hpp>  
%}

// Global exception handler
%exception {
    try {
        $action
    } catch (const std::exception &e) {
        SWIG_exception(SWIG_RuntimeError, e.what());
    } catch (...) {
        SWIG_exception(SWIG_RuntimeError, "Unknown C++ exception");
    }
}

// Execute-path context for high-value runtime operations. This keeps failures
// actionable when they bubble through type-erased algorithm/island orchestration.
%define PAGMOSHARP_EXEC_EXCEPTION(METHOD, LABEL)
%exception METHOD {
    try {
        $action
    } catch (const std::exception &e) {
        std::string pagmosharp_message = std::string(LABEL) + ": " + e.what();
        SWIG_exception(SWIG_RuntimeError, pagmosharp_message.c_str());
    } catch (...) {
        std::string pagmosharp_message = std::string(LABEL) + ": Unknown C++ exception";
        SWIG_exception(SWIG_RuntimeError, pagmosharp_message.c_str());
    }
}
%enddef

PAGMOSHARP_EXEC_EXCEPTION(pagmo::algorithm::evolve, "algorithm.evolve failed")
PAGMOSHARP_EXEC_EXCEPTION(pagmo::island::evolve, "island.evolve failed")
PAGMOSHARP_EXEC_EXCEPTION(pagmo::island::wait, "island.wait failed")
PAGMOSHARP_EXEC_EXCEPTION(pagmo::island::wait_check, "island.wait_check failed")
PAGMOSHARP_EXEC_EXCEPTION(pagmo::archipelago::evolve, "archipelago.evolve failed")
PAGMOSHARP_EXEC_EXCEPTION(pagmo::archipelago::wait, "archipelago.wait failed")
PAGMOSHARP_EXEC_EXCEPTION(pagmo::archipelago::wait_check, "archipelago.wait_check failed")
PAGMOSHARP_EXEC_EXCEPTION(pagmo::thread_island::run_evolve, "thread_island.run_evolve failed")

%module(naturalvar=1, directors="11") pagmo
%{
	#include "pagmo/problem.hpp"
	#include "pagmo/algorithm.hpp"
	#include "pagmo/island.hpp"
	#include "pagmo/archipelago.hpp"	
	#include "pagmo/bfe.hpp"
	#include "pagmo/exceptions.hpp"
	#include "pagmo/population.hpp"	
	#include "pagmo/rng.hpp"
	#include "pagmo/s11n.hpp"	// has to do with serialization of varidec templates, which swig doesn't support and I don't think is needed for this library
		#include "pagmo/threading.hpp" 
		#include "pagmo/topology.hpp"
		#include "pagmo/type_traits.hpp"
		#include "pagmo/types.hpp"
		#include "pagmo/utils/hv_algos/hv_algorithm.hpp"
	    
	#include "problem.h" // this is a manually created item.  We want to include it in the wrappers so the generated cxx code can use the handwritten code for the problem
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
	//#include "multi_objective.h"
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

// Directors + handwritten base classes (include ONCE)
// problem_callback is the director-enabled virtual base C# subclasses override.
// managed_problem is the copy-safe UDT pagmo actually stores by value (holds shared_ptr to callback).
// The partial class modifier allows hand-written C# to extend the SWIG-generated classes.
%pragma(csharp) moduleclassmodifiers = "public partial class"
%typemap(csclassmodifiers) pagmoWrap::problem_callback "public partial class"
%typemap(csclassmodifiers) pagmoWrap::managed_problem "public partial class"
%typemap(csclassmodifiers) pagmoWrap::algorithm_callback "public partial class"
%typemap(csclassmodifiers) pagmoWrap::managed_algorithm "public partial class"

%typemap(csclassmodifiers) std::vector <double> "public partial class"
%feature("director") pagmoWrap::problem_callback;
%feature("director") pagmoWrap::algorithm_callback;

// Hide the shared_ptr<> constructors from SWIG — C# never constructs managed_problem/managed_algorithm
// directly from a shared_ptr; ownership is transferred via the extern-C bridge functions in
// managed_bridge.cpp. All four spelling variants are listed because SWIG matches by the exact string
// it sees after resolving includes; different include-path orderings produce different forms
// (with/without namespace prefix, with/without spaces around the template argument).
%ignore pagmoWrap::managed_problem::managed_problem(std::shared_ptr<pagmoWrap::problem_callback>);
%ignore pagmoWrap::managed_problem::managed_problem(std::shared_ptr<problem_callback>);
%ignore pagmoWrap::managed_problem::managed_problem(std::shared_ptr< problem_callback >);
%ignore pagmoWrap::managed_problem::managed_problem(std::shared_ptr< pagmoWrap::problem_callback >);
%include "pagmoWrapper/problem.h"
%ignore pagmoWrap::managed_algorithm::managed_algorithm(std::shared_ptr<pagmoWrap::algorithm_callback>);
%ignore pagmoWrap::managed_algorithm::managed_algorithm(std::shared_ptr<algorithm_callback>);
%ignore pagmoWrap::managed_algorithm::managed_algorithm(std::shared_ptr< algorithm_callback >);
%ignore pagmoWrap::managed_algorithm::managed_algorithm(std::shared_ptr< pagmoWrap::algorithm_callback >);
%include "pagmoWrapper/algorithm_callback.h"
%shared_ptr(pagmoWrap::problem_callback);
%shared_ptr(pagmoWrap::algorithm_callback);

%feature("director") pagmoWrap::r_policyBase;
// Suppress the replace() method on r_policyPagmoWrapper (the copy-safe UDT pagmo stores).
// That overload takes pagmo::individuals_group_t (a std::tuple<...>), which SWIG cannot wrap.
// C# code calls through r_policyBase::replace(), which takes the SWIG-friendly IndividualsGroup struct.
%ignore pagmoWrap::r_policyPagmoWrapper::replace;
%include "pagmoWrapper/r_policy.h"

%feature("director") pagmoWrap::s_policyBase;
// Same rationale as r_policyPagmoWrapper::replace above.
%ignore pagmoWrap::s_policyPagmoWrapper::select;
%include "pagmoWrapper/s_policy.h"

// ------------------------------------------------------------
// C#-correct typemaps for uint64 / unsigned long long
// ------------------------------------------------------------

// Map uint64 directly to C# ulong.
// This will cause SWIG to use UInt64 marshaling (P/Invoke uses 'ulong').
%typemap(cstype) unsigned long long "ulong"
%typemap(imtype) unsigned long long "ulong"
%typemap(cstype) const unsigned long long & "ulong"
%typemap(imtype) const unsigned long long & "ulong"

// If SWIG ever needs to marshal it explicitly at runtime (rare), this helps.
// These are safe defaults for C# P/Invoke.
%typemap(in) unsigned long long {
  $1 = (unsigned long long)$input;
}
%typemap(out) unsigned long long {
  $result = $1;
}


// pagmoWrapper/multi_objective.h exposes FNDSResult, RekSum, and DecompositionWeights.
// There is no pagmoWrap::multi_objective class — only free functions and helpers in global / pagmo namespaces.
%include "pagmoWrapper/multi_objective.h"

// Map void* → IntPtr in generated C# P/Invoke signatures.
// This is required for the extern-C bridge functions (managed_bridge.cpp) that return
// heap-allocated native objects as void* — without this they would become SWIGTYPE_p_void
// (an unusable opaque type) rather than a marshallable IntPtr.
%apply void *VOID_INT_PTR { void * }

// ------------------------------------------------------------
// STL templates used across the bindings
// (ONLY define each template ONCE)
// ------------------------------------------------------------
namespace std {
  %template(DoubleVector)              std::vector<double>;
  %template(UIntVector)                std::vector<unsigned int>;
  %template(SizeTVector)               std::vector<std::size_t>;
  %template(SizeTPair)                 std::pair<std::size_t, std::size_t>;
  %template(TopologyConnections)       std::pair<std::vector<std::size_t>, std::vector<double>>;
  %template(SparsityPattern)           std::vector<std::pair<std::size_t, std::size_t>>;
  %template(VectorOfSparsityPattern)   std::vector<std::vector<std::pair<std::size_t, std::size_t>>>;
  %template(ULongLongVector)           std::vector<unsigned long long>;
  %template(VectorOfVectorOfIndices)    std::vector<std::vector<unsigned long long>>;
  %template(VectorOfVectorOfDoubles)   std::vector<std::vector<double>>;
	  %template(PairOfDoubleVectors)       std::pair<std::vector<double>, std::vector<double>>;
	  %template(HvAlgorithmSharedPtr)      std::shared_ptr<pagmo::hv_algorithm>;

  // Your tuple adapter structs:
  %template(IndividualsGroupVector)    std::vector<pagmoWrap::IndividualsGroup>;
  %template(MigrationEntryVector)      std::vector<pagmoWrap::MigrationEntry>;
  %template(PsoLogEntryVector)         std::vector<pagmoWrap::PsoLogEntry>;
  %template(XnesLogEntryVector)        std::vector<pagmoWrap::XnesLogEntry>;
  %template(MoVectorLogEntryVector)    std::vector<pagmoWrap::MoVectorLogEntry>;
  %template(MoeadLogEntryVector)       std::vector<pagmoWrap::MoeadLogEntry>;
  %template(GwoLogEntryVector)         std::vector<pagmoWrap::GwoLogEntry>;
  %template(De1220LogEntryVector)      std::vector<pagmoWrap::De1220LogEntry>;
  %template(CompassSearchLogEntryVector) std::vector<pagmoWrap::CompassSearchLogEntry>;
  %template(NloptLogEntryVector)        std::vector<pagmoWrap::NloptLogEntry>;
  %template(IpoptLogEntryVector)        std::vector<pagmoWrap::IpoptLogEntry>;
  %template(SimulatedAnnealingLogEntryVector) std::vector<pagmoWrap::SimulatedAnnealingLogEntry>;
  %template(SgaLogEntryVector)         std::vector<pagmoWrap::SgaLogEntry>;
  %template(SadeLogEntryVector)        std::vector<pagmoWrap::SadeLogEntry>;
  %template(SeaLogEntryVector)         std::vector<pagmoWrap::SeaLogEntry>;
  %template(CmaesLogEntryVector)       std::vector<pagmoWrap::CmaesLogEntry>;
  %template(CstrsLogEntryVector)       std::vector<pagmoWrap::CstrsLogEntry>;
  %template(DeLogEntryVector)          std::vector<pagmoWrap::DeLogEntry>;
  %template(GacoLogEntryVector)        std::vector<pagmoWrap::GacoLogEntry>;
  %template(IhsLogEntryVector)         std::vector<pagmoWrap::IhsLogEntry>;
  %template(MbhLogEntryVector)         std::vector<pagmoWrap::MbhLogEntry>;
}

%include swigInterfaceFiles\island.i
%include swigInterfaceFiles\islands\thread_island.i
	namespace pagmo {
		%typemap(csclassmodifiers) pagmo::DoubleVector "public partial class"
		%typemap(csclassmodifiers) pagmo::VectorDoubleVector "public partial class"
		%typemap(csclassmodifiers) pagmo::PairOfDoubleVectors "public partial class"
		%typemap(csclassmodifiers) pagmo::ULongLongVector "public partial class"
		typedef std::vector<double> vector_double;
		
		typedef std::vector<std::pair<vector_double::size_type, vector_double::size_type>> sparsity_pattern;
		//typedef std::vector<vector_double>::size_type pop_size_t;
		typedef std::vector<std::vector<double>> VectorOfVectorOfDoubles;
	


	enum class thread_safety { none, basic, constant };

	enum class evolve_status {
		idle = 0,       ///< No asynchronous operations are ongoing, and no error was generated
		/// by an asynchronous operation in the past
		busy = 1,       ///< Asynchronous operations are ongoing, and no error was generated
		/// by an asynchronous operation in the past
		idle_error = 2, ///< Idle with error: no asynchronous operations are ongoing, but an error
		/// was generated by an asynchronous operation in the past
		busy_error = 3  ///< Busy with error: asynchronous operations are ongoing, and an error
		/// was generated by an asynchronous operation in the past
	};
			
	enum class migration_type {
		p2p,      ///< Point-to-point migration.
		broadcast ///< Broadcast migration.
	};


	enum class migrant_handling {
		preserve, ///< Preserve migrants in the database.
		evict     ///< Evict migrants from the database.
	};
		%include swigInterfaceFiles\problem.i
		%include swigInterfaceFiles\population.i
	
	%include swigInterfaceFiles\bfe.i
	//NOTE: pagmo.hpp, threading.hpp and types.hpp are not really needed
	%include swigInterfaceFiles\io.i
	%include swigInterfaceFiles\rng.i
	//%include swigInterfaceFiles\r_policy.i // needs the director/problem treatment
	%include swigInterfaceFiles\topology.i

}; // end of pagmo namespace (segment 1)

	%include swigInterfaceFiles\algorithms\cstrs_self_adaptive.i
	%include swigInterfaceFiles\algorithms\ihs.i
		%include swigInterfaceFiles\algorithms\maco.i
		%include swigInterfaceFiles\algorithms\mbh.i
		%include swigInterfaceFiles\algorithms\moead.i
		%include swigInterfaceFiles\algorithms\moead_gen.i
		%include swigInterfaceFiles\algorithms\nsga2.i
		%include swigInterfaceFiles\batch_evaluators\default_bfe.i
		%include swigInterfaceFiles\batch_evaluators\member_bfe.i
		%include swigInterfaceFiles\batch_evaluators\thread_bfe.i
		%include swigInterfaceFiles\r_policies\fair_replace.i
		%include swigInterfaceFiles\s_policies\select_best.i
		%include swigInterfaceFiles\topologies\unconnected.i
		%include swigInterfaceFiles\topologies\fully_connected.i
		%include swigInterfaceFiles\topologies\ring.i
		%include swigInterfaceFiles\topologies\free_form.i
		%include swigInterfaceFiles\utils\hv_algos\hv_algorithm.i
		%include swigInterfaceFiles\utils\hypervolume.i

namespace pagmo {
	%include swigInterfaceFiles\algorithms\cmaes.i
	%include swigInterfaceFiles\algorithms\compass_search.i		
	%include swigInterfaceFiles\algorithms\de.i
		%include swigInterfaceFiles\algorithms\de1220.i
		%include swigInterfaceFiles\algorithms\gaco.i
		%include swigInterfaceFiles\algorithms\gwo.i
	#if defined(PAGMO_WITH_IPOPT)
		%include swigInterfaceFiles\algorithms\ipopt.i
	#endif
	#if defined(PAGMO_WITH_NLOPT)
		%include swigInterfaceFiles\algorithms\nlopt.i
	#endif
	%include swigInterfaceFiles\algorithms\not_population_based.i
	%include swigInterfaceFiles\algorithms\nspso.i
	%include swigInterfaceFiles\algorithms\null_algorithm.i
	%include swigInterfaceFiles\algorithms\pso.i
	%include swigInterfaceFiles\algorithms\pso_gen.i
	%include swigInterfaceFiles\algorithms\sea.i
	%include swigInterfaceFiles\algorithms\simulated_annealing.i
	%include swigInterfaceFiles\algorithms\sade.i
	%include swigInterfaceFiles\algorithms\sga.i
	%include swigInterfaceFiles\algorithms\xnes.i

		//%include swigInterfaceFiles\detail\base_sr_policy.i // not sure if this is needed, and with no public constructors...
		//%include swigInterfaceFiles\detail\bfe_impl.i // not sure if this is needed, and with no public constructors...


	}; // end of pagmo namespace

	%include swigInterfaceFiles\problems\ackley.i
	%include swigInterfaceFiles\problems\cec2006.i
	%include swigInterfaceFiles\problems\cec2009.i
	%include swigInterfaceFiles\problems\cec2013.i
	%include swigInterfaceFiles\problems\cec2014.i
	%include swigInterfaceFiles\problems\decompose.i
	%include swigInterfaceFiles\problems\dtlz.i
	%include swigInterfaceFiles\problems\hock_schittkowski_71.i
	%include swigInterfaceFiles\problems\golomb_ruler.i
	%include swigInterfaceFiles\problems\griewank.i
	%include swigInterfaceFiles\problems\inventory.i
	%include swigInterfaceFiles\problems\lennard_jones.i
	%include swigInterfaceFiles\problems\luksan_vlcek1.i
	%include swigInterfaceFiles\problems\minlp_rastrigin.i
	%include swigInterfaceFiles\problems\null_problem.i
	%include swigInterfaceFiles\problems\rosenbrock.i
	%include swigInterfaceFiles\problems\schwefel.i
	%include swigInterfaceFiles\problems\rastrigin.i
	%include swigInterfaceFiles\problems\translate.i
	%include swigInterfaceFiles\problems\unconstrain.i
	%include swigInterfaceFiles\problems\wfg.i
	%include swigInterfaceFiles\problems\zdt.i

%define PAGMOSHARP_PROBLEM_TO_PROBLEM(TYPE_NAME)
%extend pagmo::TYPE_NAME {
    pagmo::problem to_problem() const
    {
        return pagmo::problem(*self);
    }
}
%enddef

PAGMOSHARP_PROBLEM_TO_PROBLEM(ackley)
PAGMOSHARP_PROBLEM_TO_PROBLEM(cec2006)
PAGMOSHARP_PROBLEM_TO_PROBLEM(cec2009)
PAGMOSHARP_PROBLEM_TO_PROBLEM(cec2013)
PAGMOSHARP_PROBLEM_TO_PROBLEM(cec2014)
PAGMOSHARP_PROBLEM_TO_PROBLEM(decompose)
PAGMOSHARP_PROBLEM_TO_PROBLEM(dtlz)
PAGMOSHARP_PROBLEM_TO_PROBLEM(hock_schittkowski_71)
PAGMOSHARP_PROBLEM_TO_PROBLEM(golomb_ruler)
PAGMOSHARP_PROBLEM_TO_PROBLEM(griewank)
PAGMOSHARP_PROBLEM_TO_PROBLEM(inventory)
PAGMOSHARP_PROBLEM_TO_PROBLEM(lennard_jones)
PAGMOSHARP_PROBLEM_TO_PROBLEM(luksan_vlcek1)
PAGMOSHARP_PROBLEM_TO_PROBLEM(minlp_rastrigin)
PAGMOSHARP_PROBLEM_TO_PROBLEM(null_problem)
PAGMOSHARP_PROBLEM_TO_PROBLEM(rosenbrock)
PAGMOSHARP_PROBLEM_TO_PROBLEM(schwefel)
PAGMOSHARP_PROBLEM_TO_PROBLEM(rastrigin)
PAGMOSHARP_PROBLEM_TO_PROBLEM(translate)
PAGMOSHARP_PROBLEM_TO_PROBLEM(unconstrain)
PAGMOSHARP_PROBLEM_TO_PROBLEM(wfg)
PAGMOSHARP_PROBLEM_TO_PROBLEM(zdt)

	//%include swigInterfaceFiles\utils\gradients_and_hessians.i // I couldn't get this to translate through swig so I just recreated the functions in C#
	%include swigInterfaceFiles\utils\multi_objective.i
%include swigInterfaceFiles\algorithm.i
%include swigInterfaceFiles\algorithms\bee_colony.i
%include swigInterfaceFiles\archipelago.i

// Remaining wrapper backlog and prioritization are tracked in .ai/ROADMAP.md.
// Keep this interface focused on active binding definitions to reduce stale drift.
