#include "gacoExterns.h"
#include <pagmo/algorithm.hpp>
#include <pagmo/algorithms/gaco.hpp>
using namespace pagmo;

extern "C" {
	__declspec(dllexport) void get_name(pagmo::gaco* algo, char* buffer, unsigned length)
	{
		std::string name = algo->get_name();
		name.copy(buffer, length);
		buffer[length - 1] = '\0'; // null termination
	}

	__declspec(dllexport) unsigned get_length_of_name(pagmo::gaco* algo)
	{
		unsigned length = algo->get_name().size();
		return length;
	}
	
	__declspec(dllexport) pagmo::gaco* gaco(unsigned gen, unsigned ker, double q, double oracle, double acc, unsigned threshold, unsigned n_gen_mark, unsigned impstop, unsigned evalstop, double focus, bool memory, unsigned int seed)
	{
		pagmo::gaco* algo = new pagmo::gaco(gen, ker, q, oracle, acc, threshold, n_gen_mark, impstop, evalstop, focus, memory, seed);
		return algo;
	}
}