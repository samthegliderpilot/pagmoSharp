%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/island.hpp"
#include "pagmo/islands/fork_island.hpp"
%}
%typemap(csclassmodifiers) pagmo::fork_island "public partial class"
//TODO: fork_island isn't avaialble on the platform...
//class fork_island {
//public:
//	extern fork_island();
//	extern fork_island(const fork_island&);
//	extern fork_island(fork_island&&);
//	extern std::string get_name() const;
//	extern std::string get_extra_info() const;
//	extern void run_evolve(island& isl) const;
//	//extern pid_t get_child_pid() const
//};