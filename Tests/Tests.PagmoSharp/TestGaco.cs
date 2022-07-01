using System;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp
{
    public class TestGaco : TestAlgorithmBase
    {
        public override IAlgorithm CreateAlgorithm(TestProblemWrapper testProblem)
        {
            return new pagmo.gaco(10);
        }

        public override string Name
        {
            get { return "GACO: Ant Colony Optimization"; }
        }
    }
}