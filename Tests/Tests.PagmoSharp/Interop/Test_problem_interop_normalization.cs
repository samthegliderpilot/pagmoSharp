using System;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Interop;

[TestFixture]
public class Test_problem_interop_normalization
{
    [Test]
    public void WrappedNativeProblemCanBeNormalizedAndUsedAfterSourceDisposal()
    {
        using var source = new ackley(2u);
        using var normalized = new problem((IProblem)source);
        source.Dispose();

        using var decisionVector = new DoubleVector(new[] { 0.1, -0.2 });
        using var fitness = normalized.fitness(decisionVector);
        Assert.That(fitness.Count, Is.EqualTo(1), "normalized problem should evaluate as a single-objective problem");
        Assert.That(double.IsFinite(fitness[0]), Is.True, "normalized problem should remain usable after disposing the source wrapper");
    }

    [Test]
    public void PopulationConstructionFromWrappedNativeProblemRemainsUsableAfterSourceDisposal()
    {
        using var source = new ackley(2u);
        using var population = new population((IProblem)source, 16u, 2u);
        source.Dispose();

        using var normalizedProblem = population.get_problem();
        Assert.That(normalizedProblem.get_nobj(), Is.EqualTo(1u), "population should retain a valid normalized problem");
        Assert.That(population.size(), Is.EqualTo(16u), "population should keep configured size after source wrapper disposal");
    }

    [Test]
    public void IslandCreationFromWrappedNativeProblemRemainsUsableAfterSourceDisposal()
    {
        using IAlgorithm algorithm = new bee_colony();
        using var source = new ackley(2u);
        using var island = pagmo.island.Create(algorithm, (IProblem)source, 16u, 2u);
        source.Dispose();

        island.evolve(1u);
        Assert.DoesNotThrow(() => island.wait_check());
        Assert.That(island.status(), Is.EqualTo(evolve_status.idle));
    }
}

