#pragma once

#include <memory>
#include <string>
#include <pagmo/types.hpp>
#include "tuple_adapters.h"

namespace pagmoWrap
{
    class s_policyBase
    {
    public:
        virtual ~s_policyBase() {}

        virtual pagmoWrap::IndividualsGroup select(
            const pagmoWrap::IndividualsGroup& a,
            const pagmo::vector_double::size_type& b,
            const pagmo::vector_double::size_type& c,
            const pagmo::vector_double::size_type& d,
            const pagmo::vector_double::size_type& e,
            const pagmo::vector_double::size_type& f,
            const pagmo::vector_double& g
        ) const = 0;

        virtual std::string get_name() const { return ""; }
        virtual std::string get_extra_info() const { return ""; }
        virtual bool is_valid() const { return false; }
    };


    class s_policyPagmoWrapper
    {
    private:
        std::shared_ptr<s_policyBase> _base;

    public:
        s_policyPagmoWrapper() = default;
        explicit s_policyPagmoWrapper(s_policyBase* base) : _base(base) {}
        s_policyPagmoWrapper(const s_policyPagmoWrapper&) = default;
        s_policyPagmoWrapper& operator=(const s_policyPagmoWrapper&) = default;

        void setBasePolicy(s_policyBase* b) { _base.reset(b); }
        s_policyBase* getBasePolicy() { return _base.get(); }

        pagmo::individuals_group_t select(
            const pagmo::individuals_group_t& a,
            const pagmo::vector_double::size_type& b,
            const pagmo::vector_double::size_type& c,
            const pagmo::vector_double::size_type& d,
            const pagmo::vector_double::size_type& e,
            const pagmo::vector_double::size_type& f,
            const pagmo::vector_double& g
        ) const
        {
            // tuple -> IndividualsGroup for managed override
            pagmoWrap::IndividualsGroup aa = pagmoWrap::FromIndividualsGroupTuple(a);

            // call managed override (base class)
            if (!_base) {
                return a;
            }
            pagmoWrap::IndividualsGroup rr = _base->select(aa, b, c, d, e, f, g);

            // IndividualsGroup -> tuple back to pagmo
            return pagmoWrap::ToIndividualsGroupTuple(rr);
        }

        std::string get_name() const { return _base ? _base->get_name() : ""; }
        std::string get_extra_info() const { return _base ? _base->get_extra_info() : ""; }
        bool is_valid() const { return _base ? _base->is_valid() : false; }
    };
}
