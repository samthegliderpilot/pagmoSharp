using System.Collections.Generic;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Problems
{
    [TestFixture]
    public class Test_golomb_ruler : TestProblemBase
    {
        public override IProblem CreateStandardProblem(uint problemIndex = 0)
        {
            return new golomb_ruler();
        }

        protected override IEnumerable<ProblemTestData> GetRegressionData()
        {
            return new List<ProblemTestData>()
            {
                new ProblemTestData("Golomb Ruler(order 3)", "SimpleTest", new double[] { 2, 3, 4, },
                    new double[] { 9, 0, }),
            };
        }

        [Test]
        public override void TestBoilerPlate()
        {
            using var problem = CreateStandardProblem();
            Assert.AreEqual("Golomb Ruler (order 3)", problem.get_name(), "name");
            Assert.AreEqual(1, problem.get_nec(), "equality constraint count");
            Assert.AreEqual(1, problem.get_nobj(), "objective count");
            Assert.AreEqual(2, problem.get_nix(), "integer count");
            Assert.AreEqual(thread_safety.basic, problem.get_thread_safety(), "thread safety");
            using var bounds = problem.get_bounds();
            Assert.AreEqual(1.0, bounds.first[0]);
        }
        

        [Test]
        public override void TestOptimizing()
        {
            using var problemBase = CreateStandardProblem();
            using var bounds = problemBase.get_bounds();
            Assert.AreEqual(1, bounds.first[0]);
            using var algorithm = new gaco(20);
            using (var pop = new population(problemBase, 1024))
            {
                algorithm.set_seed(2); // for consistent results
                using var initialProblem = pop.get_problem();
                var initialFevals = initialProblem.get_fevals();
                
                using var finalpop = algorithm.evolve(pop);
                using var evolvedProblem = finalpop.get_problem();
                using var championDecisionVector = finalpop.champion_x();
                using var championFitness = finalpop.champion_f();
                Assert.AreEqual(2, championDecisionVector.Count, "2 in x");
                Assert.IsTrue(championDecisionVector.Contains(1.0), "1.0 for first x value");
                Assert.IsTrue(championDecisionVector.Contains(2.0), "2.0 for second x value");

                Assert.AreEqual(2, championFitness.Count, "2 in f(x)");
                Assert.IsTrue(championFitness.Contains(3.0), "3.0 for first f(x) value");
                Assert.IsTrue(championFitness.Contains(0.0), "0.0 for second f(x) value");
                Assert.Greater(evolvedProblem.get_fevals(), initialFevals, "evolution should trigger additional function evaluations");
            }
        }
    }
}
