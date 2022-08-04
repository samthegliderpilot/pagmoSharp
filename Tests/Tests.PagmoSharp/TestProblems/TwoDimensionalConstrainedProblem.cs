using System;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.TestProblems
{
    public class TwoDimensionalConstrainedProblem : TestProblemWrapper
    {
        private readonly DoubleVector _lowerBounds = new DoubleVector(new[] { -1.0, -1.0 });
        private readonly DoubleVector _upperBounds = new DoubleVector(new[] { 5.0, 5.0 });

        public override PairOfDoubleVectors get_bounds()
        {
            return new PairOfDoubleVectors(_lowerBounds, _upperBounds);
        }

        /// <inheritdoc />
        public override DoubleVector fitness(DoubleVector arg0)
        {
            double x = arg0[0];
            double y = arg0[1];

            double obj = Math.Pow(x, 2) + y;
            double yEq2Const = y - 2.0; // y == 2
            double xLtEq1Const = x - 1.0; // x == 1
            return new DoubleVector(new[] { obj, yEq2Const, xLtEq1Const });
        }

        public override string get_name()
        {
            return "Simple 2-D x^2+y test problem with constraint y==2.0, x == 1";
        }

        public override thread_safety get_thread_safety()
        {
            return thread_safety.constant;
        }

        /// <inheritdoc />
        public override uint get_nobj()
        {
            return 1;
        }

        /// <inheritdoc />
        public override uint get_nec()
        {
            return 2;
        }

        /// <inheritdoc />
        public override uint get_nic()
        {
            return 0; //TODO: I couldn't get the ant colony optimizer to converge reliably with a x<=1 constraint
        }

        /// <inheritdoc />
        public override double ExpectedOptimalFunctionValue
        {
            get { return 3.0; }
        }

        /// <inheritdoc />
        public override double[] ExpectedOptimalX
        {
            get { return new[] { 1.0, 2.0 }; }
        }

        [Test]
        public void TestBasicEvaluations()
        {
            using var problem = new TwoDimensionalConstrainedProblem();
            var answer = problem.fitness(1.0, 2.0);
            Assert.AreEqual(3.0, answer[0], 2);
            Assert.AreEqual(0.0, answer[1], 2);
            Assert.AreEqual(0.0, answer[2], 2);

            answer = problem.fitness(2.0, 3.0);
            Assert.AreEqual(7.0, answer[0], 2);
            Assert.AreEqual(1.0, answer[1], 2);
            Assert.AreEqual(-1.0, answer[2], 2);

            answer = problem.fitness(0.0, 2.0);
            Assert.AreEqual(2.0, answer[0], 2);
            Assert.AreEqual(0.0, answer[1], 2);
            Assert.AreEqual(1.0, answer[2], 2);

        }
    }
}