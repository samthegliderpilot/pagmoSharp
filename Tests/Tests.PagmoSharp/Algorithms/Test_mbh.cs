using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Algorithms;

[TestFixture]
public class Test_mbh
{
    [Test]
    public void NameAndBasicPropertiesAreAccessible()
    {
        using var algorithm = new mbh();
        Assert.AreEqual("MBH: Monotonic Basin Hopping - Generalized", algorithm.get_name());

        algorithm.set_seed(7u);
        Assert.AreEqual(7u, algorithm.get_seed());

        algorithm.set_verbosity(2u);
        Assert.AreEqual(2u, algorithm.get_verbosity());
        Assert.That(algorithm.get_extra_info(), Is.Not.Null);
    }

    [Test]
    public void PerturbationCanBeUpdated()
    {
        using var algorithm = new mbh();
        using var perturb = new DoubleVector(new[] { 0.15, 0.25 });

        algorithm.set_perturb(perturb);
        using var configured = algorithm.get_perturb();

        Assert.AreEqual(2, configured.Count);
        Assert.AreEqual(0.15, configured[0], 1e-12);
        Assert.AreEqual(0.25, configured[1], 1e-12);
    }

    [Test]
    public void TypedAndGenericLogsAreExposed()
    {
        using var inner = new ihs(40u);
        using var innerAlgo = inner.to_algorithm();
        using var algorithm = new mbh(innerAlgo, 3u, 0.05);
        using var problem = new rosenbrock(2u);
        using var population = new population(problem, 48u, 2u);
        using var initialProblem = population.get_problem();
        var initialFevals = initialProblem.get_fevals();

        algorithm.set_seed(2u);
        algorithm.set_verbosity(1u);

        using var evolvedPopulation = algorithm.evolve(population);
        using var evolvedProblem = evolvedPopulation.get_problem();
        Assert.Greater(evolvedProblem.get_fevals(), initialFevals, "evolution should trigger additional function evaluations");

        var typedLines = algorithm.GetTypedLogLines();
        Assert.That(typedLines.Count, Is.GreaterThan(0), "mbh verbosity should produce at least one log line");

        IAlgorithm algorithmInterface = algorithm;
        var genericLines = algorithmInterface.GetLogLines();
        Assert.That(genericLines.Count, Is.EqualTo(typedLines.Count));

        var raw = genericLines[0].RawFields;
        Assert.That(genericLines[0].AlgorithmName, Is.EqualTo("mbh"));
        Assert.That(raw.ContainsKey("function_evaluations"), Is.True);
        Assert.That(raw.ContainsKey("best_fitness"), Is.True);
        Assert.That(raw.ContainsKey("violated_constraints"), Is.True);
        Assert.That(raw.ContainsKey("violation_norm"), Is.True);
        Assert.That(raw.ContainsKey("trial"), Is.True);
        Assert.That(genericLines[0].ToDisplayString(), Does.Contain("fevals="));

        Assert.That((ulong)raw["function_evaluations"], Is.EqualTo(typedLines[0].FunctionEvaluations));
        Assert.That((double)raw["best_fitness"], Is.EqualTo(typedLines[0].BestFitness));
    }
}
