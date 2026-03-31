%{
#include "pagmo/problems/cec2013.hpp"
#include "pagmo/problem.hpp"
%}

%typemap(csclassmodifiers) pagmo::cec2013 "public partial class"
class pagmo::cec2013 {
public:
    extern cec2013(unsigned prob_id = 1u, unsigned dim = 2u);
    extern std::pair<vector_double, vector_double> get_bounds() const;
    extern vector_double fitness(const vector_double&) const;
    extern std::string get_name() const;
};

%extend pagmo::cec2013{
vector_double::size_type get_nec() const
{
   return 0;
} };

%extend pagmo::cec2013{
vector_double::size_type get_nic() const
{
   return 0;
} };

%extend pagmo::cec2013{
vector_double::size_type get_nix() const
{
   return 0;
} };

%extend pagmo::cec2013{
vector_double::size_type get_nobj() const
{
   return 1;
} };

%extend pagmo::cec2013{
bool has_batch_fitness() const
{
    return false;
} };

%extend pagmo::cec2013{
thread_safety get_thread_safety() const
{
    return pagmo::thread_safety::basic;
} };

