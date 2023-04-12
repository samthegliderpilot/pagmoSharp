%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/problems/ackley.hpp"
#include "pagmo/problem.hpp"
%}

%typemap(csclassmodifiers) pagmo::ackley "public partial class"
class ackley : public pagmo::problem, public pagmoWrapper::problemBase {
public:
    
    extern ackley(unsigned dim = 1u);
    
    extern vector_double fitness(const vector_double&) const;
    
    extern std::pair<vector_double, vector_double> get_bounds() const;
    
    extern std::string get_name() const;
    
    extern vector_double best_known() const;
    
    extern unsigned m_dim;
};

%extend ackley{
vector_double::size_type get_nic() const
{
   return 0;
} };

%extend ackley{
vector_double::size_type get_nec() const
{
   return 0;
} };

%extend ackley{
vector_double::size_type get_nix() const
{
   return 0;
} };

%extend ackley{
vector_double::size_type get_nobj() const
{
   return 1;
} };

%extend ackley{
bool has_batch_fitness() const
{
	return true;
} };

%extend ackley{
thread_safety get_thread_safety() const
{
	return pagmo::thread_safety::none; //TODO: What is the right answer?
} };