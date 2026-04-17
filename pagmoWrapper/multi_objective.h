#pragma once

#include <random>
#include <string>
#include <vector>

#include <pagmo/types.hpp>
#include <pagmo/utils/generic.hpp>
#include <pagmo/utils/discrepancy.hpp>
#include <pagmo/utils/multi_objective.hpp>

namespace pagmo {

    // SWIG TYPE-MAPPING — do not remove.
    // When SWIG sees this typedef inside namespace pagmo {}, it resolves pagmo::pop_size_t
    // as unsigned long long and applies the %typemap(cstype) unsigned long long "ulong" rule
    // defined in the .i file. Without it, pagmo::pop_size_t becomes an opaque
    // SWIGTYPE_p_pagmo__pop_size_t in generated C#, breaking ~20 API types.
    // On 64-bit Windows, pagmo's own pop_size_t IS size_t == unsigned long long, so this is
    // a safe same-type re-declaration with no ODR implications.
    typedef unsigned long long pop_size_t;
    // Same rationale: makes SWIG resolve pagmo::vector_double as std::vector<double> and apply
    // the DoubleVector %template rather than emitting an opaque wrapper type.
    typedef std::vector<double> vector_double;

    /// <summary>
    /// Result of fast non-dominated sorting.
    /// </summary>
    struct FNDSResult {
        std::vector<std::vector<pop_size_t>> fronts;
        std::vector<std::vector<pop_size_t>> ranks;
        std::vector<pop_size_t> rank_indices;
        std::vector<pop_size_t> domination_counts;
    };

    /// <summary>
    /// Convenience wrapper around pagmo::fast_non_dominated_sorting that returns a struct
    /// instead of a tuple (SWIG cannot wrap std::tuple directly).
    /// </summary>
    inline FNDSResult FastNonDominatedSorting(const std::vector<vector_double>& input) {
        auto [fronts, ranks, rank_indices, domination_counts] = fast_non_dominated_sorting(input);
        return { fronts, ranks, rank_indices, domination_counts };
    }

} // namespace pagmo

/// <summary>
/// Exposes pagmo::detail::reksum to SWIG as a static method.
/// reksum generates all weight vectors for decomposition-based multi-objective algorithms.
/// </summary>
class RekSum
{
public:
    static void reksum(std::vector<std::vector<double>>& out,
                       const std::vector<pagmo::pop_size_t>& in,
                       pagmo::pop_size_t m,
                       pagmo::pop_size_t s)
    {
        pagmo::detail::reksum(out, in, m, s);
    }
};

/// <summary>
/// Exposes pagmo::decomposition_weights to SWIG as a static method.
/// Note: the RNG engine is seeded from std::random_device on each call.
/// For reproducible weight generation, use the pagmo API directly with an explicit seed.
/// </summary>
class DecompositionWeights
{
public:
    static std::vector<std::vector<double>> decomposition_weights(
        pagmo::vector_double::size_type n_f,
        pagmo::vector_double::size_type n_w,
        const std::string& method)
    {
        std::mt19937 rng(std::random_device{}());
        return pagmo::decomposition_weights(n_f, n_w, method, rng);
    }

    // Valid method name constants. Using inline to avoid ODR violations when
    // this header is included in multiple translation units.
    static inline const std::string METHOD_GRID             = "grid";
    static inline const std::string METHOD_RANDOM           = "random";
    static inline const std::string METHOD_LOW_DISCREPANCY  = "low discrepancy";
};
