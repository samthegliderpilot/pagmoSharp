%module(naturalvar = 1, directors = "1") pagmo
%{
#include "pagmo/island.hpp"
#include "pagmo/archipelago.hpp"
%}

%typemap(csclassmodifiers) pagmo::archipelago "public partial class"

class archipelago {

	friend class pygmo::island;

	using container_t = std::vector<std::unique_ptr<island>>;
	using size_type_implementation = container_t::size_type;
	using iterator_implementation = boost::indirect_iterator<container_t::iterator>;
	using const_iterator_implementation = boost::indirect_iterator<container_t::const_iterator>;

public:
	using size_type = size_type_implementation;
	using migrants_db_t = std::vector<individuals_group_t>;

	using migration_log_t = std::vector<migration_entry_t>;
	using iterator = iterator_implementation;
	using const_iterator = const_iterator_implementation;

	// Default constructor.
	extern archipelago();
	// Copy constructor.
	extern archipelago(const archipelago&);
	// Move constructor.
	extern archipelago(archipelago&&) noexcept;

	//template <typename Topo, topo_ctor_enabler<Topo> = 0>
	//extern explicit archipelago(Topo&& t);

	//template <typename... Args, n_ctor_enabler<const Args &...> = 0>
	//extern explicit archipelago(size_type n, const Args &... args);

	//template <typename Topo, typename... Args, topo_n_ctor_enabler<Topo, const Args &...> = 0>
	//extern explicit archipelago(Topo&& t, size_type n, const Args &... args) : archipelago(std::forward<Topo>(t));
	// Copy assignment.
	//extern archipelago &operator=(const archipelago &);
	//extern archipelago &operator=(archipelago &&) noexcept;
	// Destructor.
	//extern ~archipelago();
	// Mutable island access.
	extern island& operator[](size_type);
	// Const island access.
	extern const island& operator[](size_type) const;
	// Size.
	extern size_type size() const;

	//template <typename... Args, push_back_enabler<Args &&...> = 0>
	//extern void push_back(Args &&... args);

	extern void evolve(unsigned n = 1);
	// Block until all evolutions have finished.
	extern void wait() noexcept;
	// Block until all evolutions have finished and raise the first exception that was encountered.
	extern void wait_check();
	// Status of the archipelago.
	extern evolve_status status() const;

	extern iterator begin();
	extern iterator end();
	extern const_iterator begin() const;
	extern const_iterator end() const;

	// Get the fitness vectors of the islands' champions.
	extern std::vector<vector_double> get_champions_f() const;
	// Get the decision vectors of the islands' champions.
	extern std::vector<vector_double> get_champions_x() const;

	// Get the database of migrants.
	extern migrants_db_t get_migrants_db() const;
	// Set the database of migrants.
	extern void set_migrants_db(migrants_db_t);

	// Topology get/set.
	//extern topology get_topology() const;
	//extern void set_topology(topology);

	// Getters/setters for the migration type and
	// the migrant handling policy.
	extern migration_type get_migration_type() const;
	extern void set_migration_type(migration_type);
	extern migrant_handling get_migrant_handling() const;
	extern void set_migrant_handling(migrant_handling);

	template <typename Archive>
	extern void save(Archive& ar, unsigned) const;

	template <typename Archive>
	extern void load(Archive& ar, unsigned);
};
