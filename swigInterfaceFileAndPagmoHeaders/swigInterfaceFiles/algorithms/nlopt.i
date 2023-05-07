
%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/nlopt.hpp"
//#include "nloptGenerated.cxx"
SWIGEXPORT void SWIGSTDCALL CSharp_pagmo_nlopt_set_local_optimizer(void* jarg1, void* jarg2) {
    pagmo::nlopt* arg1 = (pagmo::nlopt*)0;
    arg1 = (pagmo::nlopt*)jarg1;

    pagmo::nlopt* arg2 = (pagmo::nlopt*)0;
    arg2 = (pagmo::nlopt*)jarg2;
    (arg1)->set_local_optimizer(*arg2);
}
%}

//%nodefaultctor nlopt;
%ignore nlopt::set_local_optimizer;
%typemap(csclassmodifiers) nlopt "public partial class"
class nlopt : public pagmo::algorithm {
public:
    using log_line_type = std::tuple<unsigned long, double, vector_double::size_type, double, bool>;
    using log_type = std::vector<log_line_type>;

    extern nlopt();
    extern nlopt(const std::string&);
    extern nlopt(const nlopt&);
    extern nlopt(nlopt&&) = default;
    extern nlopt& operator=(nlopt&&) = default;


    extern population evolve(population) const;
    extern std::string get_name() const;
    extern void set_verbosity(unsigned n);
    extern std::string get_extra_info() const;
    extern const log_type& get_log() const;
    extern std::string get_solver_name() const;
    extern ::nlopt_result get_last_opt_result() const;
    extern double get_stopval() const;
    extern void set_stopval(double);
    extern double get_ftol_rel() const;
    extern void set_ftol_rel(double);
    extern double get_ftol_abs() const;
    extern void set_ftol_abs(double);
    extern double get_xtol_rel() const;
    extern void set_xtol_rel(double);
    extern double get_xtol_abs() const;
    extern void set_xtol_abs(double);
    extern int get_maxeval() const;
    extern void set_maxeval(int n);
    extern int get_maxtime() const;
    extern void set_maxtime(int n);
    // not supported yet, update in swig might fix, current fix is to save off a (seemingly) 
    // working, hand made function to do this
	//extern void set_local_optimizer(pagmo::nlopt);    
    extern const nlopt *get_local_optimizer() const;
    extern nlopt *get_local_optimizer();
    extern void unset_local_optimizer();
};

// this is working around a swig bug where default constructors are made
// when they shouldn't be.  We need to ignore the set_local_optimizer function
// and create our own (until that bug is fixed)

//%rename("set_local_optimizer") nlopt::set_local_optimizer;
//%newobject nlop::set_local_optimizer;
//%extend nlopt
//{
//    void set_local_optimizer(nlopt that)
//    {
//    	$self->set_local_optimizer(that);
//    }
//}
/*
SWIGEXPORT void SWIGSTDCALL CSharp_pagmo_nlopt_set_local_optimizer(void* jarg1, void* jarg2) {
    pagmo::nlopt* arg1 = (pagmo::nlopt*)0;
    arg1 = (pagmo::nlopt*)jarg1;

    pagmo::nlopt* arg2 = (pagmo::nlopt*)0;
    arg2 = (pagmo::nlopt*)jarg2;
    (arg1)->set_local_optimizer(*arg2);
}
* */


