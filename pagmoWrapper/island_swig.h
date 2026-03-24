// pagmoWrapper/island_swig.h
#pragma once

#include <cstddef>
#include <string>

namespace pagmo {
    class algorithm;
    class population;
    class problem;
    class bfe;
    class r_policy;
    class s_policy;
    class fair_replace;
    class select_best;
    class thread_island;

    enum class evolve_status;

#ifdef SWIG
    // ------------------------------------------------------------
    // SWIG-visible facade ONLY (SWIG parsing sees this).
    // Must NOT be seen by the C++ compiler.
    // ------------------------------------------------------------
    class island {
    public:
        // No default ctor exposed in bindings.
        ~island();
        island(const island&);
        island(island&&) noexcept;
        island& operator=(island&&) noexcept;
        island& operator=(const island&);

        void evolve(unsigned n = 1);
        void wait_check();
        void wait();
        evolve_status status() const;

        algorithm get_algorithm() const;
        void set_algorithm(const algorithm&);

        population get_population() const;
        void set_population(const population&);

        std::string get_name() const;
        std::string get_extra_info() const;

        bool is_valid() const;
    };
#else
    // ------------------------------------------------------------
    // Normal C++ compilation: do NOT define island here.
    // Real definition comes from <pagmo/island.hpp>.
    // ------------------------------------------------------------
    class island;
#endif
} // namespace pagmo


#ifndef SWIG
// ------------------------------------------------------------
// Shim helper functions - compiled into the wrapper build only.
// These call the real templated constructors in pagmo::island.
// ------------------------------------------------------------
#include <pagmo/island.hpp>
#include <pagmo/algorithm.hpp>
#include <pagmo/problem.hpp>
#include <pagmo/population.hpp>
#include <pagmo/bfe.hpp>
#include <pagmo/r_policy.hpp>
#include <pagmo/s_policy.hpp>
#include <pagmo/r_policies/fair_replace.hpp>
#include <pagmo/s_policies/select_best.hpp>
#include <pagmo/islands/thread_island.hpp>

namespace pagmoWrap {

    // algorithm + population
    inline pagmo::island Island_FromAlgoPop(const pagmo::algorithm& a,
        const pagmo::population& p)
    {
        return pagmo::island(a, p);
    }

    // algorithm + population + r/s policies
    inline pagmo::island Island_FromAlgoPopPolicies(const pagmo::algorithm& a,
        const pagmo::population& p,
        const pagmo::r_policy& r,
        const pagmo::s_policy& s)
    {
        return pagmo::island(a, p, r, s);
    }

    // algorithm + population + managed-policy wrappers
    inline pagmo::island Island_FromAlgoPopManagedPolicies(const pagmo::algorithm& a,
        const pagmo::population& p,
        const pagmoWrap::r_policyPagmoWrapper& r,
        const pagmoWrap::s_policyPagmoWrapper& s)
    {
        return pagmo::island(a, p, pagmo::r_policy(r), pagmo::s_policy(s));
    }

    // algorithm + population + concrete policies
    inline pagmo::island Island_FromAlgoPopFairSelect(const pagmo::algorithm& a,
        const pagmo::population& p,
        const pagmo::fair_replace& r,
        const pagmo::select_best& s)
    {
        return pagmo::island(a, p, r, s);
    }

    // algorithm + problem + pop size + seed
    inline pagmo::island Island_FromAlgoProb(const pagmo::algorithm& a,
        const pagmo::problem& prob,
        std::size_t pop_size,
        unsigned seed)
    {
        return pagmo::island(a,
            prob,
            static_cast<pagmo::population::size_type>(pop_size),
            seed);
    }

    // explicit thread_island + algorithm + problem + pop size + seed
    inline pagmo::island Island_FromThreadIslAlgoProb(const pagmo::thread_island& isl,
        const pagmo::algorithm& a,
        const pagmo::problem& prob,
        std::size_t pop_size,
        unsigned seed)
    {
        return pagmo::island(isl, a, prob, static_cast<pagmo::population::size_type>(pop_size), seed);
    }

    // algorithm + problem + pop size + r/s policies + seed
    inline pagmo::island Island_FromAlgoProbPolicies(const pagmo::algorithm& a,
        const pagmo::problem& prob,
        std::size_t pop_size,
        const pagmo::r_policy& r,
        const pagmo::s_policy& s,
        unsigned seed)
    {
        return pagmo::island(a,
            prob,
            static_cast<pagmo::population::size_type>(pop_size),
            r, s,
            seed);
    }

    // algorithm + problem + pop size + concrete policies + seed
    inline pagmo::island Island_FromAlgoProbFairSelect(const pagmo::algorithm& a,
        const pagmo::problem& prob,
        std::size_t pop_size,
        const pagmo::fair_replace& r,
        const pagmo::select_best& s,
        unsigned seed)
    {
        return pagmo::island(a,
            prob,
            static_cast<pagmo::population::size_type>(pop_size),
            r, s,
            seed);
    }

    // explicit thread_island + algorithm + problem + pop size + concrete policies + seed
    inline pagmo::island Island_FromThreadIslAlgoProbFairSelect(const pagmo::thread_island& isl,
        const pagmo::algorithm& a,
        const pagmo::problem& prob,
        std::size_t pop_size,
        const pagmo::fair_replace& r,
        const pagmo::select_best& s,
        unsigned seed)
    {
        return pagmo::island(isl, a, prob, static_cast<pagmo::population::size_type>(pop_size), r, s, seed);
    }

