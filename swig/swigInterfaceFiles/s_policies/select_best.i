%{
#include "pagmo/s_policies/select_best.hpp"
#include "tuple_adapters.h"
%}

%typemap(csclassmodifiers) pagmo::select_best "public partial class"

// ------------------------------------------------------------
// Hide tuple-based select()
// ------------------------------------------------------------
%ignore pagmo::select_best::select(
    const pagmo::individuals_group_t &,
    const pagmo::vector_double::size_type &,
    const pagmo::vector_double::size_type &,
    const pagmo::vector_double::size_type &,
    const pagmo::vector_double::size_type &,
    const pagmo::vector_double::size_type &,
    const pagmo::vector_double &
) const;

// ------------------------------------------------------------
// SWIG-visible class declaration (ONLY safe methods)
// ------------------------------------------------------------
class pagmo::select_best {
public:
    extern select_best();

#if SUPPORT_VARIDEC
    template <typename T,
        enable_if_t<detail::disjunction<std::is_integral<T>, std::is_floating_point<T>>::value, int> = 0>
    extern explicit select_best(T x);
#endif

    extern std::string get_name() const;
    extern std::string get_extra_info() const;
};

// ------------------------------------------------------------
// SWIG-safe wrapper for tuple-based select()
// ------------------------------------------------------------
%extend pagmo::select_best {

    pagmoWrap::IndividualsGroup select(
        const pagmoWrap::IndividualsGroup &a,
        const pagmo::vector_double::size_type &b,
        const pagmo::vector_double::size_type &c,
        const pagmo::vector_double::size_type &d,
        const pagmo::vector_double::size_type &e,
        const pagmo::vector_double::size_type &f,
        const pagmo::vector_double &g
    ) const {

        auto aa = pagmoWrap::ToIndividualsGroupTuple(a);
        auto rr = self->select(aa, b, c, d, e, f, g);
        return pagmoWrap::FromIndividualsGroupTuple(rr);
    }
}
