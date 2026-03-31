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
            Assert.IsNotNull(bounds);
            Assert.AreEqual(4, bounds.first.Count, "first element is length 4");
            Assert.AreEqual(4, bounds.second.Count, "second element is length 4");
        }
        
        [Test]
        public override void TestOptimizing()
        {
            using var problemBase = CreateStandardProblem();
            using var algorithm = new gwo(20);
            using (var pop = new population(problemBase, 1024))
            {
                algorithm.set_seed(2); // for consistent results
                
                using var finalpop = algorithm.evolve(pop);
                using var championDecisionVector = finalpop.champion_x();
                using var championFitness = finalpop.champion_f();
                Assert.AreEqual(4, championDecisionVector.Count, "4 in x");
                Assert.AreEqual(57.839652700387155, championDecisionVector[0], 1.0, "champX");

                Assert.AreEqual(1, championFitness.Count, "1 in f(x)");
                Assert.AreEqual(240.85449403381608, championFitness[0], 1.0, "champF");
            }
        }
    }
}
