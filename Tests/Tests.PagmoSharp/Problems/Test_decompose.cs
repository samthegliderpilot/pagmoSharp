using System.Collections.Generic;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Problems;

[TestFixture]
public class Test_decompose : TestProblemBase
{
    public override IProblem CreateStandardProblem(uint problemIndex = 0)
    {
        var method = problemIndex == 0u || problemIndex == 1u ? "weighted" : "tchebycheff";
        return decompose.Create(new zdt(1u), new[] { 0.5, 0.5 }, new[] { 0.0, 0.0 }, method);
    }

    [Test]
    public override void TestBoilerPlate()
    {
        using var problem = CreateStandardProblem();
        Assert.IsTrue(problem.get_name().Contains("Decomposed", System.StringComparison.OrdinalIgnoreCase));
        Assert.AreEqual(1u, problem.get_nobj());
        Assert.AreEqual(0u, problem.get_nec());
        Assert.AreEqual(0u, problem.get_nic());
        Assert.AreEqual(0u, problem.get_nix());
        Assert.AreEqual(ThreadSafety.Basic, problem.get_thread_safety());
        Assert.IsFalse(problem.has_batch_fitness());

        using var bounds = problem.get_bounds();
        Assert.AreEqual(30, bounds.first.Count);
        Assert.AreEqual(30, bounds.second.Count);
    }

    [Test]
    public override void TestOptimizing()
    {
        using var problem = CreateStandardProblem(1u);
        using var algorithm = new de(100u);
        algorithm.set_seed(307u);

        using var initialPopulation = new population(problem, 64u, 59u);
        using var initialProblem = initialPopulation.get_problem();
        var initialFevals = initialProblem.get_fevals();
        using var evolvedPopulation = algorithm.evolve(initialPopulation);
        using var evolvedProblem = evolvedPopulation.get_problem();
        using var championDecisionVector = evolvedPopulation.champion_x();
        using var championFitness = evolvedPopulation.champion_f();

        Assert.AreEqual(30, championDecisionVector.Count);
        Assert.AreEqual(1, championFitness.Count);
        Assert.Greater(evolvedProblem.get_fevals(), initialFevals, "evolution should trigger additional function evaluations");

        using var concreteProblem = (decompose)problem;
        using var originalFitness = concreteProblem.original_fitness(championDecisionVector);
        Assert.AreEqual(2, originalFitness.Count);
    }

    [Test]
    public void TestZReferenceMatchesConfiguration()
    {
        using var problem = (decompose)CreateStandardProblem();
        using var z = problem.get_z();
        Assert.AreEqual(2, z.Count);
        Assert.AreEqual(0.0, z[0], 1e-12);
        Assert.AreEqual(0.0, z[1], 1e-12);
    }

    [Test]
    public void TestFitnessVectorLengthForKnownValidInput()
    {
        using var problem = CreateStandardProblem(1u);
        using var x = new DoubleVector(new[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 0.1, 0.2, 0.3 });
        using var fitness = problem.fitness(x);
        Assert.AreEqual(problem.get_nobj(), (uint)fitness.Count);
    }

    protected override bool SupportsMidpointFitnessProbe()
    {
        return false;
    }

    protected override IEnumerable<ProblemTestData> GetRegressionData()
    {
        return new[]
        {
            new ProblemTestData("decompose", "weighted_zdt1_regression", new[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 0.1, 0.2, 0.3 }, new[] { 2.3568721543601 }, 1u),
        };
    }
}
