using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp.Algorithms;

[TestFixture]
public class Test_cstrs_self_adaptive
{
    [Test]
    public void NameAndBasicPropertiesAreAccessible()
    {
        using var algorithm = new cstrs_self_adaptive(6u);
        Assert.AreEqual("sa-CNSTR: Self-adaptive constraints handling", algorithm.get_name());

        algorithm.set_seed(3u);
        Assert.AreEqual(3u, algorithm.get_seed());

        algorithm.set_verbosity(2u);
        Assert.AreEqual(2u, algorithm.get_verbosity());
        Assert.IsNotNull(algorithm.get_extra_info());
    }

    [Test]
    public void EvolvesConstrainedProblemAndPreservesPopulationSize()
    {
        using var problem = new TwoDimensionalConstrainedProblem();
        using var algorithm = new cstrs_self_adaptive(10u);
        using var pop = new population(problem, 64u, 2u);

        var originalSize = pop.size();
        var evolved = algorithm.evolve(pop);

        Assert.AreEqual(originalSize, evolved.size());
        using var evolvedProblem = evolved.get_problem();
        Assert.AreEqual(problem.get_nobj(), evolvedProblem.get_nobj());
    }

    [Test]
    public void TypedAndGenericLogsAreExposed()
    {
        using var problem = new TwoDimensionalConstrainedProblem();
        using var algorithm = new cstrs_self_adaptive(12u);
        using var population = new population(problem, 64u, 2u);

        algorithm.set_seed(2u);
        algorithm.set_verbosity(1u);

        using var _ = algorithm.evolve(population);

        var typedLines = algorithm.GetTypedLogLines();
        Assert.That(typedLines.Count, Is.GreaterThan(0), "cstrs_self_adaptive verbosity should produce at least one log line");

        IAlgorithm algorithmInterface = algorithm;
        var genericLines = algorithmInterface.GetLogLines();
        Assert.That(genericLines.Count, Is.EqualTo(typedLines.Count));

        var raw = genericLines[0].RawFields;
        Assert.That(genericLines[0].AlgorithmName, Is.EqualTo("cstrs_self_adaptive"));
        Assert.That(raw.ContainsKey("iteration"), Is.True);
        Assert.That(raw.ContainsKey("function_evaluations"), Is.True);
        Assert.That(raw.ContainsKey("best_fitness"), Is.True);
        Assert.That(raw.ContainsKey("infeasibility"), Is.True);
        Assert.That(raw.ContainsKey("violated_constraints"), Is.True);
        Assert.That(raw.ContainsKey("violation_norm"), Is.True);
        Assert.That(raw.ContainsKey("feasible_count"), Is.True);
        Assert.That(genericLines[0].ToDisplayString(), Does.Contain("iter="));

        Assert.That((uint)raw["iteration"], Is.EqualTo(typedLines[0].Iteration));
        Assert.That((ulong)raw["function_evaluations"], Is.EqualTo(typedLines[0].FunctionEvaluations));
        Assert.That((double)raw["best_fitness"], Is.EqualTo(typedLines[0].BestFitness));
        Assert.That((double)raw["infeasibility"], Is.EqualTo(typedLines[0].Infeasibility));
        Assert.That((ulong)raw["feasible_count"], Is.EqualTo(typedLines[0].FeasibleCount));
    }
}
