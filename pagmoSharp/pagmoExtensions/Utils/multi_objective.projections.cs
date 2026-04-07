namespace pagmo;

public partial class pagmo
{
    public static bool ParetoDominates(DoubleVector lhsFitness, DoubleVector rhsFitness)
    {
        return pareto_dominance(lhsFitness, rhsFitness);
    }

    public static ulong[] NonDominatedFront2DIndices(VectorOfVectorOfDoubles fitnessValues)
    {
        using var indices = non_dominated_front_2d(fitnessValues);
        return indices.ToArray();
    }

    public static ulong[] SortPopulationMoIndices(VectorOfVectorOfDoubles fitnessValues)
    {
        using var indices = sort_population_mo(fitnessValues);
        return indices.ToArray();
    }

    public static ulong[] SelectBestNMoIndices(VectorOfVectorOfDoubles fitnessValues, ulong selectionCount)
    {
        using var indices = select_best_N_mo(fitnessValues, selectionCount);
        return indices.ToArray();
    }

    public static double[] IdealValues(VectorOfVectorOfDoubles fitnessValues)
    {
        using var values = ideal(fitnessValues);
        return values.ToArray();
    }

    public static double[] NadirValues(VectorOfVectorOfDoubles fitnessValues)
    {
        using var values = nadir(fitnessValues);
        return values.ToArray();
    }

    public static double[] DecomposeObjectiveValues(DoubleVector objectiveValues, DoubleVector weights, DoubleVector referencePoint, string method)
    {
        using var values = decompose_objectives(objectiveValues, weights, referencePoint, method);
        return values.ToArray();
    }
}
