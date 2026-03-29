%{
#include "pagmo/problems/ackley.hpp"
#include "pagmo/problem.hpp"
%}

%typemap(csclassmodifiers) pagmo::ackley "public partial class"
class pagmo::ackley {
public:
    
    extern ackley(unsigned dim = 1u);
    
    extern vector_double fitness(const vector_double&) const;
    
    extern std::pair<vector_double, vector_double> get_bounds() const;
    
    extern std::string get_name() const;
    
    extern vector_double best_known() const;
    
    extern unsigned m_dim;
};

%extend pagmo::ackley{
vector_double::size_type get_nic() const
{
   return 0;
} };

%extend pagmo::ackley{
vector_double::size_type get_nec() const
{
   return 0;
} };

%extend pagmo::ackley{
vector_double::size_type get_nix() const
{
   return 0;
} };

%extend pagmo::ackley{
vector_double::size_type get_nobj() const
{
   return 1;
} };

%extend pagmo::ackley{
bool has_batch_fitness() const
{
	return true;
} };

%extend pagmo::ackley{
thread_safety get_thread_safety() const
{
	return pagmo::thread_safety::basic;
} };
