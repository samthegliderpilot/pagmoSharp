%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/bfe.hpp"
#include "pagmo/batch_evaluators/default_bfe.hpp"
%}
%typemap(csclassmodifiers) pagmo::default_bfe "public partial class"
class default_bfe {
public:
	extern default_bfe();
	extern default_bfe(const default_bfe&);
	extern std::string get_name() const;
};

%extend default_bfe {
    pagmo::bfe to_bfe() const
    {
        return pagmo::bfe(*self);
    }
}
