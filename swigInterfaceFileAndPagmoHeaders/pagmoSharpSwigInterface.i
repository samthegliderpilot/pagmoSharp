/* File pagmoSharpSwigInterface.i */

%module(naturalvar=1, directors="8") pagmo
%{
	#include "pagmo/types.hpp"
	#include "pagmo/bfe.hpp"
	#include "pagmo/batch_evaluators/default_bfe.hpp"
	#include "pagmo/batch_evaluators/thread_bfe.hpp"
	#include "pagmo/batch_evaluators/member_bfe.hpp"
	#include "pagmo/algorithm.hpp"
	#include "pagmo/population.hpp"
	#include "pagmo/threading.hpp" 
	#include "pagmo/problem.hpp"
	#include "pagmo/island.hpp"
	#include "pagmo/islands/thread_island.hpp"
	//#include "pagmo/islands/fork_island.hpp"
	#include "pagmo/bfe.hpp" 
	#include "pagmo/archipelago.hpp"
    #include "pagmo/topology.hpp"
	#include "problem.h" // this is a manually created item.  We want to include it in the wrappers so the generated cxx code can use the handwritten code for the problem

	#include "pagmo/problems/golomb_ruler.hpp"
%}

// The whole problem vs. problemBase question is a little confusing.  To make it better (or worse)
// there is a C# implicit operator to convert from problem to problemBase by calling getBaseProblem on 
// the wrapper (problem).  Hence the partial class here
%include "std_string.i"
%include "std_vector.i"
%include "std_pair.i"
%pragma(csharp) moduleclassmodifiers = "public partial class"
%typemap(csclassmodifiers) pagmoWrap::problemPagomWrapper "public partial class"
%typemap(csclassmodifiers) pagmoWrap::problemBase "public partial class"
%typemap(csclassmodifiers) pagmo::thread_island "public partial class"
%typemap(csclassmodifiers) pagmo::DoubleVector "public partial class"
%typemap(csclassmodifiers) pagmo::population "public partial class"
%typemap(csclassmodifiers) pagmo::archipelago "public partial class"
%typemap(csclassmodifiers) pagmo::topology "public partial class"
//%typemap(csclassmodifiers) pagmo::fork_island "public partial class"
%feature("director") pagmoWrap::problemBase;
%include "pagmoWrapper/problem.h"

//#include <tuple> // tuple is not supported by swig yet...
%apply void *VOID_INT_PTR { void * }
namespace std {
	%template(DoubleVector) std::vector<double>;
	%template(ULongLongVector) std::vector<unsigned long long>;
	%template(VectorDoubleVector) std::vector<std::vector<double>>;
	%template(PairOfDoubleVectors) std::pair<std::vector<double>, std::vector<double>>;
}

namespace pagmo {
	typedef std::vector<double> vector_double;
	typedef std::vector<std::pair<vector_double::size_type, vector_double::size_type>> sparsity_pattern;
	typedef std::vector<vector_double>::size_type pop_size_t;

	enum class thread_safety { none, basic, constant };

	%extend default_bfe{
	vector_double Operator(const pagmoWrap::problemPagomWrapper& theProblem, const vector_double & values) const
	{
	   return self->operator()(static_cast<pagmo::problem>(theProblem), values);
	}
	};

	%extend member_bfe{
	vector_double Operator(const pagmoWrap::problemPagomWrapper& theProblem, const vector_double & values) const
	{
	   return self->operator()(static_cast<pagmo::problem>(theProblem), values);
	}
	};

	%extend thread_bfe{
	vector_double Operator(const pagmoWrap::problemPagomWrapper& theProblem, const vector_double & values) const
	{
	   return self->operator()(static_cast<pagmo::problem>(theProblem), values);
	}
	};

	class bfe {
		virtual std::string get_name() const = 0;
	};

	class default_bfe :public bfe {
	public:
		extern default_bfe();
		extern default_bfe(const default_bfe&);
		extern std::string get_name() const;
	};

	class thread_bfe :public bfe {
	public:
		extern thread_bfe();
		extern thread_bfe(const thread_bfe&);
		extern std::string get_name() const;
	};

	class member_bfe :public bfe {
		extern member_bfe();
		extern member_bfe(const member_bfe&);
		extern std::string get_name() const;
	};

	class island {
	public:
		//virtual void run_evolve(island& isl) const = 0;
		//virtual std::string get_name() const = 0;
	};

	class thread_island : public pagmo::island {
	public:
		extern thread_island();
		extern std::string get_name() const;
		extern std::string get_extra_info() const;
		extern void run_evolve(island& isl) const;
	};

	//TODO: fork_island isn't avaialble on the platform...
	//class fork_island {
	//public:
	//	extern fork_island();
	//	extern fork_island(const fork_island&);
	//	extern fork_island(fork_island&&);
	//	extern std::string get_name() const;
	//	extern std::string get_extra_info() const;
	//	extern void run_evolve(island& isl) const;
	//	//extern pid_t get_child_pid() const
	//};




