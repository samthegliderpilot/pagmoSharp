#pragma once
#include <pagmo/algorithm.hpp>
#include <pagmo/types.hpp>
#include "PagmoProblem.h"
#include <pagmo/algorithms/sade.hpp>
#include <pagmo/archipelago.hpp>
#include <pagmo/problem.hpp>
#include <string.h>
using namespace pagmo;


//PagmoProblem::PagmoProblem( )
//{
//}


//zvoid PagmoProblem::StoreFitnessCallback(void __stdcall callback(double*, double*, int sizeOfX, int sizeOfAns))
//{
//	_fitnessCallback = callback;
//}
//
//void PagmoProblem::SetBounds(double* lowerBounds, double* upperBounds, int numberOfBoundableThings)
//{
//	_sizeOfInputVector = numberOfBoundableThings;
//	_lowerBounds = vector_double();
//	_upperBounds = vector_double();
//	for (int i = 0; i < _sizeOfInputVector; i++)
//	{
//		_lowerBounds.push_back(lowerBounds[i]);
//		_upperBounds.push_back(upperBounds[i]);
//	}
//}
//
//vector_double::size_type PagmoProblem::get_nec() const
//{
//	return 0;
//}
//
//// Number of inequality constraints.
//vector_double::size_type PagmoProblem::get_nic() const
//{
//	return 0;
//}
//
//std::pair<vector_double, vector_double> PagmoProblem::get_bounds() const
//{
//	return std::pair<vector_double, vector_double>(_lowerBounds, _upperBounds);
//}
//
//vector_double PagmoProblem::fitness(const vector_double& x) const
//{
//	int lenOfAns = 1;
//	double* xAsArray = new double[_sizeOfInputVector];
//	for(int i=0;i < x.size(); i++)
//	{
//		xAsArray[i] = x[i];
//	}
//	double* backFromCs = new double[lenOfAns];
//	for(int j = 0; j < lenOfAns; j++)
//	{
//		backFromCs[j] = 0;
//	}
//	
//	_fitnessCallback(xAsArray, backFromCs, _sizeOfInputVector, lenOfAns);
//	
//	std::vector<double> temp;
//	for (int i = 0; i < lenOfAns; i++) {
//		temp.push_back(backFromCs[i]);
//	}
//	return temp;
//}
//
//[[nodiscard]] std::string PagmoProblem::get_name() const
//{
//	return "Sample Problem Function";
//}

//void PagmoProblem::Evolve(unsigned n)
//{
//	PagmoProblem prob = *this;
//	//problem realProblem{prob};
//	
//	//archipelago archi{ 16u, _algorithm, realProblem , 20u };
//
//	//// 4 - Run the evolution in parallel on the 16 separate islands 10 times.
//	//archi.evolve(10);
//
//	//// 5 - Wait for the evolutions to finish.
//	//archi.wait_check();
//
//	//// 6 - Print the fitness of the best solution in each island.
//	//
//	population pop{ prob, n };
//	//pop = _algorithm->evolve(pop);
//
//
//	((pagmo::gaco*)_algorithm)->evolve(pop);
//	//jresult = new pagmo::population((const pagmo::population&)result);
//
//
//	
//	std::vector<double> champ = pop.champion_x();
//	for (double val : champ) {
//		std::cout << std::to_string(val) << '\n';
//	}
//	//double* ansAsArray = &champ[0];
//	//return ansAsArray;
//
//	//double* fakeAns = new double[4];
//	//return fakeAns;
//}
	
