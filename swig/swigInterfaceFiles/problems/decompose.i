%{
#include "pagmo/problems/decompose.hpp"
#include "pagmo/problem.hpp"
%}

%typemap(csclassmodifiers) pagmo::decompose "public partial class"
class pagmo::decompose {
public:
    extern decompose();
    extern decompose(const problem &, const vector_double &, const vector_double &, const std::string &method = "weighted", bool adapt_ideal = false);
    extern vector_double fitness(const vector_double&) const;
    extern vector_double original_fitness(const vector_double&) const;
    extern vector_double::size_type get_nobj() const;
    extern vector_double::size_type get_nix() const;
    extern std::pair<vector_double, vector_double> get_bounds() const;
    extern vector_double get_z() const;
    extern std::string get_name() const;
    extern std::string get_extra_info() const;
    extern bool has_set_seed() const;
    extern void set_seed(unsigned);
    extern thread_safety get_thread_safety() const;
    extern const problem &get_inner_problem() const;
};

%extend pagmo::decompose{
vector_double::size_type get_nec() const
{
   return 0;
} };

%extend pagmo::decompose{
vector_double::size_type get_nic() const
{
   return 0;
} };

%extend pagmo::decompose{
bool has_batch_fitness() const
{
    return false;
} };