	class population {
		typedef std::vector<vector_double>::size_type pop_size_t;
		typedef pop_size_t size_type;
	public:
		template <typename T, generic_ctor_enabler<T> = 0>
		extern population(T&& x, size_type pop_size = 0u, unsigned seed = pagmo::random_device::next());

		extern population(pagmoWrap::problemPagomWrapper x, size_type pop_size = 0u, unsigned seed = pagmo::random_device::next());
		extern void push_back(const vector_double&);
		extern void push_back(const vector_double&, const vector_double&);
		extern vector_double random_decision_vector() const;
		extern size_type best_idx() const;
		extern size_type best_idx(const vector_double&) const;
		extern size_type best_idx(double) const;
		extern size_type worst_idx() const;
		extern size_type worst_idx(const vector_double&) const;
		extern size_type worst_idx(double) const;
		extern vector_double champion_x() const;
		extern vector_double champion_f() const;
		extern size_type size() const;
		extern void set_xf(size_type, const vector_double&, const vector_double&);
		extern void set_x(size_type, const vector_double&);
		extern const pagmoWrap::problemPagomWrapper& get_problem() const;
		extern const std::vector<vector_double>& get_f() const;
		extern const std::vector<vector_double>& get_x() const;
		extern const std::vector<unsigned long long>& get_ID() const;
		extern unsigned get_seed() const;

		template <typename Archive>
		extern void save(Archive& ar, unsigned) const;

		template <typename Archive>
		extern void load(Archive& ar, unsigned);
	};

	class algorithm {
		extern bool has_set_verbosity() const;
		extern bool has_set_seed() const;
		extern thread_safety get_thread_safety() const;

		virtual population evolve(population) const = 0;
		virtual std::string get_name() const = 0;

		void set_seed(unsigned);
		unsigned get_seed() const;
		unsigned get_verbosity() const;
		void set_verbosity(unsigned);

		extern void set_bfe(const pagmo::bfe& b);
		std::string get_extra_info() const;

		//typedef std::vector<log_line_type> log_type;
		//const log_type& get_log() const;

		//template <typename Archive>
		//void serialize(Archive&, unsigned);
	};

	class archipelago {
   
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
		extern archipelago(const archipelago &);
		// Move constructor.
		extern archipelago(archipelago &&) noexcept;

		//template <typename Topo, topo_ctor_enabler<Topo> = 0>
		//extern explicit archipelago(Topo&& t);

		//template <typename... Args, n_ctor_enabler<const Args &...> = 0>
		//extern explicit archipelago(size_type n, const Args &... args);

		//template <typename Topo, typename... Args, topo_n_ctor_enabler<Topo, const Args &...> = 0>
		//extern explicit archipelago(Topo&& t, size_type n, const Args &... args) : archipelago(std::forward<Topo>(t));
		// Copy assignment.
		extern archipelago &operator=(const archipelago &);
		extern archipelago &operator=(archipelago &&) noexcept;
		// Destructor.
		//extern ~archipelago();
		// Mutable island access.
		extern island &operator[](size_type);
		// Const island access.
		extern const island &operator[](size_type) const;
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
		std::vector<vector_double> get_champions_f() const;
		// Get the decision vectors of the islands' champions.
		std::vector<vector_double> get_champions_x() const;

		// Get the migration log.
		extern migration_log_t get_migration_log() const;
		// Get the database of migrants.
		extern migrants_db_t get_migrants_db() const;
		// Set the database of migrants.
		extern void set_migrants_db(migrants_db_t);

		// Topology get/set.
		extern topology get_topology() const;
		extern void set_topology(topology);

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


	class topology {
		
			// Enable the generic ctor only if T is not a topology (after removing
			// const/reference qualifiers), and if T is a udt.
			template <typename T>
			using generic_ctor_enabler = enable_if_t<
				detail::conjunction<detail::negation<std::is_same<topology, uncvref_t<T>>>, is_udt<uncvref_t<T>>>::value, int>;

		public:
			// Default constructor.
			extern topology();

			// Generic constructor.
			template <typename T, generic_ctor_enabler<T> = 0>
			//extern explicit topology(T&& x) : m_ptr(std::make_unique<detail::topo_inner<uncvref_t<T>>>(std::forward<T>(x)));
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
			extern topology& operator=(T&& x);

			// Extract.
			template <typename T>
			extern const T* extract() const noexcept;
			template <typename T>
			extern T* extract() noexcept;
			
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
			//extern bgl_graph_t to_bgl() const;

			// Get the type at runtime.
			extern std::type_index get_type_index() const;

			// Get a const pointer to the UDT.
			extern const void* get_ptr() const;

			// Get a mutable pointer to the UDT.
			extern void* get_ptr();

			// Serialization.
			template <typename Archive>
			extern void save(Archive& ar, unsigned) const;
			template <typename Archive>
			extern void load(Archive& ar, unsigned);

	};

	%include swigInterfaceFiles\algorithms\de.i
	%include swigInterfaceFiles\algorithms\de1220.i
	%include swigInterfaceFiles\algorithms\gaco.i
	%include swigInterfaceFiles\algorithms\sade.i

	%include swigInterfaceFiles\problems\golomb_ruler.i
};

