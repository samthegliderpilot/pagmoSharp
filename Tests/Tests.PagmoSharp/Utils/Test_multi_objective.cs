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
        var thing = pagmo.pagmo.fast_non_dominated_sorting_wrapped(new VectorOfVectorOfDoubles(new List<DoubleVector>(){new DoubleVector(0.0, 2.0, 1.0), new DoubleVector(5.0, 9.0, 10.0) }));
        Assert.IsNotNull(thing);
    }

    [Test]
    public void TestSomething()
    {
        //pagmo.pagmo.crowding_distance(new VectorDoubleVector(
    }
}