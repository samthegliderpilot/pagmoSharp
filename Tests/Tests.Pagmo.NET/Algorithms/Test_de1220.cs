using System;
using NUnit.Framework;
using pagmo;
using Tests.Pagmo.NET.TestProblems;

namespace Tests.Pagmo.NET.Algorithms
{
    [TestFixture]
    public class Test_de1220 : TestAlgorithmBase
    {
        public override IAlgorithm CreateAlgorithm()
        {
            return new pagmo.de1220(10);
        }

        [Test]
        public override void TestNameIsCorrect()
        {
            using (var problem = new TwoDimensionalSingleObjectiveProblemWrapper())
            using (var algorithm = CreateAlgorithm(problem))
            {
                Assert.AreEqual("sa-DE1220: Self-adaptive Differential Evolution 1220", algorithm.get_name());
            }
        }

        [Test]
        public void ConstructorAcceptsTypedAllowedVariantsVector()
        {
            using var allowedVariants = new UIntVector(new uint[] { 2u, 7u, 12u });
            using var algorithm = new de1220(10u, allowedVariants);

            Assert.AreEqual(10u, algorithm.get_gen(), "Typed allowed-variants constructor should preserve configured generation count.");
            Assert.AreEqual("sa-DE1220: Self-adaptive Differential Evolution 1220", algorithm.get_name());
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

