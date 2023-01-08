%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/population.hpp"
%}

%typemap(csclassmodifiers) pagmo::population "public partial class"

class population {
	typedef std::vector<vector_double>::size_type pop_size_t;
	typedef pop_size_t size_type;
public:
	template <typename T, generic_ctor_enabler<T> = 0>
	extern population(T&& x, size_type pop_size = 0u, unsigned seed = pagmo::random_device::next());

	extern population(pagmoWrap::problemPagomWrapper x, size_type pop_size = 0u, unsigned seed = pagmo::random_device::next());
	extern void push_back(const vector_double&);
	extern void push_back(const vector_double&, const vector_double&);
	extern vector_double random_decision_vector() const;
	extern size_type best_idx() const;
	extern size_type best_idx(const vector_double&) const;
	extern size_type best_idx(double) const;
	extern size_type worst_idx() const;
	extern size_type worst_idx(const vector_double&) const;
	extern size_type worst_idx(double) const;
	extern vector_double champion_x() const;
	extern vector_double champion_f() const;
	extern size_type size() const;
	extern void set_xf(size_type, const vector_double&, const vector_double&);
	extern void set_x(size_type, const vector_double&);
	extern const pagmoWrap::problemPagomWrapper& get_problem() const;
	extern const std::vector<vector_double>& get_f() const;
	extern const std::vector<vector_double>& get_x() const;
	extern const std::vector<unsigned long long>& get_ID() const;
	extern unsigned get_seed() const;

	template <typename Archive>
	extern void save(Archive& ar, unsigned) const;

	template <typename Archive>
	extern void load(Archive& ar, unsigned);
};