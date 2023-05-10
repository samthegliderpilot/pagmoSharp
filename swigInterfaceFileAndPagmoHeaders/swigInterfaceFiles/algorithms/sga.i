%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/sga.hpp"
%}
namespace detail
{
    enum class sga_selection { TOURNAMENT, TRUNCATED };

    enum class sga_crossover { EXPONENTIAL, BINOMIAL, SINGLE, SBX };

    enum class sga_mutation { GAUSSIAN, UNIFORM, POLYNOMIAL };

}
%typemap(csclassmodifiers) pagmo::sga "public partial class"
class sga : public pagmo::algorithm
{
public:
    /// Single entry of the log (gen, fevals, best, improvement)
    typedef std::tuple<unsigned, unsigned long long, double, double> log_line_type;
    /// The log
    typedef std::vector<log_line_type> log_type;


    extern sga(unsigned gen = 1u, double cr = .90, double eta_c = 1., double m = 0.02, double param_m = 1.,
        unsigned param_s = 2u, std::string crossover = "exponential", std::string mutation = "polynomial",
        std::string selection = "tournament", unsigned seed = pagmo::random_device::next());
    extern population evolve(population) const;
    extern void set_seed(unsigned);
    extern unsigned get_seed() const;
    extern void set_verbosity(unsigned level);
    extern unsigned get_verbosity() const;
    extern std::string get_name() const;
    extern std::string get_extra_info() const;
    extern const log_type& get_log() const;
};