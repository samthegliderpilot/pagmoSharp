using System;
using NUnit.Framework;
using pagmo;
using Tests.Pagmo.NET.TestProblems;

namespace Tests.Pagmo.NET.Interop;

[TestFixture]
public class Test_native_exception_bubbling
{
    // Throws inside batch_fitness to verify managed callback exceptions survive
    // native transit and are rethrown without process-lifetime corruption.
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

    // Intentionally violates callback contract by returning null fitness so tests can
    // verify wrapper-side contract enforcement and exception bubbling behavior.
    private sealed class NullFitnessProblem : ManagedProblemBase
    {
        public override PairOfDoubleVectors get_bounds()
        {
            return new PairOfDoubleVectors(new DoubleVector(new[] { -1.0 }), new DoubleVector(new[] { 1.0 }));
        }

        public override DoubleVector fitness(DoubleVector x)
        {
            return null;
        }
    }

    // Intentionally violates batch callback contract to validate null-return handling
    // on default_bfe managed callback execution paths.
    private sealed class NullBatchFitnessProblem : ManagedProblemBase
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
            return null;
        }
    }

    // Emits distinct exception messages on repeated calls to ensure deferred-capture
    // logic preserves the first failure that actually caused the operation to fail.
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

        public override ThreadSafety get_thread_safety() => ThreadSafety.Basic;
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

    [Test]
    public void ManagedNullFitnessResultBubblesAsCallbackContractFailure()
    {
        using var managed = new NullFitnessProblem();
        using var x = new DoubleVector(new[] { 0.1 });

        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            using var _ = GradientsAndHessians.EstimateGradient((IProblem)managed, x, 1e-6);
        });

        Assert.That(ex, Is.Not.Null);
        Assert.That(ex!.Message, Does.Contain("returned null"));
        Assert.That(ex.Message, Does.Contain("fitness"));
        Assert.That(ex.InnerException, Is.Not.Null);
        Assert.That(ex.InnerException!.Message, Does.Contain("returned null"));
    }

    [Test]
    public void ManagedNullBatchFitnessResultBubblesAsCallbackContractFailure()
    {
        using var evaluator = new default_bfe();
        using var managed = new NullBatchFitnessProblem();
        using var batchX = new DoubleVector(new[] { 0.25 });

        var ex = Assert.Throws<InvalidOperationException>(() =>
        {
            using var _ = evaluator.Operator(managed, batchX);
        });

        Assert.That(ex, Is.Not.Null);
        Assert.That(ex!.Message, Does.Contain("returned null"));
        Assert.That(ex.Message, Does.Contain("batch_fitness"));
        Assert.That(ex.InnerException, Is.Not.Null);
        Assert.That(ex.InnerException!.Message, Does.Contain("returned null"));
    }

}
