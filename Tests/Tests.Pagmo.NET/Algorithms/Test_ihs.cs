using System.Collections.Generic;
using NUnit.Framework;
using pagmo;
using Tests.Pagmo.NET.TestProblems;

namespace Tests.Pagmo.NET.Algorithms;

[TestFixture]
public class Test_ihs : TestAlgorithmBase
{
    public override IAlgorithm CreateAlgorithm()
    {
        return new ihs(100u);
    }

    [Test]
    public override void TestNameIsCorrect()
    {
        using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
        using var algorithm = CreateAlgorithm(problem);
        Assert.AreEqual("IHS: Improved Harmony Search", algorithm.get_name());
    }

    public override bool SupportsGeneration => false;
    public override bool Constrained => false;
    public override bool Unconstrained => true;
    public override bool SingleObjective => true;
    public override bool MultiObjective => false;
    public override bool IntegerProgramming => false;
    public override bool Stochastic => true;

    [Test]
    public void TypedAndGenericLogsAreExposed()
    {
        using var algorithm = new ihs(40u);
        using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
        using var population = new population(problem, 48u, 2u);

        algorithm.set_seed(2u);
        algorithm.set_verbosity(1u);

        using var _ = algorithm.evolve(population);

        var typedLines = algorithm.GetTypedLogLines();
        Assert.That(typedLines.Count, Is.GreaterThan(0), "ihs verbosity should produce at least one log line");

        IAlgorithm algorithmInterface = algorithm;
        var genericLines = algorithmInterface.GetLogLines();
        Assert.That(genericLines.Count, Is.EqualTo(typedLines.Count));

        var raw = genericLines[0].RawFields;
        Assert.That(genericLines[0].AlgorithmName, Is.EqualTo("ihs"));
        Assert.That(raw.ContainsKey("function_evaluations"), Is.True);
        Assert.That(raw.ContainsKey("pitch_adjustment_rate"), Is.True);
        Assert.That(raw.ContainsKey("bandwidth"), Is.True);
        Assert.That(raw.ContainsKey("decision_flatness"), Is.True);
        Assert.That(raw.ContainsKey("fitness_flatness"), Is.True);
        Assert.That(raw.ContainsKey("violated_constraints"), Is.True);
        Assert.That(raw.ContainsKey("violation_norm"), Is.True);
        Assert.That(raw.ContainsKey("ideal_point"), Is.True);
        Assert.That(genericLines[0].ToDisplayString(), Does.Contain("fevals="));

        Assert.That((ulong)raw["function_evaluations"], Is.EqualTo(typedLines[0].FunctionEvaluations));
        Assert.That((double)raw["pitch_adjustment_rate"], Is.EqualTo(typedLines[0].PitchAdjustmentRate));
        Assert.That((double)raw["bandwidth"], Is.EqualTo(typedLines[0].Bandwidth));
        Assert.That((ulong)raw["violated_constraints"], Is.EqualTo(typedLines[0].ViolatedConstraints));

        var rawIdealPoint = (IReadOnlyList<double>)raw["ideal_point"];
        Assert.That(rawIdealPoint.Count, Is.EqualTo(typedLines[0].IdealPoint.Count));
    }
}

