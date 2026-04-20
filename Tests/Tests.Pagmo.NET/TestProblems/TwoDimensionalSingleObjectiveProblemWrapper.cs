using System.Collections.Concurrent;
using System.Collections.Generic;
using pagmo;

namespace Tests.PagmoSharp.TestProblems
{
    // Canonical managed single-objective 2D fixture used for end-to-end runtime tests
    // (algorithm evolution, island/archipelago setup, and thread-safety behavior).
    public class TwoDimensionalSingleObjectiveProblemWrapper : TestProblemWrapper
    {
        private readonly DoubleVector _lowerBounds = new DoubleVector(new[] { -10.0, -10.0 });
        private readonly DoubleVector _upperBounds = new DoubleVector(new[] { 10.0, 10.0 });

        public override PairOfDoubleVectors get_bounds()
        {
            return new PairOfDoubleVectors(_lowerBounds, _upperBounds);
        }

        public override DoubleVector fitness(DoubleVector decisionVector)
        {
            TwoDimensionalSingleObjectiveProblemWrapper.ThreadIds.Add(System.Threading.Thread.CurrentThread
                .ManagedThreadId);
            double x = decisionVector[0];
            double y = decisionVector[1];
            return new DoubleVector(new[] { x * x + (y - 3) * (y - 3) });
        }

        public override bool has_gradient()
        {
            return true;
        }

        public override DoubleVector gradient(DoubleVector decisionVector)
        {
            var ans = new DoubleVector(new [] { 2 * decisionVector[0], 2 * decisionVector[1] - 6.0 });
            return ans;
        }

        public override string get_name()
        {
            return "Simple 2-D Quadratic (x^2) test problem";
        }

        public override bool has_batch_fitness()
        {
            return false;
        }

        public override ThreadSafety get_thread_safety()
        {
            return ThreadSafety.Constant;
        }
        
        public static ConcurrentBag<int> ThreadIds = new ConcurrentBag<int>();
        
        public IEnumerable<int> ThreadsExecuted
        {
            get { return ThreadIds; }
        }

        public override double ExpectedOptimalFunctionValue => 0.0;

        public override double[] ExpectedOptimalX
        {
            get { return new[] { 0.0, 3.0 }; }
        }
    }
}
