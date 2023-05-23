#pragma once
#include <pagmo/types.hpp>

namespace pagmo
{
	extern "C" __declspec(dllexport) vector_double gradientsAndHessiansCallback(vector_double x);
	
};
