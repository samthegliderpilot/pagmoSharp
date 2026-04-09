#pragma once

#include <tuple>
#include <vector>

#include "pagmo/algorithms/compass_search.hpp"
#include "pagmo/algorithms/de1220.hpp"
#include "pagmo/algorithms/gwo.hpp"
#include "pagmo/algorithms/maco.hpp"
#include "pagmo/algorithms/moead.hpp"
#include "pagmo/algorithms/moead_gen.hpp"
#include "pagmo/algorithms/nsga2.hpp"
#include "pagmo/algorithms/nspso.hpp"
#include "pagmo/algorithms/pso.hpp"
#include "pagmo/algorithms/pso_gen.hpp"
#include "pagmo/algorithms/sade.hpp"
#include "pagmo/algorithms/sea.hpp"
#include "pagmo/algorithms/sga.hpp"
#include "pagmo/algorithms/simulated_annealing.hpp"
#include "pagmo/algorithms/xnes.hpp"
#if defined(PAGMO_WITH_NLOPT)
#include "pagmo/algorithms/nlopt.hpp"
#endif
#if defined(PAGMO_WITH_IPOPT)
#include "pagmo/algorithms/ipopt.hpp"
#endif

namespace pagmoWrap {

struct PsoLogEntry {
    unsigned gen{};
    unsigned long long fevals{};
    double best{};
    double inertia{};
    double cognitive{};
    double social{};
};

struct XnesLogEntry {
    unsigned gen{};
    unsigned long long fevals{};
    double best{};
    double dx{};
    double df{};
    double sigma{};
};

struct MoVectorLogEntry {
    unsigned gen{};
    unsigned long long fevals{};
    std::vector<double> fitness;
};

struct MoeadLogEntry {
    unsigned gen{};
    unsigned long long fevals{};
    double decomposed_f{};
    std::vector<double> ideal_point;
};

struct GwoLogEntry {
    unsigned gen{};
    double alpha{};
    double beta{};
    double delta{};
};

struct De1220LogEntry {
    unsigned gen{};
    unsigned long long fevals{};
    double best{};
    double feval_difference{};
    double dx{};
    unsigned variant{};
    double f{};
    double cr{};
};

struct CompassSearchLogEntry {
    unsigned long long fevals{};
    double best{};
    unsigned long long violated{};
    double violation_norm{};
    double range{};
};

struct NloptLogEntry {
    unsigned long long fevals{};
    double objective{};
    unsigned long long violated{};
    double violation_norm{};
    bool feasible{};
};

struct IpoptLogEntry {
    unsigned long long objective_evaluations{};
    double objective{};
    unsigned long long violated{};
    double violation_norm{};
    bool feasible{};
};

struct SimulatedAnnealingLogEntry {
    unsigned long long fevals{};
    double best{};
    double current{};
    double temperature{};
    double move_range{};
};

struct SgaLogEntry {
    unsigned gen{};
    unsigned long long fevals{};
    double best{};
    double improvement{};
};

struct SadeLogEntry {
    unsigned gen{};
    unsigned long long fevals{};
    double best{};
    double f{};
    double cr{};
    double dx{};
    double df{};
};

struct SeaLogEntry {
    unsigned gen{};
    unsigned long long fevals{};
    double best{};
    double improvement{};
    unsigned long long offspring_evals{};
};

inline std::vector<PsoLogEntry> Pso_GetLogEntries(const pagmo::pso &algo)
{
    std::vector<PsoLogEntry> entries;
    const auto &log = algo.get_log();
    entries.reserve(log.size());
    for (const auto &line : log) {
        entries.push_back({std::get<0>(line), std::get<1>(line), std::get<2>(line), std::get<3>(line), std::get<4>(line), std::get<5>(line)});
    }
    return entries;
}

inline std::vector<PsoLogEntry> PsoGen_GetLogEntries(const pagmo::pso_gen &algo)
{
    std::vector<PsoLogEntry> entries;
    const auto &log = algo.get_log();
    entries.reserve(log.size());
    for (const auto &line : log) {
        entries.push_back({std::get<0>(line), std::get<1>(line), std::get<2>(line), std::get<3>(line), std::get<4>(line), std::get<5>(line)});
    }
    return entries;
}

inline std::vector<XnesLogEntry> Xnes_GetLogEntries(const pagmo::xnes &algo)
{
    std::vector<XnesLogEntry> entries;
    const auto &log = algo.get_log();
    entries.reserve(log.size());
    for (const auto &line : log) {
        entries.push_back({std::get<0>(line), std::get<1>(line), std::get<2>(line), std::get<3>(line), std::get<4>(line), std::get<5>(line)});
    }
    return entries;
}

inline std::vector<MoVectorLogEntry> Maco_GetLogEntries(const pagmo::maco &algo)
{
    std::vector<MoVectorLogEntry> entries;
    const auto &log = algo.get_log();
    entries.reserve(log.size());
    for (const auto &line : log) {
        entries.push_back({std::get<0>(line), std::get<1>(line), std::get<2>(line)});
    }
    return entries;
}

inline std::vector<MoeadLogEntry> Moead_GetLogEntries(const pagmo::moead &algo)
{
    std::vector<MoeadLogEntry> entries;
    const auto &log = algo.get_log();
    entries.reserve(log.size());
    for (const auto &line : log) {
        entries.push_back({std::get<0>(line), std::get<1>(line), std::get<2>(line), std::get<3>(line)});
    }
    return entries;
}

inline std::vector<MoeadLogEntry> MoeadGen_GetLogEntries(const pagmo::moead_gen &algo)
{
    std::vector<MoeadLogEntry> entries;
    const auto &log = algo.get_log();
    entries.reserve(log.size());
    for (const auto &line : log) {
        entries.push_back({std::get<0>(line), std::get<1>(line), std::get<2>(line), std::get<3>(line)});
    }
    return entries;
}

inline std::vector<MoVectorLogEntry> Nsga2_GetLogEntries(const pagmo::nsga2 &algo)
{
    std::vector<MoVectorLogEntry> entries;
    const auto &log = algo.get_log();
    entries.reserve(log.size());
    for (const auto &line : log) {
        entries.push_back({std::get<0>(line), std::get<1>(line), std::get<2>(line)});
    }
    return entries;
}

inline std::vector<MoVectorLogEntry> Nspso_GetLogEntries(const pagmo::nspso &algo)
{
    std::vector<MoVectorLogEntry> entries;
    const auto &log = algo.get_log();
    entries.reserve(log.size());
    for (const auto &line : log) {
        entries.push_back({std::get<0>(line), std::get<1>(line), std::get<2>(line)});
    }
    return entries;
}

inline std::vector<GwoLogEntry> Gwo_GetLogEntries(const pagmo::gwo &algo)
{
    std::vector<GwoLogEntry> entries;
    const auto &log = algo.get_log();
    entries.reserve(log.size());
    for (const auto &line : log) {
        entries.push_back({std::get<0>(line), std::get<1>(line), std::get<2>(line), std::get<3>(line)});
    }
    return entries;
}

inline std::vector<De1220LogEntry> De1220_GetLogEntries(const pagmo::de1220 &algo)
{
    std::vector<De1220LogEntry> entries;
    const auto &log = algo.get_log();
    entries.reserve(log.size());
    for (const auto &line : log) {
        entries.push_back({std::get<0>(line), std::get<1>(line), std::get<2>(line), std::get<3>(line), std::get<4>(line), std::get<5>(line), std::get<6>(line), std::get<7>(line)});
    }
    return entries;
}

inline std::vector<CompassSearchLogEntry> CompassSearch_GetLogEntries(const pagmo::compass_search &algo)
{
    std::vector<CompassSearchLogEntry> entries;
    const auto &log = algo.get_log();
    entries.reserve(log.size());
    for (const auto &line : log) {
        entries.push_back({std::get<0>(line), std::get<1>(line), static_cast<unsigned long long>(std::get<2>(line)), std::get<3>(line), std::get<4>(line)});
    }
    return entries;
}

#if defined(PAGMO_WITH_NLOPT)
inline std::vector<NloptLogEntry> Nlopt_GetLogEntries(const pagmo::nlopt &algo)
{
    std::vector<NloptLogEntry> entries;
    const auto &log = algo.get_log();
    entries.reserve(log.size());
    for (const auto &line : log) {
        entries.push_back({std::get<0>(line), std::get<1>(line), static_cast<unsigned long long>(std::get<2>(line)), std::get<3>(line), std::get<4>(line)});
    }
    return entries;
}
#endif

#if defined(PAGMO_WITH_IPOPT)
inline std::vector<IpoptLogEntry> Ipopt_GetLogEntries(const pagmo::ipopt &algo)
{
    std::vector<IpoptLogEntry> entries;
    const auto &log = algo.get_log();
    entries.reserve(log.size());
    for (const auto &line : log) {
        entries.push_back({std::get<0>(line), std::get<1>(line), static_cast<unsigned long long>(std::get<2>(line)), std::get<3>(line), std::get<4>(line)});
    }
    return entries;
}
#endif

inline std::vector<SimulatedAnnealingLogEntry> SimulatedAnnealing_GetLogEntries(const pagmo::simulated_annealing &algo)
{
    std::vector<SimulatedAnnealingLogEntry> entries;
    const auto &log = algo.get_log();
    entries.reserve(log.size());
    for (const auto &line : log) {
        entries.push_back({std::get<0>(line), std::get<1>(line), std::get<2>(line), std::get<3>(line), std::get<4>(line)});
    }
    return entries;
}

inline std::vector<SgaLogEntry> Sga_GetLogEntries(const pagmo::sga &algo)
{
    std::vector<SgaLogEntry> entries;
    const auto &log = algo.get_log();
    entries.reserve(log.size());
    for (const auto &line : log) {
        entries.push_back({std::get<0>(line), std::get<1>(line), std::get<2>(line), std::get<3>(line)});
    }
    return entries;
}

inline std::vector<SadeLogEntry> Sade_GetLogEntries(const pagmo::sade &algo)
{
    std::vector<SadeLogEntry> entries;
    const auto &log = algo.get_log();
    entries.reserve(log.size());
    for (const auto &line : log) {
        entries.push_back({std::get<0>(line), std::get<1>(line), std::get<2>(line), std::get<3>(line), std::get<4>(line), std::get<5>(line), std::get<6>(line)});
    }
    return entries;
}

inline std::vector<SeaLogEntry> Sea_GetLogEntries(const pagmo::sea &algo)
{
    std::vector<SeaLogEntry> entries;
    const auto &log = algo.get_log();
    entries.reserve(log.size());
    for (const auto &line : log) {
        entries.push_back({std::get<0>(line), std::get<1>(line), std::get<2>(line), std::get<3>(line), static_cast<unsigned long long>(std::get<4>(line))});
    }
    return entries;
}

} // namespace pagmoWrap
