%{
#include "pagmo/problems/translate.hpp"
#include "pagmo/problem.hpp"
%}

%typemap(csclassmodifiers) pagmo::translate "public partial class"
class pagmo::translate {
public:
    extern translate();
    extern translate(const problem &, const vector_double &);
    extern vector_double fitness(const vector_double&) const;
    extern vector_double batch_fitness(const vector_double&) const;
    extern bool has_batch_fitness() const;
    extern std::pair<vector_double, vector_double> get_bounds() const;
    extern vector_double::size_type get_nobj() const;
    extern vector_double::size_type get_nec() const;
    extern vector_double::size_type get_nic() const;
    extern vector_double::size_type get_nix() const;
    extern bool has_set_seed() const;
    extern void set_seed(unsigned);
    extern std::string get_name() const;
    extern std::string get_extra_info() const;
    extern const vector_double &get_translation() const;
    extern thread_safety get_thread_safety() const;
    extern const problem &get_inner_problem() const;
};
