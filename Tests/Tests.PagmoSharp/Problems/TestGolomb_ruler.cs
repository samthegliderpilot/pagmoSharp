using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Problems
{
    [TestFixture]
    public class TestGolomb_ruler
    {
        [Test]
        public void TestBoilerPlate()
        {
            using var problem = new golomb_ruler();
            Assert.AreEqual("Golomb Ruler (order 3)", problem.get_name(), "name");
            Assert.AreEqual(1, problem.get_nec(), "equality constraint count");
            Assert.AreEqual(1, problem.get_nobj(), "objective count");
            Assert.AreEqual(2, problem.get_nix(), "integer count");
            var bounds = problem.get_bounds();
            Assert.AreEqual(1.0, bounds.first[0]);
        }

        [Test]
        public void TestBasicEvaluation()
        {
            using var problem = new golomb_ruler();
            var x = new DoubleVector(new double[] { 2.0, 3.0, 4.0 });
            var fitness = problem.fitness(x);
            Assert.AreEqual(9.0, fitness[0], "value");
            Assert.AreEqual(0.0, fitness[1], "equality constraint");
        }

        [Test]
        public void TestOptimizing()
        {
            using var problemBase = new golomb_ruler();
            Assert.AreEqual(1, problemBase.get_bounds().first[0]);
            var problemBase2 = new ProblemWrapper(problemBase);
            var problem = new problem(problemBase2);
            using var algorithm = new gaco(20);
            using (var pop = new population(problem, 1024))
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