using System;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp;

[TestFixture]
public class Test_double_vector
{
    [Test]
    public void ParamsConstructorCopiesInputValues()
    {
        using var vector = new DoubleVector(1.0, 2.5, -3.0);
        Assert.That(vector.Count, Is.EqualTo(3));
        Assert.That(vector[0], Is.EqualTo(1.0).Within(1e-12));
        Assert.That(vector[1], Is.EqualTo(2.5).Within(1e-12));
        Assert.That(vector[2], Is.EqualTo(-3.0).Within(1e-12));
    }

    [Test]
    public void ParamsConstructorRejectsNullArray()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => new DoubleVector(values: null));
        Assert.That(ex!.ParamName, Is.EqualTo("values"));
    }
}
