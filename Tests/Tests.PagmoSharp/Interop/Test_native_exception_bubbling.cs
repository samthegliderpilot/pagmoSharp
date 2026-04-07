using System;
using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp.Interop;

[TestFixture]
public class Test_native_exception_bubbling
{
    private sealed class ThrowingBatchFitnessProblem : ManagedProblemBase
    {
        public override PairOfDoubleVectors get_bounds()
        {
            return new PairOfDoubleVectors(new DoubleVector(new[] { -1.0 }), new DoubleVector(new[] { 1.0 }));
        }

        public override bool has_batch_fitness() => true;

        public override DoubleVector fitness(DoubleVector x)
        {
            return new DoubleVector(new[] { 0.0 });
        }

        public override DoubleVector batch_fitness(DoubleVector x)
        {
            throw new InvalidOperationException("managed batch fitness boom");
        }
    }

    private sealed class ThrowingFitnessProblemWithChangingMessage : ManagedProblemBase
    {
        private int _callCount;

        public override PairOfDoubleVectors get_bounds()
        {
            return new PairOfDoubleVectors(new DoubleVector(new[] { -1.0 }), new DoubleVector(new[] { 1.0 }));
        }

        public override DoubleVector fitness(DoubleVector x)
        {
            _callCount++;
            throw new InvalidOperationException($"managed fitness boom {_callCount}");
        }

        public override thread_safety get_thread_safety() => thread_safety.basic;
    }

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
        Assert.That(ex.InnerException, Is.Null);
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
        Assert.That(ex.InnerException, Is.Not.Null);
        Assert.That(ex.InnerException, Is.TypeOf<InvalidOperationException>());
        Assert.That(ex.InnerException!.Message.ToLowerInvariant(), Does.Contain("managed fitness boom"));
    }

    [Test]
    public void ManagedFitnessFailureBubblesThroughGradientHEstimate()
    {
        using var managed = new ThrowingFitnessProblem();
        using var x = new DoubleVector(new[] { 0.1 });

        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            using var _ = GradientsAndHessians.EstimateGradientHighOrder((IProblem)managed, x, 1e-6);
        });

        Assert.That(ex, Is.Not.Null);
        Assert.That(ex.Message.ToLowerInvariant(), Does.Contain("managed fitness boom"));
        Assert.That(ex.Message, Does.Not.Contain("Native estimate_gradient_h() failed."));
        Assert.That(ex.InnerException, Is.Not.Null);
        Assert.That(ex.InnerException, Is.TypeOf<InvalidOperationException>());
        Assert.That(ex.InnerException!.Message.ToLowerInvariant(), Does.Contain("managed fitness boom"));
    }

    [Test]
    public void ManagedFitnessFailureBubblesThroughSparsityEstimate()
    {
        using var managed = new ThrowingFitnessProblem();
        using var x = new DoubleVector(new[] { 0.1 });

        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            using var _ = GradientsAndHessians.EstimateSparsity((IProblem)managed, x, 1e-6);
        });

        Assert.That(ex, Is.Not.Null);
        Assert.That(ex.Message.ToLowerInvariant(), Does.Contain("managed fitness boom"));
        Assert.That(ex.Message, Does.Not.Contain("Native estimate_sparsity() failed."));
        Assert.That(ex.InnerException, Is.Not.Null);
        Assert.That(ex.InnerException, Is.TypeOf<InvalidOperationException>());
        Assert.That(ex.InnerException!.Message.ToLowerInvariant(), Does.Contain("managed fitness boom"));
    }

    [Test]
    public void ManagedBoundsFailureBubblesThroughManagedPopulationConstruction()
    {
        using var managed = new ThrowingBoundsProblem();
        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            using var _ = new population((IProblem)managed, 4u, 2u);
        });

        Assert.That(ex, Is.Not.Null);
        Assert.That(ex.Message.ToLowerInvariant(), Does.Contain("managed bounds boom"));
        Assert.That(ex.Message, Does.Not.Contain("Failed to create native pagmo::population."));
        Assert.That(ex.InnerException, Is.Null);
    }

    [Test]
    public void ManagedBatchFitnessFailureBubblesThroughDefaultBfe()
    {
        using var evaluator = new default_bfe();
        using var managed = new ThrowingBatchFitnessProblem();
        using var batchX = new DoubleVector(new[] { 0.25 });

        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            using var _ = evaluator.Operator(managed, batchX);
        });

        Assert.That(ex, Is.Not.Null);
        Assert.That(ex!.Message.ToLowerInvariant(), Does.Contain("managed batch fitness boom"));
        Assert.That(ex.InnerException, Is.Not.Null);
        Assert.That(ex.InnerException, Is.TypeOf<InvalidOperationException>());
        Assert.That(ex.InnerException!.Message.ToLowerInvariant(), Does.Contain("managed batch fitness boom"));
    }

    [Test]
    public void DeferredCallbackExceptionPreservesFirstFailureMessage()
    {
        using var managed = new ThrowingFitnessProblemWithChangingMessage();
        using var x = new DoubleVector(new[] { 0.1 });

        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            using var _ = GradientsAndHessians.EstimateGradient((IProblem)managed, x, 1e-6);
        });

        Assert.That(ex, Is.Not.Null);
        Assert.That(ex!.Message.ToLowerInvariant(), Does.Contain("managed fitness boom 1"));
        Assert.That(ex.Message.ToLowerInvariant(), Does.Not.Contain("managed fitness boom 2"));
        Assert.That(ex.InnerException, Is.Not.Null);
        Assert.That(ex.InnerException, Is.TypeOf<InvalidOperationException>());
        Assert.That(ex.InnerException!.Message.ToLowerInvariant(), Does.Contain("managed fitness boom 1"));
    }

}
