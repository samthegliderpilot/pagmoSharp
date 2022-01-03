using System;
using pagmo;

namespace Tests.PagmoSharp
{
    public class TestGaco : TestAlgorithmBase
    {
        public override IDisposable CreateAlgorithm()
        {
            return new pagmo.gaco(10);
        }

        public override population EvolveAlgorithm(IDisposable algorithm, population population)
        {
            return ((gaco)algorithm).evolve(population);
            
        }
    }
}