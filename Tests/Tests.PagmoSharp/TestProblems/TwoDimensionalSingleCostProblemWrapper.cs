using pagmo;

namespace Tests.PagmoSharp.TestProblems
{
    public class TwoDimensionalSingleCostProblemWrapper : TestProblemWrapper
    {
        private class TwoDimensionalSimpleProblemFunction : pagmo.problemBase
        {
            private readonly DoubleVector _lowerBounds = new DoubleVector(new[] { -10.0, -10.0 });
            private readonly DoubleVector _upperBounds = new DoubleVector(new[] { 10.0, 10.0 });

            public override PairOfDoubleVectors get_bounds()
            {
                return new PairOfDoubleVectors(_lowerBounds, _upperBounds);
            }

            public override DoubleVector fitness(DoubleVector arg0)
            {
                double x = arg0[0];
                double y = arg0[1];
                return new DoubleVector(new[] { x*x + (y-3)*(y-3) });
            }

            public override string get_name()
            {
                return "Simple 2-D Quadratic (x^2) test problem";
            }

            public override bool has_batch_fitness()
            {
                return false;
            }
        }

        public TwoDimensionalSingleCostProblemWrapper() : base(new TwoDimensionalSimpleProblemFunction())
        { }

        public override double ExpectedOptimalFunctionValue => 0.0;
        public override double[] ExpectedOptimalX { get { return new[] { 0.0, 3.0 }; } }
    }
}