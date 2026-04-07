using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp
{
    [TestFixture]
    public class Test_bfe
    {
        public static IEnumerable<TestCaseData> BuiltInBasicThreadSafeProblemFactories()
        {
            yield return new TestCaseData((Func<IProblem>)(() => new ackley(2u))).SetName("ThreadBfe_Ackley");
            yield return new TestCaseData((Func<IProblem>)(() => new cec2009(1u, false, 2u))).SetName("ThreadBfe_Cec2009");
            yield return new TestCaseData((Func<IProblem>)(() => new cec2013(1u, 2u))).SetName("ThreadBfe_Cec2013");
            yield return new TestCaseData((Func<IProblem>)(() => new rastrigin(2u))).SetName("ThreadBfe_Rastrigin");
            yield return new TestCaseData((Func<IProblem>)(() => new schwefel(2u))).SetName("ThreadBfe_Schwefel");
        }

        [Test]
        public void TestDefaultBfe()
        {
            using var bfeSample = new default_bfe();
            using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var pop = new population(problem, 4);
            using var batchX = new DoubleVector(new[] { 1.2, 1.3, 1.1, 1.4 });
            using var bfeResult = bfeSample.Operator(problem, batchX);
            Assert.AreEqual(2, bfeResult.Count);
            Assert.AreEqual(problem.fitness(1.2, 1.3)[0], bfeResult[0]);
            Assert.AreEqual(problem.fitness(1.1, 1.4)[0], bfeResult[1]);
            Assert.AreEqual("Default batch fitness evaluator", bfeSample.get_name());
        }

        [Test]
        public void TestThreadBfe()
        {
            var usedMultipleThreads = false;
            const int maxAttempts = 16;
            for (var attempt = 0; attempt < maxAttempts && !usedMultipleThreads; attempt++) // can't guarantee thread scheduling, so sample multiple runs
            {
                using var bfeSample = new pagmo.thread_bfe();
                using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
                using var pop = new population(problem, 64);
                using var batchX = new DoubleVector(new[]
                {
                    1.2, 1.3, 1.1, 1.4, 1.2, 1.3, 1.1, 1.4, 1.2, 1.3, 1.1, 1.4, 1.2, 1.3, 1.1, 1.4, 1.2, 1.3, 1.1,
                    1.4, 1.2, 1.3, 1.1, 1.4, 1.2, 1.3, 1.1, 1.4
                });
                using var bfeResult = bfeSample.Operator(problem, batchX);
                Assert.AreEqual(14, bfeResult.Count);
                Assert.AreEqual(problem.fitness(1.2, 1.3)[0], bfeResult[0]);
                Assert.AreEqual(problem.fitness(1.1, 1.4)[0], bfeResult[1]);
                Assert.That(problem.ThreadsExecuted.Count, Is.GreaterThan(0), "managed problem should record at least one executing thread");
                usedMultipleThreads = problem.ThreadsExecuted.Distinct().Count() > 1;
            }

            Assert.That(usedMultipleThreads, Is.True, $"thread_bfe should eventually use multiple threads across {maxAttempts} attempts");
        }

        [Test]
        public void TestThreadBfeRejectsManagedProblemWithThreadSafetyNone()
        {
            using var bfeSample = new pagmo.thread_bfe();
            using var problem = new OneDimensionalSimpleProblem();
            using var batchX = new DoubleVector(new[] { 1.2 });

            var ex = Assert.Throws<InvalidOperationException>(() => bfeSample.Operator(problem, batchX));
            Assert.That(ex!.Message, Does.Contain("thread_safety.basic or thread_safety.constant"));
        }

        [Test]
        public void DefaultBfeManagedFitnessExceptionBubblesWithoutTeardownCrash()
        {
            using var evaluator = new default_bfe();
            using var problem = new ThrowingFitnessProblem();
            using var batchX = new DoubleVector(new[] { 0.5 });

            var ex = Assert.Throws<InvalidOperationException>(() => evaluator.Operator(problem, batchX));
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex!.Message.ToLowerInvariant(), Does.Contain("managed fitness boom"));
        }

        [Test]
        [TestCaseSource(nameof(BuiltInBasicThreadSafeProblemFactories))]
        public void ThreadBfeExecutesBuiltInProblemsMarkedThreadSafe(Func<IProblem> problemFactory)
        {
            using var problem = problemFactory();
            Assert.That(problem.get_thread_safety(), Is.Not.EqualTo(thread_safety.none), "built-in problem should expose thread-safe metadata for thread_bfe path");

            using var bounds = problem.get_bounds();
            var dimension = bounds.first.Count;
            Assert.That(dimension, Is.GreaterThan(0), "problem dimension should be positive");

            var firstPoint = new double[dimension];
            var secondPoint = new double[dimension];
            for (var i = 0; i < dimension; i++)
            {
                var lo = bounds.first[i];
                var hi = bounds.second[i];
                firstPoint[i] = lo;
                secondPoint[i] = lo + (hi - lo) * 0.5;
            }

            var flattened = new double[dimension * 2];
            Array.Copy(firstPoint, 0, flattened, 0, dimension);
            Array.Copy(secondPoint, 0, flattened, dimension, dimension);

            using var batchX = new DoubleVector(flattened);
            using var evaluator = new pagmo.thread_bfe();
            using var fitness = evaluator.Operator(problem, batchX);
            var outputsPerDecisionVector = (int)(problem.get_nobj() + problem.get_nec() + problem.get_nic());
            Assert.That(outputsPerDecisionVector, Is.GreaterThan(0));
            Assert.That(fitness.Count, Is.EqualTo(2 * outputsPerDecisionVector), "thread_bfe should return one full fitness vector per decision vector");
        }
    }
}
