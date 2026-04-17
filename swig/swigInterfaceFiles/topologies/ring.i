%{
#include "pagmo/topologies/ring.hpp"
#include <cstddef>
#include <string>

#include "pagmo/detail/visibility.hpp"
#include "pagmo/s11n.hpp"
#include "pagmo/topology.hpp"
#include "pagmo/types.hpp"
%}

%typemap(csclassmodifiers) pagmo::ring "public partial class"

class pagmo::ring {
public:
    extern ring();
    extern ring(double);
    extern ring(std::size_t, double);

    extern std::pair<std::vector<std::size_t>, pagmo::vector_double> get_connections(std::size_t) const;
    extern void push_back();
    extern std::size_t num_vertices() const;

    extern std::string get_name() const;
    extern double get_weight() const;
};
