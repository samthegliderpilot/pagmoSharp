%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/sga.hpp"
%}

// SGA enum types — PascalCase names for C# (R1 compliance).
%rename(SgaSelection)  detail::sga_selection;
%rename(Tournament)    detail::sga_selection::TOURNAMENT;
%rename(Truncated)     detail::sga_selection::TRUNCATED;
%rename(SgaCrossover)  detail::sga_crossover;
%rename(Exponential)   detail::sga_crossover::EXPONENTIAL;
%rename(Binomial)      detail::sga_crossover::BINOMIAL;
%rename(Single)        detail::sga_crossover::SINGLE;
%rename(Sbx)           detail::sga_crossover::SBX;
%rename(SgaMutation)   detail::sga_mutation;
%rename(Gaussian)      detail::sga_mutation::GAUSSIAN;
%rename(Uniform)       detail::sga_mutation::UNIFORM;
%rename(Polynomial)    detail::sga_mutation::POLYNOMIAL;

namespace detail
{
    enum class sga_selection { TOURNAMENT, TRUNCATED };

    enum class sga_crossover { EXPONENTIAL, BINOMIAL, SINGLE, SBX };

    enum class sga_mutation { GAUSSIAN, UNIFORM, POLYNOMIAL };

}
%typemap(csclassmodifiers) pagmo::sga "public partial class"
%ignore sga::get_log() const;
class sga
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

%extend sga {
    std::vector<pagmoWrap::SgaLogEntry> get_log_entries() const
    {
        return pagmoWrap::Sga_GetLogEntries(*self);
    }

    pagmo::algorithm to_algorithm() const
    {
        return pagmo::algorithm(*self);
    }
}

