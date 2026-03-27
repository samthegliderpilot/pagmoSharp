%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/problems/cec2014.hpp"
#include "pagmo/problem.hpp"
%}

%typemap(csclassmodifiers) pagmo::cec2014 "public partial class"
class cec2014 {
public:
    extern cec2014(unsigned prob_id = 1u, unsigned dim = 2u);
    extern std::pair<vector_double, vector_double> get_bounds() const;
    extern vector_double fitness(const vector_double&) const;
    extern std::string get_name() const;
};

%extend cec2014 {
vector_double::size_type get_nec() const
{
   return 0;
} };

%extend cec2014 {
vector_double::size_type get_nic() const
{
   return 0;
} };

%extend cec2014 {
vector_double::size_type get_nix() const
{
   return 0;
} };

%extend cec2014 {
vector_double::size_type get_nobj() const
{
   return 1;
} };

%extend cec2014 {
bool has_batch_fitness() const
{
    return false;
} };

%extend cec2014 {
thread_safety get_thread_safety() const
{
    return pagmo::thread_safety::none;
} };
