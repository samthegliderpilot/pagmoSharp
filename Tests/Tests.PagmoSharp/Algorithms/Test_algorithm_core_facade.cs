using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp.Algorithms;

[TestFixture]
public class Test_algorithm_core_facade
{
    [Test]
    public void TypeErasedAlgorithmExposesUniversalLogSurface()
    {
        using var typedAlgorithm = new bee_colony();
        using var typeErased = typedAlgorithm.to_algorithm();

        var logLines = typeErased.GetLogLines();
        Assert.That(logLines, Is.Not.Null);
        Assert.That(logLines.Count, Is.EqualTo(0), "type-erased algorithm facade intentionally exposes default-empty universal log surface.");
    }

    [Test]
    public void TypeErasedAlgorithmStillEvolvesPopulation()
    {
        using var typedAlgorithm = new bee_colony();
        using var typeErased = typedAlgorithm.to_algorithm();
        using var managedProblem = new TwoDimensionalSingleObjectiveProblemWrapper();
        using var problem = new problem(managedProblem);
        using var initialPopulation = new population(problem, 24u, 2u);

        using var evolved = typeErased.evolve(initialPopulation);
        Assert.That(evolved.size(), Is.EqualTo(initialPopulation.size()));
    }
}
