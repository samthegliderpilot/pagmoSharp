using NUnit.Framework;
using pagmo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Pagmo.NET.Problems;

// Shared contract tests for wrapped pagmo problem types.
// Derived fixtures only provide factory/regression data; this base enforces common wrapper invariants.
public abstract class TestProblemBase
{
    public abstract IProblem CreateStandardProblem(uint problemIndex = 0);

    public abstract void TestBoilerPlate();

    public abstract void TestOptimizing();

    public static bool UpdateTestData
    {
        get { return true; }
    }

    public virtual IEnumerable<uint> MultiProblemIndices
    {
        get { return new[] { 0u }; }
    }

    protected virtual IEnumerable<ProblemTestData> GetRegressionData()
    {
        return Enumerable.Empty<ProblemTestData>();
    }

    protected virtual bool SupportsMidpointFitnessProbe()
    {
        return true;
    }

    [Test]
    public void TestBase_CanConstructAndReadMetadata()
    {
        using var problem = CreateStandardProblem();
        var name = problem.get_name();

        Assert.That(name, Is.Not.Empty, "name should not be empty");
        Assert.GreaterOrEqual(problem.get_nobj(), 1u, "objective count should be >= 1");
    }

    [Test]
    public void TestBase_BoundsAreWellFormed()
    {
        using var problem = CreateStandardProblem();
        using var bounds = problem.get_bounds();

        Assert.AreEqual(bounds.first.Count, bounds.second.Count, "lower/upper bounds vector sizes must match");
        Assert.Greater(bounds.first.Count, 0, "problem dimension must be > 0");

        for (var i = 0; i < bounds.first.Count; i++)
        {
            Assert.LessOrEqual(bounds.first[i], bounds.second[i], $"lower bound > upper bound at index {i}");
        }
    }

    [Test]
    public void TestBase_FitnessVectorSizeMatchesDeclaredCounts()
    {
        if (!SupportsMidpointFitnessProbe())
        {
            Assert.Pass("Midpoint probe is disabled for this problem.");
            return;
        }

        using var problem = CreateStandardProblem();
        using var bounds = problem.get_bounds();
        var midpointValues = new double[bounds.first.Count];
        for (var i = 0; i < midpointValues.Length; i++)
        {
            midpointValues[i] = 0.5 * (bounds.first[i] + bounds.second[i]);
        }
        using var x = new DoubleVector(midpointValues);

        using var fitness = problem.fitness(x);
        var expected = problem.get_nobj() + problem.get_nec() + problem.get_nic();
        Assert.AreEqual(expected, (uint)fitness.Count, "fitness output length should match nobj + nec + nic");
    }

    [Test]
    public void TestEvaluationRegression()
    {
        var regressionData = GetRegressionData().ToList();
        if (regressionData.Count == 0)
        {
            Assert.Pass("No regression data defined for this problem wrapper.");
            return;
        }

        foreach (var testData in regressionData)
        {
            using var problem = CreateStandardProblem(testData.ProblemIndex);
            var x = new DoubleVector(testData.X);
            var fitness = problem.fitness(x);
            Assert.AreEqual(fitness.Count, testData.Y.Length,
                testData.TestCaseName + " length of returned data was wrong" + Environment.NewLine +
                testData.WriteTestCaseSource(x.ToArray(), fitness.ToArray()));
            for (int i = 0; i < testData.Y.Length; i++)
            {
                Assert.AreEqual(testData.Y[i], fitness[i], 1e-10,
                    testData.TestCaseName + " value at " + i + Environment.NewLine +
                    testData.WriteTestCaseSource(x.ToArray(), fitness.ToArray()));
            }
        }
    }
}
