%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/bfe.hpp"
#include "pagmo/batch_evaluators/member_bfe.hpp"
%}
%typemap(csclassmodifiers) pagmo::member_bfe "public partial class"
class pagmo::member_bfe {
public:
	extern member_bfe();
	extern member_bfe(const pagmo::member_bfe&);
	extern std::string get_name() const;
};

%extend pagmo::member_bfe {
    pagmo::bfe to_bfe() const
    {
        return pagmo::bfe(*self);
    }
}
