using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp
{
    [TestFixture]
    public class Test_island
    {
        [Test]
        public void IslandCanBeCreatedFromManagedProblemAndEvolved()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var algo = new bee_colony().to_algorithm();
            using var isl = island.Create(algo, managed, 32, 2);

            Assert.IsTrue(isl.is_valid());
            Assert.IsNotNull(isl.get_name());
            Assert.IsNotNull(isl.get_extra_info());

            isl.evolve(1);
            isl.wait_check();
            Assert.AreEqual(evolve_status.idle, isl.status());

            using var pop = isl.get_population();
            Assert.Greater(pop.size(), 0u);
        }

        [Test]
        public void IslandCanBeCreatedWithPoliciesFromManagedProblem()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var algo = new bee_colony().to_algorithm();
            using var r = new fair_replace();
            using var s = new select_best();
            using var isl = island.CreateWithPolicies(algo, managed, 24, r, s, 2);

            Assert.IsTrue(isl.is_valid());
            isl.evolve();
            isl.wait_check();
            Assert.AreEqual(evolve_status.idle, isl.status());
        }
    }
}
