%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/gwo.hpp"
#include "pagmo/rng.hpp"
%}

%typemap(csclassmodifiers) pagmo::gwo "public partial class"
%ignore gwo::get_log() const;

class gwo {
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

%extend gwo {
    std::vector<pagmoWrap::GwoLogEntry> get_log_entries() const
    {
        return pagmoWrap::Gwo_GetLogEntries(*self);
    }

    pagmo::algorithm to_algorithm() const
    {
        return pagmo::algorithm(*self);
    }
}

