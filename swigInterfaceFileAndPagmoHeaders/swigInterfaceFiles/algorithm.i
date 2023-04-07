%module(naturalvar = 1, directors = "1") pagmo
%{
//#include "cassert"
//#include "memory"
//#include "string"
//#include "type_traits"
//#include "typeindex"
//#include "typeinfo"
//#include "utility"

#include "boost/type_traits/integral_constant.hpp"

#include "pagmo/config.hpp"
#include "pagmo/detail/support_xeus_cling.hpp"
#include "pagmo/detail/type_name.hpp"
#include "pagmo/detail/typeid_name_extract.hpp"
#include "pagmo/detail/visibility.hpp"
#include "pagmo/exceptions.hpp"
#include "pagmo/population.hpp"
#include "pagmo/s11n.hpp"
#include "pagmo/threading.hpp"
#include "pagmo/type_traits.hpp"
#include "pagmo/algorithm.hpp"
%}

%typemap(csclassmodifiers) pagmo::algorithm "public abstract partial class"

//template <typename T> class has_set_verbosity
//{
//    template <typename U>
//    using set_verbosity_t = decltype(std::declval<U&>().set_verbosity(1u));
//    static const bool implementation_defined = std::is_same<void, detected_t<set_verbosity_t, T>>::value;
//
//public:
//    /// Value of the type trait.
//    static const bool value = implementation_defined;
//};
//
//template <typename T>
//const bool has_set_verbosity<T>::value;
//
//template <typename T>
//class override_has_set_verbosity
//{
//    template <typename U>
//    using has_set_verbosity_t = decltype(std::declval<const U&>().has_set_verbosity());
//    static const bool implementation_defined = std::is_same<bool, detected_t<has_set_verbosity_t, T>>::value;
//
//public:
//    /// Value of the type trait.
//    static const bool value = implementation_defined;
//};

//template <typename T>
//const bool override_has_set_verbosity<T>::value;

//template <typename T>
//class has_evolve
//{
//    template <typename U>
//    using evolve_t = decltype(std::declval<const U&>().evolve(std::declval<const population&>()));
//    static const bool implementation_defined = std::is_same<population, detected_t<evolve_t, T>>::value;
//
//public:
//    /// Value of the type trait.
//    static const bool value = implementation_defined;
//};
//
//template <typename T>
//const bool has_evolve<T>::value;
//
//namespace detail
//{
//    template <typename>
//    struct disable_uda_checks : std::false_type { };
//} // namespace detail
//
//
//template <typename T>
//class is_uda
//{
//    static const bool implementation_defined
//        = detail::disjunction<detail::conjunction<std::is_same<T, uncvref_t<T>>, std::is_default_constructible<T>,
//        std::is_copy_constructible<T>, std::is_move_constructible<T>,
//        std::is_destructible<T>, has_evolve<T>>,
//        detail::disable_uda_checks<T>>::value;
//
//public:
//    /// Value of the type trait.
//    static const bool value = implementation_defined;
//};

//template <typename T>
//const bool is_uda<T>::value;

