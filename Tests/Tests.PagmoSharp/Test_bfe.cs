using System;
using System.Linq;
using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp
{
    [TestFixture]
    public class Test_bfe
    {
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
    }
}
