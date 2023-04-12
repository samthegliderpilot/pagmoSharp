using NUnit.Framework;
using pagmo;
using System;
using System.Collections.Generic;

namespace Tests.PagmoSharp.Problems;

public abstract class TestProblemBase
{
    public abstract IProblem CreateStandardProblem();

    public abstract void TestBoilerPlate();

    public abstract void TestOptimizing();

    public static bool UpdateTestData
    {
        get { return true; }
    }

    public abstract IEnumerable<ProblemTestData> RegressionData();

    [Test]
    public void TestEvaluationRegression()
    {
        foreach (var testData in RegressionData())
        {
            using var problem = CreateStandardProblem();
            var x = new DoubleVector(testData.X);
            var fitness = problem.fitness(x);
            Assert.AreEqual(fitness.Count, testData.Y.Length,
                testData.TestCaseName + " length of returned data was wrong");
            for (int i = 0; i < testData.Y.Length; i++)
            {
                Assert.AreEqual(testData.Y[i], fitness[i], 1e-10,
                    testData.TestCaseName + " value at " + i + Environment.NewLine +
                    testData.WriteTestCaseSource(x.ToArray(), fitness.ToArray()));
            }
        }
    }
}