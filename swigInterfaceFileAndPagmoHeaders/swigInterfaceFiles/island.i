// swigInterfaceFiles/island.i

%{
#include "pagmo/island.hpp"
#include "pagmo/algorithm.hpp"
#include "pagmo/problem.hpp"
#include "pagmo/population.hpp"
#include "pagmo/bfe.hpp"
#include "pagmo/r_policy.hpp"
#include "pagmo/s_policy.hpp"

// Facade + shims
#include "island_swig.h"
%}

// SWIG parses the facade only.
%import "pagmoWrapper/island_swig.h"

// C# partial class support (your pattern)
%typemap(csclassmodifiers) pagmo::island "public partial class"

// We use std::size_t in factory signatures.
%include "stdint.i"

// -----------------------------------------------------------------------------
// IMPORTANT: Do NOT expose default ctor in bindings.
// -----------------------------------------------------------------------------
%ignore pagmo::island::island();

// -----------------------------------------------------------------------------
// Factory methods (shim-backed)
// These call the real templated constructors via shim functions.
// Return by value (good for C# ownership and avoids IntPtr ctor issues).
// -----------------------------------------------------------------------------

%extend pagmo::island {

    static pagmo::island CreateFromPopulation(
        const pagmo::algorithm &a,
        const pagmo::population &p
    ) {
        return pagmoWrap::Island_FromAlgoPop(a, p);
    }

    static pagmo::island CreateFromPopulationWithPolicies(
        const pagmo::algorithm &a,
        const pagmo::population &p,
        const pagmo::r_policy &r,
        const pagmo::s_policy &s
    ) {
        return pagmoWrap::Island_FromAlgoPopPolicies(a, p, r, s);
    }

    static pagmo::island Create(
        const pagmo::algorithm &a,
        const pagmo::problem &prob,
        std::size_t pop_size,
        unsigned seed
    ) {
        return pagmoWrap::Island_FromAlgoProb(a, prob, pop_size, seed);
    }

    static pagmo::island CreateWithPolicies(
        const pagmo::algorithm &a,
        const pagmo::problem &prob,
        std::size_t pop_size,
        const pagmo::r_policy &r,
        const pagmo::s_policy &s,
        unsigned seed
    ) {
        return pagmoWrap::Island_FromAlgoProbPolicies(a, prob, pop_size, r, s, seed);
    }

    static pagmo::island CreateWithBfe(
        const pagmo::algorithm &a,
        const pagmo::problem &prob,
        const pagmo::bfe &b,
        std::size_t pop_size,
        unsigned seed
    ) {
        return pagmoWrap::Island_FromAlgoProbBfe(a, prob, b, pop_size, seed);
    }

    static pagmo::island CreateWithBfeAndPolicies(
        const pagmo::algorithm &a,
        const pagmo::problem &prob,
        const pagmo::bfe &b,
        std::size_t pop_size,
        const pagmo::r_policy &r,
        const pagmo::s_policy &s,
        unsigned seed
    ) {
        return pagmoWrap::Island_FromAlgoProbBfePolicies(a, prob, b, pop_size, r, s, seed);
    }
}
