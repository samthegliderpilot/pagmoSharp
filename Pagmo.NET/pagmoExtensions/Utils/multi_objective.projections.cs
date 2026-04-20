using System;

namespace pagmo;

/// <summary>
/// C#-friendly projection helpers for pagmo multi-objective utility functions.
/// </summary>
public partial class pagmo
{
    private static T RequireNotNull<T>(T value, string parameterName) where T : class
    {
        return value ?? throw new ArgumentNullException(parameterName);
    }

    /// <summary>
    /// Returns <c>true</c> when <paramref name="lhsFitness"/> Pareto-dominates <paramref name="rhsFitness"/>.
    /// </summary>
    public static bool ParetoDominates(DoubleVector lhsFitness, DoubleVector rhsFitness)
    {
        return pareto_dominance(
            RequireNotNull(lhsFitness, nameof(lhsFitness)),
            RequireNotNull(rhsFitness, nameof(rhsFitness)));
    }

    /// <summary>
    /// Returns indices of the non-dominated front for 2D fitness vectors.
    /// </summary>
    public static ulong[] NonDominatedFront2DIndices(VectorOfVectorOfDoubles fitnessValues)
    {
        using var indices = non_dominated_front_2d(RequireNotNull(fitnessValues, nameof(fitnessValues)));
        return indices.ToArray();
    }

    /// <summary>
    /// Returns population indices sorted by multi-objective rank/crowding criteria.
    /// </summary>
    public static ulong[] SortPopulationMoIndices(VectorOfVectorOfDoubles fitnessValues)
    {
        using var indices = sort_population_mo(RequireNotNull(fitnessValues, nameof(fitnessValues)));
        return indices.ToArray();
    }

    /// <summary>
    /// Returns indices of the best <paramref name="selectionCount"/> individuals in a multi-objective set.
    /// </summary>
    public static ulong[] SelectBestNMoIndices(VectorOfVectorOfDoubles fitnessValues, ulong selectionCount)
    {
        using var indices = select_best_N_mo(RequireNotNull(fitnessValues, nameof(fitnessValues)), selectionCount);
        return indices.ToArray();
    }

    /// <summary>
    /// Returns the ideal point across the provided multi-objective fitness vectors.
    /// </summary>
    public static double[] IdealValues(VectorOfVectorOfDoubles fitnessValues)
    {
        using var values = ideal(RequireNotNull(fitnessValues, nameof(fitnessValues)));
        return values.ToArray();
    }

    /// <summary>
    /// Returns the nadir point across the provided multi-objective fitness vectors.
    /// </summary>
    public static double[] NadirValues(VectorOfVectorOfDoubles fitnessValues)
    {
        using var values = nadir(RequireNotNull(fitnessValues, nameof(fitnessValues)));
        return values.ToArray();
    }

    /// <summary>
    /// Decomposes objective values using the selected scalarization <paramref name="method"/>.
    /// </summary>
    public static double[] DecomposeObjectiveValues(DoubleVector objectiveValues, DoubleVector weights, DoubleVector referencePoint, string method)
    {
        using var values = decompose_objectives(
            RequireNotNull(objectiveValues, nameof(objectiveValues)),
            RequireNotNull(weights, nameof(weights)),
            RequireNotNull(referencePoint, nameof(referencePoint)),
            RequireNotNull(method, nameof(method)));
        return values.ToArray();
    }
}
