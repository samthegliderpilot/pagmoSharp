#pragma once

#include <tuple>
#include <vector>

#include "pagmo/algorithms/de.hpp"

namespace pagmoWrap {

struct DeLogEntry {
    unsigned gen{};
    unsigned long long fevals{};
    double best{};
    double feval_difference{};
    double dx{};

    DeLogEntry() = default;

    DeLogEntry(unsigned gen_,
               unsigned long long fevals_,
               double best_,
               double feval_difference_,
               double dx_)
        : gen(gen_),
          fevals(fevals_),
          best(best_),
          feval_difference(feval_difference_),
          dx(dx_)
    {
    }
};

inline std::vector<DeLogEntry> De_GetLogEntries(const pagmo::de &algo)
{
    std::vector<DeLogEntry> entries;
    const auto &log = algo.get_log();
    entries.reserve(log.size());

    for (const auto &line : log) {
        entries.emplace_back(
            std::get<0>(line),
            std::get<1>(line),
            std::get<2>(line),
            std::get<3>(line),
            std::get<4>(line));
    }

    return entries;
}

} // namespace pagmoWrap
