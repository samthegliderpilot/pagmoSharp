%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/bfe.hpp"
#include "pagmo/batch_evaluators/member_bfe.hpp"
%}
%typemap(csclassmodifiers) pagmo::member_bfe "public partial class"
class member_bfe :public bfe {
public:
	extern member_bfe();
	extern member_bfe(const member_bfe&);
	extern std::string get_name() const;
};