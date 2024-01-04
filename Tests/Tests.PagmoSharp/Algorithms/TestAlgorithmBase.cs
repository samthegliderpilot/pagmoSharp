using System;
using System.Runtime.ExceptionServices;
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
        public abstract bool Constrained { get; }
        public abstract bool Unconstrained { get; }
        public abstract bool SingleObjective { get; }
        public abstract bool MultiObjective { get; }
        public abstract bool IntegerPrograming { get; }
        public abstract bool Stochastic { get; }

        public population EvolveAlgorithm(IAlgorithm algorithm, population population)
        {
            return algorithm.evolve(population);
        }

        [Test]
        public virtual void TestOneDimensionalProblem()
        {
            if (!SingleObjective || !Unconstrained)
            {
                Assert.Ignore();
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
        public virtual void TestTwoDimensionalProblem()
        {
            if (!SingleObjective || !Unconstrained)
            {
                Assert.Ignore();
                return;
            }

            using (var problem = new TwoDimensionalSingleObjectiveProblemWrapper())
            using (var algorithm = CreateAlgorithm(problem))
            using (var pop = new population(problem, 512))
            {
                var finalpop = EvolveAlgorithm(algorithm, pop);
                Assert.AreEqual(problem.ExpectedOptimalFunctionValue, finalpop.champion_f()[0], 2);
                Assert.AreEqual(problem.ExpectedOptimalX[0], finalpop.champion_x()[0], 1);
            }
        }

        [Test]
        public virtual void TestProblemWithConstraints()
        {
            if (!Constrained || !SingleObjective)
            {
                Assert.Ignore();
                return; // pass, unsupported
            }

            using (var problem = new TwoDimensionalConstrainedProblem())
            using (var algorithm = CreateAlgorithm(problem))
            using (var pop = new population(problem, 1024))
            {
                algorithm.set_seed(2); // for consistent results
                var finalpop = EvolveAlgorithm(algorithm, pop);
                Assert.AreEqual(problem.ExpectedOptimalX[0], finalpop.champion_x()[0], 0.3, "x for opt");
                Assert.AreEqual(problem.ExpectedOptimalX[1], finalpop.champion_x()[1], 0.3, "y for opt");

                Assert.AreEqual(problem.ExpectedOptimalFunctionValue, finalpop.champion_f()[0], 0.3, "opt value");
            }
        }

        [Test]
        public virtual void TestBasicFunctions()
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
        public void TestGetLog()
        {
            using (var problem = new TwoDimensionalSingleObjectiveProblemWrapper())
            using (var algorithm = CreateAlgorithm(problem))
            {
                Assert.IsNotNull(((dynamic)algorithm).get_log());
            }
        }

        [Test]
        public void TestGetGen()
        {
            if (!SupportsGeneration)
            {
                return;
            }

            using (var problem = new TwoDimensionalSingleObjectiveProblemWrapper())
            using (var algorithm = CreateAlgorithm(problem))
            {
                Assert.AreNotEqual(0, ((dynamic)algorithm).get_gen());
            }
        }

        [Test]
        public void TestStocasticProblem()
        {
            if (!Stochastic)
            {
                Assert.Ignore();
                return;
            }
            using (var problem = new InventoryProblemWrapper())
            using (var algorithm = CreateAlgorithm(problem))
            using (var pop = new population(problem, 4096, 2u))
            {
                //algorithm.set_seed(2); // for consistent results
                var finalpop = EvolveAlgorithm(algorithm, pop);
                Assert.AreEqual(problem.ExpectedOptimalX[0], finalpop.champion_x()[0], 10, "x for opt");
                Assert.AreEqual(problem.ExpectedOptimalFunctionValue, finalpop.champion_f()[0], 10, "opt value");
            }
        }

        public virtual bool SupportsGeneration
        {
            get { return true; }
        }

        [Test]
        public virtual void TestIntegerProgrammingWithConstraints()
        {
            if (!IntegerPrograming || !Constrained)
            {
                Assert.Ignore();
                return; // pass, unsupported
            }
            //TODO: Need a unconstrained version of this test method

            using var problemBase = new golomb_ruler();
            var problemBase2 = new ProblemWrapper(problemBase);
            using (var algorithm = CreateAlgorithm())
            using (var pop = new population(problemBase2, 1024))
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

        [Test]
        public virtual void TestIntegerProgrammingWithUnConstraints()
        {
            if (!IntegerPrograming || !Unconstrained)
            {
                Assert.Ignore();
                return; // pass, unsupported
            }
            //TODO: Need a unconstrained version of this test method

            using var problemBase = new minlp_rastrigin(2, 2);
            var problemBase2 = new ProblemWrapper(problemBase);
            using (var algorithm = CreateAlgorithm())
            using (var pop = new population(problemBase2, 10000))
            {
                algorithm.set_seed(2); // for consistent results

                var finalpop = algorithm.evolve(pop);
                var champX = finalpop.champion_x();
                var champF = finalpop.champion_f();
                Assert.AreEqual(4, champX.Count, "3 in x");
                Assert.AreEqual(0.018910061654866972 , champX[0], 0.03, "first champ x");
                Assert.AreEqual(-0.00067151048252811485, champX[1], 0.03, "second champ x");
                Assert.AreEqual(-5, champX[2], "third champ x");
                Assert.AreEqual(-5, champX[3], "fourth champ x");

                Assert.AreEqual(1, champF.Count, "2 in f(x)");
                Assert.AreEqual(50.017521245106849, champF[0], 0.1, "first value of champ f");
            }
        }

        [Test]
        public void TestMultiobjectiveUnconstrained()
        {
            if (!MultiObjective || !Unconstrained)
            {
                Assert.Ignore();
                return; // pass, unsupported
            }
            using var problemBase = new zdt(1);
            var problemBase2 = new ProblemWrapper(problemBase);
            using (var algorithm = CreateAlgorithm())
            using (var pop = new population(problemBase2, 512))
            {
                algorithm.set_seed(2); // for consistent results

                var finalpop = algorithm.evolve(pop);
                //TODO: Multivariable functions don't have championions, need to refactor test!
                var champX = finalpop.champion_x();
                var champF = finalpop.champion_f();
                Assert.AreEqual(4, champX.Count, "3 in x");
                Assert.AreEqual(0.018910061654866972, champX[0], 0.03, "first champ x");
                Assert.AreEqual(-0.00067151048252811485, champX[1], 0.03, "second champ x");
                Assert.AreEqual(-5, champX[2], "third champ x");
                Assert.AreEqual(-5, champX[3], "fourth champ x");

                Assert.AreEqual(1, champF.Count, "2 in f(x)");
                Assert.AreEqual(50.017521245106849, champF[0], 0.1, "first value of champ f");
            }
        }
    }
}