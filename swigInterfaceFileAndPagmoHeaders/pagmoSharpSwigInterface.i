/* File pagmoSharpSwigInterface.i */

%module(naturalvar=1, directors="8") pagmo
%{
	#include "pagmo/types.hpp"
	#include "pagmo/bfe.hpp"
	#include "pagmo/batch_evaluators/default_bfe.hpp"
	#include "pagmo/batch_evaluators/thread_bfe.hpp"
	#include "pagmo/batch_evaluators/member_bfe.hpp"
	#include "pagmo/algorithm.hpp"
	#include "pagmo/population.hpp"
	#include "pagmo/threading.hpp" 
	#include "pagmo/problem.hpp"
	#include "pagmo/island.hpp"
	#include "pagmo/islands/thread_island.hpp"
	//#include "pagmo/islands/fork_island.hpp"
	#include "pagmo/bfe.hpp" 
	#include "problem.h" // this is a manually created item.  We want to include it in the wrappers so the generated cxx code can use the handwritten code for the problem

	#include "pagmo/problems/golomb_ruler.hpp"
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
%typemap(csclassmodifiers) pagmo::thread_island "public partial class"
%typemap(csclassmodifiers) pagmo::DoubleVector "public partial class"
%typemap(csclassmodifiers) pagmo::population "public partial class"
//%typemap(csclassmodifiers) pagmo::fork_island "public partial class"
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

	class bfe {
		virtual std::string get_name() const = 0;
	};

	class default_bfe :public bfe {
	public:
		extern default_bfe();
		extern default_bfe(const default_bfe&);
		extern std::string get_name() const;
	};

	class thread_bfe :public bfe {
	public:
		extern thread_bfe();
		extern thread_bfe(const thread_bfe&);
		extern std::string get_name() const;
	};

	class member_bfe :public bfe {
		extern member_bfe();
		extern member_bfe(const member_bfe&);
		extern std::string get_name() const;
	};

	class island {
	public:
		//virtual void run_evolve(island& isl) const = 0;
		//virtual std::string get_name() const = 0;
	};

	class thread_island : public pagmo::island {
	public:
		extern thread_island();
		extern std::string get_name() const;
		extern std::string get_extra_info() const;
		extern void run_evolve(island& isl) const;
	};

	//TODO: fork_island isn't avaialble on the platform...
	//class fork_island {
	//public:
	//	extern fork_island();
	//	extern fork_island(const fork_island&);
	//	extern fork_island(fork_island&&);
	//	extern std::string get_name() const;
	//	extern std::string get_extra_info() const;
	//	extern void run_evolve(island& isl) const;
	//	//extern pid_t get_child_pid() const
	//};




	class population {
		typedef std::vector<vector_double>::size_type pop_size_t;
		typedef pop_size_t size_type;
	public:
		template <typename T, generic_ctor_enabler<T> = 0>
		extern population(T&& x, size_type pop_size = 0u, unsigned seed = pagmo::random_device::next());

		extern population(pagmoWrap::problemPagomWrapper x, size_type pop_size = 0u, unsigned seed = pagmo::random_device::next());
		extern void push_back(const vector_double&);
		extern void push_back(const vector_double&, const vector_double&);
		extern vector_double random_decision_vector() const;
		extern size_type best_idx() const;
		extern size_type best_idx(const vector_double&) const;
		extern size_type best_idx(double) const;
		extern size_type worst_idx() const;
		extern size_type worst_idx(const vector_double&) const;
		extern size_type worst_idx(double) const;
		extern vector_double champion_x() const;
		extern vector_double champion_f() const;
		extern size_type size() const;
		extern void set_xf(size_type, const vector_double&, const vector_double&);
		extern void set_x(size_type, const vector_double&);
		extern const pagmoWrap::problemPagomWrapper& get_problem() const;
		extern const std::vector<vector_double>& get_f() const;
		extern const std::vector<vector_double>& get_x() const;
		extern const std::vector<unsigned long long>& get_ID() const;
		extern unsigned get_seed() const;

		template <typename Archive>
		extern void save(Archive& ar, unsigned) const;

		template <typename Archive>
		extern void load(Archive& ar, unsigned);
	};

	class algorithm {
		extern bool has_set_verbosity() const;
		extern bool has_set_seed() const;
		extern thread_safety get_thread_safety() const;

		virtual population evolve(population) const = 0;
		virtual std::string get_name() const = 0;

		void set_seed(unsigned);
		unsigned get_seed() const;
		unsigned get_verbosity() const;
		void set_verbosity(unsigned);

		extern void set_bfe(const pagmo::bfe& b);
		std::string get_extra_info() const;

		//typedef std::vector<log_line_type> log_type;
		//const log_type& get_log() const;

		//template <typename Archive>
		//void serialize(Archive&, unsigned);
	};

	%include swigInterfaceFiles\algorithms\de.i
	%include swigInterfaceFiles\algorithms\de1220.i
	%include swigInterfaceFiles\algorithms\gaco.i
	%include swigInterfaceFiles\algorithms\sade.i

	%include swigInterfaceFiles\problems\golomb_ruler.i
};

