using System;

namespace pagmo;

public partial class translate : IProblem
{
    public static translate Create(IProblem innerProblem, DoubleVector translationVector)
    {
        if (innerProblem == null)
        {
            throw new ArgumentNullException(nameof(innerProblem));
        }

        using var wrappedProblem = new problem(innerProblem);
        return new translate(wrappedProblem, translationVector);
    }

    public static translate Create(IProblem innerProblem, double[] translationVector)
    {
        return Create(innerProblem, new DoubleVector(translationVector));
    }
}
