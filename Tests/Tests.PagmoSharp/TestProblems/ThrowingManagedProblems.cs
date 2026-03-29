using System;
using pagmo;

namespace Tests.PagmoSharp.TestProblems;

public sealed class ThrowingBoundsProblem : ManagedProblemBase
{
    public override DoubleVector fitness(DoubleVector x) => new(new[] { 0.0 });

    public override PairOfDoubleVectors get_bounds()
    {
        throw new InvalidOperationException("managed bounds boom");
    }
}

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

    public override thread_safety get_thread_safety() => thread_safety.basic;
}
