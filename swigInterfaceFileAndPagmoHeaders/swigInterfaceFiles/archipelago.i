// swigInterfaceFiles/archipelago.i
//
// SWIG-visible facade for pagmo::archipelago
// - Avoid iterators, tuple aliases, template ctors, serialization methods.
// - Expose clean C# methods for migrants DB and migration log via wrapped methods + %rename.
// - Safe for inclusion OUTSIDE of any "namespace pagmo { }" block in the main .i.

%{
#include "pagmo/archipelago.hpp"
#include "tuple_adapters.h"
%}

%include "std_vector.i"
%include "pagmoWrapper/tuple_adapters.h"

%typemap(csclassmodifiers) pagmo::archipelago "public partial class"

// -----------------------------------------------------------------------------
// Make sure SWIG does NOT generate bindings for tuple-heavy or template methods.
// -----------------------------------------------------------------------------

// These exist in the real header but we do not want SWIG touching them:
%ignore pagmo::archipelago::get_migrants_db() const;
%ignore pagmo::archipelago::set_migrants_db(pagmo::archipelago::migrants_db_t);
%ignore pagmo::archipelago::get_migration_log() const;

// SWIG can't handle these, so ignore them
%ignore pagmo::archipelago::archipelago(pagmo::archipelago &&);
%ignore pagmo::archipelago::operator=(pagmo::archipelago &&);

// Better to handle the iterator on the C# side
%ignore pagmo::archipelago::operator[];

// Serialization templates — never expose them:
%ignore pagmo::archipelago::save;
%ignore pagmo::archipelago::load;

// Iterators — do not expose:
%ignore pagmo::archipelago::begin;
%ignore pagmo::archipelago::end;

// -----------------------------------------------------------------------------
// SWIG-visible facade declaration
// -----------------------------------------------------------------------------

class pagmo::archipelago {
public:
    // Use a SWIG-safe and predictable size type.
    typedef std::size_t size_type;

    archipelago();
    archipelago(const archipelago &);
    ~archipelago();

    size_type size() const;

    void evolve(unsigned n = 1);
    void wait() noexcept;
    void wait_check();
    pagmo::evolve_status status() const;

    // Topology access is OK (topology is a value wrapper type in pagmo)
    pagmo::topology get_topology() const;
    void set_topology(pagmo::topology);

    pagmo::migration_type get_migration_type() const;
    void set_migration_type(pagmo::migration_type);

    pagmo::migrant_handling get_migrant_handling() const;
    void set_migrant_handling(pagmo::migrant_handling);
};

// -----------------------------------------------------------------------------
// Wrapped replacements for tuple-heavy APIs
// We implement them as new C++ methods and then %rename them
// so the C# API looks clean (no "_wrapped").
// -----------------------------------------------------------------------------

%extend pagmo::archipelago {

    // ---- Migrants DB ----
    std::vector<pagmoWrap::IndividualsGroup> get_migrants_db_wrapped() const {
        auto db = self->get_migrants_db(); // real method returns migrants_db_t
        std::vector<pagmoWrap::IndividualsGroup> out;
        out.reserve(db.size());
        for (const auto &t : db) {
            out.push_back(pagmoWrap::FromIndividualsGroupTuple(t));
        }
        return out;
    }

    void set_migrants_db_wrapped(const std::vector<pagmoWrap::IndividualsGroup> &v) {
        pagmo::archipelago::migrants_db_t db;
        db.reserve(v.size());
        for (const auto &g : v) {
            db.push_back(pagmoWrap::ToIndividualsGroupTuple(g));
        }
        self->set_migrants_db(std::move(db));
    }

    // ---- Migration log ----
    std::vector<pagmoWrap::MigrationEntry> get_migration_log_wrapped() const {
        auto log = self->get_migration_log(); // real method returns migration_log_t
        std::vector<pagmoWrap::MigrationEntry> out;
        out.reserve(log.size());
        for (const auto &t : log) {
            out.push_back(pagmoWrap::FromMigrationEntryTuple(t));
        }
        return out;
    }

    pagmo::island &get_island(size_type i) {
        return (*self)[i];
    }

    //const pagmo::island &get_island_const(size_type i) const {
    //    return (*self)[i];
    //}

}

// Clean public names in C# (and other languages):
%rename(get_migrants_db) pagmo::archipelago::get_migrants_db_wrapped;
%rename(set_migrants_db) pagmo::archipelago::set_migrants_db_wrapped;
%rename(get_migration_log) pagmo::archipelago::get_migration_log_wrapped;
