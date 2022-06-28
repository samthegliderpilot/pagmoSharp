//#pragma once
//#include <pagmo/problem.hpp>
//#include <pagmo/threading.hpp>
//#include <pagmo/types.hpp>
//
//#include "problem.h"
//#include <pagmo/bfe.hpp>
//#include <pagmo/batch_evaluators/default_bfe.hpp>
//
//namespace pagmoWrap
//{
//	class bfeBase : public pagmo::bfe
//	{
//	public:
//        // Default ctor.
//        //bfeBase();
//        virtual ~bfeBase() {}
//        // Constructor from a UDBFE.
//        //template <typename T, generic_ctor_enabler<T> = 0>
//        //explicit bfe(T&& x) : bfe(std::forward<T>(x), std::is_function<uncvref_t<T>>{});
//        // Copy constructor.
//        //bfe(const bfe&);
//        // Move constructor.
//        //bfe(bfe&&) noexcept;
//        // Move assignment operator
//        //bfe& operator=(bfe&&) noexcept;
//        // Copy assignment operator
//        //bfe& operator=(const bfe&);
//        // Assignment from a UDBFE.
//        //template <typename T, generic_ctor_enabler<T> = 0>
//        //bfe& operator=(T&& x);
//
//        // Extraction and related.
//        //template <typename T>
//        //const T* extract() const noexcept;
//        //template <typename T>
//        //T* extract() noexcept
//        //{
//        //    return T(NULL);
//        //}
//        //template <typename T>
//        //virtual bool is() const noexcept
//        //{
//        //    return true;
//        //}
//
//        //// Call operator.
//        //pagmo::vector_double operator()(const pagmo::problem&, const pagmo::vector_double&) const;
//
//        virtual pagmo::vector_double Operator(const pagmo::problem&, const pagmo::vector_double&) const
//        {
//            throw std::exception("The Operator function must be implimented in a derived type (swig does not allow abstract types)");
//        }
//
//        // Name.
//        virtual std::string get_name() const
//        {
//            return "Base BFE";
//        }
//        // Extra info.
//        virtual std::string get_extra_info() const
//        {
//            return "";
//        }
//
//        // Thread safety level.
//        virtual pagmo::thread_safety get_thread_safety() const
//        {
//            return pagmo::thread_safety::none;
//        }
//
//        // Check if the bfe is valid.
//        virtual bool is_valid() const
//        {
//            return true;
//        }
//
//        //// Get the type at runtime. not really needed for C#
//        //std::type_index get_type_index() const;
//
//        // Get a const pointer to the UDBFE.
//        //virtual const void* get_ptr() const
//        //{
//        //    return nullptr;
//        //}
//
//
//        //// Get a mutable pointer to the UDBFE.
//        //virtual void* get_ptr()
//        //{
//        //    return nullptr; //default of 0 is fine?  or null, or -1 or...?
//        //}
//
//        // Serialisation support.
//        //template <typename Archive>
//        //virtual void save(Archive& ar, unsigned) const {}
//
//        //template <typename Archive>
//        //virtual void load(Archive& ar, unsigned) {}
//	};
//
//
//	class bfe
//	{
//	private:
//		bfeBase* _base;
//		void deleteBase() {
//			//TODO: This is something that has me worried.  pagmo will create copies of this,
//			// but if they all share the same pointer, when one of those copies gets destroyed, it
//			// deletes the whole pointer, which breaks other copies.  But not deleting it goes against
//			// the director example for swig, and I fear opens us up to a memory leak in sloppy cases
//			// (cases where the C# isn't behaving)
//			//delete _base;
//		}
//	public:
//        bfe() : _base(0) {}
//
//        bfe(bfeBase* base) : _base(base) { }
//
//        bfe(const bfe& old) : _base(0) {
//            _base = old._base;
//        }
//        ~bfe() {
//            deleteBase();
//        }
//
//        void setBaseBfe(bfeBase* b) {
//            deleteBase(); _base = b;
//        }
//
//        bfeBase* getBaseBfe() {
//            return _base;
//        }
//
//
//        std::string get_name() const
//        {
//            return _base->get_name();
//        }
//        // Extra info.
//        std::string get_extra_info() const
//        {
//            return _base->get_extra_info();
//
//        }
//
//        // Thread safety level.
//        pagmo::thread_safety get_thread_safety() const
//        {
//            return _base->get_thread_safety();
//        }
//
//        // Check if the bfe is valid.
//        bool is_valid() const
//        {
//            return _base->is_valid();
//        }
//
//        //const void* get_ptr() const
//        //{
//        //    return _base->get_ptr();
//        //}
//        //
//        //// Get a mutable pointer to the UDBFE.
//        //void* get_ptr()
//        //{
//        //    return _base->get_ptr();
//        //}
//
//        pagmo::vector_double Operator(const pagmo::problem& problem, const pagmo::vector_double& vect) const
//        {
//            return _base->Operator(problem, vect);
//        }
//
//        pagmo::default_bfe* getDefaultBfe()
//        {
//            return new pagmo::default_bfe();
//        }
//	};
//};