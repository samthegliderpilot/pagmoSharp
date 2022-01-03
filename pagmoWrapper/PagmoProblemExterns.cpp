#pragma once
#include "PagmoProblemExterns.h"
#include "problem.h"

extern "C" {
	__declspec(dllexport) void RegisterFitnessCallback(pagmoWrap::problem* problem, void _stdcall callback(double*, double*, int, int))
	{
		problem->StoreFitnessCallback(callback);
	}
}
