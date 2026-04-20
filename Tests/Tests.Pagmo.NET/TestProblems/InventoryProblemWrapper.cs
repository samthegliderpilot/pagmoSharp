using System;
using pagmo;

namespace Tests.Pagmo.NET.TestProblems;

// Adapter fixture that exposes pagmo's built-in inventory UDP through TestProblemWrapper
// so stochastic algorithm base tests can run through one shared managed test harness.
public class InventoryProblemWrapper : TestProblemWrapper
{
    public InventoryProblemWrapper()
    {
        InventoryProblem = new inventory(4u, 10u, 2u);
    }

    public readonly inventory InventoryProblem;

    public override double ExpectedOptimalFunctionValue
    {
        get { return 240.85449403381608; }
    }
    public override double[] ExpectedOptimalX
    {
        get { return new[] { 57.665307645771854 }; }
    }

    public override DoubleVector fitness(DoubleVector decisionVector)
    {
        return InventoryProblem.fitness(decisionVector);
    }

    public override PairOfDoubleVectors get_bounds()
    {
        return InventoryProblem.get_bounds();
    }
}
