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
            Assert.IsNotNull(isl.get_name());
            Assert.IsNotNull(isl.get_extra_info());

            using var configuredAlgorithm = isl.get_algorithm();
            Assert.AreEqual("ABC: Artificial Bee Colony", configuredAlgorithm.get_name());

            using var population = isl.get_population();
            Assert.AreEqual(expectedPopulationSize, population.size());

            using var championDecisionVector = population.champion_x();
            using var championFitnessVector = population.champion_f();
            Assert.AreEqual(2, championDecisionVector.Count, "Expected 2D decision vector.");
            Assert.AreEqual(1, championFitnessVector.Count, "Expected single-objective fitness.");
        }

        private sealed class UnsupportedAlgorithm : IAlgorithm
        {
            public population evolve(population pop) => pop;
            public void set_seed(uint seed) { }
            public uint get_seed() => 0;
            public uint get_verbosity() => 0;
            public void set_verbosity(uint level) { }
            public string get_name() => "UnsupportedAlgorithm";
            public string get_extra_info() => string.Empty;
            public void Dispose() { }
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
            using var evaluator = new default_bfe();
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
            using var evaluator = new default_bfe();
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
        public void IslandCreateRejectsUnsupportedManagedAlgorithmType()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using IAlgorithm algo = new UnsupportedAlgorithm();

            var ex = Assert.Throws<NotSupportedException>(() => island.Create(algo, managed, 24, 2));
            Assert.That(ex!.Message, Does.Contain("UnsupportedAlgorithm"));
        }
    }
}
