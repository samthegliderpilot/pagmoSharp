%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/topologies/unconnected.hpp"
#include <cstddef>
#include <string>
#include <utility>
#include <vector>

#include "pagmo/detail/visibility.hpp"
#include "pagmo/s11n.hpp"
#include "pagmo/topology.hpp"
#include "pagmo/types.hpp"
%}

%typemap(csclassmodifiers) pagmo::unconnected "public partial class"

class unconnected {
public:
    extern std::pair<std::vector<std::size_t>, vector_double> get_connections(std::size_t) const;

    extern void push_back();

    extern std::string get_name() const;
};