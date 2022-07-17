#pragma once

#include "pagmo/problem.hpp"

namespace pagmoWrap
{
	typedef std::vector<double> vector_double;

	class problemBase : public pagmo::problem
	{
	public:
		virtual ~problemBase() {}

		virtual vector_double fitness(const vector_double&) const
		{
			return vector_double();
		}

		virtual std::pair<vector_double, vector_double> get_bounds() const
		{
			return std::pair< vector_double, vector_double>{vector_double(), vector_double()};
		}

		virtual bool has_batch_fitness() const {
			return false;
		}

		//We cannot use [[nodiscard]] due to using this header to create the type
		virtual std::string get_name() const {
			return "Base c++ problem";
		}

		virtual std::vector<double>::size_type get_nobj() const
		{
			return static_cast<vector_double::size_type>(1); // default of 1
		}

		virtual std::vector<double>::size_type get_nec() const
		{
			return static_cast<vector_double::size_type>(0);
		}

		virtual std::vector<double>::size_type get_nic() const
		{
			return static_cast<vector_double::size_type>(0);
		}

		virtual std::vector<double>::size_type get_nix() const
		{
			return static_cast<vector_double::size_type>(0);
		}

		virtual pagmo::thread_safety get_thread_safety() const
		{
			return pagmo::thread_safety::none;
		}

		//bool has_gradient() const;
		//vector_double gradient(const vector_double&) const;
		//bool has_gradient_sparsity() const;
		//sparsity_pattern gradient_sparsity() const;
		//bool has_hessians() const;
		//std::vector<vector_double> hessians(const vector_double&) const;
		//bool has_hessians_sparsity() const;
		//std::vector<sparsity_pattern> hessians_sparsity() const;
		//bool has_set_seed() const;
		//void set_seed(unsigned);
		//thread_safety get_thread_safety() const;


	};


	class problem 
	{
	private:
		problemBase* _base;
		void deleteProblem() {
			//TODO: This is something that has me worried.  pagmo will create copies of the problem,
			// but if they all share the same pointer, when one of those copies gets destroyed, it
			// deletes the whole pointer, which breaks other copies.  But not deleting it goes against
			// the director example for swig, and I fear opens us up to a memory leak in sloppy cases
			// (cases where the C# isn't behaving)
			//delete _base;
		}
	public:
		problem() : _base(0) {}

		problem(problemBase* base) : _base(base) { }

		problem(const problem& old) : _base(0) {
			_base = old._base;
		}
		~problem() {
			deleteProblem();
		}

		void setBaseProblem(problemBase* b) {
			deleteProblem(); _base = b;
		}

		problemBase* getBaseProblem() {
			return _base;
		}

		vector_double fitness(const vector_double& x) const {
			return _base->fitness(x);
		}

		std::pair<vector_double, vector_double> get_bounds() const
		{
			return _base->get_bounds();
		}

		bool has_batch_fitness() const
		{
			return _base->has_batch_fitness();
		}

		std::string get_name() const
		{
			return _base->get_name();
		}

		std::vector<double>::size_type get_nobj() const
		{
			return _base->get_nobj();
		}

		std::vector<double>::size_type get_nec() const
		{
			return _base->get_nec();
		}

		std::vector<double>::size_type get_nic() const
		{
			return _base->get_nic();
		}

		std::vector<double>::size_type get_nix() const
		{
			return _base->get_nix();
		}

		pagmo::thread_safety get_thread_safety() const
		{
			return _base->get_thread_safety();
		}
	};
};