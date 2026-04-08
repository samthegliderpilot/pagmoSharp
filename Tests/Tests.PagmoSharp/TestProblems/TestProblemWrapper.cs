using pagmo;
using System.Collections.Concurrent;

namespace Tests.PagmoSharp.TestProblems
{
    // Shared base for managed UDP fixtures used by algorithm/island/archipelago tests.
    // It centralizes expected-optimum metadata so generic test bases can assert outcomes uniformly.
    public abstract class TestProblemWrapper : ManagedProblemBase
    {
        public abstract double ExpectedOptimalFunctionValue { get; }
        public abstract double[] ExpectedOptimalX { get; }

        public override void Dispose()
        {
            //_problem.Dispose();
            base.Dispose();
        }
    }
}
