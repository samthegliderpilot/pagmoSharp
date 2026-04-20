using System;
using NUnit.Framework;
using pagmo;
using Tests.Pagmo.NET.TestProblems;

namespace Tests.Pagmo.NET.Algorithms
{
    [TestFixture]
    public class Test_de : TestAlgorithmBase
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

        [Test]
        public void TypedAndGenericLogsAreExposed()
        {
            using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var algorithm = new pagmo.de(25u);
            using var population = new population(problem, 64u, 2u);

            algorithm.set_seed(2u);
            algorithm.set_verbosity(1u);

            using var _ = algorithm.evolve(population);

            var typedLines = algorithm.GetTypedLogLines();
            Assert.That(typedLines.Count, Is.GreaterThan(0), "de verbosity should produce at least one log line");

            IAlgorithm algorithmInterface = algorithm;
            var genericLines = algorithmInterface.GetLogLines();
            Assert.That(genericLines.Count, Is.EqualTo(typedLines.Count));

            var raw = genericLines[0].RawFields;
            Assert.That(genericLines[0].AlgorithmName, Is.EqualTo("de"));
            Assert.That(raw.ContainsKey("generation"), Is.True);
            Assert.That(raw.ContainsKey("function_evaluations"), Is.True);
            Assert.That(raw.ContainsKey("best_fitness"), Is.True);
            Assert.That(raw.ContainsKey("function_evaluation_difference"), Is.True);
            Assert.That(raw.ContainsKey("dx"), Is.True);
            Assert.That(genericLines[0].ToDisplayString(), Does.Contain("gen="));

            Assert.That((uint)raw["generation"], Is.EqualTo(typedLines[0].Generation));
            Assert.That((ulong)raw["function_evaluations"], Is.EqualTo(typedLines[0].FunctionEvaluations));
            Assert.That((double)raw["best_fitness"], Is.EqualTo(typedLines[0].BestFitness));
        }
    }
}

