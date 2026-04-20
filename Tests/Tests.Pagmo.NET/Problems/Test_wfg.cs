using System.Collections.Generic;
using NUnit.Framework;
using pagmo;

namespace Tests.Pagmo.NET.Problems;

[TestFixture]
public class Test_wfg : TestProblemBase
{
    public override IProblem CreateStandardProblem(uint problemIndex = 0)
    {
        var id = problemIndex == 0 ? 1u : problemIndex;
        return new wfg(id, 5u, 3u, 4u);
    }

    [Test]
    public override void TestBoilerPlate()
    {
        using var problem = CreateStandardProblem(1u);
        Assert.IsNotEmpty(problem.get_name());
        Assert.AreEqual(3u, problem.get_nobj());
        Assert.AreEqual(0u, problem.get_nec());
        Assert.AreEqual(0u, problem.get_nic());
        Assert.AreEqual(0u, problem.get_nix());
        Assert.AreEqual(ThreadSafety.Basic, problem.get_thread_safety());
        Assert.IsFalse(problem.has_batch_fitness());

        using var bounds = problem.get_bounds();
        Assert.AreEqual(5, bounds.first.Count);
        Assert.AreEqual(5, bounds.second.Count);
        for (var i = 0; i < bounds.first.Count; i++)
        {
            Assert.LessOrEqual(bounds.first[i], bounds.second[i]);
        }
    }

    [Test]
    public override void TestOptimizing()
    {
        using var problem = CreateStandardProblem(1u);
        using var algorithm = new nspso(20, 1);
        algorithm.set_seed(223u);

        using var initialPopulation = new population(problem, 64u, 43u);
        using var initialProblem = initialPopulation.get_problem();
        var initialFevals = initialProblem.get_fevals();
        using var evolvedPopulation = algorithm.evolve(initialPopulation);
        using var evolvedProblem = evolvedPopulation.get_problem();
        using var allFitness = evolvedPopulation.get_f();
        using var allDecisionVectors = evolvedPopulation.get_x();

        Assert.AreEqual(64, allFitness.Count);
        Assert.AreEqual(64, allDecisionVectors.Count);
        Assert.AreEqual(3, allFitness[0].Count);
        Assert.Greater(evolvedProblem.get_fevals(), initialFevals, "evolution should trigger additional function evaluations");

        using var bounds = problem.get_bounds();
        for (var individualIdx = 0; individualIdx < allDecisionVectors.Count; individualIdx++)
        {
            Assert.AreEqual(bounds.first.Count, allDecisionVectors[individualIdx].Count);
            for (var dimensionIdx = 0; dimensionIdx < allDecisionVectors[individualIdx].Count; dimensionIdx++)
            {
                var value = allDecisionVectors[individualIdx][dimensionIdx];
                Assert.GreaterOrEqual(value, bounds.first[dimensionIdx]);
                Assert.LessOrEqual(value, bounds.second[dimensionIdx]);
            }
        }
    }

    [Test]
    public void TestFitnessVectorLengthForKnownValidInput()
    {
        using var problem = CreateStandardProblem(1u);
        using var x = new DoubleVector(new[] { 0.1, 0.2, 0.3, 0.4, 0.5 });
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
            new ProblemTestData("wfg", "WFG1_regression", new[] { 0.1, 0.2, 0.3, 0.4, 0.5 }, new[] { 2.6475347756529857, 1.0109392891561515, 1.1599610472349962 }, 1u),
        };
    }
}

