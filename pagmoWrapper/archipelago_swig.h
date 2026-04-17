// pagmoWrapper/archipelago_swig.h
#pragma once

#include <cstddef>
#include <vector>

namespace pagmo {
    class island;
    class topology;
    class algorithm;
    class population;
    class problem;
    class bfe;
    class r_policy;
    class s_policy;
    class thread_island;

    enum class evolve_status;
    enum class migration_type;
    enum class migrant_handling;

#ifdef SWIG
    // ------------------------------------------------------------
    // SWIG-visible facade ONLY.
    // Must NOT be seen by the C++ compiler.
    // ------------------------------------------------------------
    class archipelago {
    public:
        // Core lifetime
        archipelago();
        archipelago(const archipelago&);
        ~archipelago();

        // Core status / execution
        std::size_t size() const;
        void evolve(unsigned n = 1);
        void wait() noexcept;
        void wait_check();
        evolve_status status() const;

        // NOTE:
        // We do NOT expose operator[], begin/end, templated ctors, etc.
        // We expose get_island and push_back via shims + %extend.
    };
#else
    // ------------------------------------------------------------
    // Normal C++ compilation: do NOT define archipelago here.
    // ------------------------------------------------------------
    class archipelago;
#endif
} // namespace pagmo


#ifndef SWIG
// ------------------------------------------------------------
// Shims - compiled into C++ wrapper build only.
// ------------------------------------------------------------
#include <pagmo/archipelago.hpp>
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
#include "r_policy.h"
#include "s_policy.h"

namespace pagmoWrap {

    // ----------------------------
    // Island access - return by VALUE snapshot
    // ----------------------------
    inline pagmo::island Archipelago_GetIslandCopy(const pagmo::archipelago& a, std::size_t idx)
    {
        return a[static_cast<pagmo::archipelago::size_type>(idx)];
    }

    //// If you want a "mutable" island (non-const) you still return by value.
    //// In C# you'll mutate the returned island instance, NOT the one in the archipelago.
    //// So this is primarily for inspection, not for modifying the archipelago's internal island.
    //inline pagmo::island Archipelago_GetIslandCopyMutable(pagmo::archipelago& a, std::size_t idx)
    //{
    //    return a[static_cast<pagmo::archipelago::size_type>(idx)];
    //}

    // ----------------------------
    // push_back shims - call real templates through concrete overload sets
    // (these mirror the common island constructor shapes)
    // ----------------------------

    inline std::size_t Archipelago_PushBack_AlgoProbSizeSeed(pagmo::archipelago& a,
        const pagmo::algorithm& algo,
        const pagmo::problem& prob,
        std::size_t pop_size,
        unsigned seed)
    {
        const auto idx = static_cast<std::size_t>(a.size());
        a.push_back(algo, prob,
            static_cast<pagmo::population::size_type>(pop_size),
            seed);
        return idx;
    }

    inline std::size_t Archipelago_PushBack_AlgoProbBfeSizeSeed(pagmo::archipelago& a,
        const pagmo::algorithm& algo,
        const pagmo::problem& prob,
        const pagmo::bfe& b,
        std::size_t pop_size,
        unsigned seed)
    {
        const auto idx = static_cast<std::size_t>(a.size());
        a.push_back(algo, prob, b,
            static_cast<pagmo::population::size_type>(pop_size),
            seed);
        return idx;
    }

    inline std::size_t Archipelago_PushBack_AlgoProbSizePoliciesSeed(pagmo::archipelago& a,
        const pagmo::algorithm& algo,
        const pagmo::problem& prob,
        std::size_t pop_size,
        const pagmo::r_policy& r,
        const pagmo::s_policy& s,
        unsigned seed)
    {
        const auto idx = static_cast<std::size_t>(a.size());
        a.push_back(algo, prob,
            static_cast<pagmo::population::size_type>(pop_size),
            r, s,
            seed);
        return idx;
    }

