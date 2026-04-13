%{
#include "pagmo/problems/dtlz.hpp"
#include "pagmo/problem.hpp"
%}

%typemap(csclassmodifiers) pagmo::dtlz "public partial class"
class pagmo::dtlz {
public:
    extern dtlz(unsigned prob_id = 1u, vector_double::size_type dim = 5u, vector_double::size_type fdim = 3u,
                unsigned alpha = 100u);
    extern vector_double fitness(const vector_double&) const;
    extern vector_double::size_type get_nobj() const;
    extern std::pair<vector_double, vector_double> get_bounds() const;
    extern double p_distance(const pagmo::population&) const;
    extern double p_distance(const vector_double&) const;
    extern std::string get_name() const;
};

%extend pagmo::dtlz{
vector_double::size_type get_nec() const
{
   return 0;
} };

%extend pagmo::dtlz{
vector_double::size_type get_nic() const
{
   return 0;
} };

%extend pagmo::dtlz{
vector_double::size_type get_nix() const
{
   return 0;
} };

%extend pagmo::dtlz{
bool has_batch_fitness() const
{
    return false;
} };

%extend pagmo::dtlz{
thread_safety get_thread_safety() const
{
    return pagmo::thread_safety::basic;
} };




