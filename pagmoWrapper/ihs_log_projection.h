#pragma once

#include <tuple>
#include <vector>

#include "pagmo/algorithms/ihs.hpp"

namespace pagmoWrap {

struct IhsLogEntry {
    unsigned long long fevals{};
    double ppar{};
    double bw{};
    double dx{};
    double df{};
    unsigned long long violated{};
    double violation_norm{};
    std::vector<double> ideal;

    IhsLogEntry() = default;

    IhsLogEntry(unsigned long long fevals_,
                double ppar_,
                double bw_,
                double dx_,
                double df_,
                unsigned long long violated_,
                double violation_norm_,
                std::vector<double> ideal_)
        : fevals(fevals_),
          ppar(ppar_),
          bw(bw_),
          dx(dx_),
          df(df_),
          violated(violated_),
          violation_norm(violation_norm_),
          ideal(std::move(ideal_))
    {
    }
};

inline std::vector<IhsLogEntry> Ihs_GetLogEntries(const pagmo::ihs &algo)
{
    std::vector<IhsLogEntry> entries;
    const auto &log = algo.get_log();
    entries.reserve(log.size());

    for (const auto &line : log) {
        entries.emplace_back(
            std::get<0>(line),
            std::get<1>(line),
            std::get<2>(line),
            std::get<3>(line),
            std::get<4>(line),
            static_cast<unsigned long long>(std::get<5>(line)),
            std::get<6>(line),
            std::get<7>(line));
    }

    return entries;
}

} // namespace pagmoWrap