//namespace detail
//{
//    struct algo_inner_base {
//        virtual ~algo_inner_base() {}
//        virtual std::unique_ptr<algo_inner_base> clone() const = 0;
//        virtual population evolve(const population& pop) const = 0;
//        virtual void set_seed(unsigned) = 0;
//        virtual bool has_set_seed() const = 0;
//        virtual void set_verbosity(unsigned) = 0;
//        virtual bool has_set_verbosity() const = 0;
//        virtual std::string get_name() const = 0;
//        virtual std::string get_extra_info() const = 0;
//        virtual thread_safety get_thread_safety() const = 0;
//        virtual std::type_index get_type_index() const = 0;
//        virtual const void* get_ptr() const = 0;
//        virtual void* get_ptr() = 0;
//    };
//
//    template <typename T>
//    struct algo_inner final : algo_inner_base{
//        // We just need the def ctor, delete everything else.
//        algo_inner() = default;
//        algo_inner(const algo_inner&) = delete;
//        algo_inner(algo_inner&&) = delete;
//        algo_inner& operator=(const algo_inner&) = delete;
//        algo_inner& operator=(algo_inner&&) = delete;
//        // Constructors from T (copy and move variants).
//        explicit algo_inner(const T& x) : m_value(x) {}
//        explicit algo_inner(T&& x) : m_value(std::move(x)) {}
//        // The clone method, used in the copy constructor of algorithm.
//        std::unique_ptr<algo_inner_base> clone() const final
//        {
//            return std::make_unique<algo_inner>(m_value);
//        }
//        // Mandatory methods.
//        population evolve(const population& pop) const final
//        {
//            return m_value.evolve(pop);
//        }
//        // Optional methods
//        void set_seed(unsigned seed) final
//        {
//            set_seed_impl(m_value, seed);
//        }
//        bool has_set_seed() const final
//        {
//            return has_set_seed_impl(m_value);
//        }
//        void set_verbosity(unsigned level) final
//        {
//            set_verbosity_impl(m_value, level);
//        }
//        bool has_set_verbosity() const final
//        {
//            return has_set_verbosity_impl(m_value);
//        }
//        std::string get_name() const final
//        {
//            return get_name_impl(m_value);
//        }
//        std::string get_extra_info() const final
//        {
//            return get_extra_info_impl(m_value);
//        }
//        thread_safety get_thread_safety() const final
//        {
//            return get_thread_safety_impl(m_value);
//        }
//        // Implementation of the optional methods.
//        template <typename U, enable_if_t<pagmo::has_set_seed<U>::value, int> = 0>
//        static void set_seed_impl(U& a, unsigned seed)
//        {
//            a.set_seed(seed);
//        }
//        template <typename U, enable_if_t<!pagmo::has_set_seed<U>::value, int> = 0>
//        static void set_seed_impl(U&, unsigned)
//        {
//            pagmo_throw(not_implemented_error,
//                        "The set_seed() method has been invoked but it is not implemented in the UDA");
//        }
//        template <typename U,
//                  enable_if_t<detail::conjunction<pagmo::has_set_seed<U>, override_has_set_seed<U>>::value, int> = 0>
//        static bool has_set_seed_impl(const U& a)
//        {
//            return a.has_set_seed();
//        }
//        template <
//            typename U,
//            enable_if_t<detail::conjunction<pagmo::has_set_seed<U>, detail::negation<override_has_set_seed<U>>>::value,
//                        int> = 0>
//        static bool has_set_seed_impl(const U&)
//        {
//            return true;
//        }
//        template <typename U, enable_if_t<!pagmo::has_set_seed<U>::value, int> = 0>
//        static bool has_set_seed_impl(const U&)
//        {
//            return false;
//        }
//        template <typename U, enable_if_t<pagmo::has_set_verbosity<U>::value, int> = 0>
//        static void set_verbosity_impl(U& value, unsigned level)
//        {
//            value.set_verbosity(level);
//        }
//        template <typename U, enable_if_t<!pagmo::has_set_verbosity<U>::value, int> = 0>
//        static void set_verbosity_impl(U&, unsigned)
//        {
//            pagmo_throw(not_implemented_error,
//                        "The set_verbosity() method has been invoked but it is not implemented in the UDA");
//        }
//        template <
//            typename U,
//            enable_if_t<detail::conjunction<pagmo::has_set_verbosity<U>, override_has_set_verbosity<U>>::value, int> = 0>
//        static bool has_set_verbosity_impl(const U& a)
//        {
//            return a.has_set_verbosity();
//        }
//        template <typename U, enable_if_t<detail::conjunction<pagmo::has_set_verbosity<U>,
//                                                              detail::negation<override_has_set_verbosity<U>>>::value,
//                                          int> = 0>
//        static bool has_set_verbosity_impl(const U&)
//        {
//            return true;
//        }
//        template <typename U, enable_if_t<!pagmo::has_set_verbosity<U>::value, int> = 0>
//        static bool has_set_verbosity_impl(const U&)
//        {
//            return false;
//        }
//        template <typename U, enable_if_t<has_name<U>::value, int> = 0>
//        static std::string get_name_impl(const U& value)
//        {
//            return value.get_name();
//        }
//        template <typename U, enable_if_t<!has_name<U>::value, int> = 0>
//        static std::string get_name_impl(const U&)
//        {
//            return detail::type_name<U>();
//        }
//        template <typename U, enable_if_t<has_extra_info<U>::value, int> = 0>
//        static std::string get_extra_info_impl(const U& value)
//        {
//            return value.get_extra_info();
//        }
//        template <typename U, enable_if_t<!has_extra_info<U>::value, int> = 0>
//        static std::string get_extra_info_impl(const U&)
//        {
//            return "";
//        }
//        template <typename U, enable_if_t<has_get_thread_safety<U>::value, int> = 0>
//        static thread_safety get_thread_safety_impl(const U& value)
//        {
//            return value.get_thread_safety();
//        }
//        template <typename U, enable_if_t<!has_get_thread_safety<U>::value, int> = 0>
//        static thread_safety get_thread_safety_impl(const U&)
//        {
//            return thread_safety::basic;
//        }
//        // Get the type at runtime.
//        std::type_index get_type_index() const final
//        {
//            return std::type_index(typeid(T));
//        }
//        // Raw getters for the internal instance.
//        const void* get_ptr() const final
//        {
//            return &m_value;
//        }
//        void* get_ptr() final
//        {
//            return &m_value;
//        }
//
//    private:
//        friend class boost::serialization::access;
//        // Serialization
//        template <typename Archive>
//        void serialize(Archive& ar, unsigned)
//        {
//            detail::archive(ar, boost::serialization::base_object<algo_inner_base>(*this), m_value);
//        }
//
//    public:
//        T m_value;
//    };
//
//} // namespace detail
//
//} // namespace pagmo

