using System;
using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp.Algorithms
{
    [TestFixture]
    public class TestNspso : TestAlgorithmBase
    {
        public override IAlgorithm CreateAlgorithm()
        {
            return new pagmo.nspso(10);
        }

        [Test]
        public override void TestNameIsCorrect()
        {
            using (var problem = new TwoDimensionalSingleObjectiveProblemWrapper())
            using (var algorithm = CreateAlgorithm(problem))
            {
                Assert.AreEqual("NSPSO", algorithm.get_name());
            }
        }

        public override bool SupportsGeneration => false;

        /// <inheritdoc />
        public override bool Constrained => false;

        /// <inheritdoc />
        public override bool Unconstrained => true;

        /// <inheritdoc />
        public override bool SingleObjective => false;

        /// <inheritdoc />
        public override bool MultiObjective => true;

        /// <inheritdoc />
        public override bool IntegerPrograming => false;

        /// <inheritdoc />
        public override bool Stochastic => false;
    }
}