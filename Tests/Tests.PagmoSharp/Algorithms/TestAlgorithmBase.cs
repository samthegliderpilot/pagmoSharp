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
        public abstract bool IntegerProgramming { get; }
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
                using var finalpop = EvolveAlgorithm(algorithm, pop);
                using var championFitness = finalpop.champion_f();
                using var championDecisionVector = finalpop.champion_x();
                Assert.AreEqual(problem.ExpectedOptimalFunctionValue, championFitness[0], 2);
                Assert.AreEqual(problem.ExpectedOptimalX[0], championDecisionVector[0], 2);
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
                using var finalpop = EvolveAlgorithm(algorithm, pop);
                using var championFitness = finalpop.champion_f();
                using var championDecisionVector = finalpop.champion_x();
                Assert.AreEqual(problem.ExpectedOptimalFunctionValue, championFitness[0], 2);
                Assert.AreEqual(problem.ExpectedOptimalX[0], championDecisionVector[0], 1);
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
                using var finalpop = EvolveAlgorithm(algorithm, pop);
                using var championDecisionVector = finalpop.champion_x();
                using var championFitness = finalpop.champion_f();
                Assert.AreEqual(problem.ExpectedOptimalX[0], championDecisionVector[0], 0.3, "x for opt");
                Assert.AreEqual(problem.ExpectedOptimalX[1], championDecisionVector[1], 0.3, "y for opt");
                Assert.AreEqual(problem.ExpectedOptimalFunctionValue, championFitness[0], 0.5, "opt value");
            }
        }

        [Test]
        public virtual void TestBasicFunctions()
        {
            using (var problem = new TwoDimensionalSingleObjectiveProblemWrapper())
            using (var algorithm = CreateAlgorithm(problem))
            {
                Assert.That(algorithm.get_extra_info(), Is.Not.Null, "getting non-null extra info");
                Assert.That(algorithm.get_name(), Is.Not.Empty, "getting non-empty name");
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
        public void TestStochasticProblem()
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
                using var finalpop = EvolveAlgorithm(algorithm, pop);
                using var championDecisionVector = finalpop.champion_x();
                using var championFitness = finalpop.champion_f();
                Assert.AreEqual(problem.ExpectedOptimalX[0], championDecisionVector[0], 10, "x for opt");
                Assert.AreEqual(problem.ExpectedOptimalFunctionValue, championFitness[0], 10, "opt value");
            }
        }

        public virtual bool SupportsGeneration
        {
            get { return true; }
        }

        [Test]
        public virtual void TestIntegerProgrammingWithConstraints()
        {
            if (!IntegerProgramming || !Constrained)
            {
                Assert.Pass("Not applicable: algorithm does not support constrained integer programming.");
                return; // pass, unsupported
            }
            using var problemBase = new golomb_ruler();
            using (var algorithm = CreateAlgorithm())
            using (var pop = new population(problemBase, 1024))
            {
                algorithm.set_seed(2); // for consistent results

                using var finalpop = algorithm.evolve(pop);
                using var championDecisionVector = finalpop.champion_x();
                using var championFitness = finalpop.champion_f();
                Assert.AreEqual(2, championDecisionVector.Count, "champion decision vector should contain 2 values");
                Assert.IsTrue(championDecisionVector.Contains(1.0), "champion decision vector should include 1.0");
                Assert.IsTrue(championDecisionVector.Contains(2.0), "champion decision vector should include 2.0");

                Assert.AreEqual(2, championFitness.Count, "champion fitness should contain objective+constraint values");
                Assert.IsTrue(championFitness.Contains(3.0), "champion fitness should include objective value 3.0");
                Assert.IsTrue(championFitness.Contains(0.0), "champion fitness should include constraint value 0.0");
            }
        }

        [Test]
        public virtual void TestIntegerProgrammingWithUnconstrained()
        {
            if (!IntegerProgramming || !Unconstrained)
            {
                Assert.Pass("Not applicable: algorithm does not support unconstrained integer programming.");
                return; // pass, unsupported
            }
            using var problemBase = new minlp_rastrigin(2, 2);
            using (var algorithm = CreateAlgorithm())
            using (var pop = new population(problemBase, 10000))
            {
                algorithm.set_seed(2); // for consistent results

                using var finalpop = algorithm.evolve(pop);
                using var championDecisionVector = finalpop.champion_x();
                using var championFitness = finalpop.champion_f();
                using var bounds = problemBase.get_bounds();
                Assert.AreEqual(4, championDecisionVector.Count, "champion decision vector should contain 4 values");
                for (var i = 0; i < championDecisionVector.Count; i++)
                {
                    Assert.That(championDecisionVector[i], Is.GreaterThanOrEqualTo(bounds.first[i]), $"champion decision value {i} should be within lower bound");
                    Assert.That(championDecisionVector[i], Is.LessThanOrEqualTo(bounds.second[i]), $"champion decision value {i} should be within upper bound");
                }

                Assert.That(championDecisionVector[2], Is.EqualTo(System.Math.Round(championDecisionVector[2])).Within(1e-12), "first integer decision variable should be integral");
                Assert.That(championDecisionVector[3], Is.EqualTo(System.Math.Round(championDecisionVector[3])).Within(1e-12), "second integer decision variable should be integral");

                Assert.AreEqual(1, championFitness.Count, "champion fitness should contain 1 objective value");
                Assert.That(championFitness[0], Is.LessThanOrEqualTo(55.0), "champion objective should improve into the expected quality band");
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

                using var finalpop = algorithm.evolve(pop);
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