    // algorithm + problem + pop size + managed-policy wrappers + seed
    inline pagmo::island Island_FromAlgoProbManagedPolicies(const pagmo::algorithm& a,
        const pagmo::problem& prob,
        std::size_t pop_size,
        const pagmoWrap::r_policyPagmoWrapper& r,
        const pagmoWrap::s_policyPagmoWrapper& s,
        unsigned seed)
    {
        return pagmo::island(a,
            prob,
            static_cast<pagmo::population::size_type>(pop_size),
            pagmo::r_policy(r), pagmo::s_policy(s),
            seed);
    }

    // explicit thread_island + algorithm + problem + pop size + managed-policy wrappers + seed
    inline pagmo::island Island_FromThreadIslAlgoProbManagedPolicies(const pagmo::thread_island& isl,
        const pagmo::algorithm& a,
        const pagmo::problem& prob,
        std::size_t pop_size,
        const pagmoWrap::r_policyPagmoWrapper& r,
        const pagmoWrap::s_policyPagmoWrapper& s,
        unsigned seed)
    {
        return pagmo::island(isl, a, prob, static_cast<pagmo::population::size_type>(pop_size), pagmo::r_policy(r),
                             pagmo::s_policy(s), seed);
    }

    // algorithm + problem + bfe + pop size + seed
    inline pagmo::island Island_FromAlgoProbBfe(const pagmo::algorithm& a,
        const pagmo::problem& prob,
        const pagmo::bfe& b,
        std::size_t pop_size,
        unsigned seed)
    {
        return pagmo::island(a,
            prob,
            b,
            static_cast<pagmo::population::size_type>(pop_size),
            seed);
    }

    // explicit thread_island + algorithm + problem + bfe + pop size + seed
    inline pagmo::island Island_FromThreadIslAlgoProbBfe(const pagmo::thread_island& isl,
        const pagmo::algorithm& a,
        const pagmo::problem& prob,
        const pagmo::bfe& b,
        std::size_t pop_size,
        unsigned seed)
    {
        return pagmo::island(isl, a, prob, b, static_cast<pagmo::population::size_type>(pop_size), seed);
    }

    // algorithm + problem + bfe + pop size + r/s policies + seed
    inline pagmo::island Island_FromAlgoProbBfePolicies(const pagmo::algorithm& a,
        const pagmo::problem& prob,
        const pagmo::bfe& b,
        std::size_t pop_size,
        const pagmo::r_policy& r,
        const pagmo::s_policy& s,
        unsigned seed)
    {
        return pagmo::island(a,
            prob,
            b,
            static_cast<pagmo::population::size_type>(pop_size),
            r, s,
            seed);
    }

    // algorithm + problem + bfe + pop size + managed-policy wrappers + seed
    inline pagmo::island Island_FromAlgoProbBfeManagedPolicies(const pagmo::algorithm& a,
        const pagmo::problem& prob,
        const pagmo::bfe& b,
        std::size_t pop_size,
        const pagmoWrap::r_policyPagmoWrapper& r,
        const pagmoWrap::s_policyPagmoWrapper& s,
        unsigned seed)
    {
        return pagmo::island(a,
            prob,
            b,
            static_cast<pagmo::population::size_type>(pop_size),
            pagmo::r_policy(r), pagmo::s_policy(s),
            seed);
    }

    // explicit thread_island + algorithm + problem + bfe + pop size + managed-policy wrappers + seed
    inline pagmo::island Island_FromThreadIslAlgoProbBfeManagedPolicies(const pagmo::thread_island& isl,
        const pagmo::algorithm& a,
        const pagmo::problem& prob,
        const pagmo::bfe& b,
        std::size_t pop_size,
        const pagmoWrap::r_policyPagmoWrapper& r,
        const pagmoWrap::s_policyPagmoWrapper& s,
        unsigned seed)
    {
        return pagmo::island(isl, a, prob, b, static_cast<pagmo::population::size_type>(pop_size), pagmo::r_policy(r),
                             pagmo::s_policy(s), seed);
    }

    // algorithm + problem + bfe + pop size + concrete policies + seed
    inline pagmo::island Island_FromAlgoProbBfeFairSelect(const pagmo::algorithm& a,
        const pagmo::problem& prob,
        const pagmo::bfe& b,
        std::size_t pop_size,
        const pagmo::fair_replace& r,
        const pagmo::select_best& s,
        unsigned seed)
    {
        return pagmo::island(a,
            prob,
            b,
            static_cast<pagmo::population::size_type>(pop_size),
            r, s,
            seed);
    }

    // explicit thread_island + algorithm + problem + bfe + pop size + concrete policies + seed
    inline pagmo::island Island_FromThreadIslAlgoProbBfeFairSelect(const pagmo::thread_island& isl,
        const pagmo::algorithm& a,
        const pagmo::problem& prob,
        const pagmo::bfe& b,
        std::size_t pop_size,
        const pagmo::fair_replace& r,
        const pagmo::select_best& s,
        unsigned seed)
    {
        return pagmo::island(isl, a, prob, b, static_cast<pagmo::population::size_type>(pop_size), r, s, seed);
    }

} // namespace pagmoWrap
#endif
