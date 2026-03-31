%{
#include "pagmo/problems/schwefel.hpp"
#include "pagmo/problem.hpp"
%}

%typemap(csclassmodifiers) pagmo::schwefel "public partial class"
class pagmo::schwefel {
public:
    extern schwefel(unsigned dim = 1u);
    extern vector_double fitness(const vector_double&) const;
    extern std::pair<vector_double, vector_double> get_bounds() const;
    extern vector_double best_known() const;
    extern std::string get_name() const;
    extern unsigned m_dim;
};

%extend pagmo::schwefel{
vector_double::size_type get_nic() const
{
   return 0;
} };

%extend pagmo::schwefel{
vector_double::size_type get_nec() const
{
   return 0;
} };

%extend pagmo::schwefel{
vector_double::size_type get_nix() const
{
   return 0;
} };

%extend pagmo::schwefel{
vector_double::size_type get_nobj() const
{
   return 1;
} };

%extend pagmo::schwefel{
bool has_batch_fitness() const
{
    return false;
} };

%extend pagmo::schwefel{
thread_safety get_thread_safety() const
{
    return pagmo::thread_safety::basic;
} };

