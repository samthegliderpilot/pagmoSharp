#pragma once

#include <vector>

#include <pagmo/algorithms/gaco.hpp>

namespace pagmoWrap {

struct GacoLogEntry {
    unsigned gen = 0u;
    unsigned long long fevals = 0u;
    double best_fit = 0.0;
    unsigned kernel = 0u;
    double oracle = 0.0;
    double dx = 0.0;
    double dp = 0.0;

    GacoLogEntry() = default;

    GacoLogEntry(unsigned gen_,
                 unsigned long long fevals_,
                 double best_fit_,
                 unsigned kernel_,
                 double oracle_,
                 double dx_,
                 double dp_)
        : gen(gen_),
          fevals(fevals_),
          best_fit(best_fit_),
          kernel(kernel_),
          oracle(oracle_),
          dx(dx_),
          dp(dp_)
    {
    }
};

#ifndef SWIG
inline std::vector<GacoLogEntry> Gaco_GetLogEntries(const pagmo::gaco &algo)
{
    std::vector<GacoLogEntry> entries;
    const auto &log = algo.get_log();
    entries.reserve(log.size());
    for (const auto &row : log) {
        entries.emplace_back(
            std::get<0>(row),
            static_cast<unsigned long long>(std::get<1>(row)),
            std::get<2>(row),
            std::get<3>(row),
            std::get<4>(row),
            std::get<5>(row),
            std::get<6>(row));
    }
    return entries;
}
#endif

} // namespace pagmoWrap
