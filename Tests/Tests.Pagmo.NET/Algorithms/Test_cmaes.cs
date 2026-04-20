using NUnit.Framework;
using pagmo;
using Tests.Pagmo.NET.TestProblems;

namespace Tests.Pagmo.NET.Algorithms;

[TestFixture]
public class Test_cmaes : TestAlgorithmBase
{
    public override IAlgorithm CreateAlgorithm()
    {
        return new pagmo.cmaes(10u, 1u);
    }

    [Test]
    public override void TestNameIsCorrect()
    {
        using (var problem = new TwoDimensionalSingleObjectiveProblemWrapper())
        using (var algorithm = CreateAlgorithm(problem))
        {
            Assert.AreEqual("CMA-ES: Covariance Matrix Adaptation Evolutionary Strategy", algorithm.get_name());
        }
    }

    public override bool SupportsGeneration => false;

    /// <inheritdoc />
    public override bool Constrained => false;

    /// <inheritdoc />
    public override bool Unconstrained => true;

    /// <inheritdoc />
    public override bool SingleObjective => true;

    /// <inheritdoc />
    public override bool MultiObjective => false;

    /// <inheritdoc />
    public override bool IntegerProgramming => false;

    /// <inheritdoc />
    public override bool Stochastic => true;

    [Test]
    public void TypedAndGenericLogsAreExposed()
    {
        using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
        using var algorithm = new pagmo.cmaes(30u, 1u);
        using var population = new population(problem, 64u, 2u);

        algorithm.set_seed(2u);
        algorithm.set_verbosity(1u);

        using var _ = algorithm.evolve(population);

        var typedLines = algorithm.GetTypedLogLines();
        Assert.That(typedLines.Count, Is.GreaterThan(0), "cmaes verbosity should produce at least one log line");

        IAlgorithm algorithmInterface = algorithm;
        var genericLines = algorithmInterface.GetLogLines();
        Assert.That(genericLines.Count, Is.EqualTo(typedLines.Count));

        var raw = genericLines[0].RawFields;
        Assert.That(genericLines[0].AlgorithmName, Is.EqualTo("cmaes"));
        Assert.That(raw.ContainsKey("generation"), Is.True);
        Assert.That(raw.ContainsKey("function_evaluations"), Is.True);
        Assert.That(raw.ContainsKey("best_fitness"), Is.True);
        Assert.That(raw.ContainsKey("sigma"), Is.True);
        Assert.That(raw.ContainsKey("min_variance"), Is.True);
        Assert.That(raw.ContainsKey("max_variance"), Is.True);
        Assert.That(genericLines[0].ToDisplayString(), Does.Contain("gen="));

        Assert.That((uint)raw["generation"], Is.EqualTo(typedLines[0].Generation));
        Assert.That((ulong)raw["function_evaluations"], Is.EqualTo(typedLines[0].FunctionEvaluations));
        Assert.That((double)raw["best_fitness"], Is.EqualTo(typedLines[0].BestFitness));
    }
}

