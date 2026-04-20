using System;
using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp.Algorithms
{
    [TestFixture]
    public class Test_gaco : TestAlgorithmBase
    {
        public override IAlgorithm CreateAlgorithm()
        {
            return new pagmo.gaco(50, 3, 1.0, 1.0);
        }

        [Test]
        public override void TestNameIsCorrect()
        {
            using (var problem = new TwoDimensionalSingleObjectiveProblemWrapper())
            using (var algorithm = CreateAlgorithm(problem))
            {
                Assert.AreEqual("GACO: Ant Colony Optimization", algorithm.get_name());
            }
        }

        /// <inheritdoc />
        public override bool Constrained => true;

        /// <inheritdoc />
        public override bool Unconstrained => true;

        /// <inheritdoc />
        public override bool SingleObjective => true;

        /// <inheritdoc />
        public override bool MultiObjective => false;

        /// <inheritdoc />
        public override bool IntegerProgramming => true;

        /// <inheritdoc />
        public override bool Stochastic => false;

        [Test]
        public void TestGetLogLinesProjection()
        {
            using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var algorithm = new pagmo.gaco(8, 3, 1.0, 1.0);
            using var population = new population(problem, 32u, 2u);

            algorithm.set_seed(2u);
            algorithm.set_verbosity(1u);

            using var _ = algorithm.evolve(population);

            var lines = algorithm.GetTypedLogLines();
            Assert.That(lines.Count, Is.GreaterThan(0), "gaco verbosity should produce at least one log line");

            var previousFevals = 0UL;
            foreach (var line in lines)
            {
                Assert.That(line.Generation, Is.GreaterThanOrEqualTo(1u), "generation should be 1-based");
                Assert.That(line.FunctionEvaluations, Is.GreaterThanOrEqualTo(previousFevals), "function evaluations should be non-decreasing");
                previousFevals = line.FunctionEvaluations;
            }
        }

        [Test]
        public void TestLogLinesExposeGenericRawFields()
        {
            using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var algorithm = new pagmo.gaco(6, 3, 1.0, 1.0);
            using var population = new population(problem, 24u, 2u);

            algorithm.set_seed(2u);
            algorithm.set_verbosity(1u);

            using var _ = algorithm.evolve(population);

            IAlgorithm algorithmInterface = algorithm;
            var lines = algorithmInterface.GetLogLines();
            Assert.That(lines.Count, Is.GreaterThan(0));

            IAlgorithmLogLine genericLine = lines[0];
            var raw = genericLine.RawFields;

            Assert.That(genericLine.AlgorithmName, Is.EqualTo("gaco"));
            Assert.That(raw.ContainsKey("generation"), Is.True);
            Assert.That(raw.ContainsKey("function_evaluations"), Is.True);
            Assert.That(raw.ContainsKey("best_fitness"), Is.True);
            Assert.That(raw.ContainsKey("kernel_size"), Is.True);
            Assert.That(raw.ContainsKey("oracle_value"), Is.True);
            Assert.That(raw.ContainsKey("dx"), Is.True);
            Assert.That(raw.ContainsKey("dp"), Is.True);
            Assert.That(genericLine.ToDisplayString(), Does.Contain("gen="));

            var typedLines = algorithm.GetTypedLogLines();
            Assert.That((uint)raw["generation"], Is.EqualTo(typedLines[0].Generation));
            Assert.That((ulong)raw["function_evaluations"], Is.EqualTo(typedLines[0].FunctionEvaluations));
            Assert.That((double)raw["best_fitness"], Is.EqualTo(typedLines[0].BestFitness));
        }
    }
}

