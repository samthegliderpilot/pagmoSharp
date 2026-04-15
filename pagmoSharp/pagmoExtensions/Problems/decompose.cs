using System;

namespace pagmo;

/// <summary>
/// Represents decompose. Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
/// </summary>
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

    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
    public static decompose Create(IProblem innerProblem, double[] weight, double[] z, string method = "weighted", bool adaptIdeal = false)
    {
        return Create(innerProblem, new DoubleVector(weight), new DoubleVector(z), method, adaptIdeal);
    }
}

