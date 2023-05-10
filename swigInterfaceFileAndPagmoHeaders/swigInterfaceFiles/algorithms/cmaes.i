%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/cmaes.hpp"
%}

%typemap(csclassmodifiers) pagmo::cmaes "public partial class"
class cmaes : public pagmo::algorithm
{
public:
    typedef std::tuple<unsigned, unsigned long long, double, double, double, double> log_line_type;
    typedef std::vector<log_line_type> log_type;

	extern cmaes(unsigned gen = 1, double cc = -1, double cs = -1, double c1 = -1, double cmu = -1, double sigma0 = 0.5,
        double ftol = 1e-6, double xtol = 1e-6, bool memory = false, bool force_bounds = false,
        unsigned seed = pagmo::random_device::next());
    extern population evolve(population) const;
    extern void set_seed(unsigned);
    extern unsigned get_seed() const;
    extern void set_verbosity(unsigned level);
    extern unsigned get_verbosity() const;
    extern unsigned get_gen() const;
    extern void set_bfe(const bfe& b);
    extern std::string get_name() const;
    extern std::string get_extra_info() const;
    extern const log_type& get_log() const;
};