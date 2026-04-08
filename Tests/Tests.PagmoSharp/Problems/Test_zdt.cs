using System;
using System.Collections.Generic;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Problems
{
    [TestFixture]
    public class Test_zdt : TestProblemBase
    {
        public override IProblem CreateStandardProblem(uint problemIndex = 0)
        {
            if (problemIndex == 0)
                problemIndex = 1;
            return new zdt(problemIndex);
        }

        public override IEnumerable<uint> MultiProblemIndices {
            get
            {
                for (uint i = 1; i <= 24; i++)
                {
                    yield return i;
                }
            }
        }

        [Test]
        public override void TestBoilerPlate()
        {
            using var problem = CreateStandardProblem(1);
            Assert.AreEqual("ZDT1", problem.get_name(), "name");
            Assert.AreEqual(0, problem.get_nec(), "equality constraint count");
            Assert.AreEqual(2, problem.get_nobj(), "objective count");
            Assert.AreEqual(0, problem.get_nix(), "integer count");
            Assert.AreEqual(thread_safety.basic, problem.get_thread_safety(), "thread safety");
            using var bounds = problem.get_bounds();
            Assert.AreEqual(0.0, bounds.first[0]);
            Assert.IsTrue(problem.has_batch_fitness(), "has batch fitness");
        }

        [Test]
        public override void TestOptimizing()
        {
            using var problemBase = CreateStandardProblem();
            using var bounds = problemBase.get_bounds();
            Assert.AreEqual(0, bounds.first[0]);
            using var algorithm = new nspso(20, 1);
            algorithm.set_seed(2);
            using (var pop = new population(problemBase, 128))
            {
            using var injected = new DoubleVector(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30);
            pop.set_x(0, injected);
            algorithm.set_seed(2); // for consistent results
            using var initialProblem = pop.get_problem();
            var initialFevals = initialProblem.get_fevals();

            using var finalpop = algorithm.evolve(pop);
            using var evolvedProblem = finalpop.get_problem();
            Assert.AreEqual(pop.size(), finalpop.size(), "population size should be preserved by evolve()");
            Assert.Greater(evolvedProblem.get_fevals(), initialFevals, "evolution should trigger additional function evaluations");

            using var evolvedFitness = finalpop.get_f();
                using var sorting = pagmo.pagmo.FastNonDominatedSorting(evolvedFitness);
                Assert.That(sorting.fronts, Is.Not.Null);
                Assert.GreaterOrEqual(sorting.fronts.Count, 1, "non-dominated sorting should return at least one front");
                Assert.GreaterOrEqual(sorting.fronts[0].Count, 1, "first Pareto front should contain at least one point");
            }
        }

        protected override IEnumerable<ProblemTestData> GetRegressionData()
        {
            return new List<ProblemTestData>()
            {
                new ProblemTestData("zdt1", "simpleTest1", new double[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, }, new double[] {1, 60.22352732137657, }, 1)
            };
        }
    }
}
