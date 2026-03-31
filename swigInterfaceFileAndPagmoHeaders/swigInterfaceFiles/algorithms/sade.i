%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/sade.hpp"
%}

%typemap(csclassmodifiers) pagmo::sade "public partial class"
%ignore sade::get_log() const;

class sade {
public:
	typedef std::tuple<unsigned, unsigned long long, double, double, double, double, double> log_line_type;
	typedef std::vector<log_line_type> log_type;
	typedef pop_size_t size_type;
	extern sade(unsigned gen = 1u, unsigned variant = 2u, unsigned variant_adptv = 1u, double ftol = 1e-6, double xtol = 1e-6, bool memory = false, unsigned seed = pagmo::random_device::next());

	extern population evolve(population) const;
	extern std::string get_name() const;
	extern void set_seed(unsigned);
	extern unsigned get_seed() const;
	extern unsigned get_verbosity() const;
	extern void set_verbosity(unsigned);
	extern unsigned get_gen() const;

	extern std::string get_extra_info() const;
	extern const log_type& get_log() const;
};

%extend sade {
    std::vector<pagmoWrap::SadeLogEntry> get_log_entries() const
    {
        return pagmoWrap::Sade_GetLogEntries(*self);
    }

    pagmo::algorithm to_algorithm() const
    {
        return pagmo::algorithm(*self);
    }
}

