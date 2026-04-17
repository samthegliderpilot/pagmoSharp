#pragma once

#include <memory>
#include <stdexcept>
#include <string>

#include <pagmo/types.hpp>

#include "tuple_adapters.h"

namespace pagmoWrap
{
    /// <summary>
    /// Director interface for managed C# selection-policy implementations.
    /// C# subclasses must override select(); get_name()/get_extra_info() are optional.
    /// Named *_callback to match the problem_callback / algorithm_callback convention:
    /// *_callback = SWIG director interface that C# subclasses;
    /// managed_* = copy-safe UDT that pagmo stores by value.
    /// </summary>
    class s_policy_callback
    {
    public:
        virtual ~s_policy_callback() = default;

        /// <summary>
        /// Selects individuals from the population to migrate out.
        /// </summary>
        virtual pagmoWrap::IndividualsGroup select(
            const pagmoWrap::IndividualsGroup& population,
            const pagmo::vector_double::size_type& n_f,
            const pagmo::vector_double::size_type& n_ec,
            const pagmo::vector_double::size_type& n_ic,
            const pagmo::vector_double::size_type& n_obj,
            const pagmo::vector_double::size_type& pop_size,
            const pagmo::vector_double& tol
        ) const = 0;

        /// <summary>Returns a human-readable policy name.</summary>
        virtual std::string get_name() const { return "C# s_policy"; }

        /// <summary>Returns additional diagnostic information.</summary>
        virtual std::string get_extra_info() const { return ""; }

        /// <summary>
        /// Returns true when this policy is properly constructed and safe for pagmo to use.
        /// </summary>
        virtual bool is_valid() const { return true; }
    };

    /// <summary>
    /// Copy-safe UDT that pagmo can store by value. Holds a shared_ptr to the director
    /// callback so copies are safe across pagmo's internal type-erasure.
    /// </summary>
    class managed_s_policy
    {
    private:
        std::shared_ptr<s_policy_callback> _base;

    public:
        managed_s_policy() = default;

        explicit managed_s_policy(s_policy_callback* base)
            : _base(base)
        {
        }

        managed_s_policy(const managed_s_policy&) = default;
        managed_s_policy& operator=(const managed_s_policy&) = default;
        ~managed_s_policy() = default;

        void setBasePolicy(s_policy_callback* b)
        {
            if (!b) {
                throw std::invalid_argument("managed_s_policy: base policy must not be null");
            }
            _base.reset(b);
        }

        s_policy_callback* getBasePolicy() const { return _base.get(); }

        pagmo::individuals_group_t select(
            const pagmo::individuals_group_t& population,
            const pagmo::vector_double::size_type& n_f,
            const pagmo::vector_double::size_type& n_ec,
            const pagmo::vector_double::size_type& n_ic,
            const pagmo::vector_double::size_type& n_obj,
            const pagmo::vector_double::size_type& pop_size,
            const pagmo::vector_double& tol
        ) const
        {
            if (!_base) {
                return population;
            }

            pagmoWrap::IndividualsGroup popWrapped = pagmoWrap::FromIndividualsGroupTuple(population);
            pagmoWrap::IndividualsGroup result = _base->select(
                popWrapped, n_f, n_ec, n_ic, n_obj, pop_size, tol);
            return pagmoWrap::ToIndividualsGroupTuple(result);
        }

        std::string get_name()       const { return _base ? _base->get_name()       : "C# s_policy"; }
        std::string get_extra_info() const { return _base ? _base->get_extra_info() : "";             }
        bool        is_valid()       const { return _base ? _base->is_valid()       : false;          }
    };

} // namespace pagmoWrap
