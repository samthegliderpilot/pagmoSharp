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

        public static IEnumerable<ProblemTestData> RegressionData
        {
            get
            {
                return new List<ProblemTestData>()
                {
                    new ProblemTestData("Golomb Ruler(order 3)", "SimpleTest", new double[] { 2, 3, 4, },
                        new double[] { 9, 0, }),
                };
            }
        }

        [Test]
        public override void TestBoilerPlate()
        {
            using var problem = CreateStandardProblem();
            Assert.AreEqual("Golomb Ruler (order 3)", problem.get_name(), "name");
            Assert.AreEqual(1, problem.get_nec(), "equality constraint count");
            Assert.AreEqual(1, problem.get_nobj(), "objective count");
            Assert.AreEqual(2, problem.get_nix(), "integer count");
            var bounds = problem.get_bounds();
            Assert.AreEqual(1.0, bounds.first[0]);
        }
        

        [Test]
        public override void TestOptimizing()
        {
            using var problemBase = CreateStandardProblem();
            Assert.AreEqual(1, problemBase.get_bounds().first[0]);
            var problemBase2 = new ProblemWrapper(problemBase);
            using var algorithm = new gaco(20);
            using (var pop = new population(problemBase2, 1024))
            {
                algorithm.set_seed(2); // for consistent results
                
                var finalpop = algorithm.evolve(pop);
                var champX = finalpop.champion_x();
                var champF = finalpop.champion_f();
                Assert.AreEqual(2, champX.Count, "2 in x");
                Assert.IsTrue(champX.Contains(1.0), "1.0 for first x value");
                Assert.IsTrue(champX.Contains(2.0), "2.0 for second x value");

                Assert.AreEqual(2, champF.Count, "2 in f(x)");
                Assert.IsTrue(champF.Contains(3.0), "3.0 for first f(x) value");
                Assert.IsTrue(champF.Contains(0.0), "0.0 for second f(x) value");
            }
        }
    }
}