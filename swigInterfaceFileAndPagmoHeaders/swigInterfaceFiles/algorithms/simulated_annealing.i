%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/simulated_annealing.hpp"
%}

%typemap(csclassmodifiers) pagmo::simulated_annealing "public partial class"

class simulated_annealing : public pagmo::algorithm {
public:
    typedef std::tuple<unsigned long long, double, double, double, double> log_line_type;
    typedef std::vector<log_line_type> log_type;

    extern simulated_annealing(double Ts = 10., double Tf = .1, unsigned n_T_adj = 10u, unsigned n_range_adj = 1u,
        unsigned bin_size = 20u, double start_range = 1., unsigned seed = pagmo::random_device::next());

    // Algorithm evolve method
    extern population evolve(population) const;
    extern void set_verbosity(unsigned level);
    extern unsigned get_verbosity() const;
    extern void set_seed(unsigned);
    extern unsigned get_seed() const;
    extern std::string get_name() const;
    extern std::string get_extra_info() const;
    extern const log_type& get_log() const;
};