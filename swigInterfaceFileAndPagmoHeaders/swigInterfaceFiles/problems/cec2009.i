%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/problems/cec2009.hpp"
#include "pagmo/problem.hpp"
%}

%typemap(csclassmodifiers) pagmo::cec2009 "public partial class"
class cec2009 {
public:
    extern cec2009(unsigned prob_id = 1u, bool is_constrained = false, unsigned dim = 30u);
    extern vector_double::size_type get_nic() const;
    extern vector_double::size_type get_nobj() const;
    extern std::pair<vector_double, vector_double> get_bounds() const;
    extern vector_double fitness(const vector_double&) const;
    extern std::string get_name() const;
};

%extend cec2009 {
vector_double::size_type get_nec() const
{
   return 0;
} };

%extend cec2009 {
vector_double::size_type get_nix() const
{
   return 0;
} };

%extend cec2009 {
bool has_batch_fitness() const
{
    return false;
} };

%extend cec2009 {
thread_safety get_thread_safety() const
{
    return pagmo::thread_safety::none;
} };
