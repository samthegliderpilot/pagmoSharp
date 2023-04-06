%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/algorithms/gwo.hpp"
#include "pagmo/rng.hpp"
%}

%typemap(csclassmodifiers) pagmo::gwo "public partial class"

class gwo : public pagmo::algorithm {
public:
    typedef std::tuple<unsigned, double, double, double> log_line_type;
    typedef std::vector<log_line_type> log_type;
    
    extern gwo(unsigned gen = 1u, unsigned seed = pagmo::random_device::next());
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