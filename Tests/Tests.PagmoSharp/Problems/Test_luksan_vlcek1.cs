using System.Collections.Generic;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Problems;

[TestFixture]
public class Test_luksan_vlcek1 : TestProblemBase
{
    protected override bool SupportsMidpointFitnessProbe()
    {
        return false;
    }

    public override IProblem CreateStandardProblem(uint problemIndex = 0)
    {
        return new luksan_vlcek1(4);
    }

    [Test]
    public override void TestBoilerPlate()
    {
        using var problem = (luksan_vlcek1)CreateStandardProblem();
        using var bounds = problem.get_bounds();

        Assert.AreEqual("luksan_vlcek1", problem.get_name());
        Assert.AreEqual(1u, problem.get_nobj());
        Assert.AreEqual(2u, problem.get_nec());
        Assert.AreEqual(0u, problem.get_nic());
        Assert.AreEqual(0u, problem.get_nix());
        Assert.AreEqual(ThreadSafety.Basic, problem.get_thread_safety());
        Assert.IsFalse(problem.has_batch_fitness());

        Assert.AreEqual(4, bounds.first.Count);
        Assert.AreEqual(4, bounds.second.Count);
        for (var i = 0; i < bounds.first.Count; i++)
        {
            Assert.AreEqual(-5.0, bounds.first[i], 0.0);
            Assert.AreEqual(5.0, bounds.second[i], 0.0);
        }
    }

    [Test]
    public void GradientHasExpectedSizeAtSafeInput()
    {
        using var problem = new luksan_vlcek1(4);
        using var x = new DoubleVector(new[] { 1.0, 1.0, 1.0, 1.0 });
        using var gradient = problem.gradient(x);

        // gradient includes objective + constraints blocks
        Assert.AreEqual(10, gradient.Count);
    }

    [Test]
    public override void TestOptimizing()
    {
        using var problem = new luksan_vlcek1(4);
        using var algorithm = new gaco(20);
        algorithm.set_seed(37u);

        using var initialPopulation = new population(problem, 80u, 777u);
        using var initialProblem = initialPopulation.get_problem();
        var initialFevals = initialProblem.get_fevals();
        using var evolvedPopulation = algorithm.evolve(initialPopulation);
        using var evolvedProblem = evolvedPopulation.get_problem();
        using var championX = evolvedPopulation.champion_x();
        using var championF = evolvedPopulation.champion_f();

        Assert.AreEqual(4, championX.Count);
        Assert.AreEqual(3, championF.Count);
        Assert.Greater(evolvedProblem.get_fevals(), initialFevals, "evolution should trigger additional function evaluations");

        using var bounds = problem.get_bounds();
        for (var i = 0; i < championX.Count; i++)
        {
            Assert.GreaterOrEqual(championX[i], bounds.first[i]);
            Assert.LessOrEqual(championX[i], bounds.second[i]);
        }
    }

    [Test]
    public void FitnessVectorSizeMatchesDeclaredCountsWithSafeInput()
    {
        using var problem = new luksan_vlcek1(4);
        using var x = new DoubleVector(new[] { 1.0, 1.0, 1.0, 1.0 });
        using var fitness = problem.fitness(x);

        var expected = problem.get_nobj() + problem.get_nec() + problem.get_nic();
        Assert.AreEqual(expected, (uint)fitness.Count);
    }

    protected override IEnumerable<ProblemTestData> GetRegressionData()
    {
        return new[]
        {
            new ProblemTestData("luksan_vlcek1", "simpleTest", new[] { 1.0, 1.0, 1.0, 1.0 }, new[] { 0.0, 0.0, 0.0 })
        };
    }
}

