using System.Collections.Generic;

namespace pagmo;

/// <summary>
/// Represents cstrs_self_adaptive. Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
/// </summary>
public partial class cstrs_self_adaptive : IAlgorithm
{
    /// <summary>
    /// Represents a typed algorithm log entry projected from pagmo runtime data.
    /// </summary>
    public readonly record struct CstrsLogLine(
        uint Iteration,
        ulong FunctionEvaluations,
        double BestFitness,
        double Infeasibility,
        ulong ViolatedConstraints,
        double ViolationNorm,
        ulong FeasibleCount) : IAlgorithmLogLine
    {
        /// <summary>
        /// Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
        /// </summary>
        public string AlgorithmName => "cstrs_self_adaptive";

        /// <summary>
        /// Gets a generic field map for algorithm-agnostic log processing.
        /// </summary>
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

        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public string ToDisplayString() =>
            $"iter={Iteration}, fevals={FunctionEvaluations}, best={BestFitness}, infeas={Infeasibility}, violated={ViolatedConstraints}, feasible={FeasibleCount}";
    }

    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
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

    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
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

