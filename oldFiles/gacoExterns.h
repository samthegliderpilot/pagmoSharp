#pragma once
#include <pagmo/algorithm.hpp>
#include <pagmo/rng.hpp>
#include <pagmo/algorithms/gaco.hpp>
using namespace pagmo;

extern "C" {
	__declspec(dllexport) pagmo::gaco* gaco(unsigned gen = 1u, unsigned ker = 63u, double q = 1.0, double oracle = 0., double acc = 0.01,
        unsigned threshold = 1u, unsigned n_gen_mark = 7u, unsigned impstop = 100000u, unsigned evalstop = 100000u,
        double focus = 0., bool memory = false, unsigned seed = random_device::next());

    __declspec(dllexport) void get_name(pagmo::gaco* algo, char* buffer, unsigned length);

    __declspec(dllexport) unsigned get_length_of_name(pagmo::gaco* algo);

}
