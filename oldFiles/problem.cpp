
#pragma once
#include <pagmo/algorithm.hpp>
#include <pagmo/algorithms/sade.hpp>
#include <pagmo/archipelago.hpp>
#include <pagmo/problem.hpp>
#include "problem.h"
using namespace pagmo;

namespace pagmoWrap {

	//problemBase::problemBase() {}

	//[[nodiscard]] bool problemBase::has_batch_fitness() const
	//{
	//	return false;
	//}

	//[[nodiscard]] std::string problemBase::get_name() const
	//{
	//	return "Base c++ problem";
	//}

	//[[nodiscard]] vector_double problemBase::fitness(const vector_double&) const
	//{
	//	return vector_double();
	//}

	//[[nodiscard]] std::pair<vector_double, vector_double> problemBase::get_bounds() const
	//{
	//	return std::pair< vector_double, vector_double>{vector_double(), vector_double()};
	//}
};