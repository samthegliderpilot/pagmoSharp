using System;
using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp.Algorithms
{
    [TestFixture]
    public class TestCompass_Search : TestAlgorithmBase
    {
        public override IAlgorithm CreateAlgorithm()
        {
            return new pagmo.compass_search(128);
        }

        [Test]
        public override void TestNameIsCorrect()
        {
            using (var problem = new TwoDimensionalSingleObjectiveProblemWrapper())
            using (var algorithm = CreateAlgorithm(problem))
            {
                Assert.AreEqual("CS: Compass Search", algorithm.get_name());
            }
        }

        [Test]
        public override void TestBasicFunctions()
        {
            using (var problem = new TwoDimensionalSingleObjectiveProblemWrapper())
            using (var algorithm = CreateAlgorithm(problem))
            {
                Assert.NotNull(algorithm.get_extra_info(), "getting non-null extra info");
                Assert.NotNull(algorithm.get_name(), "getting non-null name");
                Assert.AreEqual(0, algorithm.get_verbosity(), "getting original verbosity");
                algorithm.set_verbosity(2);
                Assert.AreEqual(2, algorithm.get_verbosity(), "getting set verbosity");
            }
        }

        public override bool SupportsGeneration => false;

        /// <inheritdoc />
        public override bool Constrained => true;

        /// <inheritdoc />
        public override bool Unconstrained => true;

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