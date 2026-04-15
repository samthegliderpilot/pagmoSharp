using System;

namespace pagmo;

/// <summary>
/// Represents unconstrain. Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
/// </summary>
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

    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
    public static unconstrain Create(IProblem innerProblem, string method, DoubleVector weights)
    {
        if (innerProblem == null)
        {
            throw new ArgumentNullException(nameof(innerProblem));
        }

        using var wrappedProblem = new problem(innerProblem);
        return new unconstrain(wrappedProblem, method, weights);
    }

    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
    public static unconstrain Create(IProblem innerProblem, string method, double[] weights)
    {
        return Create(innerProblem, method, new DoubleVector(weights));
    }
}

