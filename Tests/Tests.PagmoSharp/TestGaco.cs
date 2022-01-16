using System;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp
{
    public class TestGaco : TestAlgorithmBase
    {
        public override IDisposable CreateAlgorithm(TestProblemWrapper testProblem)
        {
            return new pagmo.gaco(10);
        }

        public override population EvolveAlgorithm(IDisposable algorithm, population population)
        {
            return ((gaco)algorithm).evolve(population);
            
        }
    }
}