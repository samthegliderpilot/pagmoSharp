%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/mbh.hpp"
%}

%typemap(csclassmodifiers) pagmo::mbh "public partial class"

class mbh
{
public:
    typedef std::tuple<unsigned long long, double, vector_double::size_type, double, unsigned> log_line_type;
    typedef std::vector<log_line_type> log_type;

    extern mbh();
    extern mbh(const algorithm &a, unsigned stop, double perturb, unsigned seed = pagmo::random_device::next());
    extern mbh(const algorithm &a, unsigned stop, vector_double perturb, unsigned seed = pagmo::random_device::next());

    extern population evolve(population) const;
    extern void set_seed(unsigned);
    extern unsigned get_seed() const;
    extern void set_verbosity(unsigned level);
    extern unsigned get_verbosity() const;
    extern const vector_double &get_perturb() const;
    extern void set_perturb(const vector_double &);
    extern thread_safety get_thread_safety() const;
    extern const algorithm &get_inner_algorithm() const;
    extern algorithm &get_inner_algorithm();
    extern const log_type &get_log() const;
    extern std::string get_name() const;
    extern std::string get_extra_info() const;
};

%extend mbh {
    pagmo::algorithm to_algorithm() const
    {
        return pagmo::algorithm(*self);
    }
}
