using System;
using NUnit.Framework;
using pagmo;
using Tests.Pagmo.NET.TestProblems;

namespace Tests.Pagmo.NET.Algorithms
{
    [TestFixture]
    public class Test_bee_Colony : TestAlgorithmBase
    {
        public override IAlgorithm CreateAlgorithm()
        {
            return new pagmo.bee_colony(10);
        }

        [Test]
        public override void TestNameIsCorrect()
        {
            using (var problem = new TwoDimensionalSingleObjectiveProblemWrapper())
            using (var algorithm = CreateAlgorithm(problem))
            {
                Assert.AreEqual("ABC: Artificial Bee Colony", algorithm.get_name());
            }
        }

        /// <inheritdoc />
        public override bool Constrained => false;

        /// <inheritdoc />
        public override bool Unconstrained => true;

        /// <inheritdoc />
        public override bool SingleObjective => true;

        /// <inheritdoc />
        public override bool MultiObjective => false;

        /// <inheritdoc />
        public override bool IntegerProgramming => false;

        /// <inheritdoc />
        public override bool Stochastic => false;
    }
}
