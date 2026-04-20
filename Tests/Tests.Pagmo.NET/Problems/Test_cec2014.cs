using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using pagmo;

namespace Tests.Pagmo.NET.Problems;

[TestFixture]
public class Test_cec2014 : TestProblemBase
{
    public override IProblem CreateStandardProblem(uint problemIndex = 0)
    {
        var id = problemIndex == 0 ? 1u : problemIndex;
        return new cec2014(id, 2u);
    }

    [Test]
    public override void TestBoilerPlate()
    {
        using var problem = CreateStandardProblem(1u);
        Assert.IsNotEmpty(problem.get_name());
        Assert.AreEqual(1u, problem.get_nobj());
        Assert.AreEqual(0u, problem.get_nec());
        Assert.AreEqual(0u, problem.get_nic());
        Assert.AreEqual(0u, problem.get_nix());
        Assert.AreEqual(ThreadSafety.Basic, problem.get_thread_safety());
        Assert.IsFalse(problem.has_batch_fitness());

        using var bounds = problem.get_bounds();
        Assert.AreEqual(2, bounds.first.Count);
        Assert.AreEqual(2, bounds.second.Count);
        for (var i = 0; i < bounds.first.Count; i++)
        {
            Assert.LessOrEqual(bounds.first[i], bounds.second[i]);
        }
    }

    [Test]
    public override void TestOptimizing()
    {
        using var problem = CreateStandardProblem(1u);
        using var algorithm = new de(100u);
        algorithm.set_seed(137u);

        using var initialPopulation = new population(problem, 64u, 29u);
        using var initialFitness = initialPopulation.get_f();
        var initialBest = initialFitness.Min(fitnessVector => fitnessVector[0]);

        using var evolvedPopulation = algorithm.evolve(initialPopulation);
        using var championFitness = evolvedPopulation.champion_f();
        using var championDecisionVector = evolvedPopulation.champion_x();

        Assert.AreEqual(1, championFitness.Count);
        Assert.AreEqual(2, championDecisionVector.Count);
        Assert.LessOrEqual(championFitness[0], initialBest, "evolution should not worsen the best single-objective fitness");

        using var bounds = problem.get_bounds();
        for (var i = 0; i < championDecisionVector.Count; i++)
        {
            Assert.GreaterOrEqual(championDecisionVector[i], bounds.first[i]);
            Assert.LessOrEqual(championDecisionVector[i], bounds.second[i]);
        }
    }

    protected override IEnumerable<ProblemTestData> GetRegressionData()
    {
        return new[]
        {
            new ProblemTestData("cec2014", "F1_regression", new[] { 0.1, -0.2 }, new[] { 321652556.05714166 }, 1u),
        };
    }
}

