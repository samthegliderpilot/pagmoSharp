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
    /// </summary>
    class s_policyBase
    {
    public:
        virtual ~s_policyBase() = default;

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
    class s_policyPagmoWrapper
    {
    private:
        std::shared_ptr<s_policyBase> _base;

    public:
        s_policyPagmoWrapper() = default;

        explicit s_policyPagmoWrapper(s_policyBase* base)
            : _base(base)
        {
        }

        s_policyPagmoWrapper(const s_policyPagmoWrapper&) = default;
        s_policyPagmoWrapper& operator=(const s_policyPagmoWrapper&) = default;
        ~s_policyPagmoWrapper() = default;

        void setBasePolicy(s_policyBase* b)
        {
            if (!b) {
                throw std::invalid_argument("s_policyPagmoWrapper: base policy must not be null");
            }
            _base.reset(b);
        }

        s_policyBase* getBasePolicy() const { return _base.get(); }

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
