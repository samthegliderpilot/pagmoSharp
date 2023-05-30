#pragma once
#include <cassert>
#include <iostream>
#include <memory>
#include <string>
#include <type_traits>
#include <typeindex>
#include <typeinfo>
#include <utility>

#include <boost/type_traits/integral_constant.hpp>

#include <pagmo/config.hpp>
#include <pagmo/detail/support_xeus_cling.hpp>
#include <pagmo/detail/type_name.hpp>
#include <pagmo/detail/typeid_name_extract.hpp>
#include <pagmo/detail/visibility.hpp>
#include <pagmo/s11n.hpp>
#include <pagmo/s_policy.hpp>
#include <pagmo/type_traits.hpp>
#include <pagmo/types.hpp>

#include "problem.h"


//namespace detail
//{
//
//    struct s_pol_inner_base {
//        virtual ~s_pol_inner_base() {}
//        virtual std::unique_ptr<s_pol_inner_base> clone() const = 0;
//        virtual pagmo::individuals_group_t select(const pagmo::individuals_group_t&, const pagmo::vector_double::size_type&,
//            const pagmo::vector_double::size_type&, const pagmo::vector_double::size_type&,
//            const pagmo::vector_double::size_type&, const pagmo::vector_double::size_type&,
//            const pagmo::vector_double&) const = 0;
//        virtual std::string get_name() const = 0;
//        virtual std::string get_extra_info() const = 0;
//        virtual std::type_index get_type_index() const = 0;
//        virtual const void* get_ptr() const = 0;
//        virtual void* get_ptr() = 0;
//
//    private:
//        friend class boost::serialization::access;
//        template <typename Archive>
//        void serialize(Archive&, unsigned)
//        {
//        }
//    };
//};
namespace pagmoWrap
{
    class s_policyBase
    {
        // Enable the generic ctor only if T is not an s_policy (after removing
        // const/reference qualifiers), and if T is a udsp.
        //template <typename T>
        //using generic_ctor_enabler = pagmo::enable_if_t<
        //    detail::conjunction<detail::negation<std::is_same<s_policy, pagmo::uncvref_t<T>>>, pagmo::is_udsp<pagmo::uncvref_t<T>>>::value, int>;
        //// Implementation of the generic ctor.
        //void generic_ctor_impl();

    public:
        // Default constructor.
        virtual ~s_policyBase() {}
        // Constructor from a UDSP.
        //template <typename T, generic_ctor_enabler<T> = 0>
        //explicit s_policy(T&& x) : m_ptr(std::make_unique<detail::s_pol_inner<uncvref_t<T>>>(std::forward<T>(x)))
        //{
        //    generic_ctor_impl();
        //}
        // Copy constructor.
        //s_policyBase(const s_policyBase&) {}
        // Move constructor.
        //s_policyBase(s_policyBase&&) noexcept {}
        // Move assignment operator
        //virtual s_policyBase& operator=(s_policyBase&&) noexcept;
        ////// Copy assignment operator
        //virtual s_policyBase& operator=(const s_policyBase&);
        // Assignment from a UDSP.
        //template <typename T, generic_ctor_enabler<T> = 0>
        //s_policy& operator=(T&& x)
        //{
        //    return (*this) = s_policy(std::forward<T>(x));
        //}

        // Extraction and related.
        //template <typename T>
        //const T* extract() const noexcept
        //{

        //}
        //template <typename T>
        //bool is() const noexcept
        //{
        //    return extract<T>() != nullptr;
        //}

        // Select.
        virtual pagmo::individuals_group_t select(const pagmo::individuals_group_t& a, const pagmo::vector_double::size_type&,
            const pagmo::vector_double::size_type&, const pagmo::vector_double::size_type&,
            const pagmo::vector_double::size_type&, const pagmo::vector_double::size_type&,
            const pagmo::vector_double&) const {
            return a;
        }

        // Name.
        virtual std::string get_name() const
        {
            return "";
        }
        // Extra info.
        virtual std::string get_extra_info() const { return ""; }

        // Check if the s_policy is valid.
        virtual bool is_valid() const { return false; }

        // Get the type at runtime.
        //virtual std::type_index get_type_index() const;

        //// Get a const pointer to the UDSP.
        //virtual const void* get_ptr() const { return 0; }

        //// Get a mutable pointer to the UDSP.
        //virtual void* get_ptr() { return 0; }


    };
    class s_policyPagmoWrapper
    {
    private:
        s_policyBase* _base;
        void deleteProblem() {
            //TODO: This is something that has me worried.  pagmo will create copies of the problem,
            // but if they all share the same pointer, when one of those copies gets destroyed, it
            // deletes the whole pointer, which breaks other copies.  But not deleting it goes against
            // the director example for swig, and I fear opens us up to a memory leak in sloppy cases
            // (cases where the C# isn't behaving)
            //delete _base;
        }
    public:
        s_policyPagmoWrapper() : _base(0) {}

        //s_policyPagmoWrapper(s_policyBase* base) : _base(base) { }

        //s_policyPagmoWrapper(const s_policyPagmoWrapper& old) : _base(0) {
        //    _base = old._base;
        //}
        //~s_policyPagmoWrapper() {
        //    deleteProblem();
        //}

        void setBasePolicy(s_policyBase* b) {
            deleteProblem(); _base = b;
        }

        s_policyBase* getBasePolicy() {
            return _base;
        }
        //s_policyBase& operator=(s_policyBase&& a) noexcept
        //{
        //    return a;
        //}
        //s_policyBase& operator=(const s_policyBase& a)
        //{
        //    return a;
        //}

        pagmo::individuals_group_t select(const pagmo::individuals_group_t& a, const pagmo::vector_double::size_type& b,
            const pagmo::vector_double::size_type& c, const pagmo::vector_double::size_type& d,
            const pagmo::vector_double::size_type& e, const pagmo::vector_double::size_type& f,
            const pagmo::vector_double& g) const
        {
            return _base->select(a, b, c, d, e, f, g);
        }

        // Name.
        std::string get_name() const
        {
            return _base->get_name();
        }
        // Extra info.
        std::string get_extra_info() const
        {
            return _base->get_extra_info();
        }

        // Check if the s_policy is valid.
        bool is_valid() const
        {
            return _base->is_valid();
        }

        // Get the type at runtime.
        //std::type_index get_type_index() const
        //{
        //    return _base->get_type_index();
        //}

        // Get a const pointer to the UDSP.
        //const void* get_ptr() const
        //{
        //    return _base->get_ptr();
        //}

        //// Get a mutable pointer to the UDSP.
        //void* get_ptr()
        //{
        //    return _base->get_ptr();
        //}

    };
};
