using System;
using pagmo;

namespace Tests.PagmoSharp.TestProblems;

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

    public override DoubleVector fitness(DoubleVector arg0)
    {
        return InventoryProblem.fitness(arg0);
    }

    public override PairOfDoubleVectors get_bounds()
    {
        return InventoryProblem.get_bounds();
    }
}