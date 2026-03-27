using System;

namespace pagmo;

public partial class decompose : IProblem
{
    public static decompose Create(IProblem innerProblem, DoubleVector weight, DoubleVector z, string method = "weighted", bool adaptIdeal = false)
    {
        if (innerProblem == null)
        {
            throw new ArgumentNullException(nameof(innerProblem));
        }

        using var wrappedProblem = new problem(innerProblem);
        return new decompose(wrappedProblem, weight, z, method, adaptIdeal);
    }

    public static decompose Create(IProblem innerProblem, double[] weight, double[] z, string method = "weighted", bool adaptIdeal = false)
    {
        return Create(innerProblem, new DoubleVector(weight), new DoubleVector(z), method, adaptIdeal);
    }
}
