using System.Collections.Generic;

namespace pagmo;

/// <summary>
/// Represents de1220. Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
/// </summary>
public partial class de1220 : IAlgorithm
{
    /// <summary>
    /// Represents a typed algorithm log entry projected from pagmo runtime data.
    /// </summary>
    public readonly record struct De1220LogLine(
        uint Generation,
        ulong FunctionEvaluations,
        double BestFitness,
        double FunctionEvaluationDifference,
        double Dx,
        uint Variant,
        double F,
        double Cr) : IAlgorithmLogLine
    {
        /// <summary>
        /// Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
        /// </summary>
        public string AlgorithmName => "de1220";
        /// <summary>
        /// Gets a generic field map for algorithm-agnostic log processing.
        /// </summary>
        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["generation"] = Generation,
            ["function_evaluations"] = FunctionEvaluations,
            ["best_fitness"] = BestFitness,
            ["function_evaluation_difference"] = FunctionEvaluationDifference,
            ["dx"] = Dx,
            ["variant"] = Variant,
            ["f"] = F,
            ["cr"] = Cr
        };
        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public string ToDisplayString() => $"gen={Generation}, fevals={FunctionEvaluations}, best={BestFitness}, variant={Variant}";
    }

    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
    public IReadOnlyList<De1220LogLine> GetTypedLogLines()
    {
        using var rawEntries = get_log_entries();
        var lines = new List<De1220LogLine>(rawEntries.Count);
        for (var i = 0; i < rawEntries.Count; i++)
        {
            using var entry = rawEntries[i];
            lines.Add(new De1220LogLine(entry.gen, entry.fevals, entry.best, entry.feval_difference, entry.dx, entry.variant, entry.f, entry.cr));
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
        foreach (var line in typedLines) projected.Add(line);
        return projected;
    }
}

