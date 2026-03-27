%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/moead.hpp"
%}

%typemap(csclassmodifiers) pagmo::moead "public partial class"

class moead
{
public:
    typedef std::tuple<unsigned, unsigned long long, double, vector_double> log_line_type;
    typedef std::vector<log_line_type> log_type;

    extern moead(unsigned gen = 1u, std::string weight_generation = "grid", std::string decomposition = "tchebycheff",
        population::size_type neighbours = 20u, double CR = 1.0, double F = 0.5, double eta_m = 20.,
        double realb = 0.9, unsigned limit = 2u, bool preserve_diversity = true,
        unsigned seed = pagmo::random_device::next());

    extern population evolve(population) const;
    extern void set_seed(unsigned);
    extern unsigned get_seed() const;
    extern void set_verbosity(unsigned level);
    extern unsigned get_verbosity() const;
    extern unsigned get_gen() const;
    extern std::string get_name() const;
    extern std::string get_extra_info() const;
    extern const log_type &get_log() const;
};

%extend moead {
    pagmo::algorithm to_algorithm() const
    {
        return pagmo::algorithm(*self);
    }
}
