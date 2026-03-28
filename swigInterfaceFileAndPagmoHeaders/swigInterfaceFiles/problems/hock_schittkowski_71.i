%{
#include "pagmo/problems/hock_schittkowski_71.hpp"
#include "pagmo/problem.hpp"
%}

%typemap(csclassmodifiers) pagmo::hock_schittkowski_71 "public partial class"
class pagmo::hock_schittkowski_71 {
public:
    extern vector_double fitness(const vector_double&) const;
    extern vector_double::size_type get_nec() const;
    extern vector_double::size_type get_nic() const;
    extern std::pair<vector_double, vector_double> get_bounds() const;
    extern vector_double gradient(const vector_double&) const;
    extern std::vector<vector_double> hessians(const vector_double&) const;
    extern std::string get_name() const;
    extern std::string get_extra_info() const;
    extern vector_double best_known() const;
};

%extend pagmo::hock_schittkowski_71{
vector_double::size_type get_nix() const
{
   return 0;
} };

%extend pagmo::hock_schittkowski_71{
vector_double::size_type get_nobj() const
{
   return 1;
} };

%extend pagmo::hock_schittkowski_71{
bool has_batch_fitness() const
{
    return false;
} };

%extend pagmo::hock_schittkowski_71{
thread_safety get_thread_safety() const
{
    return pagmo::thread_safety::none;
} };
