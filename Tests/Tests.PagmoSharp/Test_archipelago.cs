using System;
using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp
{
    [TestFixture]
    public class Test_archipelago
    {
        private static void AssertArchipelagoIslandConfiguration(
            archipelago archi,
            uint islandIndex,
            uint expectedPopulationSize,
            string expectedAlgorithmName = "ABC: Artificial Bee Colony",
            uint? expectedObjectiveCount = 1u,
            bool assertChampionShape = true)
        {
            using var islandSnapshot = archi.GetIslandCopy(islandIndex);
            using var configuredAlgorithm = islandSnapshot.get_algorithm();
            Assert.AreEqual(expectedAlgorithmName, configuredAlgorithm.get_name());

            using var population = islandSnapshot.get_population();
            Assert.AreEqual(expectedPopulationSize, population.size());

            using var problem = population.get_problem();
            if (expectedObjectiveCount.HasValue)
            {
                Assert.AreEqual(expectedObjectiveCount.Value, problem.get_nobj());
            }

            if (assertChampionShape)
            {
                using var championDecisionVector = population.champion_x();
                using var championFitnessVector = population.champion_f();
                Assert.AreEqual(2, championDecisionVector.Count);
                Assert.AreEqual(1, championFitnessVector.Count);
            }
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
        public void PushBackIslandRejectsManagedProblemWithThreadSafetyNone()
        {
            using var archi = new archipelago();
            using IAlgorithm algo = new bee_colony();
            using var problem = new OneDimensionalSimpleProblem();

            var ex = Assert.Throws<InvalidOperationException>(
                () => archi.push_back_island(algo, problem, 8, 2));
            Assert.That(ex!.Message, Does.Contain("thread_safety.basic or thread_safety.constant"));
        }

        [Test]
        public void ManagedThreadSafeProblemCanEvolveInArchipelago()
        {
            using var archi = new archipelago();
            using IAlgorithm algo = new bee_colony();
            using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();

            archi.push_back_island(algo, problem, 32, 2);
            Assert.AreEqual(1u, archi.size());
            AssertArchipelagoIslandConfiguration(archi, 0, 32);

            archi.evolve(1);
            archi.wait_check();
            Assert.AreEqual(evolve_status.idle, archi.status());
            AssertArchipelagoIslandConfiguration(archi, 0, 32);

            var db = archi.MigrantsDb;
            Assert.IsNotNull(db);
            var log = archi.MigrationLog;
            Assert.IsNotNull(log);
        }

        [Test]
        public void ManagedThreadSafeProblemCanEvolveInArchipelagoWithIhs()
        {
            using var archi = new archipelago();
            using IAlgorithm algo = new ihs(12u);
            using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();

            var expectedName = algo.get_name();
            var expectedObjectiveCount = problem.get_nobj();
            archi.push_back_island(algo, problem, 32, 2);
            Assert.AreEqual(1u, archi.size());
            AssertArchipelagoIslandConfiguration(archi, 0, 32, expectedName, expectedObjectiveCount, false);

            archi.evolve(1);
            archi.wait_check();
            Assert.AreEqual(evolve_status.idle, archi.status());
            AssertArchipelagoIslandConfiguration(archi, 0, 32, expectedName, expectedObjectiveCount, false);
        }

        [Test]
        public void ManagedMultiObjectiveProblemCanEvolveInArchipelagoWithNsga2()
        {
            using var archi = new archipelago();
            using IAlgorithm algo = new nsga2(8u);
            using var problem = new TwoDimensionalMultiObjectiveProblemWrapper();

            var expectedName = algo.get_name();
            var expectedObjectiveCount = problem.get_nobj();
            archi.push_back_island(algo, problem, 32, 2);
            Assert.AreEqual(1u, archi.size());
            AssertArchipelagoIslandConfiguration(archi, 0, 32, expectedName, expectedObjectiveCount, false);

            archi.evolve(1);
            archi.wait_check();
            Assert.AreEqual(evolve_status.idle, archi.status());
            AssertArchipelagoIslandConfiguration(archi, 0, 32, expectedName, expectedObjectiveCount, false);
        }

        [Test]
        public void ManagedConstrainedProblemCanEvolveInArchipelagoWithSelfAdaptiveConstraints()
        {
            using var archi = new archipelago();
            using IAlgorithm algo = new cstrs_self_adaptive(5u);
            using var problem = new TwoDimensionalConstrainedProblem();

            var expectedName = algo.get_name();
            archi.push_back_island(algo, problem, 32, 2);
            Assert.AreEqual(1u, archi.size());
            AssertArchipelagoIslandConfiguration(archi, 0, 32, expectedName, null, false);

            archi.evolve(1);
            archi.wait_check();
            Assert.AreEqual(evolve_status.idle, archi.status());
            AssertArchipelagoIslandConfiguration(archi, 0, 32, expectedName, null, false);
        }

        [Test]
        public void ManagedThreadSafeProblemCanEvolveInArchipelagoWithBfeAndThreadIsland()
        {
            using var archi = new archipelago();
            using IAlgorithm algo = new bee_colony();
            using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var bfe = new default_bfe();
            using var threadIsland = new thread_island();
            using var replacementPolicy = new fair_replace();
            using var selectionPolicy = new select_best();

            archi.push_back_island(threadIsland, algo, problem, bfe, 24, replacementPolicy, selectionPolicy, 2);
            Assert.AreEqual(1u, archi.size());
            AssertArchipelagoIslandConfiguration(archi, 0, 24);

            archi.evolve(1);
            archi.wait_check();
            Assert.AreEqual(evolve_status.idle, archi.status());
            AssertArchipelagoIslandConfiguration(archi, 0, 24);
        }

        [Test]
        public void PushBackIslandRejectsUnsupportedManagedAlgorithmType()
        {
            using var archi = new archipelago();
            using IAlgorithm algo = new UnsupportedAlgorithm();
            using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();

            var ex = Assert.Throws<NotSupportedException>(() => archi.push_back_island(algo, problem, 8, 2));
            Assert.That(ex!.Message, Does.Contain("UnsupportedAlgorithm"));
        }

        [Test]
        public void MigrationAndTopologyControlsCanBeSet()
        {
            using var archi = new archipelago();
            using var ringTopo = new ring(4, 0.7);

            archi.set_migration_type(migration_type.broadcast);
            archi.set_migrant_handling(migrant_handling.evict);
            archi.set_topology_ring(ringTopo);

            Assert.AreEqual(migration_type.broadcast, archi.get_migration_type());
            Assert.AreEqual(migrant_handling.evict, archi.get_migrant_handling());
            Assert.AreEqual("Ring", archi.get_topology_name());
        }
    }
}
