using NUnit.Framework;
using NUnit.Framework.Interfaces;
using pagmo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests.PagmoSharp.Problems;

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
    
    
    [TestCaseSource("RegressionData")]
    public void TestEvaluationRegression(ProblemTestData testData)
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