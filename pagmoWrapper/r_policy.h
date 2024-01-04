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
#include <pagmo/r_policy.hpp>
#include <pagmo/detail/support_xeus_cling.hpp>
#include <pagmo/detail/type_name.hpp>
#include <pagmo/detail/typeid_name_extract.hpp>
#include <pagmo/detail/visibility.hpp>
#include <pagmo/s11n.hpp>
#include <pagmo/type_traits.hpp>
#include <pagmo/types.hpp>
#include <tuple>
namespace pagmoWrap
{
    class individuals_group
    {
    public:
        std::vector<unsigned long long> Item1;
        std::vector<vector_double> Item2;
        std::vector<vector_double> Item3;

        individuals_group(std::tuple<std::vector<unsigned long long>, std::vector<vector_double>, std::vector<vector_double>> tu)
        {
            this->Item1 = get<0>(tu);
            this->Item2 = get<1>(tu);
            this->Item3 = get<2>(tu);
        }
    };

    using individuals_group_t = std::tuple<std::vector<unsigned long long>, std::vector<vector_double>, std::vector<vector_double>>;
    class r_policyBase
    {
        //template <typename T>
        //using generic_ctor_enabler = pagmo::enable_if_t<
        //    detail::conjunction<detail::negation<std::is_same<pagmo::r_policy, pagmo::uncvref_t<T>>>, pagmo::is_udrp<pagmo::uncvref_t<T>>>::value, int>;
        //// Implementation of the generic ctor.
        //void generic_ctor_impl();

    public:
        virtual ~r_policyBase() {}

        //// Copy constructor.
        //r_policy(const r_policy&);
        //// Move constructor.
        //r_policy(r_policy&&) noexcept;
        //// Move assignment operator
        //r_policy& operator=(r_policy&&) noexcept;
        //// Copy assignment operator
        //r_policy& operator=(const r_policy&);
        //// Assignment from a UDRP.
        //template <typename T, generic_ctor_enabler<T> = 0>
        //r_policy& operator=(T&& x)
        //{
        //    return (*this) = r_policy(std::forward<T>(x));
        //}

        // Extraction and related.
        //template <typename T>
        //virtual const T* extract() const noexcept
        //{
        //    return (T*)null;

        //}
        /*template <typename T>
        T* extract() noexcept
        {

        }*/
        //template <typename T>
        //bool is() const noexcept
        //{
        //    return extract<T>() != nullptr;
        //}

        // Replace.
        virtual pagmo::individuals_group_t replace(const pagmo::individuals_group_t& a, const pagmo::vector_double::size_type& b,
            const pagmo::vector_double::size_type& c, const pagmo::vector_double::size_type& d,
            const pagmo::vector_double::size_type& e, const pagmo::vector_double::size_type& f,
            const pagmo::vector_double& g, const pagmo::individuals_group_t& h) const
        {
            
            return a;
        }

        individuals_group replaceAndWrap(const pagmo::individuals_group_t& a, const pagmo::vector_double::size_type& b,
            const pagmo::vector_double::size_type& c, const pagmo::vector_double::size_type& d,
            const pagmo::vector_double::size_type& e, const pagmo::vector_double::size_type& f,
            const pagmo::vector_double& g, const pagmo::individuals_group_t& h) const
        {
            return individuals_group(replace(a, b, c, d, e, f, g, h));
        }

        // Name.
        virtual std::string get_name() const
        {
            return "";
        }

        // Extra info.
        virtual std::string get_extra_info() const
        {
            return "";
        }

        // Check if the r_policy is valid.
        virtual bool is_valid() const
        {
            return false;
        }

        // Get the type at runtime.
        //virtual std::type_index get_type_index() const
        //{
        //    return -1;
        //}

        //// Get a const pointer to the UDRP.
        //const void* get_ptr() const;

        //// Get a mutable pointer to the UDRP.
        //virtual void* get_ptr()
        //{
        //    return null;
        //}

    };

    class r_policyPagmoWrapper
    {
    private:
        r_policyBase* _base;
        void deleteProblem() {
            //TODO: This is something that has me worried.  pagmo will create copies of the problem,
            // but if they all share the same pointer, when one of those copies gets destroyed, it
            // deletes the whole pointer, which breaks other copies.  But not deleting it goes against
            // the director example for swig, and I fear opens us up to a memory leak in sloppy cases
            // (cases where the C# isn't behaving)
            //delete _base;
        }
    public:
        r_policyPagmoWrapper() : _base(0) {}

        r_policyPagmoWrapper(r_policyBase* base) : _base(base) { }

        r_policyPagmoWrapper(const r_policyPagmoWrapper& old) : _base(0) {
            _base = old._base;
        }
        ~r_policyPagmoWrapper() {
            deleteProblem();
        }

        void setBasePolicy(r_policyBase* b) {
            deleteProblem(); _base = b;
        }

        r_policyBase* getBasePolicy() {
            return _base;
        }

        pagmo::individuals_group_t replace(const pagmo::individuals_group_t& a, const pagmo::vector_double::size_type& b,
            const pagmo::vector_double::size_type& c, const pagmo::vector_double::size_type& d,
            const pagmo::vector_double::size_type& e, const pagmo::vector_double::size_type& f,
            const pagmo::vector_double& g, const pagmo::individuals_group_t& h) const
        {

            return _base->replace(a, b, c, d, e, f, g, h);
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

        // Check if the r_policy is valid.
        bool is_valid() const
        {
            return _base->is_valid();
        }
    };
};