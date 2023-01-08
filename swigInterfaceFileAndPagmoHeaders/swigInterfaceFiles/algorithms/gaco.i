%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/algorithms/gaco.hpp"
#include "pagmo/rng.hpp"
%}

%typemap(csclassmodifiers) pagmo::gaco "public partial class"

class gaco : public pagmo::algorithm {
public:
	typedef pop_size_t size_type;
	typedef std::tuple<unsigned, vector_double::size_type, double, unsigned, double, double, double> log_line_type;
	extern gaco(unsigned gen = 1u, unsigned ker = 63u, double q = 1.0, double oracle = 0., double acc = 0.01, unsigned threshold = 1u, unsigned n_gen_mark = 7u, unsigned impstop = 100000u, unsigned evalstop = 100000u, double focus = 0., bool memory = false, unsigned seed = pagmo::random_device::next());

	extern unsigned get_gen() const;

	extern population evolve(population) const;
	extern std::string get_name() const;
	extern void set_seed(unsigned);
	extern unsigned get_seed() const;
	extern unsigned get_verbosity() const;
	extern void set_verbosity(unsigned);

	extern void set_bfe(const pagmo::bfe& b);
	extern std::string get_extra_info() const;
	typedef std::vector<log_line_type> log_type;
	extern const log_type& get_log() const;
};