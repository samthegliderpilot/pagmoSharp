%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/topologies/free_form.hpp"
#include <string>

#include "pagmo/detail/visibility.hpp"
#include "pagmo/s11n.hpp"
#include "pagmo/topology.hpp"
#include "pagmo/types.hpp"
%}

%typemap(csclassmodifiers) pagmo::free_form "public partial class"

class free_form {
public:
    extern free_form();

    extern void push_back();

    extern std::string get_name() const;
};
