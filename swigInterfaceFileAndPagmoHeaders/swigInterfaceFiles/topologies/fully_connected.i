%{
#include "pagmo/topologies/fully_connected.hpp"
#include <cstddef>
#include <string>
#include <utility>
#include <vector>

#include "pagmo/detail/visibility.hpp"
#include "pagmo/s11n.hpp"
#include "pagmo/topology.hpp"
#include "pagmo/types.hpp"
%}

%typemap(csclassmodifiers) pagmo::fully_connected "public partial class"

class pagmo::fully_connected {
public:
    extern fully_connected();
    extern fully_connected(double);
    extern fully_connected(std::size_t, double);

    extern std::pair<std::vector<std::size_t>, pagmo::vector_double> get_connections(std::size_t) const;

    extern void push_back();

    extern std::string get_name() const;
    extern std::string get_extra_info() const;

    extern double get_weight() const;
    extern std::size_t num_vertices() const;
};
