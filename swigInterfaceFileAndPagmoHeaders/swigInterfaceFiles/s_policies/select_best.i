%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/s_policies/select_best.hpp"
#include "pagmo/s_policy.hpp"

#include <string>
#include <type_traits>

#include "pagmo/detail/base_sr_policy.hpp"
#include "pagmo/detail/visibility.hpp"
#include "pagmo/s11n.hpp"
#include "pagmo/s_policy.hpp"
#include "pagmo/type_traits.hpp"
#include "pagmo/types.hpp"
%}

%typemap(csclassmodifiers) pagmo::select_best "public partial class"
class select_best : public pagmo::s_policy, public pagmoWrapper::s_policyBase {
public:

    extern select_best();
#if SUPPORT_VARIDEC
    template <typename T,
        enable_if_t<detail::disjunction<std::is_integral<T>, std::is_floating_point<T>>::value, int> = 0>
    extern explicit select_best(T x) : detail::base_sr_policy(x);
#endif
    extern individuals_group_t select(const individuals_group_t&, const vector_double::size_type&,
        const vector_double::size_type&, const vector_double::size_type&,
        const vector_double::size_type&, const vector_double::size_type&,
        const vector_double&) const;

    extern std::string get_name() const;
    extern std::string get_extra_info() const;
};