using System;
using System.Collections.Generic;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Utils;

[TestFixture]
public class Test_multi_objective
{
    [Test]
    public void TestPareto_dominance()
    {
        var thing = pagmo.pagmo.FastNonDominatedSorting(new VectorOfVectorOfDoubles(new List<DoubleVector>(){new DoubleVector(0.0, 2.0, 1.0), new DoubleVector(5.0, 9.0, 10.0) }));
        Assert.IsNotNull(thing);
    }

    [Test]
    public void TestRekSum()
    {
        var thing = new VectorOfVectorOfDoubles(new List<DoubleVector>()
            { new DoubleVector(0.0, 2.0, 1.0), new DoubleVector(5.0, 9.0, 10.0) });
        pagmo.RekSum.reksum(thing, new ULongLongVector(new ulong[]{1l, 2l, 3l}), 1, 2);
    }

    [Test]
    public void TestDecompositionWeights()
    {
        pagmo.DecompositionWeights.decomposition_weights(2, 2, "huh");

        try
        {
            pagmo.DecompositionWeights.decomposition_weights(2, 2, "huh");

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}