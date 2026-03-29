using System;
using System.Reflection;
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
        public virtual bool MultiObjectiveUnconstrainedValid => MultiObjective && Unconstrained;
        public virtual bool MultiObjectiveConstrainedValid => MultiObjective && Constrained;

        public population EvolveAlgorithm(IAlgorithm algorithm, population population)
        {
            return algorithm.evolve(population);
        }

        private static void AssertChampionUnavailableForMultiObjectivePopulation(population finalPopulation)
        {
            var championFitnessException = Assert.Throws<ApplicationException>(() =>
            {
                using var _ = finalPopulation.champion_f();
            });
            Assert.That(championFitnessException, Is.Not.Null);
            Assert.That(championFitnessException!.Message.ToLowerInvariant(), Does.Contain("single objective"));

            var championDecisionException = Assert.Throws<ApplicationException>(() =>
            {
                using var _ = finalPopulation.champion_x();
            });
            Assert.That(championDecisionException, Is.Not.Null);
            Assert.That(championDecisionException!.Message.ToLowerInvariant(), Does.Contain("single objective"));
        }

        [Test]
        public virtual void TestOneDimensionalProblem()
        {
            if (!SingleObjective || !Unconstrained)
            {
                Assert.Pass("Not applicable: algorithm does not support single-objective unconstrained problems.");
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
                Assert.Pass("Not applicable: algorithm does not support single-objective unconstrained problems.");
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
                Assert.Pass("Not applicable: algorithm does not support constrained single-objective problems.");
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

                Assert.AreEqual(problem.ExpectedOptimalFunctionValue, finalpop.champion_f()[0], 0.5, "opt value");
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
            if (!SingleObjective && !Constrained && !MultiObjectiveUnconstrainedValid && !MultiObjectiveConstrainedValid)
            {
                Assert.Pass("Not applicable: no supported test problem shape for log validation.");
                return;
            }

            if (SingleObjective && Unconstrained)
            {
                using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
                using var algorithm = CreateAlgorithm(problem);
                AssertLogSurface(algorithm, problem, 48u);
                return;
            }

            if (SingleObjective && Constrained)
            {
                using var problem = new TwoDimensionalConstrainedProblem();
                using var algorithm = CreateAlgorithm(problem);
                AssertLogSurface(algorithm, problem, 48u);
                return;
            }

            if (MultiObjectiveUnconstrainedValid)
            {
                using var problem = new TwoDimensionalMultiObjectiveProblemWrapper();
                using var algorithm = CreateAlgorithm(problem);
                AssertLogSurface(algorithm, problem, 64u);
                return;
            }

            if (MultiObjectiveConstrainedValid)
            {
                using var problem = new TwoDimensionalMultiObjectiveConstrainedProblemWrapper();
                using var algorithm = CreateAlgorithm(problem);
                AssertLogSurface(algorithm, problem, 64u);
                return;
            }

            Assert.Pass("Not applicable: algorithm/problem combination unavailable for log validation.");
        }

        private static void AssertLogSurface(IAlgorithm algorithm, TestProblemWrapper managedProblem, uint populationSize)
        {
            var algorithmType = algorithm.GetType();
            var setVerbosity = algorithmType.GetMethod("set_verbosity", BindingFlags.Public | BindingFlags.Instance);
            setVerbosity?.Invoke(algorithm, new object[] { 1u });

            using var problem = new problem(managedProblem);
            using var population = new population(problem, populationSize, 2u);
            using var _ = algorithm.evolve(population);

            var logLines = algorithm.GetLogLines();
            Assert.That(logLines, Is.Not.Null, "universal GetLogLines() should always return a list");

            var hasNativeLogMethod =
                algorithmType.GetMethod("get_log_entries", BindingFlags.Public | BindingFlags.Instance) != null ||
                algorithmType.GetMethod("get_log_lines", BindingFlags.Public | BindingFlags.Instance) != null ||
                algorithmType.GetMethod("get_log", BindingFlags.Public | BindingFlags.Instance) != null;

            if (hasNativeLogMethod && setVerbosity != null)
            {
                Assert.That(logLines.Count, Is.GreaterThan(0), "verbosity-enabled logging should emit at least one line");
                Assert.That(logLines[0].RawFields.Count, Is.GreaterThan(0), "projected log line should expose raw fields");
                Assert.That(logLines[0].ToDisplayString(), Is.Not.Empty, "projected log line should expose display text");
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
                Assert.Pass("Not applicable: algorithm is not stochastic.");
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
                Assert.Pass("Not applicable: algorithm does not support constrained integer programming.");
                return; // pass, unsupported
            }
            //TODO: Need a unconstrained version of this test method

            using var problemBase = new golomb_ruler();
            using (var algorithm = CreateAlgorithm())
            using (var pop = new population(problemBase, 1024))
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
                Assert.Pass("Not applicable: algorithm does not support unconstrained integer programming.");
                return; // pass, unsupported
            }
            //TODO: Need a unconstrained version of this test method

            using var problemBase = new minlp_rastrigin(2, 2);
            using (var algorithm = CreateAlgorithm())
            using (var pop = new population(problemBase, 10000))
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
        public virtual void TestMultiobjectiveUnconstrained()
        {
            if (!MultiObjectiveUnconstrainedValid)
            {
                Assert.Pass("Not applicable: algorithm does not support unconstrained multi-objective problems.");
                return;
            }
            using var problemBase = new TwoDimensionalMultiObjectiveProblemWrapper();
            using (var algorithm = CreateAlgorithm())
            using (var pop = new population(problemBase, 128u, 2u))
            {
                algorithm.set_seed(2);

                var expectedObjectiveCount = problemBase.get_nobj();
                var expectedFitnessVectorCount = (int)(problemBase.get_nobj() + problemBase.get_nec() + problemBase.get_nic());
                var expectedPopulationSize = pop.size();

                var finalpop = algorithm.evolve(pop);
                Assert.AreEqual(expectedPopulationSize, finalpop.size(), "population size should be preserved");

                using var finalProblem = finalpop.get_problem();
                Assert.AreEqual(expectedObjectiveCount, finalProblem.get_nobj(), "objective count should be preserved");
                Assert.GreaterOrEqual(finalProblem.get_nobj(), 2u, "multi-objective problems must report 2+ objectives");

                using var allFitness = finalpop.get_f();
                using var allDecisionVectors = finalpop.get_x();
                Assert.AreEqual((int)expectedPopulationSize, allFitness.Count, "all individuals should have fitness values");
                Assert.AreEqual((int)expectedPopulationSize, allDecisionVectors.Count, "all individuals should have decision vectors");

                for (var individualIndex = 0; individualIndex < allFitness.Count; individualIndex++)
                {
                    Assert.AreEqual(expectedFitnessVectorCount, allFitness[individualIndex].Count, "fitness vector size should match objective+constraint counts");
                }

                AssertChampionUnavailableForMultiObjectivePopulation(finalpop);
            }
        }

        [Test]
        public virtual void TestMultiobjectiveConstrained()
        {
            if (!MultiObjectiveConstrainedValid)
            {
                Assert.Pass("Not applicable: algorithm does not support constrained multi-objective problems.");
                return;
            }

            using var problemBase = new TwoDimensionalMultiObjectiveConstrainedProblemWrapper();
            using var algorithm = CreateAlgorithm();
            using var pop = new population(problemBase, 128u, 2u);
            algorithm.set_seed(2);

            var expectedObjectiveCount = problemBase.get_nobj();
            var expectedFitnessVectorCount = (int)(problemBase.get_nobj() + problemBase.get_nec() + problemBase.get_nic());
            var expectedPopulationSize = pop.size();

            using var finalPopulation = algorithm.evolve(pop);
            Assert.AreEqual(expectedPopulationSize, finalPopulation.size(), "population size should be preserved");

            using var finalProblem = finalPopulation.get_problem();
            Assert.AreEqual(expectedObjectiveCount, finalProblem.get_nobj(), "objective count should be preserved");
            Assert.GreaterOrEqual(finalProblem.get_nobj(), 2u, "multi-objective problems must report 2+ objectives");
            Assert.Greater(finalProblem.get_nec() + finalProblem.get_nic(), 0u, "constrained test problem must contain constraints");

            using var allFitness = finalPopulation.get_f();
            Assert.AreEqual((int)expectedPopulationSize, allFitness.Count, "all individuals should have fitness values");
            for (var individualIndex = 0; individualIndex < allFitness.Count; individualIndex++)
            {
                Assert.AreEqual(expectedFitnessVectorCount, allFitness[individualIndex].Count, "fitness vector size should match objective+constraint counts");
            }

            AssertChampionUnavailableForMultiObjectivePopulation(finalPopulation);
        }
    }
}
