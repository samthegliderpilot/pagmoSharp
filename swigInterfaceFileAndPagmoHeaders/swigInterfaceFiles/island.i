%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/island.hpp"
%}

%typemap(csclassmodifiers) pagmo::island "public partial class"

class island {
public:
	//virtual void run_evolve(island& isl) const = 0;
	//virtual std::string get_name() const = 0;
};