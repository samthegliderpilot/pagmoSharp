%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/problems/minlp_rastrigin.hpp"
#include "pagmo/problem.hpp"
%}

%typemap(csclassmodifiers) pagmo::minlp_rastrigin "public partial class"
class minlp_rastrigin : public pagmo::problem, public pagmoWrapper::problemBase {
public:
    extern minlp_rastrigin(unsigned dim_c = 1u, unsigned dim_i = 1u);
    extern vector_double fitness(const vector_double&) const;
    extern std::pair<vector_double, vector_double> get_bounds() const;
    extern vector_double::size_type get_nix() const;
    extern vector_double gradient(const vector_double&) const;
    extern std::vector<vector_double> hessians(const vector_double&) const;
    extern std::vector<sparsity_pattern> hessians_sparsity() const;
    extern std::string get_name() const;
    extern std::string get_extra_info() const;
};


%extend minlp_rastrigin{
vector_double::size_type get_nic() const
{
   return 0;
} };

%extend minlp_rastrigin{
vector_double::size_type get_nec() const
{
   return 0;
} };
//
//%extend minlp_rastrigin{
//vector_double::size_type get_nix() const
//{
//   return 0;
//} };
//
%extend minlp_rastrigin{
vector_double::size_type get_nobj() const
{
   return 1;
} };

%extend minlp_rastrigin{
bool has_batch_fitness() const
{
	return true;
} };

%extend minlp_rastrigin{
thread_safety get_thread_safety() const
{
	return pagmo::thread_safety::none; //TODO: What is the right answer?
} };