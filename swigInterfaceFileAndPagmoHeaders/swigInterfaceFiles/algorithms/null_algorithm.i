%{
#include "pagmo/algorithm.hpp"
#include "pagmo/algorithms/null_algorithm.hpp"
%}

%typemap(csclassmodifiers) pagmo::null_algorithm "public partial class"

class null_algorithm {
public:
    extern population evolve(const population &) const;
    extern std::string get_name() const;
};
