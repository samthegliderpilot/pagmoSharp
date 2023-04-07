%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/island.hpp"
#include "pagmo/islands/thread_island.hpp"
%}
%typemap(csclassmodifiers) pagmo::thread_island "public partial class"
class thread_island : public pagmo::island {
	public:
	extern thread_island();
	extern std::string get_name() const;
	extern std::string get_extra_info() const;
	extern void run_evolve(island& isl) const;
};