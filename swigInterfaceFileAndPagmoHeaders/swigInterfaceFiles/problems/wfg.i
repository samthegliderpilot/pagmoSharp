%{
#include "pagmo/problems/wfg.hpp"
#include "pagmo/problem.hpp"
%}

%typemap(csclassmodifiers) pagmo::wfg "public partial class"
class pagmo::wfg {
public:
    extern wfg(unsigned prob_id = 1u, vector_double::size_type dim_dvs = 5u, vector_double::size_type dim_obj = 3u,
               vector_double::size_type dim_k = 4u);
    extern vector_double fitness(const vector_double&) const;
    extern vector_double::size_type get_nobj() const;
    extern std::pair<vector_double, vector_double> get_bounds() const;
    extern std::string get_name() const;
};

%extend pagmo::wfg{
vector_double::size_type get_nec() const
{
   return 0;
} };

%extend pagmo::wfg{
vector_double::size_type get_nic() const
{
   return 0;
} };

%extend pagmo::wfg{
vector_double::size_type get_nix() const
{
   return 0;
} };

%extend pagmo::wfg{
bool has_batch_fitness() const
{
    return false;
} };

%extend pagmo::wfg{
thread_safety get_thread_safety() const
{
    return pagmo::thread_safety::basic;
} };

