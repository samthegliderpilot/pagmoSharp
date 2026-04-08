using System;
using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp;

[TestFixture]
public class Test_core_facade_null_contracts
{
    [Test]
    public void ProblemConstructorRejectsNullManagedProblem()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => new problem((IProblem)null));
        Assert.That(ex!.ParamName, Is.EqualTo("managedProblem"));
    }

    [Test]
    public void PopulationConstructorRejectsNullManagedProblem()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => new population((IProblem)null, 8u, 2u));
        Assert.That(ex!.ParamName, Is.EqualTo("problem"));
    }

    [Test]
    public void IslandCreateRejectsNullManagedProblem()
    {
        using IAlgorithm algorithm = new bee_colony();
        var ex = Assert.Throws<ArgumentNullException>(() => island.Create(algorithm, (IProblem)null, 8u, 2u));
        Assert.That(ex!.ParamName, Is.EqualTo("managedProblem"));
    }

    [Test]
    public void IslandCreateRejectsNullManagedAlgorithm()
    {
        using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
        var ex = Assert.Throws<ArgumentNullException>(() => island.Create((IAlgorithm)null, problem, 8u, 2u));
        Assert.That(ex!.ParamName, Is.EqualTo("source"));
    }

    [Test]
    public void ArchipelagoPushBackIslandRejectsNullManagedProblem()
    {
        using var archipelago = new archipelago();
        using IAlgorithm algorithm = new bee_colony();

        var ex = Assert.Throws<ArgumentNullException>(() => archipelago.push_back_island(algorithm, null, 8u, 2u));
        Assert.That(ex!.ParamName, Is.EqualTo("problem"));
    }

    [Test]
    public void ArchipelagoPushBackIslandRejectsNullManagedAlgorithm()
    {
        using var archipelago = new archipelago();
        using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();

        var ex = Assert.Throws<ArgumentNullException>(() => archipelago.push_back_island((IAlgorithm)null, problem, 8u, 2u));
        Assert.That(ex!.ParamName, Is.EqualTo("source"));
    }
}
