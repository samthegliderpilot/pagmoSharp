using System.Collections.Generic;

namespace pagmo;

public partial class cstrs_self_adaptive : IAlgorithm
{
    public readonly record struct CstrsLogLine(
        uint Iteration,
        ulong FunctionEvaluations,
        double BestFitness,
        double Infeasibility,
        ulong ViolatedConstraints,
        double ViolationNorm,
        ulong FeasibleCount) : IAlgorithmLogLine
    {
        public string AlgorithmName => "cstrs_self_adaptive";

        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["iteration"] = Iteration,
            ["function_evaluations"] = FunctionEvaluations,
            ["best_fitness"] = BestFitness,
            ["infeasibility"] = Infeasibility,
            ["violated_constraints"] = ViolatedConstraints,
            ["violation_norm"] = ViolationNorm,
            ["feasible_count"] = FeasibleCount
        };

        public string ToDisplayString() =>
            $"iter={Iteration}, fevals={FunctionEvaluations}, best={BestFitness}, infeas={Infeasibility}, violated={ViolatedConstraints}, feasible={FeasibleCount}";
    }

    public IReadOnlyList<CstrsLogLine> GetTypedLogLines()
    {
        using var rawEntries = get_log_entries();
        var lines = new List<CstrsLogLine>(rawEntries.Count);

        for (var i = 0; i < rawEntries.Count; i++)
        {
            using var entry = rawEntries[i];
            lines.Add(new CstrsLogLine(
                entry.iter,
                entry.fevals,
                entry.best,
                entry.infeasibility,
                entry.violated,
                entry.violation_norm,
                entry.feasible));
        }

        return lines;
    }

    public IReadOnlyList<IAlgorithmLogLine> GetLogLines()
    {
        var typedLines = GetTypedLogLines();
        var projected = new List<IAlgorithmLogLine>(typedLines.Count);
        foreach (var line in typedLines)
        {
            projected.Add(line);
        }

        return projected;
    }
}
