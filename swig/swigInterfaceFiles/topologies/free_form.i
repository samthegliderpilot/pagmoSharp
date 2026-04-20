%{
#include "pagmo/topologies/free_form.hpp"
#include <string>

#include "pagmo/detail/visibility.hpp"
#include "pagmo/s11n.hpp"
#include "pagmo/topology.hpp"
#include "pagmo/types.hpp"
%}

%typemap(csclassmodifiers) pagmo::free_form "public partial class"

class pagmo::free_form {
public:
    extern free_form();

    extern void push_back();
    extern std::size_t num_vertices() const;
    extern std::pair<std::vector<std::size_t>, pagmo::vector_double> get_connections(std::size_t) const;
    extern double get_edge_weight(std::size_t, std::size_t) const;
    extern void add_edge(std::size_t, std::size_t, double);
    extern void remove_edge(std::size_t, std::size_t);
    extern void set_weight(std::size_t, std::size_t, double);
    extern void set_all_weights(double);

    extern std::string get_name() const;
    extern std::string get_extra_info() const;
};
