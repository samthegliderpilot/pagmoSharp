using System.Collections.Generic;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Problems;

[TestFixture]
public class Test_hock_schittkowski_71 : TestProblemBase
{
    protected override bool SupportsMidpointFitnessProbe()
    {
        return false;
    }

    public override IProblem CreateStandardProblem(uint problemIndex = 0)
    {
        return new hock_schittkowski_71();
    }

    [Test]
    public override void TestBoilerPlate()
    {
        using var problem = (hock_schittkowski_71)CreateStandardProblem();
        using var bounds = problem.get_bounds();

        Assert.AreEqual("Hock Schittkowski 71", problem.get_name());
        Assert.AreEqual(1u, problem.get_nobj());
        Assert.AreEqual(1u, problem.get_nec());
        Assert.AreEqual(1u, problem.get_nic());
        Assert.AreEqual(0u, problem.get_nix());
        Assert.AreEqual(thread_safety.basic, problem.get_thread_safety());
        Assert.IsFalse(problem.has_batch_fitness());

        Assert.AreEqual(4, bounds.first.Count);
        Assert.AreEqual(4, bounds.second.Count);
        for (var i = 0; i < bounds.first.Count; i++)
        {
            Assert.AreEqual(1.0, bounds.first[i], 0.0);
            Assert.AreEqual(5.0, bounds.second[i], 0.0);
        }

        Assert.IsNotEmpty(problem.get_extra_info());
    }

    [Test]
    public void DifferentialInfoAndBestKnownAreAvailable()
    {
        using var problem = new hock_schittkowski_71();
        using var optimum = problem.best_known();
        using var fitness = problem.fitness(optimum);
        using var gradient = problem.gradient(optimum);
        using var hessians = problem.hessians(optimum);

        Assert.AreEqual(4, optimum.Count);
        Assert.AreEqual((int)(problem.get_nobj() + problem.get_nec() + problem.get_nic()), fitness.Count);
        Assert.AreEqual(12, gradient.Count);
        Assert.AreEqual(3, hessians.Count);
    }

    [Test]
    public override void TestOptimizing()
    {
        using var problem = new hock_schittkowski_71();
        using var algorithm = new gaco(20);
        algorithm.set_seed(31u);

        using var initialPopulation = new population(problem, 64u, 555u);
        using var evolvedPopulation = algorithm.evolve(initialPopulation);
        using var championX = evolvedPopulation.champion_x();
        using var championF = evolvedPopulation.champion_f();

        Assert.AreEqual(4, championX.Count);
        Assert.AreEqual(3, championF.Count);

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
        using var problem = new hock_schittkowski_71();
        using var x = new DoubleVector(new[] { 1.0, 4.0, 4.0, 1.0 });
        using var fitness = problem.fitness(x);

        var expected = problem.get_nobj() + problem.get_nec() + problem.get_nic();
        Assert.AreEqual(expected, (uint)fitness.Count);
    }

    protected override IEnumerable<ProblemTestData> GetRegressionData()
    {
        return new[]
        {
            new ProblemTestData("Hock Schittkowski 71", "simpleTest", new[] { 1.0, 4.0, 4.0, 1.0 }, new[] { 13.0, -6.0, 9.0 })
        };
    }
}

