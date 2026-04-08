using pagmo;

namespace Tests.PagmoSharp.TestProblems
{
    // Educational constrained fixture used for simple end-to-end optimization tests:
    // easy to reason about objective/constraint geometry while still exercising pagmo flow.
    public class TwoParabolaConstrainedProblemWrapper : TestProblemWrapper
    {
        private readonly DoubleVector _lowerBounds = new DoubleVector(new[] { -5.0, -5.0 });
        private readonly DoubleVector _upperBounds = new DoubleVector(new[] { 5.0, 5.0 });

        public override PairOfDoubleVectors get_bounds()
        {
            return new PairOfDoubleVectors(_lowerBounds, _upperBounds);
        }

        public override DoubleVector fitness(DoubleVector decisionVector)
        {
            var x = decisionVector[0];
            var y = decisionVector[1];

            // Sum of two paraboloids centered at (0,0) and (1,-1).
            var objective = (x * x + y * y) + ((x - 1.0) * (x - 1.0) + (y + 1.0) * (y + 1.0));

            // g(x) <= 0
            var inequalityConstraint = x + y;
            return new DoubleVector(new[] { objective, inequalityConstraint });
        }

        public override uint get_nic()
        {
            return 1u;
        }

        public override thread_safety get_thread_safety()
        {
            return thread_safety.constant;
        }

        public override string get_name()
        {
            return "Two-parabola constrained single-objective test problem";
        }

        public override double ExpectedOptimalFunctionValue => 1.0;
        public override double[] ExpectedOptimalX => new[] { 0.5, -0.5 };
    }
}
