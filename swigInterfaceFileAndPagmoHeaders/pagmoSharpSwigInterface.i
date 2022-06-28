/* File pagmoSharpSwigInterface.i */
 
%module(naturalvar=1, directors="2") pagmo
%{
 #include "pagmo/types.hpp"
 #include "pagmo/bfe.hpp"
 #include "pagmo/batch_evaluators/default_bfe.hpp"
 #include "pagmo/batch_evaluators/thread_bfe.hpp"
 #include "pagmo/batch_evaluators/member_bfe.hpp"
 #include "pagmo/population.hpp"
 #include "pagmo/algorithms/gaco.hpp"
 #include "pagmo/threading.hpp" 
 #include "pagmo/problem.hpp"
 #include "pagmo/bfe.hpp" // this is a manually created item. 
 #include "problem.h" // this is a manually created item.  We want to include it in the wrappers so the generated cxx code can use the handwritten code for the problem
%}

// The whole problem vs. problemBase question is a little confusing.  To make it better (or worse)
// there is a C# implicit operator to convert from problem to problemBase by calling getBaseProblem on 
// the wrapper (problem).  Hence the partial class here
%typemap(csclassmodifiers) pagmoWrap::problem "public partial class"
%typemap(csclassmodifiers) pagmoWrap::problemBase "public partial class"


%feature("director") pagmoWrap::problemBase;
%feature("director") pagmoWrap::bfeBase;
%include "std_string.i"
%include "std_vector.i"
%include "std_pair.i"
%include "pagmoWrapper/problem.h"
%include "pagmoWrapper/bfe.h"

//#include <tuple> // tuple is not supported by swig yet...
%apply void *VOID_INT_PTR { void * }
namespace std {
	%template(DoubleVector) std::vector<double>;
	%template(ULongLongVector) std::vector<unsigned long long>;
	%template(VectorDoubleVector) std::vector<std::vector<double>>;
	%template(PairOfDoubleVectors) std::pair<std::vector<double>, std::vector<double>>;
	//%template(gacoLogLineType) std::tuple<unsigned, vector_double::size_type, double, unsigned, double, double, double>;
}

namespace pagmo {
	%typemap(csclassmodifiers) pagmo::DoubleVector "public partial class"
	typedef std::vector<double> vector_double;
    typedef std::vector<std::pair<vector_double::size_type, vector_double::size_type>> sparsity_pattern;
    typedef std::vector<vector_double>::size_type pop_size_t;
	
enum class thread_safety { none, basic, constant };


%extend default_bfe {
vector_double Operator(const pagmoWrap::problem & theProblem, const vector_double & values) const
{
   return self->operator()(static_cast<pagmo::problem>(theProblem), values);
}
}

%extend member_bfe {
vector_double Operator(const pagmoWrap::problem & theProblem, const vector_double & values) const
{
   return self->operator()(static_cast<pagmo::problem>(theProblem), values);
}
}

%extend thread_bfe {
vector_double Operator(const pagmoWrap::problem & theProblem, const vector_double & values) const
{
   return self->operator()(static_cast<pagmo::problem>(theProblem), values);
}
}

class default_bfe : public bfe {
public:  
    extern default_bfe();
    extern default_bfe(const default_bfe&);
    extern std::string get_name() const;
};

class thread_bfe : public bfe {
public:
    extern thread_bfe();
    extern thread_bfe(const thread_bfe&);
    extern std::string get_name() const;
};

class member_bfe : public bfe {
public:
    extern member_bfe();
    extern member_bfe(const member_bfe&);
    extern std::string get_name() const;
};

class population {
typedef std::vector<vector_double>::size_type pop_size_t;
typedef pop_size_t size_type;
public:	
    template <typename T, generic_ctor_enabler<T> = 0>
    extern population(T &&x, size_type pop_size = 0u, unsigned seed = pagmo::random_device::next());
	
	extern population(pagmoWrap::problem x, size_type pop_size = 0u, unsigned seed = pagmo::random_device::next());
    extern void push_back(const vector_double &);
    extern void push_back(const vector_double &, const vector_double &);
    extern vector_double random_decision_vector() const;
    extern size_type best_idx() const;
    extern size_type best_idx(const vector_double &) const;
    extern size_type best_idx(double) const;
    extern size_type worst_idx() const;
    extern size_type worst_idx(const vector_double &) const;
    extern size_type worst_idx(double) const;
    extern vector_double champion_x() const;    
    extern vector_double champion_f() const;
    extern size_type size() const;
    extern void set_xf(size_type, const vector_double &, const vector_double &);
    extern void set_x(size_type, const vector_double &);
    extern const pagmoWrap::problem &get_problem() const;
    extern const std::vector<vector_double> &get_f() const;
    extern const std::vector<vector_double> &get_x() const;
    extern const std::vector<unsigned long long> &get_ID() const;
    extern unsigned get_seed() const;
    
	template <typename Archive>
    extern void save(Archive &ar, unsigned) const;
    
	template <typename Archive>
    extern void load(Archive &ar, unsigned);
};

class gaco : pagmo::algorithm  {
	typedef std::tuple<unsigned, vector_double::size_type, double, unsigned, double, double, double> log_line_type;	
public:	
	typedef pop_size_t size_type;
	extern gaco(unsigned gen = 1u, unsigned ker = 63u, double q = 1.0, double oracle = 0., double acc = 0.01, unsigned threshold = 1u, unsigned n_gen_mark = 7u, unsigned impstop = 100000u, unsigned evalstop = 100000u, double focus = 0., bool memory = false, unsigned seed = pagmo::random_device::next());
	extern population evolve(population) const;
	extern void set_seed(unsigned);
	extern unsigned get_seed() const;
	extern unsigned get_verbosity() const;
	extern unsigned get_gen() const;
	extern void set_bfe(const pagmo::bfe &b);
	extern std::string get_name() const;
	extern std::string get_extra_info() const;



	typedef std::vector<log_line_type> log_type;
	extern const log_type &get_log() const;

	template <typename Archive>
	extern void serialize(Archive &, unsigned);
};
};