class algorithm
{
    // Enable the generic ctor only if T is not an algorithm (after removing
    // const/reference qualifiers), and if T is a uda.
    //template <typename T>
    //using generic_ctor_enabler = enable_if_t<detail::conjunction<detail::negation<std::is_same<algorithm, uncvref_t<T>>>, is_uda<uncvref_t<T>>>::value, int>;

public:
    extern algorithm();

    //template <typename T, generic_ctor_enabler<T> = 0>
    //extern explicit algorithm(T&& x);
    extern algorithm(const algorithm&);
    extern algorithm(algorithm&&) noexcept;
    extern algorithm& operator=(algorithm&&) noexcept;
    extern algorithm& operator=(const algorithm&);

    template <typename T, generic_ctor_enabler<T> = 0>
    extern algorithm& operator=(T&& x);

    template <typename T>
    extern bool is() const noexcept;

    // Evolve method.
    //extern population evolve(const population&) const;

    // Set the seed for the stochastic evolution.
    //extern void set_seed(unsigned);

    //extern unsigned get_seed();

    //extern unsigned get_verbosity();

    extern bool has_set_seed() const;

    extern bool is_stochastic() const;

    // Set the verbosity of logs and screen output.
    extern void set_verbosity(unsigned);

    extern bool has_set_verbosity() const;

    extern std::string get_name() const;

    extern std::string get_extra_info() const;

    extern thread_safety get_thread_safety() const;

    extern bool is_valid() const;

    //extern std::type_index get_type_index() const;

    extern const void* get_ptr() const;

    extern void* get_ptr();

};

//%extend algorithm{
//unsigned get_seed()
//{
//   return 0;
//} };
//
//%extend algorithm{
//unsigned get_verbosity()
//{
//    return 0;
//} };