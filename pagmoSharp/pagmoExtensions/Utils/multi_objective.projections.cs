using System;

namespace pagmo;

public partial class pagmo
{
    private static T RequireNotNull<T>(T value, string parameterName) where T : class
    {
        return value ?? throw new ArgumentNullException(parameterName);
    }

    public static bool ParetoDominates(DoubleVector lhsFitness, DoubleVector rhsFitness)
    {
        return pareto_dominance(
            RequireNotNull(lhsFitness, nameof(lhsFitness)),
            RequireNotNull(rhsFitness, nameof(rhsFitness)));
    }

    public static ulong[] NonDominatedFront2DIndices(VectorOfVectorOfDoubles fitnessValues)
    {
        using var indices = non_dominated_front_2d(RequireNotNull(fitnessValues, nameof(fitnessValues)));
        return indices.ToArray();
    }

    public static ulong[] SortPopulationMoIndices(VectorOfVectorOfDoubles fitnessValues)
    {
        using var indices = sort_population_mo(RequireNotNull(fitnessValues, nameof(fitnessValues)));
        return indices.ToArray();
    }

    public static ulong[] SelectBestNMoIndices(VectorOfVectorOfDoubles fitnessValues, ulong selectionCount)
    {
        using var indices = select_best_N_mo(RequireNotNull(fitnessValues, nameof(fitnessValues)), selectionCount);
        return indices.ToArray();
    }

    public static double[] IdealValues(VectorOfVectorOfDoubles fitnessValues)
    {
        using var values = ideal(RequireNotNull(fitnessValues, nameof(fitnessValues)));
        return values.ToArray();
    }

    public static double[] NadirValues(VectorOfVectorOfDoubles fitnessValues)
    {
        using var values = nadir(RequireNotNull(fitnessValues, nameof(fitnessValues)));
        return values.ToArray();
    }

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
