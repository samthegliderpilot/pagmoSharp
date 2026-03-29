#pragma once

#include <tuple>
#include <vector>

#include "pagmo/algorithms/cmaes.hpp"

namespace pagmoWrap {

struct CmaesLogEntry {
    unsigned gen{};
    unsigned long long fevals{};
    double best{};
    double sigma{};
    double min_variance{};
    double max_variance{};

    CmaesLogEntry() = default;

    CmaesLogEntry(unsigned gen_,
                  unsigned long long fevals_,
                  double best_,
                  double sigma_,
                  double min_variance_,
                  double max_variance_)
        : gen(gen_),
          fevals(fevals_),
          best(best_),
          sigma(sigma_),
          min_variance(min_variance_),
          max_variance(max_variance_)
    {
    }
};

inline std::vector<CmaesLogEntry> Cmaes_GetLogEntries(const pagmo::cmaes &algo)
{
    std::vector<CmaesLogEntry> entries;
    const auto &log = algo.get_log();
    entries.reserve(log.size());

    for (const auto &line : log) {
        entries.emplace_back(
            std::get<0>(line),
            std::get<1>(line),
            std::get<2>(line),
            std::get<3>(line),
            std::get<4>(line),
            std::get<5>(line));
    }

    return entries;
}

} // namespace pagmoWrap
