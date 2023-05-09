﻿/* File pagmoSharpSwigInterface.i */

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
%}

// The whole problem vs. problemBase question is a little confusing.  To make it better (or worse)
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

//#include <tuple> // tuple is not supported by swig yet...
%apply void *VOID_INT_PTR { void * }
namespace std {
	%template(DoubleVector) std::vector<double>;
	%template(ULongLongVector) std::vector<unsigned long long>;
	%template(VectorDoubleVector) std::vector<std::vector<double>>;
	%template(PairOfDoubleVectors) std::pair<std::vector<double>, std::vector<double>>;
}

namespace pagmo {
	%typemap(csclassmodifiers) pagmo::DoubleVector "public partial class"
	typedef std::vector<double> vector_double;
	typedef std::vector<std::pair<vector_double::size_type, vector_double::size_type>> sparsity_pattern;
	typedef std::vector<vector_double>::size_type pop_size_t;

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
	//%include swigInterfaceFiles\rng.i	%include swigInterfaceFiles\exceptions.i
	
	%include swigInterfaceFiles\topology.i

	%include swigInterfaceFiles\batch_evaluators\default_bfe.i
	%include swigInterfaceFiles\batch_evaluators\member_bfe.i
	%include swigInterfaceFiles\batch_evaluators\thread_bfe.i

	%include swigInterfaceFiles\islands\thread_island.i

	%include swigInterfaceFiles\algorithms\bee_colony.i
	%include swigInterfaceFiles\algorithms\compass_search.i			%include swigInterfaceFiles\algorithms\de.i
	%include swigInterfaceFiles\algorithms\de1220.i
	%include swigInterfaceFiles\algorithms\gaco.i
	%include swigInterfaceFiles\algorithms\gwo.i
	//%include swigInterfaceFiles\algorithms\ipopt.i
	%include swigInterfaceFiles\algorithms\nlopt.i
	%include swigInterfaceFiles\algorithms\pso.i
	%include swigInterfaceFiles\algorithms\pso_gen.i
	%include swigInterfaceFiles\algorithms\simulated_annealing.i
	%include swigInterfaceFiles\algorithms\sade.i

	%include swigInterfaceFiles\problems\ackley.i
	%include swigInterfaceFiles\problems\cec2006.i
	%include swigInterfaceFiles\problems\golomb_ruler.i
	%include swigInterfaceFiles\problems\inventory.i
	

	%include swigInterfaceFiles\utils\hv_algos\hv_algorithm.i
	%include swigInterfaceFiles\utils\gradients_and_hessians.i
	%include swigInterfaceFiles\utils\hypervolume.i


}

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


