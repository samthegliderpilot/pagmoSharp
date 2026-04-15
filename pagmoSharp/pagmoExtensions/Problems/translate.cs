using System;

namespace pagmo;

/// <summary>
/// Represents translate. Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
/// </summary>
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

    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
    public static translate Create(IProblem innerProblem, double[] translationVector)
    {
        return Create(innerProblem, new DoubleVector(translationVector));
    }
}

