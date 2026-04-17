%{
#include "pagmo/problems/zdt.hpp"
#include "pagmo/problem.hpp"
%}

%typemap(csclassmodifiers) pagmo::zdt "public partial class"
class pagmo::zdt {
public:
    extern zdt(unsigned prob_id = 1u, unsigned param = 30u);
    extern vector_double fitness(const vector_double&) const;
    extern vector_double::size_type get_nobj() const;
    extern std::pair<vector_double, vector_double> get_bounds() const;
    extern vector_double::size_type get_nix() const;
    extern std::string get_name() const;
    extern double p_distance(const population&) const;
    extern double p_distance(const vector_double&) const;
};



%extend pagmo::zdt{
vector_double::size_type get_nic() const
{
   return 0;
} };

%extend pagmo::zdt{
vector_double::size_type get_nec() const
{
   return 0;
} };

%extend pagmo::zdt{
bool has_batch_fitness() const
{
	return true;
} };

%extend pagmo::zdt{
thread_safety get_thread_safety() const
{
	return pagmo::thread_safety::basic;
} };



