using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Utils;

[TestFixture]
public class Test_multi_objective
{
    [Test]
    public void TestPareto_dominance()
    {        
        Assert.False(pagmo.pagmo.pareto_dominance(new DoubleVector{1.0, 2.0, 3.0}, new DoubleVector(3.0, 2.0, 1.0)));
    }

    [Test]
    public void TestSomething()
    {
        //pagmo.pagmo.crowding_distance(new VectorDoubleVector(
    }
}