%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/de.hpp"
%}

%typemap(csclassmodifiers) pagmo::de "public partial class"

class de : public pagmo::algorithm {
public:
    typedef std::tuple<unsigned, unsigned long long, double, double, double> log_line_type;
    typedef std::vector<log_line_type> log_type;

    extern de(unsigned gen = 1u, double F = 0.8, double CR = 0.9, unsigned variant = 2u, double ftol = 1e-6, double xtol = 1e-6, unsigned seed = pagmo::random_device::next());
    extern population evolve(population) const;
    extern void set_seed(unsigned);
    extern unsigned get_seed() const;
    extern void set_verbosity(unsigned level);
    extern unsigned get_verbosity() const;
    extern unsigned get_gen() const;
    extern std::string get_name() const;
    extern std::string get_extra_info() const;
    extern const log_type& get_log() const;
};