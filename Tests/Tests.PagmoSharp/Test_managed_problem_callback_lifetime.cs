using System;
using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp;

[TestFixture]
public class Test_managed_problem_callback_lifetime
{
    [Test]
    public void PopulationFromManagedProblemSurvivesForcedGcBeforeEvolve()
    {
        using var managedProblem = new TwoDimensionalSingleObjectiveProblemWrapper();
        using var population = new population(managedProblem, 32u, 2u);
        using IAlgorithm algorithm = new bee_colony();

        ForceFullCollection();

        using var evolved = algorithm.evolve(population);
        Assert.That(evolved.size(), Is.EqualTo(population.size()));
    }

    [Test]
    public void ArchipelagoManagedProblemPathSurvivesForcedGcBeforeEvolve()
    {
        using var archipelago = new archipelago();
        using var managedProblem = new TwoDimensionalSingleObjectiveProblemWrapper();
        using IAlgorithm algorithm = new bee_colony();

        archipelago.push_back_island(algorithm, managedProblem, 24u, 2u);
        ForceFullCollection();

        archipelago.evolve(1u);
        archipelago.wait_check();
        Assert.That(archipelago.status(), Is.EqualTo(evolve_status.idle));
    }

    private static void ForceFullCollection()
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }
}
