using System;
using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp
{
    [TestFixture]
    public class Test_island
    {
        private static void AssertIslandIsConfiguredForBeeColonyAndTwoDimensionalProblem(island isl, uint expectedPopulationSize)
        {
            Assert.IsTrue(isl.is_valid());
            Assert.That(isl.get_name(), Is.Not.Empty);
            Assert.That(isl.get_extra_info(), Is.Not.Null);

            using var configuredAlgorithm = isl.get_algorithm();
            Assert.AreEqual("ABC: Artificial Bee Colony", configuredAlgorithm.get_name());

            using var population = isl.get_population();
            Assert.AreEqual(expectedPopulationSize, population.size());

            using var championDecisionVector = population.champion_x();
            using var championFitnessVector = population.champion_f();
            Assert.AreEqual(2, championDecisionVector.Count, "Expected 2D decision vector.");
            Assert.AreEqual(1, championFitnessVector.Count, "Expected single-objective fitness.");
        }

        // Managed custom algorithm used to validate callback-bridge type-erased execution.
        private sealed class ThrowingAlgorithm : IAlgorithm
        {
            public population evolve(population pop) => throw new InvalidOperationException("Managed algorithm failure from ThrowingAlgorithm.");
            public void set_seed(uint seed) { }
            public uint get_seed() => 0;
            public uint get_verbosity() => 0;
            public void set_verbosity(uint level) { }
            public string get_name() => "ThrowingAlgorithm";
            public string get_extra_info() => string.Empty;
            public void Dispose() { }
        }

        private sealed class MinimalManagedProblem : ManagedProblemBase
        {
            public override PairOfDoubleVectors get_bounds() => Bounds(new[] { -1.0, -1.0 }, new[] { 1.0, 1.0 });
            public override DoubleVector fitness(DoubleVector decisionVector) => new(new[] { decisionVector[0] * decisionVector[0] + decisionVector[1] * decisionVector[1] });
            public override thread_safety get_thread_safety() => thread_safety.constant;
            public override string get_name() => "MinimalManagedProblem";
        }

        [Test]
        public void Diagnostic_ManagedIsland_CreateOnly()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using IAlgorithm algo = new bee_colony();
            using var isl = island.Create(algo, managed, 32, 2);
            Assert.That(isl.is_valid(), Is.True);
        }

        [Test]
        public void Diagnostic_ManagedIsland_GetAlgorithmName()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using IAlgorithm algo = new bee_colony();
            using var isl = island.Create(algo, managed, 32, 2);
            TestContext.Progress.WriteLine("Before get_algorithm()");
            var configuredAlgorithm = isl.get_algorithm();
            TestContext.Progress.WriteLine("After get_algorithm()");
            var name = configuredAlgorithm.get_name();
            TestContext.Progress.WriteLine("After get_name()");
            Assert.That(name, Is.EqualTo("ABC: Artificial Bee Colony"));
            configuredAlgorithm.Dispose();
            TestContext.Progress.WriteLine("After configuredAlgorithm.Dispose()");
        }

        [Test]
        public void Diagnostic_ManagedIsland_GetAlgorithmName_WhenTypeErasedAlgorithmProvided()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var algo = new bee_colony().to_algorithm();
            using var isl = island.Create(algo, managed, 32, 2);
            using var configuredAlgorithm = isl.get_algorithm();
            Assert.That(configuredAlgorithm.get_name(), Is.EqualTo("ABC: Artificial Bee Colony"));
        }

        [Test]
        public void Diagnostic_ManagedIsland_GetAlgorithmName_WhenManagedAlgorithmProvided()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using IAlgorithm algo = new ThrowingAlgorithm();
            using var isl = island.Create(algo, managed, 32, 2);
            using var configuredAlgorithm = isl.get_algorithm();
            Assert.That(configuredAlgorithm.get_name(), Is.EqualTo("ThrowingAlgorithm"));
        }

        [Test]
        public void Diagnostic_ManagedIsland_GetAlgorithmName_WithMinimalManagedProblem()
        {
            using var managed = new MinimalManagedProblem();
            using IAlgorithm algo = new bee_colony();
            using var isl = island.Create(algo, managed, 32, 2);
            using var configuredAlgorithm = isl.get_algorithm();
            Assert.That(configuredAlgorithm.get_name(), Is.EqualTo("ABC: Artificial Bee Colony"));
        }

        [Test]
        public void Diagnostic_ManagedProblemWrapper_DoesNotCorruptAlgorithmGetName()
        {
            using var managed = new MinimalManagedProblem();
            TestContext.Progress.WriteLine("Before new problem(managed)");
            using var wrapped = new problem(managed);
            TestContext.Progress.WriteLine("After new problem(managed)");
            using var algo = new bee_colony().to_algorithm();
            TestContext.Progress.WriteLine("After to_algorithm()");
            var algoName = algo.get_name();
            TestContext.Progress.WriteLine($"After algo.get_name(): {algoName}");
            var wrappedName = wrapped.get_name();
            TestContext.Progress.WriteLine($"After wrapped.get_name(): {wrappedName}");
            Assert.That(algoName, Is.EqualTo("ABC: Artificial Bee Colony"));
            Assert.That(wrappedName, Is.EqualTo("MinimalManagedProblem"));
        }

        [Test]
        public void Diagnostic_ManagedIsland_GetPopulationSize()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using IAlgorithm algo = new bee_colony();
            using var isl = island.Create(algo, managed, 32, 2);
            using var population = isl.get_population();
            Assert.That(population.size(), Is.EqualTo(32u));
        }

        [Test]
        public void Diagnostic_ManagedIsland_ChampionAccess()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using IAlgorithm algo = new bee_colony();
            using var isl = island.Create(algo, managed, 32, 2);
            using var population = isl.get_population();
            using var championDecisionVector = population.champion_x();
            using var championFitnessVector = population.champion_f();
            Assert.That(championDecisionVector.Count, Is.EqualTo(2));
            Assert.That(championFitnessVector.Count, Is.EqualTo(1));
        }

        [Test]
        public void IslandCanBeCreatedFromManagedProblemAndEvolved()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using IAlgorithm algo = new bee_colony();
            using var isl = island.Create(algo, managed, 32, 2);

            AssertIslandIsConfiguredForBeeColonyAndTwoDimensionalProblem(isl, 32);

            isl.evolve(1);
            isl.wait_check();
            Assert.AreEqual(evolve_status.idle, isl.status());
            AssertIslandIsConfiguredForBeeColonyAndTwoDimensionalProblem(isl, 32);
        }

        [Test]
        public void IslandCanBeCreatedWithPoliciesFromManagedProblem()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using IAlgorithm algo = new bee_colony();
            using var r = new fair_replace();
            using var s = new select_best();
            using var isl = island.CreateWithPolicies(algo, managed, 24, r, s, 2);

            AssertIslandIsConfiguredForBeeColonyAndTwoDimensionalProblem(isl, 24);
            isl.evolve();
            isl.wait_check();
            Assert.AreEqual(evolve_status.idle, isl.status());
            AssertIslandIsConfiguredForBeeColonyAndTwoDimensionalProblem(isl, 24);
        }

        [Test]
        public void IslandCanBeCreatedWithBfeFromManagedProblem()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using IAlgorithm algo = new bee_colony();
            using var evaluator = new default_bfe().to_bfe();
            using var isl = island.CreateWithBfe(algo, managed, evaluator, 24, 2);

            AssertIslandIsConfiguredForBeeColonyAndTwoDimensionalProblem(isl, 24);
            isl.evolve();
            isl.wait_check();
            Assert.AreEqual(evolve_status.idle, isl.status());
            AssertIslandIsConfiguredForBeeColonyAndTwoDimensionalProblem(isl, 24);
        }

        [Test]
        public void IslandCanBeCreatedWithBfeAndPoliciesFromManagedProblem()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using IAlgorithm algo = new bee_colony();
            using var evaluator = new default_bfe().to_bfe();
            using var replacementPolicy = new fair_replace();
            using var selectionPolicy = new select_best();
            using var isl = island.CreateWithBfeAndPolicies(algo, managed, evaluator, 24, replacementPolicy, selectionPolicy, 2);

            AssertIslandIsConfiguredForBeeColonyAndTwoDimensionalProblem(isl, 24);
            isl.evolve();
            isl.wait_check();
            Assert.AreEqual(evolve_status.idle, isl.status());
            AssertIslandIsConfiguredForBeeColonyAndTwoDimensionalProblem(isl, 24);
        }

        [Test]
        public void IslandWaitCheckBubblesManagedAlgorithmCallbackException()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using IAlgorithm algo = new ThrowingAlgorithm();
            using var isl = island.Create(algo, managed, 24, 2);

            isl.evolve(1u);
            var ex = Assert.Throws<ApplicationException>(() => isl.wait_check());
            Assert.That(ex!.Message, Does.Contain("managed algorithm evolve"));
            Assert.That(ex!.Message, Does.Contain("ThrowingAlgorithm"));
        }
    }
}
