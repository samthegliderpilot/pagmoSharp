//#include <iostream>
//#include <pagmo/algorithm.hpp>
//#include <pagmo/algorithms/sade.hpp>
//#include <pagmo/algorithms/gaco.hpp>
//#include <pagmo/archipelago.hpp>
//#include <pagmo/problem.hpp>
//#include <pagmo/problems/schwefel.hpp>
//
//#include "SampleProblem.h"
//#include "PagmoProblem.h"
//
//using namespace pagmo;
//extern "C" {
//    __declspec(dllexport) void SampleProblem()
//    {
//        // 1 - Instantiate a pagmo problem constructing it from a UDP
//    // (i.e., a user-defined problem, in this case the 30-dimensional
//    // generalised Schwefel test function).
//        problem prob{ schwefel(30) };
//
//        // 2 - Instantiate a pagmo algorithm (self-adaptive differential
//        // evolution, 100 generations).
//        algorithm algo{ sade(100) };
//
//        // 3 - Instantiate an archipelago with 16 islands having each 20 individuals.
//        archipelago archi{ 16u, algo, prob, 20u };
//
//        // 4 - Run the evolution in parallel on the 16 separate islands 10 times.
//        archi.evolve(10);
//
//        // 5 - Wait for the evolutions to finish.
//        archi.wait_check();
//
//        // 6 - Print the fitness of the best solution in each island.
//        for (const auto& isl : archi) {
//            std::cout << isl.get_population().champion_f()[0] << '\n';
//        }
//    }
//
//	__declspec(dllexport) gaco* CreateExtendedAntColonyAlgorithm(unsigned gen, unsigned ker, double q, double oracle, double acc, unsigned threshold , unsigned n_gen_mark, unsigned impstop , unsigned evalstop , double focus , bool memory , unsigned seed)
//    {
//        return new gaco(gen, ker, q, oracle, acc, threshold, n_gen_mark, impstop, evalstop, focus, memory, seed);
//    }
//
//    __declspec(dllexport) void CallbackExample(const double __stdcall p(double a, double b))
//    {
//        std::cout << p(5.0, 6.0);
//    }
//
//	
//}
//
//int main()
//{
//    SampleProblem();
//    std::cin.ignore();
//}