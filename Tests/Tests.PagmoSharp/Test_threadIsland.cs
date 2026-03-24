using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp
{
    [TestFixture]
    public class Test_threadIsland
    {
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
            Assert.IsNotNull(islandImpl.get_extra_info());
        }

        [Test]
        public void IslandCanBeCreatedWithExplicitThreadIslandAndManagedProblem()
        {
            using var islandImpl = new thread_island();
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var algo = new bee_colony().to_algorithm();
            using var isl = island.CreateWithThreadIsland(islandImpl, algo, managed, 24, 2);

            Assert.IsTrue(isl.is_valid());
            isl.evolve();
            isl.wait_check();
            Assert.AreEqual(evolve_status.idle, isl.status());
        }

        [Test]
        public void IslandCanBeCreatedWithExplicitThreadIslandAndManagedPolicies()
        {
            using var islandImpl = new thread_island();
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var algo = new bee_colony().to_algorithm();
            using var replacementPolicyBase = new ManagedReplacementPolicy();
            using var selectionPolicyBase = new ManagedSelectionPolicy();
            using var replacementPolicy = new r_policy(replacementPolicyBase);
            using var selectionPolicy = new s_policy(selectionPolicyBase);
            using var isl = island.CreateWithThreadIslandAndPolicies(islandImpl, algo, managed, 24, replacementPolicy, selectionPolicy, 2);

            Assert.IsTrue(isl.is_valid());
            isl.evolve();
            isl.wait_check();
            Assert.AreEqual(evolve_status.idle, isl.status());
        }

    }
}
