#pragma once

#ifndef PAGMO_MULTI_OBJECTIVE_WRAPPER_HPP
#define PAGMO_MULTI_OBJECTIVE_WRAPPER_HPP

#include <vector>
#include <pagmo/types.hpp>
#include <pagmo/utils/generic.hpp>
#include <pagmo/utils/discrepancy.hpp>
#include <pagmo/utils/multi_objective.hpp>

namespace pagmo {

    typedef unsigned long long pop_size_t;
    typedef std::vector<double> vector_double;

    struct FNDSResult {
        std::vector<std::vector<pop_size_t>> fronts;
        std::vector<std::vector<pop_size_t>> ranks;
        std::vector<pop_size_t> rank_indices;
        std::vector<pop_size_t> domination_counts;
    };

    inline FNDSResult FastNonDominatedSorting(const std::vector<vector_double>& input) {
        auto [fronts, ranks, rank_indices, domination_counts] = fast_non_dominated_sorting(input);
        return { fronts, ranks, rank_indices, domination_counts };
    }

} // namespace pagmo

class RekSum
{
public:
    static void reksum(std::vector<std::vector<double>> &out, const std::vector<pagmo::pop_size_t> &in, pagmo::pop_size_t m, pagmo::pop_size_t s)
    {
        pagmo::detail::reksum(out, in, m, s);
    }
        
};

class DecompositionWeights
{
    //TODO: Document the limitations of the rng engine (that we hardcoded one)
public :
    static std::vector<std::vector<double>>decomposition_weights(
        pagmo::vector_double::size_type n_f,
        pagmo::vector_double::size_type n_w,
        const std::string& method)
    {
        std::mt19937 rng(std::random_device{}());
        return pagmo::decomposition_weights(n_f, n_w, method, rng);
    }

    // Static constants for valid methods
    static const std::string METHOD_GRID;
    static const std::string METHOD_RANDOM;
    static const std::string METHOD_LOW_DISCREPANCY;
};

///
// Definitions of static members (in .cpp if you separate)
const std::string DecompositionWeights::METHOD_GRID = "grid";
const std::string DecompositionWeights::METHOD_RANDOM = "random";
const std::string DecompositionWeights::METHOD_LOW_DISCREPANCY = "low discrepancy";
#endif


//namespace pagmoWrap {
//	// typedefs for our sanity
//	typedef unsigned long long pop_size_t;
//	typedef std::vector<unsigned long long> indexVector;
//	typedef std::vector<std::vector<unsigned long long> > VectorOfVectorIndexes;
//
//	// pagmo's original tuple
//	typedef std::tuple<std::vector<std::vector<pop_size_t> >, std::vector<std::vector<pop_size_t>>, std::vector<pop_size_t>, std::vector<pop_size_t> > fnds_return_type; 
//	// the original declarition of the tuple is:
//	//using fnds_return_type = std::tuple<std::vector<std::vector<pop_size_t>>, std::vector<std::vector<pop_size_t>> std::vector<pop_size_t>, std::vector<pop_size_t>>;
//
//	class Fnds_Return_Type {
//	public:
//		VectorOfVectorIndexes Item1; // We should consider picking better names...
//		VectorOfVectorIndexes Item2;
//		std::vector<unsigned long long> Item3;
//		std::vector<unsigned long long> Item4;
//
//		Fnds_Return_Type(fnds_return_type theTuple)
//		{//TODO: Proper pointer/reference?
//			this->Item1 = get<0>(theTuple);
//			this->Item2 = get<1>(theTuple);
//			this->Item3 = get<2>(theTuple);
//			this->Item4 = get<3>(theTuple);
//		}
//	};
//
//	// Fast non dominated sorting..., the native swig decliration would look like this
//	//extern fnds_return_type fast_non_dominated_sorting(const std::vector<vector_double>&); 
//	Fnds_Return_Type fast_non_dominated_sorting(const std::vector<pagmoWrap::vector_double>& vector)
//	{
//		return Fnds_Return_Type(pagmo::fast_non_dominated_sorting(vector));
//	};
//};
