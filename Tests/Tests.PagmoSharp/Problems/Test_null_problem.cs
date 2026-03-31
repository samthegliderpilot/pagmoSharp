using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Problems;

[TestFixture]
public class Test_null_problem : TestProblemBase
{
    public override IProblem CreateStandardProblem(uint problemIndex = 0)
    {
        return new null_problem(2, 1, 3, 1);
    }

    [Test]
    public override void TestBoilerPlate()
    {
        using var problem = (null_problem)CreateStandardProblem();

        Assert.AreEqual("Null problem", problem.get_name());
        Assert.AreEqual(2u, problem.get_nobj());
        Assert.AreEqual(1u, problem.get_nec());
        Assert.AreEqual(3u, problem.get_nic());
        Assert.AreEqual(1u, problem.get_nix());
        Assert.AreEqual(thread_safety.basic, problem.get_thread_safety());
        Assert.IsFalse(problem.has_batch_fitness());
    }

    [Test]
    public override void TestOptimizing()
    {
        using var problem = new null_problem(3, 2, 1, 0);
        using var bounds = problem.get_bounds();
        using var x = new DoubleVector(bounds.first.Count);

        for (var i = 0; i < x.Count; i++)
        {
            x[i] = 0.5 * (bounds.first[i] + bounds.second[i]);
        }

        using var firstEval = problem.fitness(x);
        using var secondEval = problem.fitness(x);

        Assert.AreEqual(problem.get_nobj() + problem.get_nec() + problem.get_nic(), (uint)firstEval.Count);
        Assert.AreEqual(firstEval.Count, secondEval.Count);
        for (var i = 0; i < firstEval.Count; i++)
        {
            Assert.AreEqual(firstEval[i], secondEval[i], 0.0);
        }
    }
}

