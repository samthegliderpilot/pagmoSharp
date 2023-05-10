%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/sea.hpp"
%}

%typemap(csclassmodifiers) pagmo::sea "public partial class"
class sea : public pagmo::algorithm
{
public:
    typedef std::tuple<unsigned, unsigned long long, double, double, vector_double::size_type> log_line_type;
    typedef std::vector<log_line_type> log_type;

	extern sea(unsigned gen = 1u, unsigned seed = pagmo::random_device::next());
    extern population evolve(population) const;
    extern void set_verbosity(unsigned level);
    extern unsigned get_verbosity() const;
    extern void set_seed(unsigned);
    extern unsigned get_seed() const;
    extern std::string get_name() const;
    extern std::string get_extra_info() const;
    extern const log_type& get_log() const;
};