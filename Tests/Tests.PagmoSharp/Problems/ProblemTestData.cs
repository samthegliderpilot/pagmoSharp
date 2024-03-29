﻿using System;
using System.Runtime.InteropServices;

namespace Tests.PagmoSharp.Problems;

[Serializable]
public class ProblemTestData
{
    public ProblemTestData()
    {
    }

    public ProblemTestData(string problemName, string testCaseName, double[] x, double[] y, uint problemIndex = 0)
    {
        ProblemName = problemName;
        TestCaseName = testCaseName;
        X = x;
        Y = y;
        ProblemIndex = problemIndex;
    }

    public string ProblemName { get; init; }
    public string TestCaseName { get; init; }
    public double[] X { get; init; }
    public double[] Y { get; init; }
    public uint ProblemIndex { get; init; }

    public string WriteTestCaseSource(double[] newX = null, double[] newY = null)
    {
        if (newX == null)
        {
            newX = X;
        }

        string xString = "";
        foreach (var x in newX)
        {
            xString += x + ", ";
        }

        if (newY == null)
        {
            newY = Y;
        }
        string yString = "";
        foreach (var y in newY)
        {
            yString += y + ", ";
        }
        return "testData.Add(new ProblemTestData(\"" + ProblemName + "\", \"" + TestCaseName + "\", new double[] {" + xString + "}, new double[] {" + yString + $"}}, {this.ProblemIndex}));";
    }
}