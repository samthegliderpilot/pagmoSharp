%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/ihs.hpp"
%}

%typemap(csclassmodifiers) pagmo::ihs "public partial class"
%ignore pagmo::ihs::get_log() const;
class pagmo::ihs
{
public:
    typedef std::tuple<unsigned long long, double, double, double, double, pagmo::vector_double::size_type, double, pagmo::vector_double> log_line_type;
    typedef std::vector<log_line_type> log_type;

    extern ihs(unsigned gen = 1u, double phmcr = 0.85, double ppar_min = 0.35, double ppar_max = 0.99, double bw_min = 1E-5, double bw_max = 1., unsigned seed = pagmo::random_device::next());
    extern pagmo::population evolve(pagmo::population) const;
    extern void set_verbosity(unsigned);
    extern unsigned get_verbosity() const;
    extern void set_seed(unsigned);
    extern unsigned get_seed() const;
    extern std::string get_name() const;
    extern std::string get_extra_info() const;
    extern const log_type &get_log() const;
};

%extend pagmo::ihs {
    std::vector<pagmoWrap::IhsLogEntry> get_log_entries() const
    {
        return pagmoWrap::Ihs_GetLogEntries(*self);
    }

    pagmo::algorithm to_algorithm() const
    {
        return pagmo::algorithm(*self);
    }
}

