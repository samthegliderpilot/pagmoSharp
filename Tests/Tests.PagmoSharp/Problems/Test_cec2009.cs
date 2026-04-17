using System.Collections.Generic;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Problems;

[TestFixture]
public class Test_cec2009 : TestProblemBase
{
    private static VectorOfVectorOfDoubles SnapshotObjectives(population population)
    {
        var points = new VectorOfVectorOfDoubles();
        using var allFitness = population.get_f();
        for (var i = 0; i < allFitness.Count; i++)
        {
            points.Add(new DoubleVector(new[] { allFitness[i][0], allFitness[i][1] }));
        }

        return points;
    }

    public override IProblem CreateStandardProblem(uint problemIndex = 0)
    {
        var id = problemIndex == 0 ? 1u : problemIndex;
        return new cec2009(id, false, 10u);
    }

    [Test]
    public override void TestBoilerPlate()
    {
        using var unconstrained = new cec2009(1u, false, 10u);
        using var constrained = new cec2009(1u, true, 10u);

        Assert.IsNotEmpty(unconstrained.get_name());
        Assert.AreEqual(2u, unconstrained.get_nobj());
        Assert.AreEqual(0u, unconstrained.get_nec());
        Assert.AreEqual(0u, unconstrained.get_nic());
        Assert.AreEqual(0u, unconstrained.get_nix());
        Assert.AreEqual(ThreadSafety.Basic, unconstrained.get_thread_safety());
        Assert.IsFalse(unconstrained.has_batch_fitness());

        Assert.AreEqual(2u, constrained.get_nobj());
        Assert.AreEqual(0u, constrained.get_nec());
        Assert.GreaterOrEqual(constrained.get_nic(), 1u);

        using var bounds = unconstrained.get_bounds();
        Assert.AreEqual(10, bounds.first.Count);
        Assert.AreEqual(10, bounds.second.Count);
        for (var i = 0; i < bounds.first.Count; i++)
        {
            Assert.LessOrEqual(bounds.first[i], bounds.second[i]);
        }
    }

    [Test]
    public override void TestOptimizing()
    {
        using var problem = new cec2009(1u, false, 10u);
        using var algorithm = new nspso(20, 1);
        algorithm.set_seed(41u);

        using var initialPopulation = new population(problem, 64u, 101u);
        using var initialObjectivePoints = SnapshotObjectives(initialPopulation);
        using var initialIdeal = pagmo.pagmo.ideal(initialObjectivePoints);

        using var evolvedPopulation = algorithm.evolve(initialPopulation);
        using var allFitness = evolvedPopulation.get_f();
        using var allDecisionVectors = evolvedPopulation.get_x();
        using var evolvedObjectivePoints = SnapshotObjectives(evolvedPopulation);
        using var evolvedIdeal = pagmo.pagmo.ideal(evolvedObjectivePoints);

        Assert.AreEqual(64, allFitness.Count);
        Assert.AreEqual(64, allDecisionVectors.Count);
        Assert.AreEqual(2, allFitness[0].Count);
        Assert.That(evolvedIdeal[0], Is.LessThanOrEqualTo(initialIdeal[0] + 1e-12));
        Assert.That(evolvedIdeal[1], Is.LessThanOrEqualTo(initialIdeal[1] + 1e-12));
        Assert.That(
            evolvedIdeal[0] < initialIdeal[0] - 1e-9 || evolvedIdeal[1] < initialIdeal[1] - 1e-9,
            Is.True,
            "evolution should improve at least one ideal-point objective");

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
        using var x = new DoubleVector(new[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 0.1 });
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
            new ProblemTestData("cec2009", "UF1_regression", new[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 0.1 }, new[] { 3.3708610463437156, 3.0406242064492637 }, 1),
        };
    }
}

