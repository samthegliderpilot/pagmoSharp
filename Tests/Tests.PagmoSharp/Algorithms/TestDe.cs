using System;
using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp.Algorithms
{
    [TestFixture]
    public class TestDe : TestAlgorithmBase
    {
        public override IAlgorithm CreateAlgorithm()
        {
            return new pagmo.de(10);
        }

        [Test]
        public override void TestNameIsCorrect()
        {
            using (var problem = new TwoDimensionalSingleObjectiveProblemWrapper())
            using (var algorithm = CreateAlgorithm(problem))
            {
                Assert.AreEqual("DE: Differential Evolution", algorithm.get_name());
            }
        }

        /// <inheritdoc />
        public override bool Constraints => false;

        /// <inheritdoc />
        public override bool Unconstraned => true;

        /// <inheritdoc />
        public override bool SingleObjective => true;

        /// <inheritdoc />
        public override bool MultiObjective => false;

        /// <inheritdoc />
        public override bool IntegerPrograming => false;

        /// <inheritdoc />
        public override bool Stochastic => false;
    }
}