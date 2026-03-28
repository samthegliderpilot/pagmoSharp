%{
#include "pagmo/problems/cec2006.hpp"
#include "pagmo/problem.hpp"
%}

%typemap(csclassmodifiers) pagmo::cec2006 "public partial class"
class pagmo::cec2006 {
public:
    extern cec2006(unsigned prob_id = 1u);
    // Equality constraint dimension
    extern vector_double::size_type get_nec() const;
    // Inequality constraint dimension
    extern vector_double::size_type get_nic() const;
    // Box-bounds
    extern std::pair<vector_double, vector_double> get_bounds() const;
    // Fitness computation
    extern vector_double fitness(const vector_double&) const;
    // Optimal solution
    extern vector_double best_known() const;
    // Problem name
    extern std::string get_name() const;
};

%extend pagmo::cec2006{
vector_double::size_type get_nix() const
{
   return 0;
} };

%extend pagmo::cec2006{
vector_double::size_type get_nobj() const
{
   return 1;
} };

%extend pagmo::cec2006{
thread_safety get_thread_safety() const
{
	return pagmo::thread_safety::none; //TODO: What is the right answer?
} };

%extend pagmo::cec2006{
bool has_batch_fitness() const
{
	return false;
} };
