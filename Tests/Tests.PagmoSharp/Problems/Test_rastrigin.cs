using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Problems;

[TestFixture]
public class Test_rastrigin : TestProblemBase
{
    public override IProblem CreateStandardProblem(uint problemIndex = 0)
    {
        return new rastrigin(3);
    }

    [Test]
    public override void TestBoilerPlate()
    {
        using var problem = (rastrigin)CreateStandardProblem();
        using var bounds = problem.get_bounds();

        Assert.AreEqual("Rastrigin Function", problem.get_name());
        Assert.AreEqual(1u, problem.get_nobj());
        Assert.AreEqual(0u, problem.get_nec());
        Assert.AreEqual(0u, problem.get_nic());
        Assert.AreEqual(0u, problem.get_nix());
        Assert.AreEqual(thread_safety.basic, problem.get_thread_safety());
        Assert.IsFalse(problem.has_batch_fitness());

        Assert.AreEqual(3, bounds.first.Count);
        Assert.AreEqual(3, bounds.second.Count);
        for (var i = 0; i < bounds.first.Count; i++)
        {
            Assert.AreEqual(-5.12, bounds.first[i], 0.0);
            Assert.AreEqual(5.12, bounds.second[i], 0.0);
        }
    }

    [Test]
    public void DifferentialInfoAndOptimumAreAvailable()
    {
        using var problem = new rastrigin(2);
        using var optimum = problem.best_known();
        using var fitness = problem.fitness(optimum);
        using var gradient = problem.gradient(optimum);
        using var hessians = problem.hessians(optimum);

        Assert.AreEqual(2, optimum.Count);
        Assert.AreEqual(1, fitness.Count);
        Assert.AreEqual(0.0, fitness[0], 1e-12);

        Assert.AreEqual(2, gradient.Count);
        Assert.AreEqual(1, hessians.Count);
        Assert.AreEqual(2, hessians[0].Count);
    }

    [Test]
    public override void TestOptimizing()
    {
        using var problem = new rastrigin(2);
        using var algorithm = new de(gen: 20, F: 0.8, CR: 0.9, variant: 2u, ftol: 1e-6, xtol: 1e-6);
        algorithm.set_seed(7u);

        using var initialPopulation = new population(problem, 30u, 12u);
        using var evolvedPopulation = algorithm.evolve(initialPopulation);
        using var championX = evolvedPopulation.champion_x();
        using var championF = evolvedPopulation.champion_f();

        Assert.AreEqual(2, championX.Count);
        Assert.AreEqual(1, championF.Count);

        using var bounds = problem.get_bounds();
        for (var i = 0; i < championX.Count; i++)
        {
            Assert.GreaterOrEqual(championX[i], bounds.first[i]);
            Assert.LessOrEqual(championX[i], bounds.second[i]);
        }
    }
}

