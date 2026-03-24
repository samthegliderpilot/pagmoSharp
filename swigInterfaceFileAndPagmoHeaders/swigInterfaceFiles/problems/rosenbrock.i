%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/problems/rosenbrock.hpp"
#include "pagmo/problem.hpp"
%}

%typemap(csclassmodifiers) pagmo::rosenbrock "public partial class"
class rosenbrock {
public:
    extern rosenbrock(unsigned dim = 2u);
    extern vector_double fitness(const vector_double&) const;
    extern std::pair<vector_double, vector_double> get_bounds() const;
    extern vector_double gradient(const vector_double&) const;
    extern vector_double best_known() const;
    extern std::string get_name() const;
    extern thread_safety get_thread_safety() const;
    extern vector_double::size_type m_dim;
};

%extend rosenbrock {
vector_double::size_type get_nic() const
{
   return 0;
} };

%extend rosenbrock {
vector_double::size_type get_nec() const
{
   return 0;
} };

%extend rosenbrock {
vector_double::size_type get_nix() const
{
   return 0;
} };

%extend rosenbrock {
vector_double::size_type get_nobj() const
{
   return 1;
} };

%extend rosenbrock {
bool has_batch_fitness() const
{
    return false;
} };
