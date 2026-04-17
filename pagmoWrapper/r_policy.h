#pragma once

#include <memory>
#include <stdexcept>
#include <string>

#include <pagmo/r_policy.hpp>
#include <pagmo/types.hpp>

#include "tuple_adapters.h"

namespace pagmoWrap
{
    /// <summary>
    /// Director interface for managed C# replacement-policy implementations.
    /// C# subclasses override replace() and optionally get_name()/get_extra_info().
    /// Named *_callback to match the problem_callback / algorithm_callback convention:
    /// *_callback = SWIG director interface that C# subclasses;
    /// managed_* = copy-safe UDT that pagmo stores by value.
    /// </summary>
    class r_policy_callback
    {
    public:
        virtual ~r_policy_callback() = default;

        /// <summary>
        /// Selects individuals from the incoming group to replace into the island population.
        /// The default pass-through returns the incoming group unchanged.
        /// </summary>
        virtual pagmoWrap::IndividualsGroup replace(
            const pagmoWrap::IndividualsGroup& incoming,
            const pagmo::vector_double::size_type& n_f,
            const pagmo::vector_double::size_type& n_ec,
            const pagmo::vector_double::size_type& n_ic,
            const pagmo::vector_double::size_type& n_obj,
            const pagmo::vector_double::size_type& pop_size,
            const pagmo::vector_double& tol,
            const pagmoWrap::IndividualsGroup& current
        ) const
        {
            return incoming;
        }

        /// <summary>Returns a human-readable policy name.</summary>
        virtual std::string get_name() const { return "C# r_policy"; }

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
    class managed_r_policy
    {
    private:
        std::shared_ptr<r_policy_callback> _base;

    public:
        managed_r_policy() = default;

        explicit managed_r_policy(r_policy_callback* base)
            : _base(base)
        {
        }

        managed_r_policy(const managed_r_policy&) = default;
        managed_r_policy& operator=(const managed_r_policy&) = default;
        ~managed_r_policy() = default;

        void setBasePolicy(r_policy_callback* b)
        {
            if (!b) {
                throw std::invalid_argument("managed_r_policy: base policy must not be null");
            }
            _base.reset(b);
        }

        r_policy_callback* getBasePolicy() const { return _base.get(); }

        pagmo::individuals_group_t replace(
            const pagmo::individuals_group_t& incoming,
            const pagmo::vector_double::size_type& n_f,
            const pagmo::vector_double::size_type& n_ec,
            const pagmo::vector_double::size_type& n_ic,
            const pagmo::vector_double::size_type& n_obj,
            const pagmo::vector_double::size_type& pop_size,
            const pagmo::vector_double& tol,
            const pagmo::individuals_group_t& current
        ) const
        {
            if (!_base) {
                return incoming;
            }

            pagmoWrap::IndividualsGroup incomingWrapped = pagmoWrap::FromIndividualsGroupTuple(incoming);
            pagmoWrap::IndividualsGroup currentWrapped  = pagmoWrap::FromIndividualsGroupTuple(current);
            pagmoWrap::IndividualsGroup result = _base->replace(
                incomingWrapped, n_f, n_ec, n_ic, n_obj, pop_size, tol, currentWrapped);
            return pagmoWrap::ToIndividualsGroupTuple(result);
        }

        std::string get_name()       const { return _base ? _base->get_name()       : "C# r_policy"; }
        std::string get_extra_info() const { return _base ? _base->get_extra_info() : "";             }
        bool        is_valid()       const { return _base ? _base->is_valid()       : false;          }
    };

} // namespace pagmoWrap
