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

%extend not_population_based {
    bool selection_uses_count() const
    {
        return self->get_selection().type() == typeid(pagmo::population::size_type);
    }

    pagmo::population::size_type selection_count() const
    {
        return boost::any_cast<pagmo::population::size_type>(self->get_selection());
    }

    std::string selection_policy() const
    {
        return boost::any_cast<std::string>(self->get_selection());
    }

    bool replacement_uses_count() const
    {
        return self->get_replacement().type() == typeid(pagmo::population::size_type);
    }

    pagmo::population::size_type replacement_count() const
    {
        return boost::any_cast<pagmo::population::size_type>(self->get_replacement());
    }

    std::string replacement_policy() const
    {
        return boost::any_cast<std::string>(self->get_replacement());
    }
};
