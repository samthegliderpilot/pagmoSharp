using System;
using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp
{
    public abstract class TestAlgorithmBase
    {
        public abstract IAlgorithm CreateAlgorithm(TestProblemWrapper testProblem);

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
            if(!SingleObjective || !Unconstraned)
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
            if(!Constraints || !SingleObjective)
            {
                Assert.Pass();
                return; // pass, unsupported
            }

            using (var problem = new TwoDimensionalConstrainedProblem())
            using (var algorithm = CreateAlgorithm(problem))
            using (var pop = new population(problem, 128))
            {
                algorithm.set_seed(2); // for consistant results
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
    }
}