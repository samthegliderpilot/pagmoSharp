%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/detail/base_sr_policy.hpp"
#include <type_traits>

#include "boost/numeric/conversion/cast.hpp"
#include "boost/serialization/variant.hpp"
#include "boost/variant/variant.hpp"

#include "pagmo/detail/visibility.hpp"
#include "pagmo/s11n.hpp"
#include "pagmo/type_traits.hpp"
#include "pagmo/types.hpp"
%}

%typemap(csclassmodifiers) pagmo::detail::base_sr_policy "public partial class"
namespace detail {
class base_sr_policy {
public:
#if SUPPORT_VARIDEC
    template <typename T, enable_if_t<detail::disjunction<std::is_integral<T>, std::is_floating_point<T>>::value, int> = 0>
    extern explicit base_sr_policy(T x) : base_sr_policy(ptag{}, x);
#endif
        extern const boost::variant<pop_size_t, double>& get_migr_rate() const;

    protected:
        extern boost::variant<pop_size_t, double> m_migr_rate;
};
};