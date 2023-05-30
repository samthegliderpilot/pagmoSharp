%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/r_policies/fair_replace.hpp"
#include "pagmo/r_policy.hpp"

#include <string>
#include <type_traits>

#include "pagmo/detail/base_sr_policy.hpp"
#include "pagmo/detail/visibility.hpp"
#include "pagmo/r_policy.hpp"
#include "pagmo/s11n.hpp"
#include "pagmo/type_traits.hpp"
#include "pagmo/types.hpp"
%}

%typemap(csclassmodifiers) pagmo::fair_replace "public partial class"
class fair_replace : public pagmo::r_policy, public pagmoWrapper::r_policyBase {
public:
    extern fair_replace();
#if SUPPORT_VARIDEC
    template <typename T,
        enable_if_t<detail::disjunction<std::is_integral<T>, std::is_floating_point<T>>::value, int> = 0>
    extern explicit fair_replace(T x) : detail::base_sr_policy(x);
#endif
    extern individuals_group_t replace(const individuals_group_t&, const vector_double::size_type&,
        const vector_double::size_type&, const vector_double::size_type&,
        const vector_double::size_type&, const vector_double::size_type&,
        const vector_double&, const individuals_group_t&) const;

    extern std::string get_name() const;
    extern std::string get_extra_info() const;
};