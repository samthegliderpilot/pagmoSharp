using System.Collections.Generic;
using NUnit.Framework;
using pagmo;

namespace Tests.Pagmo.NET.Problems;

[TestFixture]
public class Test_lennard_jones : TestProblemBase
{
    protected override bool SupportsMidpointFitnessProbe()
    {
        return false;
    }

    public override IProblem CreateStandardProblem(uint problemIndex = 0)
    {
        return new lennard_jones(3);
    }

    [Test]
    public override void TestBoilerPlate()
    {
        using var problem = (lennard_jones)CreateStandardProblem();
        using var bounds = problem.get_bounds();

        Assert.That(problem.get_name(), Is.Not.Empty);
        Assert.AreEqual(1u, problem.get_nobj());
        Assert.AreEqual(0u, problem.get_nec());
        Assert.AreEqual(0u, problem.get_nic());
        Assert.AreEqual(0u, problem.get_nix());
        Assert.AreEqual(ThreadSafety.Basic, problem.get_thread_safety());
        Assert.IsFalse(problem.has_batch_fitness());

        Assert.AreEqual(3, bounds.first.Count);
        Assert.AreEqual(3, bounds.second.Count);
        for (var i = 0; i < bounds.first.Count; i++)
        {
            Assert.LessOrEqual(bounds.first[i], bounds.second[i]);
        }
    }

    [Test]
    public override void TestOptimizing()
    {
        using var problem = new lennard_jones(3);
        using var algorithm = new de(gen: 25, F: 0.8, CR: 0.9, variant: 2u, ftol: 1e-6, xtol: 1e-6);
        algorithm.set_seed(29u);

        using var initialPopulation = new population(problem, 30u, 333u);
        using var initialProblem = initialPopulation.get_problem();
        var initialFevals = initialProblem.get_fevals();
        using var evolvedPopulation = algorithm.evolve(initialPopulation);
        using var evolvedProblem = evolvedPopulation.get_problem();
        using var championX = evolvedPopulation.champion_x();
        using var championF = evolvedPopulation.champion_f();

        Assert.AreEqual(3, championX.Count);
        Assert.AreEqual(1, championF.Count);
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
        using var problem = new lennard_jones(3);
        using var x = new DoubleVector(new[] { 1.0, 1.0, 1.0 });
        using var fitness = problem.fitness(x);

        var expected = problem.get_nobj() + problem.get_nec() + problem.get_nic();
        Assert.AreEqual(expected, (uint)fitness.Count);
    }

    protected override IEnumerable<ProblemTestData> GetRegressionData()
    {
        return new[]
        {
            new ProblemTestData("lennard_jones", "simpleTest", new[] { 1.0, 1.0, 1.0 }, new[] { -0.4375 })
        };
    }
}

