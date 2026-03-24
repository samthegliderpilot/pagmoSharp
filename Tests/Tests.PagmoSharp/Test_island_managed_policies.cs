using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp
{
    [TestFixture]
    public class Test_island_managed_policies
    {
        private sealed class ManagedReplacementPolicy : r_policyBase
        {
            public override IndividualsGroup replace(IndividualsGroup a, uint b, uint c, uint d, uint e, uint f, DoubleVector g, IndividualsGroup h)
            {
                return a;
            }

            public override string get_name() => "ManagedReplacementPolicy";
            public override string get_extra_info() => "managed r_policyBase test";
            public override bool is_valid() => true;
        }

        private sealed class ManagedSelectionPolicy : s_policyBase
        {
            public override IndividualsGroup select(IndividualsGroup a, uint b, uint c, uint d, uint e, uint f, DoubleVector g)
            {
                return a;
            }

            public override string get_name() => "ManagedSelectionPolicy";
            public override string get_extra_info() => "managed s_policyBase test";
            public override bool is_valid() => true;
        }

        [Test]
        public void IslandCanBeCreatedWithManagedPoliciesFromManagedProblem()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var algo = new bee_colony().to_algorithm();
            using var rBase = new ManagedReplacementPolicy();
            using var sBase = new ManagedSelectionPolicy();
            using var r = new r_policy(rBase);
            using var s = new s_policy(sBase);

            using var isl = island.CreateWithPolicies(algo, managed, 24, r, s, 2);

            Assert.IsTrue(isl.is_valid());
            isl.evolve();
            isl.wait_check();
            Assert.AreEqual(evolve_status.idle, isl.status());
        }
    }
}
