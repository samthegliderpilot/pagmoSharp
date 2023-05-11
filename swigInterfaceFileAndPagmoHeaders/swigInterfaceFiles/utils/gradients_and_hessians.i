%module pagmo
%{
#include <algorithm>
#include <cmath>
#include <stdexcept>
#include <vector>
#include <functional>
#include "pagmo/exceptions.hpp"
#include "pagmo/problem.hpp"
#include "pagmo/types.hpp"
#include "pagmo/utils/gradients_and_hessians.hpp"
%}


typedef std::vector<double> vector_double;
typedef std::vector<std::pair<vector_double::size_type, vector_double::size_type>> sparsity_pattern;
typedef std::vector<vector_double>::size_type pop_size_t;
typedef std::Func Func;


template <typename Func>
sparsity_pattern estimate_sparsity(Func f, const vector_double &x, double dx = 1e-8);

template <typename Func>
vector_double estimate_gradient(Func f, const vector_double &x, double dx = 1e-8);

template <typename Func>
vector_double estimate_gradient_h(Func f, const vector_double &x, double dx = 1e-2);
