using System;
using pagmo;

namespace Tests.Pagmo.NET.TestProblems;

// Purpose-built fixture that throws from get_bounds() to verify callback exceptions
// bubble through interop without destabilizing native/managed lifetime teardown.
public sealed class ThrowingBoundsProblem : ManagedProblemBase
{
    public override DoubleVector fitness(DoubleVector x) => new(new[] { 0.0 });

    public override PairOfDoubleVectors get_bounds()
    {
        throw new InvalidOperationException("managed bounds boom");
    }
}

// Purpose-built fixture that throws from fitness() to validate deferred exception capture
// and ensure exception propagation works on runtime evaluation paths.
public sealed class ThrowingFitnessProblem : ManagedProblemBase
{
    public override DoubleVector fitness(DoubleVector x)
    {
        throw new InvalidOperationException("managed fitness boom");
    }

    public override PairOfDoubleVectors get_bounds()
    {
        return new PairOfDoubleVectors(new DoubleVector(new[] { -1.0 }), new DoubleVector(new[] { 1.0 }));
    }

    public override ThreadSafety get_thread_safety() => ThreadSafety.Basic;
}
