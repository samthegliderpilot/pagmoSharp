#pragma once

#include <vector>
#include <tuple>
#include <cstddef>

#include <pagmo/types.hpp>

namespace pagmoWrap {

    // Replaces individuals_group_t
    struct IndividualsGroup {
        std::vector<unsigned long long> ids;
        std::vector<pagmo::vector_double> xs;
        std::vector<pagmo::vector_double> fs;

        IndividualsGroup() = default;

        IndividualsGroup(std::vector<unsigned long long> ids_,
            std::vector<pagmo::vector_double> xs_,
            std::vector<pagmo::vector_double> fs_)
            : ids(std::move(ids_)), xs(std::move(xs_)), fs(std::move(fs_)) {
        }
    };

    // Replaces migration_entry_t
    struct MigrationEntry {
        double t = 0.0;
        unsigned long long island_id = 0;
        pagmo::vector_double x;
        pagmo::vector_double f;
        std::size_t migration_id = 0;
        std::size_t immigrant_id = 0;

        MigrationEntry() = default;

        MigrationEntry(double t_,
            unsigned long long island_id_,
            pagmo::vector_double x_,
            pagmo::vector_double f_,
            std::size_t migration_id_,
            std::size_t immigrant_id_)
            : t(t_),
            island_id(island_id_),
            x(std::move(x_)),
            f(std::move(f_)),
            migration_id(migration_id_),
            immigrant_id(immigrant_id_) {
        }
    };

} // namespace pagmoWrap


// ------------------------------------------------------------
// Tuple helpers — NOT for SWIG parsing
// ------------------------------------------------------------
#ifndef SWIG

namespace pagmoWrap {

    inline IndividualsGroup FromIndividualsGroupTuple(
        const std::tuple<
        std::vector<unsigned long long>,
        std::vector<pagmo::vector_double>,
        std::vector<pagmo::vector_double>
        >& tup)
    {
        return IndividualsGroup(std::get<0>(tup), std::get<1>(tup), std::get<2>(tup));
    }

    inline MigrationEntry FromMigrationEntryTuple(
        const std::tuple<
        double,
        unsigned long long,
        pagmo::vector_double,
        pagmo::vector_double,
        std::size_t,
        std::size_t
        >& tup)
    {
        return MigrationEntry(
            std::get<0>(tup),
            std::get<1>(tup),
            std::get<2>(tup),
            std::get<3>(tup),
            std::get<4>(tup),
            std::get<5>(tup)
        );
    }

    inline pagmo::individuals_group_t
        ToIndividualsGroupTuple(const IndividualsGroup& g)
    {
        return pagmo::individuals_group_t(g.ids, g.xs, g.fs);
    }
} // namespace pagmoWrap

#endif
