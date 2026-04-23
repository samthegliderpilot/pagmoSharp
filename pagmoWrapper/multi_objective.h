#pragma once

#include <random>
#include <string>
#include <vector>

#include <pagmo/types.hpp>
#include <pagmo/utils/generic.hpp>
#include <pagmo/utils/discrepancy.hpp>
#include <pagmo/utils/multi_objective.hpp>

namespace pagmo {

    // SWIG TYPE-MAPPING NOTE:
    // pop_size_t is NOT re-declared here. pagmo defines it as
    // std::vector<vector_double>::size_type (= size_t). On Linux x64, size_t is
    // unsigned long; on Windows x64 it is unsigned long long. Re-declaring it as
    // unsigned long long in the pagmo namespace conflicts with pagmo's own definition
    // on Linux. Instead, all SWIG-facing types in this header use unsigned long long
    // directly (matching the C# ulong typemap) and convert to/from pagmo::pop_size_t
    // at the implementation boundary.

    // Makes SWIG resolve pagmo::vector_double as std::vector<double> and apply
    // the DoubleVector %template rather than emitting an opaque wrapper type.
    typedef std::vector<double> vector_double;

    /// <summary>
    /// Result of fast non-dominated sorting.
    /// </summary>
    struct FNDSResult {
        std::vector<std::vector<unsigned long long>> fronts;
        std::vector<std::vector<unsigned long long>> ranks;
        std::vector<unsigned long long> rank_indices;
        std::vector<unsigned long long> domination_counts;
    };

    /// <summary>
    /// Convenience wrapper around pagmo::fast_non_dominated_sorting that returns a struct
    /// instead of a tuple (SWIG cannot wrap std::tuple directly).
    /// </summary>
    inline FNDSResult FastNonDominatedSorting(const std::vector<vector_double>& input) {
        auto [raw_fronts, raw_ranks, raw_rank_indices, raw_domination_counts] = fast_non_dominated_sorting(input);
        FNDSResult result;
        for (const auto& f : raw_fronts)
            result.fronts.push_back(std::vector<unsigned long long>(f.begin(), f.end()));
        for (const auto& r : raw_ranks)
            result.ranks.push_back(std::vector<unsigned long long>(r.begin(), r.end()));
        result.rank_indices.assign(raw_rank_indices.begin(), raw_rank_indices.end());
        result.domination_counts.assign(raw_domination_counts.begin(), raw_domination_counts.end());
        return result;
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
                       const std::vector<unsigned long long>& in,
                       unsigned long long m,
                       unsigned long long s)
    {
        // pagmo::pop_size_t is size_t: unsigned long on Linux, unsigned long long on Windows.
        // The SWIG boundary always uses unsigned long long (C# ulong), so we convert here.
        std::vector<pagmo::pop_size_t> in_converted(in.begin(), in.end());
        pagmo::detail::reksum(out, in_converted,
                              static_cast<pagmo::pop_size_t>(m),
                              static_cast<pagmo::pop_size_t>(s));
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
