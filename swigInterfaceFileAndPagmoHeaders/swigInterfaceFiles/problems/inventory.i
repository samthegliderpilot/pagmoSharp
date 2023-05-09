%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/problems/inventory.hpp"
#include "pagmo/problem.hpp"
%}

%typemap(csclassmodifiers) pagmo::inventory "public partial class"

class inventory : public pagmo::problem, public pagmoWrapper::problemBase {
public:
    extern inventory(unsigned weeks = 4u, unsigned sample_size = 10u, unsigned seed = pagmo::random_device::next())
        : m_weeks(weeks), m_sample_size(sample_size), m_e(seed), m_seed(seed);
    extern vector_double fitness(const vector_double&) const;
    extern std::pair<vector_double, vector_double> get_bounds() const;
    extern void set_seed(unsigned seed);
    extern std::string get_name() const;
    extern std::string get_extra_info() const;
};


%extend inventory{
vector_double::size_type get_nic() const
{
   return 0;
} };

%extend inventory{
vector_double::size_type get_nec() const
{
   return 0;
} };

%extend inventory{
vector_double::size_type get_nix() const
{
   return 0;
} };

%extend inventory{
vector_double::size_type get_nobj() const
{
   return 1;
} };

%extend inventory{
bool has_batch_fitness() const
{
	return true;
} };

%extend inventory{
thread_safety get_thread_safety() const
{
	return pagmo::thread_safety::none; //TODO: What is the right answer?
} };