#pragma once

#include <vector>

#include <pagmo/algorithms/mbh.hpp>

namespace pagmoWrap {

struct MbhLogEntry {
    unsigned long long fevals = 0u;
    double best = 0.0;
    unsigned violated = 0u;
    double violation_norm = 0.0;
    unsigned trial = 0u;

    MbhLogEntry() = default;

    MbhLogEntry(unsigned long long fevals_,
                double best_,
                unsigned violated_,
                double violation_norm_,
                unsigned trial_)
        : fevals(fevals_),
          best(best_),
          violated(violated_),
          violation_norm(violation_norm_),
          trial(trial_)
    {
    }
};

#ifndef SWIG
inline std::vector<MbhLogEntry> Mbh_GetLogEntries(const pagmo::mbh &algo)
{
    std::vector<MbhLogEntry> entries;
    const auto &log = algo.get_log();
    entries.reserve(log.size());
    for (const auto &row : log) {
        entries.emplace_back(
            std::get<0>(row),
            std::get<1>(row),
            static_cast<unsigned>(std::get<2>(row)),
            std::get<3>(row),
            std::get<4>(row));
    }
    return entries;
}
#endif

} // namespace pagmoWrap