    inline std::size_t Archipelago_PushBack_AlgoProbSizeFairSelectSeed(pagmo::archipelago& a,
        const pagmo::algorithm& algo,
        const pagmo::problem& prob,
        std::size_t pop_size,
        const pagmo::fair_replace& r,
        const pagmo::select_best& s,
        unsigned seed)
    {
        const auto idx = static_cast<std::size_t>(a.size());
        a.push_back(algo, prob,
            static_cast<pagmo::population::size_type>(pop_size),
            r, s,
            seed);
        return idx;
    }

    inline std::size_t Archipelago_PushBack_AlgoProbSizeManagedPoliciesSeed(pagmo::archipelago& a,
        const pagmo::algorithm& algo,
        const pagmo::problem& prob,
        std::size_t pop_size,
        const pagmoWrap::managed_r_policy& r,
        const pagmoWrap::managed_s_policy& s,
        unsigned seed)
    {
        const auto idx = static_cast<std::size_t>(a.size());
        a.push_back(algo, prob,
            static_cast<pagmo::population::size_type>(pop_size),
            pagmo::r_policy(r), pagmo::s_policy(s),
            seed);
        return idx;
    }

    inline std::size_t Archipelago_PushBack_AlgoProbBfeSizePoliciesSeed(pagmo::archipelago& a,
        const pagmo::algorithm& algo,
        const pagmo::problem& prob,
        const pagmo::bfe& b,
        std::size_t pop_size,
        const pagmo::r_policy& r,
        const pagmo::s_policy& s,
        unsigned seed)
    {
        const auto idx = static_cast<std::size_t>(a.size());
        a.push_back(algo, prob, b,
            static_cast<pagmo::population::size_type>(pop_size),
            r, s,
            seed);
        return idx;
    }

    inline std::size_t Archipelago_PushBack_AlgoProbBfeSizeFairSelectSeed(pagmo::archipelago& a,
        const pagmo::algorithm& algo,
        const pagmo::problem& prob,
        const pagmo::bfe& b,
        std::size_t pop_size,
        const pagmo::fair_replace& r,
        const pagmo::select_best& s,
        unsigned seed)
    {
        const auto idx = static_cast<std::size_t>(a.size());
        a.push_back(algo, prob, b,
            static_cast<pagmo::population::size_type>(pop_size),
            r, s,
            seed);
        return idx;
    }

    inline std::size_t Archipelago_PushBack_AlgoProbBfeSizeManagedPoliciesSeed(pagmo::archipelago& a,
        const pagmo::algorithm& algo,
        const pagmo::problem& prob,
        const pagmo::bfe& b,
        std::size_t pop_size,
        const pagmoWrap::managed_r_policy& r,
        const pagmoWrap::managed_s_policy& s,
        unsigned seed)
    {
        const auto idx = static_cast<std::size_t>(a.size());
        a.push_back(algo, prob, b,
            static_cast<pagmo::population::size_type>(pop_size),
            pagmo::r_policy(r), pagmo::s_policy(s),
            seed);
        return idx;
    }

    inline std::size_t Archipelago_PushBack_ThreadIslAlgoProbSizeSeed(pagmo::archipelago& a,
        const pagmo::thread_island& isl,
        const pagmo::algorithm& algo,
        const pagmo::problem& prob,
        std::size_t pop_size,
        unsigned seed)
    {
        const auto idx = static_cast<std::size_t>(a.size());
        a.push_back(isl, algo, prob,
            static_cast<pagmo::population::size_type>(pop_size),
            seed);
        return idx;
    }

    inline std::size_t Archipelago_PushBack_ThreadIslAlgoProbSizePoliciesSeed(pagmo::archipelago& a,
        const pagmo::thread_island& isl,
        const pagmo::algorithm& algo,
        const pagmo::problem& prob,
        std::size_t pop_size,
        const pagmo::r_policy& r,
        const pagmo::s_policy& s,
        unsigned seed)
    {
        const auto idx = static_cast<std::size_t>(a.size());
        a.push_back(isl, algo, prob,
            static_cast<pagmo::population::size_type>(pop_size),
            r, s,
            seed);
        return idx;
    }

