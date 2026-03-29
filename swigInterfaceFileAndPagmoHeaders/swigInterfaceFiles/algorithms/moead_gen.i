%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/moead_gen.hpp"
%}

%typemap(csclassmodifiers) pagmo::moead_gen "public partial class"

class pagmo::moead_gen
{
public:
    typedef std::tuple<unsigned, unsigned long long, double, pagmo::vector_double> log_line_type;
    typedef std::vector<log_line_type> log_type;

    extern moead_gen(unsigned gen = 1u, std::string weight_generation = "grid", std::string decomposition = "tchebycheff",
        pagmo::population::size_type neighbours = 20u, double CR = 1.0, double F = 0.5, double eta_m = 20.,
        double realb = 0.9, unsigned limit = 2u, bool preserve_diversity = true,
        unsigned seed = pagmo::random_device::next());

    extern pagmo::population evolve(pagmo::population) const;
    extern void set_seed(unsigned);
    extern unsigned get_seed() const;
    extern void set_verbosity(unsigned level);
    extern unsigned get_verbosity() const;
    extern void set_bfe(const pagmo::bfe &b);
    extern unsigned get_gen() const;
    extern std::string get_name() const;
    extern std::string get_extra_info() const;
    extern const log_type &get_log() const;
};

%extend pagmo::moead_gen {
    std::vector<pagmoWrap::MoeadLogEntry> get_log_entries() const
    {
        return pagmoWrap::MoeadGen_GetLogEntries(*self);
    }

    pagmo::algorithm to_algorithm() const
    {
        return pagmo::algorithm(*self);
    }
}
