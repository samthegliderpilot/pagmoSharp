%{
#include "pagmo/algorithms/not_population_based.hpp"
%}

%typemap(csclassmodifiers) pagmo::not_population_based "public partial class"

class not_population_based
{
public:
    extern not_population_based();
    extern void set_random_sr_seed(unsigned);
    extern void set_selection(const std::string &);
    extern void set_selection(population::size_type n);
    extern void set_replacement(const std::string &);
    extern void set_replacement(population::size_type n);
};
