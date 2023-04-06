%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/bfe.hpp"
%}
%typemap(csclassmodifiers) pagmo::bfe "public partial class"
class bfe {
	virtual std::string get_name() const = 0;
};