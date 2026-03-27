%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/problems/lennard_jones.hpp"
#include "pagmo/problem.hpp"
%}

%typemap(csclassmodifiers) pagmo::lennard_jones "public partial class"
class lennard_jones {
public:
    extern lennard_jones(unsigned atoms = 3u);
    extern vector_double fitness(const vector_double&) const;
    extern std::pair<vector_double, vector_double> get_bounds() const;
    extern std::string get_name() const;
};

%extend lennard_jones {
vector_double::size_type get_nic() const
{
   return 0;
} };

%extend lennard_jones {
vector_double::size_type get_nec() const
{
   return 0;
} };

%extend lennard_jones {
vector_double::size_type get_nix() const
{
   return 0;
} };

%extend lennard_jones {
vector_double::size_type get_nobj() const
{
   return 1;
} };

%extend lennard_jones {
bool has_batch_fitness() const
{
    return false;
} };

%extend lennard_jones {
thread_safety get_thread_safety() const
{
    return pagmo::thread_safety::none;
} };
