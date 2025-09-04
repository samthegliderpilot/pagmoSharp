/* File pagmoSharpSwigInterface.i */
#define SUPPORT_VARIDEC FALSE
%include "exception.i"
%{
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
%module(naturalvar=1, directors="11") pagmo
%{
	#include "pagmo/algorithm.hpp"
	#include "pagmo/archipelago.hpp"	
	#include "pagmo/bfe.hpp"
	#include "pagmo/exceptions.hpp"
	#include "pagmo/island.hpp"
	#include "pagmo/population.hpp"	
	#include "pagmo/problem.hpp"
	#include "pagmo/rng.hpp"
	#include "pagmo/s11n.hpp"	// has to do with serialization of varidec templates, which swig doesn't support and I don't think is needed for this library
	#include "pagmo/threading.hpp" 
	#include "pagmo/topology.hpp"
	#include "pagmo/type_traits.hpp"
	#include "pagmo/types.hpp"
	    
	#include "problem.h" // this is a manually created item.  We want to include it in the wrappers so the generated cxx code can use the handwritten code for the problem
	#include "r_policy.h"
	#include "s_policy.h"
	//#include "multi_objective.h"
%}
// need other languages?


%{
#   define SWIG_CS_EXTRA_NATIVE_CONTAINERS 
%} 
%typemap(out) std::vector<std::vector<ns::uint64_t> >::value_type { 
$result = SWIG_NewPointerObj(SWIG_as_voidptr(&$1), $descriptor(std::vector<ns::uint64_t>), 0 |  0 ); 
} 

// In front(), back(), __getitem__()
%typemap(out) std::vector<std::vector<ns::uint64_t> >::value_type & { 
    $result = SWIG_NewPointerObj(SWIG_as_voidptr($1), $descriptor(std::vector<ns::uint64_t>), 0 |  0 ); 
} 

// In __getitem__()
%typemap(out) ns::uint64_t {
    $result = CSLong_FromUnsignedLongLong($1);
}
// Not used (but probably useful to have, just in case)
%typemap(in) ns::uint64_t {
    $1 = CSLong_AsUnsignedLongLong($input);
}
// In pop()
%typemap(out) std::vector<ns::uint64_t>::value_type {
    $result = CSLong_FromUnsignedLongLong($1);
}
// In __getitem__(), front(), back()
%typemap(out) std::vector<ns::uint64_t>::value_type & {
    $result = CsLong_FromUnsignedLongLong(*$1);
}
// In __setitem__(), append(), new Uint64Vector, push_back(), assign(), resize(), insert()
// This allows a python long literal number to be used as a parameter to the above methods. 
// Note the use of a local variable declared at the SWIG wrapper function scope,
// by placing the variable declaration in parentheses () prior to the open brace {
%typemap(in) std::vector<ns::uint64_t>::value_type & (std::vector<ns::uint64_t>::value_type temp) {
    temp = CsLong_FromUnsignedLongLong($input);
    $1 = &temp;
}


// The whole problem vs. problemBase question is a little confusing.  To make it better (or wo// rs// e)
// there is a C# implicit operator to convert from problem to problemBase by calling getBaseProblem on 
// the wrapper (problem).  Hence the partial class here
%include "std_string.i"
%include "std_vector.i"
%include "std_pair.i"
%pragma(csharp) moduleclassmodifiers = "public partial class"
%typemap(csclassmodifiers) pagmoWrap::problemPagomWrapper "public partial class"
%typemap(csclassmodifiers) pagmoWrap::problemBase "public partial class"
%typemap(csclassmodifiers) std::vector <double> "public partial class"

%feature("director") pagmoWrap::problemBase;
%include "pagmoWrapper/problem.h"
%feature("director") pagmoWrap::r_policyBase;
%include "pagmoWrapper/r_policy.h"
%feature("director") pagmoWrap::s_policyBase;
%include "pagmoWrapper/s_policy.h"
%feature("director") pagmoWrap::multi_objective;
%include "pagmoWrapper/multi_objective.h"
//#include <tuple> // tuple is not supported by swig yet...
%apply void *VOID_INT_PTR { void * }
namespace std {
	%template(DoubleVector) std::vector<double>;
	%template(ULongLongVector) std::vector<unsigned long long>;
	%template(PairOfDoubleVectors) std::pair<std::vector<double>, std::vector<double> >;
	%template(VectorOfVectorIndexes) std::vector<std::vector<unsigned long long> >;
	%template(VectorOfVectorOfDoubles) std::vector<std::vector<double> >;
	//%template() std::tuple<std::vector<std::vector<pop_size_t>>, std::vector<std::vector<pop_size_t>> std::vector<pop_size_t>, std::vector<pop_size_t>>;
}
	
namespace pagmo {
	%typemap(csclassmodifiers) pagmo::DoubleVector "public partial class"
	%typemap(csclassmodifiers) pagmo::VectorDoubleVector "public partial class"
	%typemap(csclassmodifiers) pagmo::PairOfDoubleVectors "public partial class"
	%typemap(csclassmodifiers) pagmo::ULongLongVector "public partial class"
	typedef std::vector<double> vector_double;
	
	typedef std::vector<std::pair<vector_double::size_type, vector_double::size_type>> sparsity_pattern;
	%rename(SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t) sparsity_pattern;
	//typedef std::vector<vector_double>::size_type pop_size_t;
	typedef std::vector<std::vector<double>> VectorOfVectorOfDoubles;
	


	enum class thread_safety { none, basic, constant };

	%extend default_bfe{
	vector_double Operator(const pagmoWrap::problemPagomWrapper& theProblem, const vector_double & values) const
	{
	   return self->operator()(static_cast<pagmo::problem>(theProblem), values);
	}
	};

	%extend member_bfe{
	vector_double Operator(const pagmoWrap::problemPagomWrapper& theProblem, const vector_double & values) const
	{
	   return self->operator()(static_cast<pagmo::problem>(theProblem), values);
	}
	};

	%extend thread_bfe{
	vector_double Operator(const pagmoWrap::problemPagomWrapper& theProblem, const vector_double & values) const
	{
	   return self->operator()(static_cast<pagmo::problem>(theProblem), values);
	}
	};

	using individuals_group_t = std::tuple<std::vector<unsigned long long>, std::vector<vector_double>, std::vector<vector_double>>;

	using migration_entry_t = std::tuple<double, unsigned long long, vector_double, vector_double, size_type, size_type>;

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
	%include swigInterfaceFiles\island.i
	%include swigInterfaceFiles\population.i
	%include swigInterfaceFiles\algorithm.i
	%include swigInterfaceFiles\archipelago.i
	%include swigInterfaceFiles\bfe.i
	//%include swigInterfaceFiles\exceptions.i // causing errors, not sure why, and not really implimented anyway
	//NOTE: pagmo.hpp, threading.hpp and types.hpp are not really needed
	%include swigInterfaceFiles\io.i
	%include swigInterfaceFiles\rng.i
	//%include swigInterfaceFiles\r_policy.i // needs the director/problem treatment
	%include swigInterfaceFiles\topology.i

	%include swigInterfaceFiles\algorithms\bee_colony.i
	%include swigInterfaceFiles\algorithms\cmaes.i
	%include swigInterfaceFiles\algorithms\compass_search.i		
	%include swigInterfaceFiles\algorithms\de.i
	%include swigInterfaceFiles\algorithms\de1220.i
	%include swigInterfaceFiles\algorithms\gaco.i
	%include swigInterfaceFiles\algorithms\gwo.i
	//%include swigInterfaceFiles\algorithms\ipopt.i // my build of pagmo doesn't include ipopt
	%include swigInterfaceFiles\algorithms\nlopt.i
	%include swigInterfaceFiles\algorithms\nspso.i
	%include swigInterfaceFiles\algorithms\pso.i
	%include swigInterfaceFiles\algorithms\pso_gen.i
	%include swigInterfaceFiles\algorithms\sea.i
	%include swigInterfaceFiles\algorithms\simulated_annealing.i
	%include swigInterfaceFiles\algorithms\sade.i
	%include swigInterfaceFiles\algorithms\sga.i
	%include swigInterfaceFiles\algorithms\xnes.i

	%include swigInterfaceFiles\batch_evaluators\default_bfe.i
	%include swigInterfaceFiles\batch_evaluators\member_bfe.i
	%include swigInterfaceFiles\batch_evaluators\thread_bfe.i

	//%include swigInterfaceFiles\detail\base_sr_policy.i // not sure if this is needed, and with no public constructors...
	//%include swigInterfaceFiles\detail\bfe_impl.i // not sure if this is needed, and with no public constructors...

	%include swigInterfaceFiles\islands\thread_island.i

	%include swigInterfaceFiles\problems\ackley.i
	%include swigInterfaceFiles\problems\cec2006.i
	%include swigInterfaceFiles\problems\golomb_ruler.i
	%include swigInterfaceFiles\problems\inventory.i
	%include swigInterfaceFiles\problems\minlp_rastrigin.i
	%include swigInterfaceFiles\problems\zdt.i

	%include swigInterfaceFiles\r_policies\fair_replace.i
	%include swigInterfaceFiles\s_policies\select_best.i
	%include swigInterfaceFiles\topologies\unconnected.i

	%include swigInterfaceFiles\utils\hv_algos\hv_algorithm.i
	//%include swigInterfaceFiles\utils\gradients_and_hessians.i // I couldn't get this to translate through swig so I just recreated the functions in C#
	%include swigInterfaceFiles\utils\hypervolume.i
	%include swigInterfaceFiles\utils\multi_objective.i
};


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


// TODO:
// Fix eigen3 path in c++ project
// algorithms:
//   ihs
//   nsga2
//   moead
//   moead_gen
//   maco
//   nspso
//   ipopt (if possible)
//   mbh
//   cstrs_self_adaptive
//   nlopt-augemented lagragian (might just need testing)

// Problems: Not as critical
//  griewank
//  hock_schittkowski_71
//  lennard_jones
//  luksan_vlcek1
//  rastrigin
//  rosenbrock
//  schwefel

// Problem suites
//   cec2006
//   cec2009
//   cec2013
//   cec2014
//   zdt (incomplete)
//   dtlz
//   wfg

// Meta Problems
//  decompose
//  translate
//  unconstrain

// other .hpp files (note that some [maybe many] are taken care of in various ways already or not truly needed)
//  utils/constrained.hpp
//  utils/discrepancy.hpp
//  utils/generic.hpp
//  utils/genetic_operators.hpp
//  utils/hv_algos/hv_bf_approx.hpp
//  utils/hv_algos/hv_bf_fpras.hpp
//  utils/hv_algos/hv_hv2d.hpp
//  utils/hv_algos/hv_hv3d.hpp
//  utils/hv_algos/hv_hvwfg.hpp
//  topologies/free_form.hpp
//  topologies/fully_connected.hpp
//  topologies/ring.hpp
//  detail/custom_comparison.hpp
//  detail/gte_getter.hpp
//  detail/prime_numbers.hpp
//  detail/task_queue.hpp

// NEEDS TESTING
//  utils/multi_objective.hpp
//  utils/bfe_impl.hpp
//  utils/base_sr_policy.hpp



//DONE BUT TAKING NOTE OF
// CUSTOM C++ TYPES HELPING OUT:
//  r_policy.hpp
//  s_policy.hpp
//  problem.hpp

// NOT NEEDED or too low priority (or I don't understand why they really are needed)
//  s11n.hpp (probably not, don't care about serialization)
//  detail/constants.hpp
//  detail/eigen.hpp
//  detail/eigen_s11n.hpp
//  detail/s1nn_wrappers.hpp
//  detail/support_xeus_cling.hpp
//  detail//type_names.hpp
//  detail/typeid_name_extract.hpp
//  detail/visibility.hpp

// IMPOSSIBLE TO COMPLETE (varaidec templates)
//  type_traits.hpp