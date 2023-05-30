//The functions in this file have been manually implimented in C# (copied and trascribed)

%module(directors = "1") gradients_and_hessians
%{
#include <algorithm>
#include <cmath>
#include <stdexcept>
#include <vector>
#include <functional>
//typedef std::vector<double> vector_double;
//typedef vector_double(*fn_ptr)(vector_double x);
//fn_ptr make_fn_ptr() {
//	return f;
//};
//#include "gradientsAndHessiansCallback.h"
#include "pagmo/exceptions.hpp"
#include "pagmo/problem.hpp"
#include "pagmo/types.hpp"
#include "pagmo/utils/gradients_and_hessians.hpp"
%}
%include <stdint.i>
//%include "pagmoWrapper/gradientsAndHessiansCallback.h"
//typedef std::vector<double> vector_double;
typedef std::vector<std::pair<vector_double::size_type, vector_double::size_type>> sparsity_pattern;
typedef std::vector<double> vector_double;

vector_double gradientsAndHessiansCallback(vector_double x);

sparsity_pattern estimate_sparsity(gradientsAndHessiansCallback f, const vector_double &x, double dx = 1e-8);

//template <class Func>
vector_double estimate_gradient(gradientsAndHessiansCallback f, const vector_double &x, double dx = 1e-8);

//template <class Func>
vector_double estimate_gradient_h(gradientsAndHessiansCallback f, const vector_double &x, double dx = 1e-2);
