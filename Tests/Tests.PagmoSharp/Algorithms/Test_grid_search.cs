using System;
using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp.Algorithms
{
    [TestFixture]
    public class Test_grid_search
    {
        [Test]
        public void GridSearchFindsExpectedOptimumForTwoDimensionalSingleObjectiveProblem()
        {
            using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var algorithm = new grid_search(new uint[] { 20, 20 });
            using var population = new population(problem, 32, 7);

            using var finalPopulation = algorithm.evolve(population);
            using var championX = finalPopulation.champion_x();
            using var championF = finalPopulation.champion_f();

            Assert.AreEqual(0.0, championX[0], 1e-12, "Champion x[0] should match sampled optimum.");
            Assert.AreEqual(3.0, championX[1], 1e-12, "Champion x[1] should match sampled optimum.");
            Assert.AreEqual(0.0, championF[0], 1e-12, "Champion objective should match sampled optimum.");
        }

        [Test]
        public void GridSearchFindsFeasibleOptimumForConstrainedProblem()
        {
            using var problem = new TwoDimensionalConstrainedProblem();
            using var algorithm = new grid_search(new uint[] { 6, 6 });
            using var population = new population(problem, 16, 9);

            using var finalPopulation = algorithm.evolve(population);
            using var championX = finalPopulation.champion_x();
            using var championF = finalPopulation.champion_f();

            Assert.AreEqual(1.0, championX[0], 1e-12, "Champion x[0] should satisfy x == 1.");
            Assert.AreEqual(2.0, championX[1], 1e-12, "Champion x[1] should satisfy y == 2.");
            Assert.AreEqual(3.0, championF[0], 1e-12, "Champion objective should match feasible sampled optimum.");
            Assert.AreEqual(0.0, championF[1], 1e-12, "Equality constraint y == 2 should be satisfied.");
            Assert.AreEqual(0.0, championF[2], 1e-12, "Equality constraint x == 1 should be satisfied.");
        }

        [Test]
        public void GridSearchExposesEmptyUniversalLogSurface()
        {
            using IAlgorithm algorithm = new grid_search(new uint[] { 4, 4 });
            var logLines = algorithm.GetLogLines();
            Assert.That(logLines, Is.Not.Null);
            Assert.That(logLines.Count, Is.EqualTo(0));
        }

        [Test]
        public void GridSearchReportsManagedOnlyWhenUsedInTypeErasedIslandPath()
        {
            using IAlgorithm algorithm = new grid_search(new uint[] { 4, 4 });
            using var managedProblem = new TwoDimensionalSingleObjectiveProblemWrapper();

            var ex = Assert.Throws<NotSupportedException>(() => island.Create(algorithm, managedProblem, 8u, 2u));
            Assert.That(ex!.Message, Does.Contain("grid_search"));
            Assert.That(ex!.Message, Does.Contain("managed-only"));
        }
    }
}
