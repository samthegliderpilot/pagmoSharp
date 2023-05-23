%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/topology.hpp"
%}

%typemap(csclassmodifiers) pagmo::topology "public partial class"

class topology {

    // Enable the generic ctor only if T is not a topology (after removing
    // const/reference qualifiers), and if T is a udt.
    template <typename T>
    using generic_ctor_enabler = enable_if_t<
        detail::conjunction<detail::negation<std::is_same<topology, uncvref_t<T>>>, is_udt<uncvref_t<T>>>::value, int>;

public:
    // Generic constructor.
    template <typename T, generic_ctor_enabler<T> = 0>
    explicit topology(T&& x) : m_ptr(std::make_unique<detail::topo_inner<uncvref_t<T>>>(std::forward<T>(x)))
    {
        generic_ctor_impl();
    }
    // Copy ctor.
    extern topology(const topology&);
    // Move ctor.
    extern topology(topology&&) noexcept;
    // Move assignment.
    extern topology& operator=(topology&&) noexcept;
    // Copy assignment.
    extern topology& operator=(const topology&);
    // Generic assignment.
    template <typename T, generic_ctor_enabler<T> = 0>
    topology& operator=(T&& x)
    {
        return (*this) = topology(std::forward<T>(x));
    }

    // Extract.
    template <typename T>
    extern const T* extract() const noexcept;
    

    template <typename T>
    extern bool is() const noexcept;

    // Name.
    extern std::string get_name() const;

    // Extra info.
    extern std::string get_extra_info() const;

    // Check if the topology is valid.
    extern bool is_valid() const;

    // Get the connections to a vertex.
    extern std::pair<std::vector<std::size_t>, vector_double> get_connections(std::size_t) const;

    // Add a vertex.
    extern void push_back();
    // Add multiple vertices.
    extern void push_back(unsigned);

    // Convert to BGL.
    //extern  bgl_graph_t to_bgl() const;

    // Get the type at runtime.
    //extern std::type_index get_type_index() const;

    // Get a const pointer to the UDT.
    extern const void* get_ptr() const;

    // Get a mutable pointer to the UDT.
    extern void* get_ptr();

};