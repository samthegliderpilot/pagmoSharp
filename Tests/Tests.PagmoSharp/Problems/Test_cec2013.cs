using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Problems;

[TestFixture]
public class Test_cec2013 : TestProblemBase
{
    public override IProblem CreateStandardProblem(uint problemIndex = 0)
    {
        var id = problemIndex == 0 ? 1u : problemIndex;
        return new cec2013(id, 2u);
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
        Assert.AreEqual(thread_safety.basic, problem.get_thread_safety());
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
        algorithm.set_seed(131u);

        using var initialPopulation = new population(problem, 64u, 13u);
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

    [Test]
    public void TestFitnessVectorLengthForKnownValidInput()
    {
        using var problem = CreateStandardProblem(1u);
        using var x = new DoubleVector(new[] { 0.1, -0.2 });
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
            new ProblemTestData("cec2013", "F1_regression", new[] { 0.1, -0.2 }, new[] { -774.0812279737056 }, 1u),
        };
    }
}

