using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Problems;

[TestFixture]
public class Testminlp_rastrigin : TestProblemBase
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
        var bounds = problem.get_bounds();
        Assert.AreEqual(-5.12, bounds.first[0]);
    }

    [Test]
    public override void TestOptimizing()
    {
        using var problemBase = CreateStandardProblem();
        Assert.AreEqual(-5.12, problemBase.get_bounds().first[0], 0.000001);
        var problemBase2 = new ProblemWrapper(problemBase);
        using var algorithm = new gaco(20);
        using (var pop = new population(problemBase2, 1024))
        {
            algorithm.set_seed(2); // for consistent results

            var finalpop = algorithm.evolve(pop);
            var champX = finalpop.champion_x().ToArray();
            var champF = finalpop.champion_f().ToArray();
            Assert.AreEqual(3, champX.Length, "2 in x");
            Assert.AreEqual(0, champX[0], 1e-3, "1.0 for first x value");
            Assert.AreEqual(0, champX[1], 1e-3, "2.0 for second x value");

            Assert.AreEqual(1, champF.Length, "1 in f(x)");
            Assert.AreEqual(25.000003609857814, champF[0], 1e-3, "optimal function value");
            
        }
    }

    public static IEnumerable<ProblemTestData> RegressionData
    {
        get
        {
            return new List<ProblemTestData>()
            {
                new ProblemTestData("MINLP Rastrigin Function", "simpleTest", new double[] { 1, 1, },
                    new double[] { 2.0, })
            };
        }
    }
}