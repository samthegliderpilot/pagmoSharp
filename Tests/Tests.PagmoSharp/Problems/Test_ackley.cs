using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Problems;

[TestFixture]
public class Test_ackley : TestProblemBase
{
    public override IProblem CreateStandardProblem(uint problemIndex = 0)
    {
        return new ackley(2);
    }

    [Test]
    public override void TestBoilerPlate()
    {
        using var problem = CreateStandardProblem();
        Assert.AreEqual("Ackley Function", problem.get_name(), "name");
        Assert.AreEqual(0, problem.get_nec(), "equality constraint count");
        Assert.AreEqual(1, problem.get_nobj(), "objective count");
        Assert.AreEqual(0, problem.get_nix(), "integer count");
        Assert.AreEqual(thread_safety.basic, problem.get_thread_safety(), "thread safety");
        using var bounds = problem.get_bounds();
        Assert.AreEqual(-15.0, bounds.first[0]);
    }

    [Test]
    public override void TestOptimizing()
    {
        using var problemBase = CreateStandardProblem();
        using var bounds = problemBase.get_bounds();
        Assert.AreEqual(-15, bounds.first[0]);
        using var algorithm = new gaco(20);
        using (var pop = new population(problemBase, 1024))
        {
            algorithm.set_seed(2); // for consistent results
            using var initialChampionFitness = pop.champion_f();
            var initialBest = initialChampionFitness[0];

            using var finalpop = algorithm.evolve(pop);
            using var championDecisionVector = finalpop.champion_x();
            using var championFitness = finalpop.champion_f();
            var champX = championDecisionVector.ToArray();
            var champF = championFitness.ToArray();
            Assert.AreEqual(2, champX.Length, "2 in x");
            Assert.AreEqual(0, champX[0], 1e-3, "1.0 for first x value");
            Assert.AreEqual(0, champX[1], 1e-3, "2.0 for second x value");

            Assert.AreEqual(1, champF.Length, "1 in f(x)");
            Assert.LessOrEqual(champF[0], initialBest + 1e-12, "evolution should not worsen champion fitness");
            Assert.AreEqual(0.00010319491007537707, champF[0], 1e-3, "optimal function value");

        }
    }

    protected override IEnumerable<ProblemTestData> GetRegressionData()
    {
        return new List<ProblemTestData>()
        {
            new ProblemTestData("ackley", "simpleTest", new double[] { 1, 1, },
                new double[] { 3.6253849384403627, })
        };
    }
}
