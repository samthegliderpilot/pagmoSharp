#pragma once

#include <tuple>
#include <vector>

#include "pagmo/algorithms/cstrs_self_adaptive.hpp"

namespace pagmoWrap {

struct CstrsLogEntry {
    unsigned iter{};
    unsigned long long fevals{};
    double best{};
    double infeasibility{};
    unsigned long long violated{};
    double violation_norm{};
    unsigned long long feasible{};

    CstrsLogEntry() = default;

    CstrsLogEntry(unsigned iter_,
                  unsigned long long fevals_,
                  double best_,
                  double infeasibility_,
                  unsigned long long violated_,
                  double violation_norm_,
                  unsigned long long feasible_)
        : iter(iter_),
          fevals(fevals_),
          best(best_),
          infeasibility(infeasibility_),
          violated(violated_),
          violation_norm(violation_norm_),
          feasible(feasible_)
    {
    }
};

inline std::vector<CstrsLogEntry> Cstrs_GetLogEntries(const pagmo::cstrs_self_adaptive &algo)
{
    std::vector<CstrsLogEntry> entries;
    const auto &log = algo.get_log();
    entries.reserve(log.size());

    for (const auto &line : log) {
        entries.emplace_back(
            std::get<0>(line),
            std::get<1>(line),
            std::get<2>(line),
            std::get<3>(line),
            static_cast<unsigned long long>(std::get<4>(line)),
            std::get<5>(line),
            static_cast<unsigned long long>(std::get<6>(line)));
    }

    return entries;
}

} // namespace pagmoWrap
