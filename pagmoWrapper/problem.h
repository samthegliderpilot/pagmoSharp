#pragma once

#include <pagmo/algorithm.hpp>
#include <pagmo/algorithms/gaco.hpp>

namespace pagmoWrap
{
	//typedef void (__stdcall *Operation)(double* x, double* ans, int sizeOfX, int sizeOfAns);
	typedef std::vector<double> vector_double;
	class problemBase
	{
	public:
		//problemBase() {};
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
		virtual std::string get_name() const {
			return "Base c++ problem";
		}
		//vector_double::size_type get_nobj() const;
		//vector_double::size_type get_nec() const;
		//vector_double::size_type get_nic() const;
		//vector_double::size_type get_nix() const;
		//vector_double batch_fitness(const vector_double&) const;
		//bool has_batch_fitness() const;
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


	class problem {
	private:
		problemBase* m_baseProblem;
		void deleteProblem() {
			//TODO: This is something that has me worried.  pagmo will create copies of the problem,
			// but if they all share the same pointer, when one of those copies gets destroyed, it
			// deletes the whole pointer, which breaks other copies.  But not deleting it goes against
			// the director example for swig, and I fear opens us up to a memory leak in sloppy cases
			// (cases where the C# isn't behaving)
			//delete m_baseProblem;
		}
	public:
		problem() : m_baseProblem(0) {}
		problem(const problem& old) : m_baseProblem(0) {
			m_baseProblem = old.m_baseProblem;
		}
		~problem() {
			deleteProblem();
		}

		void setBaseProblem(problemBase* b) {
			deleteProblem(); m_baseProblem = b;
		}

		problemBase* getBaseProblem() {
			return m_baseProblem;
		}

		vector_double fitness(const vector_double& x) const {
			return m_baseProblem->fitness(x);
		}

		std::pair<vector_double, vector_double> get_bounds() const
		{
			return m_baseProblem->get_bounds();
		}

		bool has_batch_fitness() const
		{
			return m_baseProblem->has_batch_fitness();
		}

		std::string get_name() const
		{
			return m_baseProblem->get_name();
		}
	};
};