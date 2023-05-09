using System.Collections.Generic;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Problems
{
    [TestFixture]
    public class TestInventory : TestProblemBase
    {
        public override IProblem CreateStandardProblem(uint problemIndex = 0)
        {
            return new inventory(4u, 10u, 2u);
        }

        public static IEnumerable<ProblemTestData> RegressionData
        {
            get
            {
                return new List<ProblemTestData>()
                {
                    new ProblemTestData("inventory", "SimpleTest", new double[] { 1,2,3,4, },
                        new double[] { 294.60824716582886d }),
                };
            }
        }

        [Test]
        public override void TestBoilerPlate()
        {
            using var problem = CreateStandardProblem();
            Assert.AreEqual("Inventory problem", problem.get_name(), "name");
            Assert.AreEqual(0, problem.get_nec(), "equality constraint count");
            Assert.AreEqual(1, problem.get_nobj(), "objective count");
            Assert.AreEqual(0, problem.get_nix(), "integer count");
            var bounds = problem.get_bounds();
            Assert.IsNotNull(bounds);
            Assert.AreEqual(4, bounds.first.Count, "first element is length 4");
            Assert.AreEqual(4, bounds.second.Count, "second element is length 4");
        }
        
        [Test]
        public override void TestOptimizing()
        {
            using var problemBase = CreateStandardProblem();
            var problemBase2 = new ProblemWrapper(problemBase);
            using var algorithm = new gwo(20);
            using (var pop = new population(problemBase2, 1024))
            {
                algorithm.set_seed(2); // for consistent results
                
                var finalpop = algorithm.evolve(pop);
                var champX = finalpop.champion_x();
                var champF = finalpop.champion_f();
                Assert.AreEqual(4, champX.Count, "4 in x");
                Assert.AreEqual(57.839652700387155, champX[0], 1.0, "champX");

                Assert.AreEqual(1, champF.Count, "1 in f(x)");
                Assert.AreEqual(240.85449403381608, champF[0], 1.0,"champF");
            }
        }
    }
}