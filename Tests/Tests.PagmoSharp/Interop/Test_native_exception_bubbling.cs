using System;
using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp.Interop;

[TestFixture]
public class Test_native_exception_bubbling
{
    [Test]
    public void ManagedBoundsFailureBubblesThroughProblemConstruction()
    {
        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            using var _ = new problem(new ThrowingBoundsProblem());
        });

        Assert.That(ex, Is.Not.Null);
        Assert.That(ex!.Message.ToLowerInvariant(), Does.Contain("managed bounds boom"));
        Assert.That(ex.Message, Does.Not.Contain("Failed to build native pagmo::problem from IProblem callback."));
    }

    [Test]
    public void ManagedFitnessFailureBubblesThroughGradientEstimate()
    {
        using var managed = new ThrowingFitnessProblem();
        using var x = new DoubleVector(new[] { 0.1 });

        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            using var _ = GradientsAndHessians.EstimateGradient((IProblem)managed, x, 1e-6);
        });

        Assert.That(ex, Is.Not.Null);
        Assert.That(ex.Message.ToLowerInvariant(), Does.Contain("managed fitness boom"));
        Assert.That(ex.Message, Does.Not.Contain("Native estimate_gradient() failed."));
    }

}
