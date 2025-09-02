%module(naturalvar = 1, directors = "1") multi_objective
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

typedef std::vector<double> vector_double;
typedef std::vector<std::vector<double> > VectorOfVectorOfDoubles;


%include "pagmoWrapper/multi_objective.h"


//namespace detail {
//	extern void reksum(VectorOfVectorOfDoubles&, const std::vector<pop_size_t>&, pop_size_t, pop_size_t, std::vector<double> = std::vector<double>());
//}
// Pareto-dominance
extern bool pareto_dominance(const vector_double&, const vector_double&);

// Non dominated front 2D (Kung's algorithm)
extern std::vector<pop_size_t> non_dominated_front_2d(const std::vector<vector_double>&);

// Crowding distance
extern vector_double crowding_distance(const std::vector<vector_double>&);

// Sorts a population in multi-objective optimization
extern std::vector<pop_size_t> sort_population_mo(const std::vector<vector_double>&);

// Selects the best N individuals in multi-objective optimization
extern std::vector<pop_size_t> select_best_N_mo(const std::vector<vector_double>&, pop_size_t);

// Ideal point
extern vector_double ideal(const std::vector<vector_double>&);

// Nadir point
extern vector_double nadir(const std::vector<vector_double>&);

//template <typename Rng>
//extern std::vector<vector_double> decomposition_weights(vector_double::size_type n_f, vector_double::size_type n_w, const std::string& method, Rng& r_engine);

// Decomposes a vector of objectives.
extern vector_double decompose_objectives(const vector_double&, const vector_double&, const vector_double&,
    const std::string&);

//fast_non_dominated_sorting has manually written code to handle the tuple