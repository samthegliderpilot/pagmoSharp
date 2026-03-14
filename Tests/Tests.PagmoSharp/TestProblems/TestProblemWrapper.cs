using pagmo;
using System.Collections.Concurrent;

namespace Tests.PagmoSharp.TestProblems
{
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
