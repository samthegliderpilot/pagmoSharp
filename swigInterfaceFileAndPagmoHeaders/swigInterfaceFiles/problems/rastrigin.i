%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/problems/rastrigin.hpp"
#include "pagmo/problem.hpp"
%}

%typemap(csclassmodifiers) pagmo::rastrigin "public partial class"
class rastrigin {
public:
    extern rastrigin(unsigned dim = 1u);
    extern vector_double fitness(const vector_double&) const;
    extern std::pair<vector_double, vector_double> get_bounds() const;
    extern vector_double gradient(const vector_double&) const;
    extern std::vector<vector_double> hessians(const vector_double&) const;
    extern vector_double best_known() const;
    extern std::string get_name() const;
    extern unsigned m_dim;
};

%extend rastrigin {
vector_double::size_type get_nic() const
{
   return 0;
} };

%extend rastrigin {
vector_double::size_type get_nec() const
{
   return 0;
} };

%extend rastrigin {
vector_double::size_type get_nix() const
{
   return 0;
} };

%extend rastrigin {
vector_double::size_type get_nobj() const
{
   return 1;
} };

%extend rastrigin {
bool has_batch_fitness() const
{
    return false;
} };

%extend rastrigin {
thread_safety get_thread_safety() const
{
    return pagmo::thread_safety::none;
} };
