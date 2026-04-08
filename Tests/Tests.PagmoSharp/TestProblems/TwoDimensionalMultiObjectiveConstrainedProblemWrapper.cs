using pagmo;

namespace Tests.PagmoSharp.TestProblems
{
    // Managed constrained multi-objective fixture used to validate MO+constraints
    // wrapper behavior and fitness vector layout (objectives + constraints).
    public class TwoDimensionalMultiObjectiveConstrainedProblemWrapper : TestProblemWrapper
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
            var f1 = x * x + y * y;
            var f2 = (x - 1.0) * (x - 1.0) + (y + 1.0) * (y + 1.0);
            var inequalityConstraint = x + y - 1.0; // <= 0
            return new DoubleVector(new[] { f1, f2, inequalityConstraint });
        }

        public override string get_name()
        {
            return "2-D Constrained Multiobjective Quadratic Test Problem";
        }

        public override thread_safety get_thread_safety()
        {
            return thread_safety.constant;
        }

        public override uint get_nobj()
        {
            return 2u;
        }

        public override uint get_nic()
        {
            return 1u;
        }

        public override double ExpectedOptimalFunctionValue => 0.0;
        public override double[] ExpectedOptimalX => new[] { 0.0, 0.0 };
    }
}
