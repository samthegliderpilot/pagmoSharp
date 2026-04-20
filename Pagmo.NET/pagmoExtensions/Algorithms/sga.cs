using System.Collections.Generic;

namespace pagmo;

/// <summary>
/// Represents sga. Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
/// </summary>
public partial class sga : IAlgorithm
{
    /// <summary>
    /// Represents a typed algorithm log entry projected from pagmo runtime data.
    /// </summary>
    public readonly record struct SgaLogLine(
        uint Generation,
        ulong FunctionEvaluations,
        double BestFitness,
        double Improvement) : IAlgorithmLogLine
    {
        /// <summary>
        /// Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
        /// </summary>
        public string AlgorithmName => "sga";
        /// <summary>
        /// Gets a generic field map for algorithm-agnostic log processing.
        /// </summary>
        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["generation"] = Generation,
            ["function_evaluations"] = FunctionEvaluations,
            ["best_fitness"] = BestFitness,
            ["improvement"] = Improvement
        };
        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public string ToDisplayString() => $"gen={Generation}, fevals={FunctionEvaluations}, best={BestFitness}";
    }

    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
    public IReadOnlyList<SgaLogLine> GetTypedLogLines()
    {
        using var rawEntries = get_log_entries();
        var lines = new List<SgaLogLine>(rawEntries.Count);
        for (var i = 0; i < rawEntries.Count; i++)
        {
            using var entry = rawEntries[i];
            lines.Add(new SgaLogLine(entry.gen, entry.fevals, entry.best, entry.improvement));
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

