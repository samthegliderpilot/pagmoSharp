%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/mbh.hpp"
%}

%typemap(csclassmodifiers) pagmo::mbh "public partial class"
%ignore pagmo::mbh::get_log() const;

class pagmo::mbh
{
public:
    typedef std::tuple<unsigned long long, double, pagmo::vector_double::size_type, double, unsigned> log_line_type;
    typedef std::vector<log_line_type> log_type;

    extern mbh();
    extern mbh(const pagmo::algorithm &a, unsigned stop, double perturb, unsigned seed = pagmo::random_device::next());
    extern mbh(const pagmo::algorithm &a, unsigned stop, pagmo::vector_double perturb, unsigned seed = pagmo::random_device::next());

    extern pagmo::population evolve(pagmo::population) const;
    extern void set_seed(unsigned);
    extern unsigned get_seed() const;
    extern void set_verbosity(unsigned level);
    extern unsigned get_verbosity() const;
    extern const pagmo::vector_double &get_perturb() const;
    extern void set_perturb(const pagmo::vector_double &);
    extern pagmo::thread_safety get_thread_safety() const;
    extern const pagmo::algorithm &get_inner_algorithm() const;
    extern pagmo::algorithm &get_inner_algorithm();
    extern const log_type &get_log() const;
    extern std::string get_name() const;
    extern std::string get_extra_info() const;
};

%extend pagmo::mbh {
    std::vector<pagmoWrap::MbhLogEntry> get_log_entries() const
    {
        return pagmoWrap::Mbh_GetLogEntries(*$self);
    }

    pagmo::algorithm to_algorithm() const
    {
        return pagmo::algorithm(*self);
    }
}

