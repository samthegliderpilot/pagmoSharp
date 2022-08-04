using pagmo;
using System.Collections.Concurrent;

namespace Tests.PagmoSharp.TestProblems
{
    public abstract class TestProblemWrapper : problemBase
    {
        public abstract double ExpectedOptimalFunctionValue { get; }
        public abstract double[] ExpectedOptimalX { get; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //_problem.Dispose();
                base.Dispose(disposing);
            }
        }
    }
}