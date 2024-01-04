#pragma once

#include <tuple>
#include <vector>

#include "problem.h"
#include "pagmo/pagmo.hpp"

namespace pagmoWrap {
	// typedefs for our sanity
	typedef unsigned long long pop_size_t;
	typedef std::vector<unsigned long long> indexVector;
	typedef std::vector<std::vector<unsigned long long> > VectorOfVectorIndexes;

	// pagmo's original tuple
	typedef std::tuple<std::vector<std::vector<pop_size_t> >, std::vector<std::vector<pop_size_t>>, std::vector<pop_size_t>, std::vector<pop_size_t> > fnds_return_type; 
	// the original declarition of the tuple is:
	//using fnds_return_type = std::tuple<std::vector<std::vector<pop_size_t>>, std::vector<std::vector<pop_size_t>> std::vector<pop_size_t>, std::vector<pop_size_t>>;

	class Fnds_Return_Type {
	public:
		VectorOfVectorIndexes Item1; // We should consider picking better names...
		VectorOfVectorIndexes Item2;
		std::vector<unsigned long long> Item3;
		std::vector<unsigned long long> Item4;

		Fnds_Return_Type(fnds_return_type theTuple)
		{//TODO: Proper pointer/reference?
			this->Item1 = get<0>(theTuple);
			this->Item2 = get<1>(theTuple);
			this->Item3 = get<2>(theTuple);
			this->Item4 = get<3>(theTuple);
		}
	};

	// Fast non dominated sorting, the native swig decliration would look like this
	//extern fnds_return_type fast_non_dominated_sorting(const std::vector<vector_double>&); 
	Fnds_Return_Type fast_non_dominated_sorting(const std::vector<pagmoWrap::vector_double>& vector)
	{
		return Fnds_Return_Type(pagmo::fast_non_dominated_sorting(vector));
	};
};
