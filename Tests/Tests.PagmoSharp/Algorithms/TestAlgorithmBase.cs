using System;
using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp.Algorithms
{
    public abstract class TestAlgorithmBase
    {
        public abstract IAlgorithm CreateAlgorithm();

        public virtual IAlgorithm CreateAlgorithm(TestProblemWrapper testProblem)
        {
            return CreateAlgorithm();
        }

        public abstract void TestNameIsCorrect();
        public abstract bool Constraints { get; }
        public abstract bool Unconstraned { get; }
        public abstract bool SingleObjective { get; }
        public abstract bool MultiObjective { get; }
        public abstract bool IntegerPrograming { get; }
        public abstract bool Stochastic { get; }

        public population EvolveAlgorithm(IAlgorithm algorithm, population population)
        {
            return algorithm.evolve(population);
        }

        [Test]
        public void TestOneDimensionalProblem()
        {
            if (!SingleObjective || !Unconstraned)
            {
                Assert.Pass();
                return;
            }

            using (var problem = new OneDimensionalSimpleProblem())
            using (var algorithm = CreateAlgorithm(problem))
            using (var pop = new population(problem, 64))
            {
                var finalpop = EvolveAlgorithm(algorithm, pop);
                Assert.AreEqual(problem.ExpectedOptimalFunctionValue, finalpop.champion_f()[0], 2);
                Assert.AreEqual(problem.ExpectedOptimalX[0], finalpop.champion_x()[0], 2);
            }
        }

        [Test]
        public void TestTwoDimensionalProblem()
        {
            if (!SingleObjective || !Unconstraned)
            {
                Assert.Pass();
                return;
            }

            using (var problem = new TwoDimensionalSingleObjectiveProblemWrapper())
            using (var algorithm = CreateAlgorithm(problem))
            using (var pop = new population(problem, 128))
            {
                var finalpop = EvolveAlgorithm(algorithm, pop);
                Assert.AreEqual(problem.ExpectedOptimalFunctionValue, finalpop.champion_f()[0], 2);
                Assert.AreEqual(problem.ExpectedOptimalX[0], finalpop.champion_x()[0], 1);
            }
        }

        [Test]
        public void TestProblemWithConstraints()
        {
            if (!Constraints || !SingleObjective)
            {
                Assert.Pass();
                return; // pass, unsupported
            }

            using (var problem = new TwoDimensionalConstrainedProblem())
            using (var algorithm = CreateAlgorithm(problem))
            using (var pop = new population(problem, 128))
            {
                algorithm.set_seed(2); // for consistent results
                var finalpop = EvolveAlgorithm(algorithm, pop);
                Assert.AreEqual(problem.ExpectedOptimalX[0], finalpop.champion_x()[0], 0.3, "x for opt");
                Assert.AreEqual(problem.ExpectedOptimalX[1], finalpop.champion_x()[1], 0.3, "y for opt");

                Assert.AreEqual(problem.ExpectedOptimalFunctionValue, finalpop.champion_f()[0], 0.3, "opt value");

            }
        }

        [Test]
        public void TestBasicFunctions()
        {
            using (var problem = new TwoDimensionalSingleObjectiveProblemWrapper())
            using (var algorithm = CreateAlgorithm(problem))
            {
                Assert.NotNull(algorithm.get_extra_info(), "getting non-null extra info");
                Assert.NotNull(algorithm.get_name(), "getting non-null name");
                algorithm.set_seed(2);
                Assert.AreEqual(2u, algorithm.get_seed(), "getting set seed");
                Assert.AreEqual(0, algorithm.get_verbosity(), "getting original verbosity");
                algorithm.set_verbosity(2);
                Assert.AreEqual(2, algorithm.get_verbosity(), "getting set verbosity");
            }
        }

        [Test]
        public void TestIntegerProgramming()
        {
            if (!IntegerPrograming)
            {
                Assert.Pass();
                return; // pass, unsupported
            }

            using var problemBase = new golomb_ruler();
            var problemBase2 = new ProblemWrapper(problemBase);
            var problem = new problem(problemBase2);
            using (var algorithm = CreateAlgorithm())
            using (var pop = new population(problem, 1024))
            {
                algorithm.set_seed(2); // for consistent results

                var finalpop = algorithm.evolve(pop);
                var champX = finalpop.champion_x();
                var champF = finalpop.champion_f();
                Assert.AreEqual(2, champX.Count, "2 in x");
                Assert.IsTrue(champX.Contains(1.0), "1.0 for first x value");
                Assert.IsTrue(champX.Contains(2.0), "2.0 for second x value");

                Assert.AreEqual(2, champF.Count, "2 in f(x)");
                Assert.IsTrue(champF.Contains(3.0), "3.0 for first f(x) value");
                Assert.IsTrue(champF.Contains(0.0), "0.0 for second f(x) value");
            }
        }
    }
}