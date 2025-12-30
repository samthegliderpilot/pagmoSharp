%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/r_policies/fair_replace.hpp"
#include "tuple_adapters.h"   // your IndividualsGroup + tuple converters
%}

%typemap(csclassmodifiers) pagmo::fair_replace "public partial class"

// We do NOT want SWIG to wrap tuple-based replace().
%ignore pagmo::fair_replace::replace(
    const pagmo::individuals_group_t &,
    const pagmo::vector_double::size_type &,
    const pagmo::vector_double::size_type &,
    const pagmo::vector_double::size_type &,
    const pagmo::vector_double::size_type &,
    const pagmo::vector_double::size_type &,
    const pagmo::vector_double &,
    const pagmo::individuals_group_t &
) const;

// Now we define the class with only what SWIG can safely handle.
class fair_replace {
public:
    extern fair_replace();

#if SUPPORT_VARIDEC
    template <typename T,
        enable_if_t<detail::disjunction<std::is_integral<T>, std::is_floating_point<T>>::value, int> = 0>
    extern explicit fair_replace(T x);
#endif

    extern std::string get_name() const;
    extern std::string get_extra_info() const;
};

// Add a SWIG-safe wrapper for the tuple-based method.
%extend pagmo::fair_replace {

    pagmoWrap::IndividualsGroup replace_wrapped(
        const pagmoWrap::IndividualsGroup &a,
        const pagmo::vector_double::size_type &b,
        const pagmo::vector_double::size_type &c,
        const pagmo::vector_double::size_type &d,
        const pagmo::vector_double::size_type &e,
        const pagmo::vector_double::size_type &f,
        const pagmo::vector_double &g,
        const pagmoWrap::IndividualsGroup &h
    ) const {

        // Convert struct -> tuple
        auto aa = pagmoWrap::ToIndividualsGroupTuple(a);
        auto hh = pagmoWrap::ToIndividualsGroupTuple(h);

        // Call real pagmo implementation
        auto rr = self->replace(aa, b, c, d, e, f, g, hh);

        // Convert tuple -> struct
        return pagmoWrap::FromIndividualsGroupTuple(rr);
    }
}
