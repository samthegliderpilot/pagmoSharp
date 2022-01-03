#pragma once

#include <pagmo/algorithm.hpp>
#include <pagmo/algorithms/gaco.hpp>

namespace pagmoWrap
{
	typedef void (__stdcall *Operation)(double* x, double* ans, int sizeOfX, int sizeOfAns);
	typedef std::vector<double> vector_double;
	class problem
	{
	private:
		Operation _fitnessCallback = nullptr;
		vector_double _lowerBounds{};
		vector_double _upperBounds{};
		int _sizeOfInputVector = -1;

	public:
		problem();

		void StoreFitnessCallback(void __stdcall callback(double*, double*, int, int));
		void SetBounds(vector_double lowerBounds, vector_double upperBounds);

		[[nodiscard]] vector_double::size_type get_nec() const;

		// Number of inequality constraints.
		[[nodiscard]] vector_double::size_type get_nic() const;

		[[nodiscard]] std::pair<vector_double, vector_double> get_bounds() const;
		[[nodiscard]] vector_double fitness(const vector_double&) const;

		[[nodiscard]] std::string get_name() const;

	};
};