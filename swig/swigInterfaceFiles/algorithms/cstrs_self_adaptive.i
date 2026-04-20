%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/cstrs_self_adaptive.hpp"
%}

%typemap(csclassmodifiers) pagmo::cstrs_self_adaptive "public partial class"
%ignore pagmo::cstrs_self_adaptive::get_log() const;

class pagmo::cstrs_self_adaptive
{
public:
    typedef std::tuple<unsigned, unsigned long long, double, double, pagmo::vector_double::size_type, double,
        pagmo::vector_double::size_type> log_line_type;
    typedef std::vector<log_line_type> log_type;

    extern cstrs_self_adaptive(unsigned iters = 1u);
    extern cstrs_self_adaptive(unsigned iters, const pagmo::algorithm &a, unsigned seed = pagmo::random_device::next());

    extern pagmo::population evolve(pagmo::population) const;
    extern void set_seed(unsigned);
    extern unsigned get_seed() const;
    extern void set_verbosity(unsigned level);
    extern unsigned get_verbosity() const;

    extern std::string get_name() const;
    extern std::string get_extra_info() const;
    extern pagmo::thread_safety get_thread_safety() const;

    extern const pagmo::algorithm &get_inner_algorithm() const;
    extern const log_type &get_log() const;
};

%extend pagmo::cstrs_self_adaptive {
    std::vector<pagmoWrap::CstrsLogEntry> get_log_entries() const
    {
        return pagmoWrap::Cstrs_GetLogEntries(*self);
    }

    pagmo::algorithm to_algorithm() const
    {
        return pagmo::algorithm(*self);
    }
}