    inline std::size_t Archipelago_PushBack_ThreadIslAlgoProbSizeFairSelectSeed(pagmo::archipelago& a,
        const pagmo::thread_island& isl,
        const pagmo::algorithm& algo,
        const pagmo::problem& prob,
        std::size_t pop_size,
        const pagmo::fair_replace& r,
        const pagmo::select_best& s,
        unsigned seed)
    {
        const auto idx = static_cast<std::size_t>(a.size());
        a.push_back(isl, algo, prob,
            static_cast<pagmo::population::size_type>(pop_size),
            r, s,
            seed);
        return idx;
    }

    inline std::size_t Archipelago_PushBack_ThreadIslAlgoProbSizeManagedPoliciesSeed(pagmo::archipelago& a,
        const pagmo::thread_island& isl,
        const pagmo::algorithm& algo,
        const pagmo::problem& prob,
        std::size_t pop_size,
        const pagmoWrap::managed_r_policy& r,
        const pagmoWrap::managed_s_policy& s,
        unsigned seed)
    {
        const auto idx = static_cast<std::size_t>(a.size());
        a.push_back(isl, algo, prob,
            static_cast<pagmo::population::size_type>(pop_size),
            pagmo::r_policy(r), pagmo::s_policy(s),
            seed);
        return idx;
    }

    inline std::size_t Archipelago_PushBack_ThreadIslAlgoProbBfeSizeSeed(pagmo::archipelago& a,
        const pagmo::thread_island& isl,
        const pagmo::algorithm& algo,
        const pagmo::problem& prob,
        const pagmo::bfe& b,
        std::size_t pop_size,
        unsigned seed)
    {
        const auto idx = static_cast<std::size_t>(a.size());
        a.push_back(isl, algo, prob, b,
            static_cast<pagmo::population::size_type>(pop_size),
            seed);
        return idx;
    }

    inline std::size_t Archipelago_PushBack_ThreadIslAlgoProbBfeSizeFairSelectSeed(pagmo::archipelago& a,
        const pagmo::thread_island& isl,
        const pagmo::algorithm& algo,
        const pagmo::problem& prob,
        const pagmo::bfe& b,
        std::size_t pop_size,
        const pagmo::fair_replace& r,
        const pagmo::select_best& s,
        unsigned seed)
    {
        const auto idx = static_cast<std::size_t>(a.size());
        a.push_back(isl, algo, prob, b,
            static_cast<pagmo::population::size_type>(pop_size),
            r, s,
            seed);
        return idx;
    }

    inline std::size_t Archipelago_PushBack_ThreadIslAlgoProbBfeSizeManagedPoliciesSeed(pagmo::archipelago& a,
        const pagmo::thread_island& isl,
        const pagmo::algorithm& algo,
        const pagmo::problem& prob,
        const pagmo::bfe& b,
        std::size_t pop_size,
        const pagmoWrap::managed_r_policy& r,
        const pagmoWrap::managed_s_policy& s,
        unsigned seed)
    {
        const auto idx = static_cast<std::size_t>(a.size());
        a.push_back(isl, algo, prob, b,
            static_cast<pagmo::population::size_type>(pop_size),
            pagmo::r_policy(r), pagmo::s_policy(s),
            seed);
        return idx;
    }

    inline std::size_t Archipelago_PushBack_ThreadIslAlgoProbBfeSizePoliciesSeed(pagmo::archipelago& a,
        const pagmo::thread_island& isl,
        const pagmo::algorithm& algo,
        const pagmo::problem& prob,
        const pagmo::bfe& b,
        std::size_t pop_size,
        const pagmo::r_policy& r,
        const pagmo::s_policy& s,
        unsigned seed)
    {
        const auto idx = static_cast<std::size_t>(a.size());
        a.push_back(isl, algo, prob, b,
            static_cast<pagmo::population::size_type>(pop_size),
            r, s,
            seed);
        return idx;
    }



} // namespace pagmoWrap
#endif
