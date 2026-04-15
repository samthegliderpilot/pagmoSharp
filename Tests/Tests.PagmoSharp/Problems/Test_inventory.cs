using System;
using System.Collections.Generic;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Problems
{
    [TestFixture]
    public class Test_inventory : TestProblemBase
    {
        public override IProblem CreateStandardProblem(uint problemIndex = 0)
        {
            return new inventory(4u, 10u, 2u);
        }

        protected override IEnumerable<ProblemTestData> GetRegressionData()
        {
            return new List<ProblemTestData>()
            {
                new ProblemTestData("inventory", "SimpleTest", new double[] { 1,2,3,4, },
                    new double[] { 294.60824716582886d }),
            };
        }

        [Test]
        public override void TestBoilerPlate()
        {
            using var problem = CreateStandardProblem();
            Assert.AreEqual("Inventory problem", problem.get_name(), "name");
            Assert.AreEqual(0, problem.get_nec(), "equality constraint count");
            Assert.AreEqual(1, problem.get_nobj(), "objective count");
            Assert.AreEqual(0, problem.get_nix(), "integer count");
            Assert.AreEqual(thread_safety.basic, problem.get_thread_safety(), "thread safety");
            using var bounds = problem.get_bounds();
            Assert.AreEqual(4, bounds.first.Count, "first element is length 4");
            Assert.AreEqual(4, bounds.second.Count, "second element is length 4");
        }
        
        [Test]
        public override void TestOptimizing()
        {
            using var problemBase = CreateStandardProblem();
            using var algorithm = new sga(20u);
            using var pop = new population(problemBase, 1024);
            algorithm.set_seed(2); // for consistent results

            using var initialProblem = pop.get_problem();
            var initialFevals = initialProblem.get_fevals();
            var initialSize = pop.size();

            using var finalpop = algorithm.evolve(pop);
            using var evolvedProblem = finalpop.get_problem();
            Assert.Greater(evolvedProblem.get_fevals(), initialFevals, "evolution should trigger additional function evaluations");
            Assert.AreEqual(initialSize, finalpop.size(), "evolution should preserve population size");

            using var allDecisionVectors = finalpop.get_x();
            using var allFitnessVectors = finalpop.get_f();
            Assert.AreEqual((int)initialSize, allDecisionVectors.Count, "all individuals should expose decision vectors after evolution");
            Assert.AreEqual((int)initialSize, allFitnessVectors.Count, "all individuals should expose fitness values after evolution");
            Assert.AreEqual(4, allDecisionVectors[0].Count, "inventory decision vectors should keep 4 dimensions");
            Assert.AreEqual(1, allFitnessVectors[0].Count, "inventory fitness vectors should contain one objective");

            // Inventory is stochastic in pagmo, so champion extraction is intentionally unavailable.
            Assert.Throws<ApplicationException>(() =>
            {
                using var _ = finalpop.champion_f();
            });
            Assert.Throws<ApplicationException>(() =>
            {
                using var _ = finalpop.champion_x();
            });
        }
    }
}
