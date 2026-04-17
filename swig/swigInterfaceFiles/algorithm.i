// swigInterfaceFiles/algorithm.i

%{
#include "pagmo/algorithm.hpp"
%}

%typemap(csclassmodifiers) pagmo::algorithm "public partial class"

// -----------------------------------------------------------------------------
// The templated UDA constructor cannot be wrapped by SWIG, and we don't want to.
// We will construct algorithms via per-UDA conversions like bee_colony::to_algorithm().
// -----------------------------------------------------------------------------

%ignore pagmo::algorithm::algorithm;

// Ignore the templated constructor specifically.
// NOTE: This is a template, so SWIG sees it as 'algorithm(T &&)' and cannot wrap it anyway,
// but explicitly ignoring it avoids accidental bad emissions.
%ignore pagmo::algorithm::algorithm(T &&);

// Ignore templated assignment from UDA.
%ignore pagmo::algorithm::operator=;

// -----------------------------------------------------------------------------
// SWIG-visible API for pagmo::algorithm
// -----------------------------------------------------------------------------

class pagmo::algorithm
{
public:
    // Default constructor (valid in pagmo)
    algorithm();

    // Copy/move
    algorithm(const pagmo::algorithm &);
    algorithm(pagmo::algorithm &&) noexcept;

    pagmo::algorithm &operator=(const pagmo::algorithm &);
    pagmo::algorithm &operator=(pagmo::algorithm &&) noexcept;

    // Core algorithm behavior
    pagmo::population evolve(const pagmo::population &) const;

    void set_seed(unsigned);

    bool has_set_seed() const;
    bool is_stochastic() const;

    void set_verbosity(unsigned);
    bool has_set_verbosity() const;

    std::string get_name() const;
    std::string get_extra_info() const;

    pagmo::thread_safety get_thread_safety() const;

    bool is_valid() const;

    // We will NOT expose extract<T>(), is<T>(), etc (templates).
    // We also will not expose get_ptr()/void* access (not needed for Pattern C).
};
