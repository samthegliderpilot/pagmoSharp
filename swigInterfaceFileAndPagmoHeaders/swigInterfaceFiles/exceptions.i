%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/exceptions.hpp"
%}

#include "stdexcept"
#include "pagmo/string.hpp"
#include "pagmo/type_traits.hpp"
#include "pagmo/utility.hpp"

%typemap(csclassmodifiers) pagmo::not_implemented_error "public partial class"

struct PAGMO_DLL_PUBLIC_INLINE_CLASS not_implemented_error final : std::runtime_error{
    using std::runtime_error::runtime_error;
};
