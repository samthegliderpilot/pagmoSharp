//#pragma once
//
//#include <pagmo/algorithm.hpp>
//#include <pagmo/algorithms/gaco.hpp>
//
//using namespace pagmo;
//typedef void (__stdcall* Operation)(double* x, double* ans, int sizeOfX, int sizeOfAns);
//
//class PagmoProblem
//{
//private:
//	Operation _fitnessCallback = nullptr;
//	vector_double _lowerBounds;
//	vector_double _upperBounds;
//	int _sizeOfInputVector=-1;
//
//public:
//	PagmoProblem();
//	
//	void StoreFitnessCallback(void __stdcall callback(double*, double*, int, int));
//	void SetBounds(double* lowerBounds, double* upperBounds, int numberOfBoundableThings);
//
//	vector_double::size_type get_nec() const;
//	
//	// Number of inequality constraints.
//	vector_double::size_type get_nic() const;
//	
//	[[nodiscard]] std::pair<vector_double, vector_double> get_bounds() const;
//	[[nodiscard]] vector_double fitness(const vector_double&) const;
//	
//	//double* Evolve(unsigned n);
//	void Evolve(unsigned n);
//
//	[[nodiscard]] std::string get_name() const;
//} ;
