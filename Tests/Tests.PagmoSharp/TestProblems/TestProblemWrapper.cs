using pagmo;

namespace Tests.PagmoSharp.TestProblems
{
    public abstract class TestProblemWrapper : problem
    {
        private readonly problemBase _problem;

        protected TestProblemWrapper(problemBase baseProblem)
        {
            _problem = baseProblem;
            setBaseProblem(baseProblem);
        }

        public abstract double ExpectedOptimalFunctionValue { get; }
        public abstract double[] ExpectedOptimalX { get; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _problem.Dispose();
                base.Dispose(disposing);
            }
        }
    }
}