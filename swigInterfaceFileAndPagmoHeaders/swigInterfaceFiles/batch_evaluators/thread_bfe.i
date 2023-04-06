%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/bfe.hpp"
#include "pagmo/batch_evaluators/thread_bfe.hpp"
%}
%typemap(csclassmodifiers) pagmo::thread_bfe "public partial class"
class thread_bfe :public bfe {
public:
	extern thread_bfe();
	extern thread_bfe(const thread_bfe&);
	extern std::string get_name() const;
};