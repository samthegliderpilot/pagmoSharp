%{
#include "pagmo/problems/golomb_ruler.hpp"
#include "pagmo/problem.hpp"
%}

%typemap(csclassmodifiers) pagmo::golomb_ruler "public partial class"

class pagmo::golomb_ruler {
public:
	extern golomb_ruler(unsigned order = 3u, unsigned upper_bound = 10);

	extern vector_double fitness(const vector_double&) const;
	extern std::pair<vector_double, vector_double> get_bounds() const;
	extern vector_double::size_type get_nix() const;
	//extern vector_double::size_type get_nobj() const;
	extern vector_double::size_type get_nec() const;
	//extern vector_double::size_type get_nic() const;
	//extern bool has_batch_fitness() const;
	//extern thread_safety get_thread_safety() const;

	extern std::string get_name() const;
};

%extend pagmo::golomb_ruler{
vector_double::size_type get_nic() const
{
   return 0;
} };

%extend pagmo::golomb_ruler{
vector_double::size_type get_nobj() const
{
   return 1;
} };

%extend pagmo::golomb_ruler{
bool has_batch_fitness() const
{
	return true;
} };

%extend pagmo::golomb_ruler{
thread_safety get_thread_safety() const
{
	return pagmo::thread_safety::basic;
} };



