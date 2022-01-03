
#pragma once
#include <pagmo/algorithm.hpp>
#include <pagmo/algorithms/sade.hpp>
#include <pagmo/archipelago.hpp>
#include <pagmo/problem.hpp>
#include "problem.h"
using namespace pagmo;

namespace pagmoWrap {
	problem::problem()
	= default;


	void problem::StoreFitnessCallback(void __stdcall callback(double*, double*, int sizeOfX, int sizeOfAns))
	{
		_fitnessCallback = callback;
	}

	void problem::SetBounds(vector_double lowerBounds, vector_double upperBounds)
	{
		_sizeOfInputVector = lowerBounds.size();
		_lowerBounds = vector_double();
		_upperBounds = vector_double();
		for (int i = 0; i < _sizeOfInputVector; i++)
		{
			_lowerBounds.push_back(lowerBounds[i]);
			_upperBounds.push_back(upperBounds[i]);
		}
	}

	vector_double::size_type problem::get_nec() const
	{
		return 0;
	}

	// Number of inequality constraints.
	vector_double::size_type problem::get_nic() const
	{
		return 0;
	}

	std::pair<vector_double, vector_double> problem::get_bounds() const
	{
		return std::pair<vector_double, vector_double>(_lowerBounds, _upperBounds);
	}

	vector_double problem::fitness(const vector_double& x) const
	{
		unsigned lenOfAns = 1;
		double* xAsArray = new double[_sizeOfInputVector];
		for (unsigned i = 0; i < x.size(); i++)
		{
			xAsArray[i] = x[i];
		}
		double* backFromCs = new double[lenOfAns];
		for (unsigned j = 0; j < lenOfAns; j++)
		{
			backFromCs[j] = 0;
		}

		_fitnessCallback(xAsArray, backFromCs, _sizeOfInputVector, lenOfAns);

		std::vector<double> temp;
		for (unsigned i = 0; i < lenOfAns; i++) {
			temp.push_back(backFromCs[i]);
		}
		return temp;
	}

	[[nodiscard]] std::string problem::get_name() const
	{
		return "Sample Problem Function";
	}
};