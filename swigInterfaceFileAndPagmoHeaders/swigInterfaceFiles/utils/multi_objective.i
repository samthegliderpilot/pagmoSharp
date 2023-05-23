%module multi_objective
%{
#include <cmath>
#include <numeric>
#include <random>
#include <sstream>
#include <stdexcept>
#include <string>
#include <tuple>
#include <vector>

#include "boost/numeric/conversion/cast.hpp"

#include "pagmo/detail/visibility.hpp"
#include "pagmo/exceptions.hpp"
#include "pagmo/types.hpp"
#include "pagmo/utils/discrepancy.hpp"
#include "pagmo/utils/generic.hpp"
#include "pagmo/utils/multi_objective.hpp"
%}
%include <stdint.i>

	namespace detail {
		extern void reksum(std::vector<std::vector<double>>&, const std::vector<pop_size_t>&, pop_size_t, pop_size_t, std::vector<double> = std::vector<double>());
	}
	extern bool pareto_dominance(const vector_double&, const vector_double&);

	extern std::vector<pop_size_t> non_dominated_front_2d(const std::vector<vector_double>&);

	using fnds_return_type = std::tuple<std::vector<std::vector<pop_size_t>>, std::vector<std::vector<pop_size_t>>, std::vector<pop_size_t>, std::vector<pop_size_t>>;
	extern fnds_return_type fast_non_dominated_sorting(const std::vector<vector_double>&);

	extern vector_double crowding_distance(const std::vector<vector_double>&);

	extern std::vector<pop_size_t> sort_population_mo(const std::vector<vector_double>&);

	extern std::vector<pop_size_t> select_best_N_mo(const std::vector<vector_double>&, pop_size_t);

	extern vector_double ideal(const std::vector<vector_double>&);

	extern vector_double nadir(const std::vector<vector_double>&);

	template <typename Rng>
	extern inline std::vector<vector_double> decomposition_weights(vector_double::size_type n_f, vector_double::size_type n_w, const std::string& method, Rng& r_engine);

	extern vector_double decompose_objectives(const vector_double&, const vector_double&, const vector_double&, const std::string&);
