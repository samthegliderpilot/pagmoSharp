%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/algorithms/de1220.hpp"
%}

%typemap(csclassmodifiers) pagmo::de1220 "public partial class"

class de1220 : public pagmo::algorithm {
public:
	typedef std::tuple<unsigned, unsigned long long, double, double, double, unsigned, double, double> log_line_type;
	typedef std::vector<log_line_type> log_type;
	typedef pop_size_t size_type;
	extern de1220(unsigned gen = 1u, std::vector<unsigned> allowed_variants = de1220_statics<void>::allowed_variants,
		unsigned variant_adptv = 1u, double ftol = 1e-6, double xtol = 1e-6, bool memory = false,
		unsigned seed = pagmo::random_device::next());


	extern population evolve(population) const;
	extern std::string get_name() const;
	extern void set_seed(unsigned);
	extern unsigned get_gen() const;
	extern unsigned get_seed() const;
	extern unsigned get_verbosity() const;
	extern void set_verbosity(unsigned);

	extern std::string get_extra_info() const;
	extern const log_type& get_log() const;
};