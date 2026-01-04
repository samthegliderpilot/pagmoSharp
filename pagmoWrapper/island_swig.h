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

        r_policy get_r_policy() const;
        s_policy get_s_policy() const;

        std::string get_name() const;
        std::string get_extra_info() const;

        bool is_valid() const;

        const void* get_ptr() const;
        void* get_ptr();
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

} // namespace pagmoWrap
#endif
