%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/problems/unconstrain.hpp"
#include "pagmo/problem.hpp"
%}

%typemap(csclassmodifiers) pagmo::unconstrain "public partial class"
class unconstrain {
public:
    extern unconstrain();
    extern unconstrain(const problem &, const std::string &method = "death penalty", const vector_double &weights = vector_double());
    extern vector_double fitness(const vector_double&) const;
    extern bool has_batch_fitness() const;
    extern vector_double batch_fitness(const vector_double &) const;
    extern vector_double::size_type get_nobj() const;
    extern vector_double::size_type get_nix() const;
    extern std::pair<vector_double, vector_double> get_bounds() const;
    extern bool has_set_seed() const;
    extern void set_seed(unsigned);
    extern thread_safety get_thread_safety() const;
    extern const problem &get_inner_problem() const;
    extern std::string get_name() const;
    extern std::string get_extra_info() const;
};

%extend unconstrain {
vector_double::size_type get_nec() const
{
   return 0;
} };

%extend unconstrain {
vector_double::size_type get_nic() const
{
   return 0;
} };
