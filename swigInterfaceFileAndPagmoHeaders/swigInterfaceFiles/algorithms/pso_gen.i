%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/pso_gen.hpp"
%}

%typemap(csclassmodifiers) pagmo::pso_gen "public partial class"
class pso_gen : public pagmo::algorithm
{
public:
    typedef std::tuple<unsigned, unsigned long long, double, double, double, double> log_line_type;
    typedef std::vector<log_line_type> log_type;


    extern pso_gen(unsigned gen = 1u, double omega = 0.7298, double eta1 = 2.05, double eta2 = 2.05, double max_vel = 0.5,
        unsigned variant = 5u, unsigned neighb_type = 2u, unsigned neighb_param = 4u, bool memory = false,
        unsigned seed = pagmo::random_device::next());

    extern population evolve(population) const;
    extern void set_verbosity(unsigned level);
    extern unsigned get_verbosity() const;
    extern void set_bfe(const bfe& b);
    extern void set_seed(unsigned);
    extern unsigned get_seed() const;
    extern std::string get_name() const;
    extern std::string get_extra_info() const;
    extern const log_type& get_log() const;
};