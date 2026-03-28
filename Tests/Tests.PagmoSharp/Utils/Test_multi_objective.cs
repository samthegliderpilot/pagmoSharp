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
        pagmo.RekSum.reksum(thing, new ULongLongVector(new ulong[]{1L, 2L, 3L}), 1, 2);
    }

    [Test]
    public void TestDecompositionWeights()
    {
        var ex = Assert.Throws<ApplicationException>(
            () => pagmo.DecompositionWeights.decomposition_weights(2, 2, "huh"));
        Assert.That(ex!.Message, Does.Contain("unknown"));

        var weights = pagmo.DecompositionWeights.decomposition_weights(2, 4, "grid");
        Assert.That(weights.Count, Is.GreaterThan(0));
    }
}
