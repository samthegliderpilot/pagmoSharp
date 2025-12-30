
%{
#include "pagmo/archipelago.hpp"
#include "tuple_adapters.h"
%}

%include "pagmoWrapper/tuple_adapters.h"
%typemap(csclassmodifiers) pagmo::archipelago "public partial class"
// ------------------------------------------------------------
// SWIG-visible facade of pagmo::archipelago
// (NO tuple aliases, NO migrants_db_t, NO migration_log_t)
// ------------------------------------------------------------

class archipelago {
public:
    typedef std::vector<std::unique_ptr<island>>::size_type size_type;

    archipelago();
    archipelago(const archipelago &);
    archipelago(archipelago &&) noexcept;
    ~archipelago();

    island &operator[](size_type);
    const island &operator[](size_type) const;
    size_type size() const;

    void evolve(unsigned n = 1);
    void wait() noexcept;
    void wait_check();
    evolve_status status() const;

    // We do NOT declare get_migrants_db() or set_migrants_db() here.
};


// ------------------------------------------------------------
// Add wrapped versions for migrants DB
// ------------------------------------------------------------
%extend pagmo::archipelago {

  std::vector<pagmoWrap::IndividualsGroup> get_migrants_db_wrapped() const {
    auto db = self->get_migrants_db(); // real method, real type
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

  std::vector<pagmoWrap::MigrationEntry> get_migration_log() const {
    auto log = self->get_migration_log(); // vector<migration_entry_t>
    std::vector<pagmoWrap::MigrationEntry> out;
    out.reserve(log.size());
    for (const auto &t : log) {
      out.push_back(pagmoWrap::FromMigrationEntryTuple(t));
    }
    return out;
  }
}