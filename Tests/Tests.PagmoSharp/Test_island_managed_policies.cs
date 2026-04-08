using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp
{
    [TestFixture]
    public class Test_island_managed_policies
    {
        // Managed problem fixture that supports batch_fitness so member_bfe and policy
        // wrappers can be validated with island creation paths.
        private sealed class ManagedBatchProblem : ManagedProblemBase
        {
            public override thread_safety get_thread_safety() => thread_safety.basic;

            public override PairOfDoubleVectors get_bounds()
            {
                return new PairOfDoubleVectors(new DoubleVector(new[] { -5.0, -5.0 }), new DoubleVector(new[] { 5.0, 5.0 }));
            }

            public override DoubleVector fitness(DoubleVector x)
            {
                var x0 = x[0];
                var x1 = x[1];
                return new DoubleVector(new[] { x0 * x0 + x1 * x1 });
            }

            public override bool has_batch_fitness() => true;

            public override DoubleVector batch_fitness(DoubleVector dvs)
            {
                var result = new DoubleVector();
                for (var i = 0; i < dvs.Count; i += 2)
                {
                    var x0 = dvs[i];
                    var x1 = dvs[i + 1];
                    result.Add(x0 * x0 + x1 * x1);
                }

                return result;
            }
        }

        // Managed replacement policy stub used to validate direct r_policyBase support
        // in island helper overloads and ownership transfer behavior.
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

        // Managed selection policy stub paired with the replacement policy for island
        // policy callback wiring and lifecycle coverage.
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

        [Test]
        public void IslandCanBeCreatedDirectlyFromManagedPolicyBases()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var algo = new bee_colony().to_algorithm();
            using var rBase = new ManagedReplacementPolicy();
            using var sBase = new ManagedSelectionPolicy();

            using var isl = island.CreateWithPolicies(algo, managed, 24, rBase, sBase, 2);

            Assert.IsTrue(isl.is_valid());
            isl.evolve();
            isl.wait_check();
            Assert.AreEqual(evolve_status.idle, isl.status());
        }

        [Test]
        public void IslandCanBeCreatedWithBfeDirectlyFromManagedPolicyBases()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var algo = new bee_colony().to_algorithm();
            using var evaluator = new default_bfe().to_bfe();
            using var rBase = new ManagedReplacementPolicy();
            using var sBase = new ManagedSelectionPolicy();

            using var isl = island.CreateWithBfeAndPolicies(algo, managed, evaluator, 24, rBase, sBase, 2);

            Assert.IsTrue(isl.is_valid());
            isl.evolve();
            isl.wait_check();
            Assert.AreEqual(evolve_status.idle, isl.status());
        }

        [Test]
        public void ThreadIslandCanBeCreatedWithBfeDirectlyFromManagedPolicyBases()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var algo = new bee_colony().to_algorithm();
            using var evaluator = new default_bfe().to_bfe();
            using var threadIsland = new thread_island();
            using var rBase = new ManagedReplacementPolicy();
            using var sBase = new ManagedSelectionPolicy();

            using var isl = island.CreateWithThreadIslandAndBfeAndPolicies(threadIsland, algo, managed, evaluator, 24, rBase, sBase, 2);

            Assert.IsTrue(isl.is_valid());
            isl.evolve();
            isl.wait_check();
            Assert.AreEqual(evolve_status.idle, isl.status());
        }

        [Test]
        public void IslandCanBeCreatedWithDefaultBfeDirectlyFromManagedPolicyBases()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var algo = new bee_colony().to_algorithm();
            using var evaluator = new default_bfe().to_bfe();
            using var rBase = new ManagedReplacementPolicy();
            using var sBase = new ManagedSelectionPolicy();

            using var isl = island.CreateWithBfeAndPolicies(algo, managed, evaluator, 24, rBase, sBase, 2);

            Assert.IsTrue(isl.is_valid());
            isl.evolve();
            isl.wait_check();
            Assert.AreEqual(evolve_status.idle, isl.status());
        }

        [Test]
        public void IslandCanBeCreatedWithThreadBfeDirectlyFromManagedPolicyBases()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var algo = new bee_colony().to_algorithm();
            using var evaluator = new thread_bfe().to_bfe();
            using var rBase = new ManagedReplacementPolicy();
            using var sBase = new ManagedSelectionPolicy();

            using var isl = island.CreateWithBfeAndPolicies(algo, managed, evaluator, 24, rBase, sBase, 2);

            Assert.IsTrue(isl.is_valid());
            isl.evolve();
            isl.wait_check();
            Assert.AreEqual(evolve_status.idle, isl.status());
        }

        [Test]
        public void IslandCanBeCreatedWithMemberBfeDirectlyFromManagedPolicyBases()
        {
            using var managed = new ManagedBatchProblem();
            using var algo = new bee_colony().to_algorithm();
            using var evaluator = new member_bfe().to_bfe();
            using var rBase = new ManagedReplacementPolicy();
            using var sBase = new ManagedSelectionPolicy();

            using var isl = island.CreateWithBfeAndPolicies(algo, managed, evaluator, 24, rBase, sBase, 2);

            Assert.IsTrue(isl.is_valid());
            isl.evolve();
            isl.wait_check();
            Assert.AreEqual(evolve_status.idle, isl.status());
        }
    }
}
