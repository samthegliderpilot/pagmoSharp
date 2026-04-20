using System;
using NUnit.Framework;
using pagmo;
using Tests.Pagmo.NET.TestProblems;

namespace Tests.Pagmo.NET.Interop;

[TestFixture]
public class Test_size_t_interop
{
    private const ulong OversizedValue = (ulong)uint.MaxValue + 1UL;

    [Test]
    public void ManagedPopulationConstructorRejectsOversizedPopulationSize()
    {
        using var managedProblem = new TwoDimensionalSingleObjectiveProblemWrapper();

        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            using var _ = new population((IProblem)managedProblem, OversizedValue, 2u);
        });

        Assert.That(ex, Is.Not.Null);
        Assert.That(ex!.ParamName, Is.EqualTo("popSize"));
        Assert.That(ex.Message, Does.Contain("size_t range"));
    }

    [Test]
    public void IslandCreateRejectsOversizedPopulationSize()
    {
        using IAlgorithm managedAlgorithm = new bee_colony();
        using var managedProblem = new TwoDimensionalSingleObjectiveProblemWrapper();

        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            using var _ = island.Create(managedAlgorithm, managedProblem, OversizedValue, 2u);
        });

        Assert.That(ex, Is.Not.Null);
        Assert.That(ex!.ParamName, Is.EqualTo("popSize"));
        Assert.That(ex.Message, Does.Contain("size_t range"));
    }

    [Test]
    public void ArchipelagoPushBackIslandRejectsOversizedPopulationSize()
    {
        using var arch = new archipelago();
        using IAlgorithm managedAlgorithm = new bee_colony();
        using var managedProblem = new TwoDimensionalSingleObjectiveProblemWrapper();

        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            _ = arch.push_back_island(managedAlgorithm, managedProblem, OversizedValue, 2u);
        });

        Assert.That(ex, Is.Not.Null);
        Assert.That(ex!.ParamName, Is.EqualTo("popSize"));
        Assert.That(ex.Message, Does.Contain("size_t range"));
    }

    [Test]
    public void ArchipelagoGetIslandCopyRejectsOversizedIndex()
    {
        using var arch = new archipelago();

        var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            using var _ = arch.GetIslandCopy(OversizedValue);
        });

        Assert.That(ex, Is.Not.Null);
        Assert.That(ex!.ParamName, Is.EqualTo("index"));
        Assert.That(ex.Message, Does.Contain("size_t range"));
    }
}
