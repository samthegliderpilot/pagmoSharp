/* File pagmoSharpSwigInterface.i */
#define SUPPORT_VARIDEC FALSE
%include "exception.i"
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
	#include "tuple_adapters.h"
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

// Directors + handwritten base classes (include ONCE)
// The whole problem vs. problemBase question is a little confusing.  To make it better (or wo// rs// e)
// there is a C# implicit operator to convert from problem to problemBase by calling getBaseProblem on 
// the wrapper (problem).  Hence the partial class here
%pragma(csharp) moduleclassmodifiers = "public partial class"
%typemap(csclassmodifiers) pagmoWrap::problem_callback "public partial class"
%typemap(csclassmodifiers) pagmoWrap::managed_problem "public partial class"

%typemap(csclassmodifiers) std::vector <double> "public partial class"
%feature("director") pagmoWrap::problem_callback;
%include "pagmoWrapper/problem.h"
%shared_ptr(pagmoWrap::problem_callback);

%feature("director") pagmoWrap::r_policyBase;
%include "pagmoWrapper/r_policy.h"

%feature("director") pagmoWrap::s_policyBase;
%include "pagmoWrapper/s_policy.h"

// need other languages?


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






%feature("director") pagmoWrap::multi_objective;
%include "pagmoWrapper/multi_objective.h"
//#include <tuple> // tuple is not supported by swig yet...
%apply void *VOID_INT_PTR { void * }
%include "pagmoWrapper/tuple_adapters.h"

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
  %template(VectorOfVectorIndexes)     std::vector<std::vector<unsigned long long>>;
  %template(VectorOfVectorOfDoubles)   std::vector<std::vector<double>>;
	  %template(PairOfDoubleVectors)       std::pair<std::vector<double>, std::vector<double>>;
	  %template(HvAlgorithmSharedPtr)      std::shared_ptr<pagmo::hv_algorithm>;

  // Your tuple adapter structs:
  %template(IndividualsGroupVector)    std::vector<pagmoWrap::IndividualsGroup>;
  %template(MigrationEntryVector)      std::vector<pagmoWrap::MigrationEntry>;
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

	//%extend default_bfe{
	//vector_double Operator(const pagmoWrap::problemPagomWrapper& theProblem, const vector_double & values) const
	//{
	//   return self->operator()(static_cast<pagmo::problem>(theProblem), values);
	//}
	//};

	//%extend member_bfe{
	//vector_double Operator(const pagmoWrap::problemPagomWrapper& theProblem, const vector_double & values) const
	//{
	//   return self->operator()(static_cast<pagmo::problem>(theProblem), values);
	//}
	//};

	//%extend thread_bfe{
	//vector_double Operator(const pagmoWrap::problemPagomWrapper& theProblem, const vector_double & values) const
	//{
	//   return self->operator()(static_cast<pagmo::problem>(theProblem), values);
	//}
	//};



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
	//%include swigInterfaceFiles\exceptions.i // causing errors, not sure why, and not really implimented anyway
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
		//%include swigInterfaceFiles\algorithms\ipopt.i // my build of pagmo doesn't include ipopt
	//%include swigInterfaceFiles\algorithms\nlopt.i
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

	//%include swigInterfaceFiles\utils\gradients_and_hessians.i // I couldn't get this to translate through swig so I just recreated the functions in C#
	%include swigInterfaceFiles\utils\multi_objective.i
%include swigInterfaceFiles\algorithm.i
%include swigInterfaceFiles\algorithms\bee_colony.i
%include swigInterfaceFiles\archipelago.i

// this exception logic might not be necessary after adding the global exception handling at the top of the file
%{
#include <exception>
struct wrapped_exception : std::exception {
  wrapped_exception(const std::string& msg) : msg(msg) {}
private:
  virtual const char * what () const noexcept {
    return msg.c_str();
  }
  std::string msg;
};
%}

%typemap(csdirectorout) void %{
  try {
    $cscall;
  }
  catch(System.Exception e) {
    test.throw_native(e.ToString());
  }
%};


// Remaining wrapper backlog and prioritization are tracked in .ai/ROADMAP.md.
// Keep this interface focused on active binding definitions to reduce stale drift.
