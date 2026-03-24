// swigInterfaceFiles/archipelago.i

%{
#include "pagmo/archipelago.hpp"
#include "pagmo/island.hpp"
#include "pagmo/algorithm.hpp"
#include "pagmo/problem.hpp"
#include "pagmo/population.hpp"
#include "pagmo/bfe.hpp"
#include "pagmo/r_policy.hpp"
#include "pagmo/s_policy.hpp"
#include "pagmo/topology.hpp"
#include "pagmo/topologies/unconnected.hpp"
#include "pagmo/topologies/fully_connected.hpp"
#include "pagmo/topologies/ring.hpp"
#include "pagmo/topologies/free_form.hpp"

#include "tuple_adapters.h"
#include "archipelago_swig.h"
%}

// Your pattern.
%typemap(csclassmodifiers) pagmo::archipelago "public partial class"
%typemap(csclassmodifiers) archipelago "public partial class"

// SWIG parses only the facade.
%include "pagmoWrapper/archipelago_swig.h"

// We return std::vector<...> in extensions.
%include "std_vector.i"

// Adapter structs for IndividualsGroup and MigrationEntry.
%include "pagmoWrapper/tuple_adapters.h"

// ------------------------------------------------------------
// IMPORTANT: We do NOT declare class pagmo::archipelago here.
// The facade lives in archipelago_swig.h (under #ifdef SWIG).
// ------------------------------------------------------------


// ------------------------------------------------------------
// Explicit ignores for things we are not wrapping directly.
// ------------------------------------------------------------

// SWIG can't wrap operator[] cleanly
%ignore pagmo::archipelago::operator[];

// Iterators are not worth it in v1.
%ignore pagmo::archipelago::begin;
%ignore pagmo::archipelago::end;


// ------------------------------------------------------------
// Extensions
// ------------------------------------------------------------
%extend pagmo::archipelago {

    // ----------------------------
    // Island access
    // Return by value (copy) to avoid pointer/ref lifetime and island(IntPtr,...) ctor issues.
    // ----------------------------
    /*pagmo::island get_island(std::size_t idx) {
        return pagmoWrap::Archipelago_GetIslandCopyMutable(*self, idx);
    }

    pagmo::island get_island_const(std::size_t idx) const {
        return pagmoWrap::Archipelago_GetIslandCopy(*self, idx);
    }*/

    // ----------------------------
    // push_back wrappers
    // These take pagmo::algorithm (so you can pass bee_colony.to_algorithm()).

    std::size_t push_back_island(const pagmo::algorithm &algo,
                                 const pagmo::problem &prob,
                                 std::size_t pop_size,
                                 unsigned seed) {
        return pagmoWrap::Archipelago_PushBack_AlgoProbSizeSeed(*self, algo, prob, pop_size, seed);
    }

    
    // ----------------------------
    // Migrants DB wrapper
    // Expose as vector<IndividualsGroup>
    // ----------------------------
    std::vector<pagmoWrap::IndividualsGroup> get_migrants_db() const {
        auto db = self->get_migrants_db();
        std::vector<pagmoWrap::IndividualsGroup> out;
        out.reserve(db.size());
        for (const auto &t : db) {
            out.push_back(pagmoWrap::FromIndividualsGroupTuple(t));
        }
        return out;
    }

    void set_migrants_db_items(const std::vector<pagmoWrap::IndividualsGroup> &v) {
        pagmo::archipelago::migrants_db_t db;
        db.reserve(v.size());
        for (const auto &g : v) {
            db.push_back(pagmoWrap::ToIndividualsGroupTuple(g));
        }
        self->set_migrants_db(std::move(db));
    }

    migration_type get_migration_type() const {
        return self->get_migration_type();
    }

    void set_migration_type(migration_type t) {
        self->set_migration_type(t);
    }

    migrant_handling get_migrant_handling() const {
        return self->get_migrant_handling();
    }

    void set_migrant_handling(migrant_handling m) {
        self->set_migrant_handling(m);
    }

    std::string get_topology_name() const {
        return self->get_topology().get_name();
    }

    void set_topology_unconnected(const pagmo::unconnected &t) {
        self->set_topology(pagmo::topology(t));
    }

    void set_topology_fully_connected(const pagmo::fully_connected &t) {
        self->set_topology(pagmo::topology(t));
    }

    void set_topology_ring(const pagmo::ring &t) {
        self->set_topology(pagmo::topology(t));
    }

    void set_topology_free_form(const pagmo::free_form &t) {
        self->set_topology(pagmo::topology(t));
    }

    // ----------------------------
    // Migration log wrapper
    // Expose as vector<MigrationEntry>
    // ----------------------------
    std::vector<pagmoWrap::MigrationEntry> get_migration_log_entries() const {
        auto log = self->get_migration_log();
        std::vector<pagmoWrap::MigrationEntry> out;
        out.reserve(log.size());
        for (const auto &t : log) {
            out.push_back(pagmoWrap::FromMigrationEntryTuple(t));
        }
        return out;
    }
}
