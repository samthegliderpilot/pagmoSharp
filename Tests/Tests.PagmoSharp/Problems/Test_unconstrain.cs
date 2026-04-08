using System.Collections.Generic;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Problems;

[TestFixture]
public class Test_unconstrain : TestProblemBase
{
    public override IProblem CreateStandardProblem(uint problemIndex = 0)
    {
        return unconstrain.Create(new hock_schittkowski_71(), "death penalty");
    }

    [Test]
    public override void TestBoilerPlate()
    {
        using var problem = CreateStandardProblem();
        Assert.IsTrue(problem.get_name().Contains("unconstrain", System.StringComparison.OrdinalIgnoreCase));
        Assert.AreEqual(1u, problem.get_nobj());
        Assert.AreEqual(0u, problem.get_nec());
        Assert.AreEqual(0u, problem.get_nic());
        Assert.AreEqual(0u, problem.get_nix());

        using var bounds = problem.get_bounds();
        Assert.AreEqual(4, bounds.first.Count);
        Assert.AreEqual(4, bounds.second.Count);
    }

    [Test]
    public override void TestOptimizing()
    {
        using var problem = CreateStandardProblem();
        using var algorithm = new de(100u);
        algorithm.set_seed(503u);

        using var initialPopulation = new population(problem, 64u, 83u);
        using var initialProblem = initialPopulation.get_problem();
        var initialFevals = initialProblem.get_fevals();
        using var evolvedPopulation = algorithm.evolve(initialPopulation);
        using var evolvedProblem = evolvedPopulation.get_problem();
        using var championDecisionVector = evolvedPopulation.champion_x();
        using var championFitness = evolvedPopulation.champion_f();

        Assert.AreEqual(4, championDecisionVector.Count);
        Assert.AreEqual(1, championFitness.Count);
        Assert.Greater(evolvedProblem.get_fevals(), initialFevals, "evolution should trigger additional function evaluations");

        using var bounds = problem.get_bounds();
        for (var i = 0; i < championDecisionVector.Count; i++)
        {
            Assert.GreaterOrEqual(championDecisionVector[i], bounds.first[i]);
            Assert.LessOrEqual(championDecisionVector[i], bounds.second[i]);
        }
    }

    [Test]
    public void TestFitnessVectorLengthForKnownValidInput()
    {
        using var problem = CreateStandardProblem();
        using var x = new DoubleVector(new[] { 1.0, 4.0, 4.0, 1.0 });
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
            new ProblemTestData("unconstrain", "death_penalty_hs71_regression", new[] { 1.0, 4.0, 4.0, 1.0 }, new[] { 1.7976931348623157E+308 }, 0u),
        };
    }
}
