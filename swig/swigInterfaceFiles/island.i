// swigInterfaceFiles/island.i

%{
#include "pagmo/island.hpp"
#include "pagmo/algorithm.hpp"
#include "pagmo/problem.hpp"
#include "pagmo/population.hpp"
#include "pagmo/bfe.hpp"
#include "pagmo/r_policy.hpp"
#include "pagmo/s_policy.hpp"
#include "pagmo/islands/thread_island.hpp"
#include "pagmo/r_policies/fair_replace.hpp"
#include "pagmo/s_policies/select_best.hpp"

// Facade + shims
#include "island_swig.h"
%}

// C# partial class support (your pattern)
%typemap(csclassmodifiers) pagmo::island "public partial class"
%typemap(csclassmodifiers) island "public partial class"

// SWIG parses and wraps the facade.
%include "pagmoWrapper/island_swig.h"

// We use std::size_t in factory signatures.
%include "stdint.i"

// -----------------------------------------------------------------------------
// IMPORTANT: Do NOT expose default ctor in bindings.
// -----------------------------------------------------------------------------
%ignore pagmo::island::island();
%ignore pagmo::island::get_ptr;
%ignore pagmo::island::get_r_policy;
%ignore pagmo::island::get_s_policy;

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
        const pagmo::fair_replace &r,
        const pagmo::select_best &s
    ) {
        return pagmoWrap::Island_FromAlgoPopFairSelect(a, p, r, s);
    }

    static pagmo::island CreateFromPopulationWithPolicies(
        const pagmo::algorithm &a,
        const pagmo::population &p,
        const pagmoWrap::managed_r_policy &r,
        const pagmoWrap::managed_s_policy &s
    ) {
        return pagmoWrap::Island_FromAlgoPopManagedPolicies(a, p, r, s);
    }

    static pagmo::island Create(
        const pagmo::algorithm &a,
        const pagmo::problem &prob,
        std::size_t pop_size,
        unsigned seed
    ) {
        return pagmoWrap::Island_FromAlgoProb(a, prob, pop_size, seed);
    }

    static pagmo::island CreateWithThreadIsland(
        const pagmo::thread_island &isl,
        const pagmo::algorithm &a,
        const pagmo::problem &prob,
        std::size_t pop_size,
        unsigned seed
    ) {
        return pagmoWrap::Island_FromThreadIslAlgoProb(isl, a, prob, pop_size, seed);
    }

    static pagmo::island CreateWithPolicies(
        const pagmo::algorithm &a,
        const pagmo::problem &prob,
        std::size_t pop_size,
        const pagmo::fair_replace &r,
        const pagmo::select_best &s,
        unsigned seed
    ) {
        return pagmoWrap::Island_FromAlgoProbFairSelect(a, prob, pop_size, r, s, seed);
    }

    static pagmo::island CreateWithThreadIslandAndPolicies(
        const pagmo::thread_island &isl,
        const pagmo::algorithm &a,
        const pagmo::problem &prob,
        std::size_t pop_size,
        const pagmo::fair_replace &r,
        const pagmo::select_best &s,
        unsigned seed
    ) {
        return pagmoWrap::Island_FromThreadIslAlgoProbFairSelect(isl, a, prob, pop_size, r, s, seed);
    }

    static pagmo::island CreateWithPolicies(
        const pagmo::algorithm &a,
        const pagmo::problem &prob,
        std::size_t pop_size,
        const pagmoWrap::managed_r_policy &r,
        const pagmoWrap::managed_s_policy &s,
        unsigned seed
    ) {
        return pagmoWrap::Island_FromAlgoProbManagedPolicies(a, prob, pop_size, r, s, seed);
    }

    static pagmo::island CreateWithThreadIslandAndPolicies(
        const pagmo::thread_island &isl,
        const pagmo::algorithm &a,
        const pagmo::problem &prob,
        std::size_t pop_size,
        const pagmoWrap::managed_r_policy &r,
        const pagmoWrap::managed_s_policy &s,
        unsigned seed
    ) {
        return pagmoWrap::Island_FromThreadIslAlgoProbManagedPolicies(isl, a, prob, pop_size, r, s, seed);
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

    static pagmo::island CreateWithThreadIslandAndBfe(
        const pagmo::thread_island &isl,
        const pagmo::algorithm &a,
        const pagmo::problem &prob,
        const pagmo::bfe &b,
        std::size_t pop_size,
        unsigned seed
    ) {
        return pagmoWrap::Island_FromThreadIslAlgoProbBfe(isl, a, prob, b, pop_size, seed);
    }

    static pagmo::island CreateWithBfeAndPolicies(
        const pagmo::algorithm &a,
        const pagmo::problem &prob,
        const pagmo::bfe &b,
        std::size_t pop_size,
        const pagmo::fair_replace &r,
        const pagmo::select_best &s,
        unsigned seed
    ) {
        return pagmoWrap::Island_FromAlgoProbBfeFairSelect(a, prob, b, pop_size, r, s, seed);
    }

    static pagmo::island CreateWithThreadIslandAndBfeAndPolicies(
        const pagmo::thread_island &isl,
        const pagmo::algorithm &a,
        const pagmo::problem &prob,
        const pagmo::bfe &b,
        std::size_t pop_size,
        const pagmo::fair_replace &r,
        const pagmo::select_best &s,
        unsigned seed
    ) {
        return pagmoWrap::Island_FromThreadIslAlgoProbBfeFairSelect(isl, a, prob, b, pop_size, r, s, seed);
    }

    static pagmo::island CreateWithBfeAndPolicies(
        const pagmo::algorithm &a,
        const pagmo::problem &prob,
        const pagmo::bfe &b,
        std::size_t pop_size,
        const pagmoWrap::managed_r_policy &r,
        const pagmoWrap::managed_s_policy &s,
        unsigned seed
    ) {
        return pagmoWrap::Island_FromAlgoProbBfeManagedPolicies(a, prob, b, pop_size, r, s, seed);
    }

    static pagmo::island CreateWithThreadIslandAndBfeAndPolicies(
        const pagmo::thread_island &isl,
        const pagmo::algorithm &a,
        const pagmo::problem &prob,
        const pagmo::bfe &b,
        std::size_t pop_size,
        const pagmoWrap::managed_r_policy &r,
        const pagmoWrap::managed_s_policy &s,
        unsigned seed
    ) {
        return pagmoWrap::Island_FromThreadIslAlgoProbBfeManagedPolicies(isl, a, prob, b, pop_size, r, s, seed);
    }

}
