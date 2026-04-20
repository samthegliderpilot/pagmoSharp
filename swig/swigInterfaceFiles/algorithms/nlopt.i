%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/nlopt.hpp"
%}

%typemap(csclassmodifiers) pagmo::nlopt "public partial class"
%ignore nlopt::get_log() const;
%ignore nlopt::nlopt(nlopt &&);
%ignore nlopt::operator=;

class nlopt {
public:
    typedef std::tuple<unsigned long, double, vector_double::size_type, double, bool> log_line_type;
    typedef std::vector<log_line_type> log_type;

    extern nlopt();
    extern nlopt(const std::string &);
    extern nlopt(const nlopt &);
    extern population evolve(population) const;
    extern std::string get_name() const;
    extern void set_verbosity(unsigned n);
    extern std::string get_extra_info() const;
    extern const log_type &get_log() const;
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
    extern const nlopt *get_local_optimizer() const;
    extern void unset_local_optimizer();
};

%extend nlopt {
    std::vector<pagmoWrap::NloptLogEntry> get_log_entries() const
    {
        return pagmoWrap::Nlopt_GetLogEntries(*self);
    }

    pagmo::algorithm to_algorithm() const
    {
        return pagmo::algorithm(*self);
    }
}
