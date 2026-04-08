using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Problems;

[TestFixture]
public class Test_minlp_Rastrigin : TestProblemBase
{
    public override IProblem CreateStandardProblem(uint problemIndex = 0)
    {
        return new minlp_rastrigin(2);
    }

    [Test]
    public override void TestBoilerPlate()
    {
        using var problem = CreateStandardProblem();
        Assert.AreEqual("MINLP Rastrigin Function", problem.get_name(), "name");
        Assert.AreEqual(0, problem.get_nec(), "equality constraint count");
        Assert.AreEqual(1, problem.get_nobj(), "objective count");
        Assert.AreEqual(1, problem.get_nix(), "integer count");
        Assert.AreEqual(thread_safety.basic, problem.get_thread_safety(), "thread safety");
        using var bounds = problem.get_bounds();
        Assert.AreEqual(-5.12, bounds.first[0]);

        var hessianSparsity = ((minlp_rastrigin)problem).GetHessiansSparsityEntries();
        Assert.AreEqual(problem.get_nobj() + problem.get_nec() + problem.get_nic(), (uint)hessianSparsity.Length);
        Assert.Greater(hessianSparsity[0].Length, 0);
        var decisionDimension = (uint)bounds.first.Count;
        foreach (var entry in hessianSparsity[0])
        {
            Assert.Less(entry.Row, decisionDimension);
            Assert.Less(entry.Column, decisionDimension);
        }
    }

    [Test]
    public override void TestOptimizing()
    {
        using var problemBase = CreateStandardProblem();
        using var bounds = problemBase.get_bounds();
        Assert.AreEqual(-5.12, bounds.first[0], 0.000001);
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
            var champX = championDecisionVector.ToArray();
            var champF = championFitness.ToArray();
            Assert.AreEqual(3, champX.Length, "2 in x");
            Assert.AreEqual(0, champX[0], 1e-3, "1.0 for first x value");
            Assert.AreEqual(0, champX[1], 1e-3, "2.0 for second x value");

            Assert.AreEqual(1, champF.Length, "1 in f(x)");
            Assert.AreEqual(25.000003609857814, champF[0], 1e-3, "optimal function value");
            Assert.Greater(evolvedProblem.get_fevals(), initialFevals, "evolution should trigger additional function evaluations");
            
        }
    }

    protected override IEnumerable<ProblemTestData> GetRegressionData()
    {
        return new List<ProblemTestData>()
        {
            new ProblemTestData("MINLP Rastrigin Function", "simpleTest", new double[] { 1, 1, },
                new double[] { 2.0, })
        };
    }
}
