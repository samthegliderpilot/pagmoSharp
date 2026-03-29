%{
#include <cmath>
#include <numeric>
#include <random>
#include <sstream>
#include <stdexcept>
#include <string>
//#include <tuple>
#include <vector>

#include "boost/numeric/conversion/cast.hpp"

#include "pagmo/detail/visibility.hpp"
#include "pagmo/exceptions.hpp"
#include "pagmo/types.hpp"
#include "pagmo/utils/discrepancy.hpp"
#include "pagmo/utils/generic.hpp"
#include "pagmo/utils/multi_objective.hpp"
#include "multi_objective.h"
%}

%include "std_vector.i"
%{ 
#include <vector> 
%} 
%include <std_vector.i>
%include <std_string.i>
%include <stdint.i>

%include "pagmoWrapper/multi_objective.h"

namespace pagmo {
// Pareto-dominance
extern bool pareto_dominance(const pagmo::vector_double&, const pagmo::vector_double&);

// Non dominated front 2D (Kung's algorithm)
extern std::vector<pagmo::pop_size_t> non_dominated_front_2d(const std::vector<pagmo::vector_double>&);

// Crowding distance
extern pagmo::vector_double crowding_distance(const std::vector<pagmo::vector_double>&);

// Sorts a population in multi-objective optimization
extern std::vector<pagmo::pop_size_t> sort_population_mo(const std::vector<pagmo::vector_double>&);

// Selects the best N individuals in multi-objective optimization
extern std::vector<pagmo::pop_size_t> select_best_N_mo(const std::vector<pagmo::vector_double>&, pagmo::pop_size_t);

// Ideal point
extern pagmo::vector_double ideal(const std::vector<pagmo::vector_double>&);

// Nadir point
extern pagmo::vector_double nadir(const std::vector<pagmo::vector_double>&);

//DONE CUSTOM DUE TO Rng
//template <typename Rng>
//extern std::vector<vector_double> decomposition_weights(vector_double::size_type n_f, vector_double::size_type n_w, const std::string& method, Rng& r_engine);

// Decomposes a vector of objectives.
extern pagmo::vector_double decompose_objectives(const pagmo::vector_double&, const pagmo::vector_double&, const pagmo::vector_double&,
    const std::string&);
}
