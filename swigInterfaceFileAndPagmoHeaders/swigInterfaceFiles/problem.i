%{
#include "pagmo/problem.hpp"
%}

%typemap(csclassmodifiers) pagmo::problem "public partial class"
%feature("csmethodmodifiers", "internal") pagmo::problem::gradient_sparsity;
%feature("csmethodmodifiers", "internal") pagmo::problem::hessians_sparsity;

%ignore pagmo::problem::problem(T &&);
%ignore pagmo::problem::operator=;

class pagmo::problem
{
public:
    problem();
    problem(const pagmo::problem &);
    problem(pagmo::problem &&) noexcept;

    vector_double fitness(const vector_double &) const;
    vector_double batch_fitness(const vector_double &) const;
    bool has_batch_fitness() const;

    vector_double gradient(const vector_double &) const;
    bool has_gradient() const;
    sparsity_pattern gradient_sparsity() const;
    bool has_gradient_sparsity() const;

    std::vector<vector_double> hessians(const vector_double &) const;
    bool has_hessians() const;
    std::vector<sparsity_pattern> hessians_sparsity() const;
    bool has_hessians_sparsity() const;

    vector_double::size_type get_nobj() const;
    vector_double::size_type get_nx() const;
    vector_double::size_type get_nix() const;
    vector_double::size_type get_ncx() const;
    vector_double::size_type get_nf() const;
    vector_double::size_type get_nec() const;
    vector_double::size_type get_nic() const;
    vector_double::size_type get_nc() const;

    std::pair<vector_double, vector_double> get_bounds() const;
    const vector_double &get_lb() const;
    const vector_double &get_ub() const;

    void set_c_tol(const vector_double &);
    void set_c_tol(double);
    vector_double get_c_tol() const;

    unsigned long long get_fevals() const;
    unsigned long long get_gevals() const;
    unsigned long long get_hevals() const;

    void set_seed(unsigned);
    bool has_set_seed() const;
    bool is_stochastic() const;

    bool feasibility_x(const vector_double &) const;
    bool feasibility_f(const vector_double &) const;

    std::string get_name() const;
    std::string get_extra_info() const;
    thread_safety get_thread_safety() const;

    bool is_valid() const;
};
