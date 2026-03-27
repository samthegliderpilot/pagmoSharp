%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/cstrs_self_adaptive.hpp"
%}

%typemap(csclassmodifiers) pagmo::cstrs_self_adaptive "public partial class"

class cstrs_self_adaptive
{
public:
    typedef std::tuple<unsigned, unsigned long long, double, double, vector_double::size_type, double,
        vector_double::size_type> log_line_type;
    typedef std::vector<log_line_type> log_type;

    extern cstrs_self_adaptive(unsigned iters = 1u);
    extern cstrs_self_adaptive(unsigned iters, const algorithm &a, unsigned seed = pagmo::random_device::next());

    extern population evolve(population) const;
    extern void set_seed(unsigned);
    extern unsigned get_seed() const;
    extern void set_verbosity(unsigned level);
    extern unsigned get_verbosity() const;

    extern std::string get_name() const;
    extern std::string get_extra_info() const;
    extern thread_safety get_thread_safety() const;

    extern const algorithm &get_inner_algorithm() const;
    extern algorithm &get_inner_algorithm();
    extern const log_type &get_log() const;
};
