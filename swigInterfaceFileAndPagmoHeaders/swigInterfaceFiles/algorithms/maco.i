%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/maco.hpp"
%}

%typemap(csclassmodifiers) pagmo::maco "public partial class"
%ignore pagmo::maco::get_log() const;

class pagmo::maco
{
public:
    typedef std::tuple<unsigned, unsigned long long, pagmo::vector_double> log_line_type;
    typedef std::vector<log_line_type> log_type;

    extern maco(unsigned gen = 1u, unsigned ker = 63u, double q = 1.0, unsigned threshold = 1u,
        unsigned n_gen_mark = 7u, unsigned evalstop = 100000u, double focus = 0., bool memory = false,
        unsigned seed = pagmo::random_device::next());
    extern pagmo::population evolve(pagmo::population) const;
    extern void set_seed(unsigned);
    extern unsigned get_seed() const;
    extern void set_verbosity(unsigned level);
    extern unsigned get_verbosity() const;
    extern unsigned get_gen() const;
    extern void set_bfe(const pagmo::bfe &b);
    extern std::string get_name() const;
    extern std::string get_extra_info() const;
    extern const log_type &get_log() const;
};

%extend pagmo::maco {
    std::vector<pagmoWrap::MoVectorLogEntry> get_log_entries() const
    {
        return pagmoWrap::Maco_GetLogEntries(*self);
    }

    pagmo::algorithm to_algorithm() const
    {
        return pagmo::algorithm(*self);
    }
}

