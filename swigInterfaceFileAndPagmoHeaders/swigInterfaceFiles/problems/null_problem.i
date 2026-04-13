%{
#include "pagmo/problems/null_problem.hpp"
#include "pagmo/problem.hpp"
%}

%typemap(csclassmodifiers) pagmo::null_problem "public partial class"
class pagmo::null_problem {
public:
    extern null_problem(vector_double::size_type nobj = 1u, vector_double::size_type nec = 0u,
                        vector_double::size_type nic = 0u, vector_double::size_type nix = 0u);
    extern vector_double fitness(const vector_double &) const;
    extern std::pair<vector_double, vector_double> get_bounds() const;
    extern vector_double::size_type get_nobj() const;
    extern vector_double::size_type get_nec() const;
    extern vector_double::size_type get_nic() const;
    extern vector_double::size_type get_nix() const;
    extern std::string get_name() const;
};

%extend pagmo::null_problem{
bool has_batch_fitness() const
{
    return false;
} };

%extend pagmo::null_problem{
thread_safety get_thread_safety() const
{
    return pagmo::thread_safety::basic;
} };




