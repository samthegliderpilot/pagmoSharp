using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp
{
    [TestFixture]
    public class Test_threadIsland
    {
        private static void AssertThreadIslandConfiguration(island isl, uint expectedPopulationSize)
        {
            Assert.IsTrue(isl.is_valid());
            using var configuredAlgorithm = isl.get_algorithm();
            Assert.AreEqual("ABC: Artificial Bee Colony", configuredAlgorithm.get_name());

            using var population = isl.get_population();
            Assert.AreEqual(expectedPopulationSize, population.size());
            using var championDecisionVector = population.champion_x();
            using var championFitnessVector = population.champion_f();
            Assert.AreEqual(2, championDecisionVector.Count);
            Assert.AreEqual(1, championFitnessVector.Count);
        }

        private sealed class ManagedReplacementPolicy : r_policyBase
        {
            public override IndividualsGroup replace(IndividualsGroup migrants, uint numReplace, uint numIncoming, uint numOutgoing, uint islandCount, uint migrationRound, DoubleVector migrationData, IndividualsGroup current)
            {
                return migrants;
            }

            public override string get_name() => "ManagedReplacementPolicy";
            public override string get_extra_info() => "managed r_policyBase test";
            public override bool is_valid() => true;
        }

        private sealed class ManagedSelectionPolicy : s_policyBase
        {
            public override IndividualsGroup select(IndividualsGroup populationGroup, uint requested, uint populationSize, uint islandIndex, uint islandCount, uint migrationRound, DoubleVector migrationData)
            {
                return populationGroup;
            }

            public override string get_name() => "ManagedSelectionPolicy";
            public override string get_extra_info() => "managed s_policyBase test";
            public override bool is_valid() => true;
        }

        [Test]
        public void ThreadIslandBasicMethods()
        {
            using var islandImpl = new thread_island();
            Assert.AreEqual("Thread island", islandImpl.get_name());
            Assert.That(islandImpl.get_extra_info(), Is.Not.Null);
        }

        [Test]
        public void IslandCanBeCreatedWithExplicitThreadIslandAndManagedProblem()
        {
            using var islandImpl = new thread_island();
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using IAlgorithm algo = new bee_colony();
            using var isl = island.CreateWithThreadIsland(islandImpl, algo, managed, 24, 2);

            AssertThreadIslandConfiguration(isl, 24);
            isl.evolve();
            isl.wait_check();
            Assert.AreEqual(evolve_status.idle, isl.status());
            AssertThreadIslandConfiguration(isl, 24);
        }

        [Test]
        public void IslandCanBeCreatedWithExplicitThreadIslandAndManagedPolicies()
        {
            using var islandImpl = new thread_island();
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using IAlgorithm algo = new bee_colony();
            using var replacementPolicyBase = new ManagedReplacementPolicy();
            using var selectionPolicyBase = new ManagedSelectionPolicy();
            using var replacementPolicy = new r_policy(replacementPolicyBase);
            using var selectionPolicy = new s_policy(selectionPolicyBase);
            using var isl = island.CreateWithThreadIslandAndPolicies(islandImpl, algo, managed, 24, replacementPolicy, selectionPolicy, 2);

            AssertThreadIslandConfiguration(isl, 24);
            isl.evolve();
            isl.wait_check();
            Assert.AreEqual(evolve_status.idle, isl.status());
            AssertThreadIslandConfiguration(isl, 24);
        }

        [Test]
        public void IslandCanBeCreatedWithExplicitThreadIslandAndBfe()
        {
            using var islandImpl = new thread_island();
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using IAlgorithm algo = new bee_colony();
            using var evaluator = new default_bfe();
            using var isl = island.CreateWithThreadIslandAndBfe(islandImpl, algo, managed, evaluator, 24, 2);

            AssertThreadIslandConfiguration(isl, 24);
            isl.evolve();
            isl.wait_check();
            Assert.AreEqual(evolve_status.idle, isl.status());
            AssertThreadIslandConfiguration(isl, 24);
        }

        [Test]
        public void IslandCanBeCreatedWithExplicitThreadIslandBfeAndPolicies()
        {
            using var islandImpl = new thread_island();
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using IAlgorithm algo = new bee_colony();
            using var evaluator = new default_bfe();
            using var replacementPolicy = new fair_replace();
            using var selectionPolicy = new select_best();
            using var isl = island.CreateWithThreadIslandAndBfeAndPolicies(
                islandImpl, algo, managed, evaluator, 24, replacementPolicy, selectionPolicy, 2);

            AssertThreadIslandConfiguration(isl, 24);
            isl.evolve();
            isl.wait_check();
            Assert.AreEqual(evolve_status.idle, isl.status());
            AssertThreadIslandConfiguration(isl, 24);
        }

    }
}
