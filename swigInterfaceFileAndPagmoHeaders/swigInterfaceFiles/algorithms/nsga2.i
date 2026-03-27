%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/nsga2.hpp"
%}

%typemap(csclassmodifiers) pagmo::nsga2 "public partial class"

class nsga2
{
public:
    typedef std::tuple<unsigned, unsigned long long, vector_double> log_line_type;
    typedef std::vector<log_line_type> log_type;

    extern nsga2(unsigned gen = 1u, double cr = 0.95, double eta_c = 10., double m = 0.01, double eta_m = 50.,
        unsigned seed = pagmo::random_device::next());

    extern population evolve(population) const;
    extern void set_seed(unsigned);
    extern unsigned get_seed() const;
    extern void set_verbosity(unsigned level);
    extern unsigned get_verbosity() const;
    extern void set_bfe(const bfe &b);
    extern std::string get_name() const;
    extern std::string get_extra_info() const;
    extern const log_type &get_log() const;
};

%extend nsga2 {
    pagmo::algorithm to_algorithm() const
    {
        return pagmo::algorithm(*self);
    }
}
