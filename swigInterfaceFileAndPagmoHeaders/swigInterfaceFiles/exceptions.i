%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/exceptions.hpp"
#include <stdexcept>
#include <string>
#include <type_traits>
#include <utility>

#include "pagmo/detail/visibility.hpp"
#include "pagmo/type_traits.hpp"
%}

#include "stdexcept"
#include "pagmo/string.hpp"
#include "pagmo/type_traits.hpp"
#include "pagmo/utility.hpp"

%typemap(csclassmodifiers) pagmo::not_implemented_error "public partial class"


template <typename Exception>
struct ex_thrower {
    
    //using line_type = std::decay_t<decltype(__LINE__)>;
    extern ex_thrower(const char* file, line_type line, const char* func) : m_file(file), m_line(line), m_func(func);

    //template <typename... Args, enable_if_t<std::is_constructible<Exception, Args...>::value, int> = 0>
    //extern [[noreturn]] void operator()(Args &&...args) const;
 //   template <typename Str, typename... Args,
 //       enable_if_t<conjunction<std::is_constructible<Exception, std::string, Args...>,
 //       disjunction<std::is_same<std::decay_t<Str>, std::string>,
 //       std::is_same<std::decay_t<Str>, char*>,
 //       std::is_same<std::decay_t<Str>, const char*>>>::value,
 //       int> = 0>
	//extern [[noreturn]] void operator()(Str&& desc, Args &&...args) const;
    extern const char *m_file;
    extern const line_type m_line;
    extern const char *m_func;
};

struct not_implemented_error : std::runtime_error{
    using std::runtime_error::runtime_error;

public:
    extern not_implemented_error(const string& _Message);

    extern not_implemented_error(const char* _Message);
};
