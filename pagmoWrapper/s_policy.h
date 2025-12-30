#pragma once

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
        s_policyBase* _base;

    public:
        s_policyPagmoWrapper() : _base(nullptr) {}
        s_policyPagmoWrapper(s_policyBase* base) : _base(base) {}
        s_policyPagmoWrapper(const s_policyPagmoWrapper& old) : _base(old._base) {}

        void setBasePolicy(s_policyBase* b) { _base = b; }
        s_policyBase* getBasePolicy() { return _base; }

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
            pagmoWrap::IndividualsGroup rr = _base->select(aa, b, c, d, e, f, g);

            // IndividualsGroup -> tuple back to pagmo
            return pagmoWrap::ToIndividualsGroupTuple(rr);
        }

        std::string get_name() const { return _base->get_name(); }
        std::string get_extra_info() const { return _base->get_extra_info(); }
        bool is_valid() const { return _base->is_valid(); }
    };
}
