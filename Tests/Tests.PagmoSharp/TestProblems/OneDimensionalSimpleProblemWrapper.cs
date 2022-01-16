using pagmo;

namespace Tests.PagmoSharp.TestProblems
{
    public class OneDimensionalSimpleProblem : TestProblemWrapper
    {
        private class OneDimensionalSimpleProblemFunction : pagmo.problemBase
        {
            private readonly DoubleVector _lowerBounds = new DoubleVector(new[] { -10.0 });
            private readonly DoubleVector _upperBounds = new DoubleVector(new[] { 10.0 });

            public override PairOfDoubleVectors get_bounds()
            {
                return new PairOfDoubleVectors(_lowerBounds,_upperBounds);
            }

            public override DoubleVector fitness(DoubleVector arg0)
            {
                return new DoubleVector(new[] { arg0[0] * arg0[0] });
            }

            public override string get_name()
            {
                return "Simple 1-D Quadratic (x^2) test problem";
            }

            public override bool has_batch_fitness()
            {
                return false;
            }

            public override uint get_nobj()
            {
                return 1;
            }

            public override uint get_nix()
            {
                return 0;
            }

            public override uint get_nec()
            {
                return 0;
            }

            public override uint get_nic()
            {
                return 0;
            }
        }

        public OneDimensionalSimpleProblem() :base(new OneDimensionalSimpleProblemFunction())
        { }

        public override double ExpectedOptimalFunctionValue => 0.0;
        public override double[] ExpectedOptimalX { get { return new[] { 0.0 }; } }
    }
}