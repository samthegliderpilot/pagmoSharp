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
