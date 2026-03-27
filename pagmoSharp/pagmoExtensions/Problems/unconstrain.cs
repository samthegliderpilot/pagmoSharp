using System;

namespace pagmo;

public partial class unconstrain : IProblem
{
    public static unconstrain Create(IProblem innerProblem, string method = "death penalty")
    {
        if (innerProblem == null)
        {
            throw new ArgumentNullException(nameof(innerProblem));
        }

        using var wrappedProblem = new problem(innerProblem);
        return new unconstrain(wrappedProblem, method);
    }

    public static unconstrain Create(IProblem innerProblem, string method, DoubleVector weights)
    {
        if (innerProblem == null)
        {
            throw new ArgumentNullException(nameof(innerProblem));
        }

        using var wrappedProblem = new problem(innerProblem);
        return new unconstrain(wrappedProblem, method, weights);
    }

    public static unconstrain Create(IProblem innerProblem, string method, double[] weights)
    {
        return Create(innerProblem, method, new DoubleVector(weights));
    }
}
