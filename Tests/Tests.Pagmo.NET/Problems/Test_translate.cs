using System.Collections.Generic;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Problems;

[TestFixture]
public class Test_translate : TestProblemBase
{
    public override IProblem CreateStandardProblem(uint problemIndex = 0)
    {
        return translate.Create(new ackley(2u), new[] { 1.0, -1.0 });
    }

    [Test]
    public override void TestBoilerPlate()
    {
        using var problem = (translate)CreateStandardProblem();
        Assert.IsTrue(problem.get_name().Contains("translated", System.StringComparison.OrdinalIgnoreCase));
        Assert.AreEqual(1u, problem.get_nobj());
        Assert.AreEqual(0u, problem.get_nec());
        Assert.AreEqual(0u, problem.get_nic());
        Assert.AreEqual(0u, problem.get_nix());

        using var translationVector = problem.get_translation();
        Assert.AreEqual(2, translationVector.Count);
        Assert.AreEqual(1.0, translationVector[0], 1e-12);
        Assert.AreEqual(-1.0, translationVector[1], 1e-12);

        using var bounds = problem.get_bounds();
        Assert.AreEqual(2, bounds.first.Count);
        Assert.AreEqual(2, bounds.second.Count);
    }

    [Test]
    public override void TestOptimizing()
    {
        using var problem = CreateStandardProblem();
        using var algorithm = new de(100u);
        algorithm.set_seed(401u);

        using var initialPopulation = new population(problem, 64u, 67u);
        using var initialProblem = initialPopulation.get_problem();
        var initialFevals = initialProblem.get_fevals();
        using var evolvedPopulation = algorithm.evolve(initialPopulation);
        using var evolvedProblem = evolvedPopulation.get_problem();
        using var championDecisionVector = evolvedPopulation.champion_x();
        using var championFitness = evolvedPopulation.champion_f();

        Assert.AreEqual(2, championDecisionVector.Count);
        Assert.AreEqual(1, championFitness.Count);
        Assert.Greater(evolvedProblem.get_fevals(), initialFevals, "evolution should trigger additional function evaluations");

        // Ackley translated by [1,-1] has optimum around [1,-1].
        Assert.AreEqual(1.0, championDecisionVector[0], 0.5);
        Assert.AreEqual(-1.0, championDecisionVector[1], 0.5);
    }

    [Test]
    public void TestFitnessVectorLengthForKnownValidInput()
    {
        using var problem = CreateStandardProblem();
        using var x = new DoubleVector(new[] { 0.25, -0.5 });
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
            new ProblemTestData("translate", "translated_ackley_regression", new[] { 0.25, -0.5 }, new[] { 4.505451288908446 }, 0u),
        };
    }
}
