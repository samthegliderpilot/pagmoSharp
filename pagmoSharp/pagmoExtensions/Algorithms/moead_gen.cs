using System.Collections.Generic;

namespace pagmo;

/// <summary>
/// Represents moead_gen. Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
/// </summary>
public partial class moead_gen : IAlgorithm
{
    public readonly record struct MoeadGenLogLine(
        uint Generation,
        ulong FunctionEvaluations,
        double DecomposedFitness,
        IReadOnlyList<double> IdealPoint) : IAlgorithmLogLine
    {
        /// <summary>
        /// Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
        /// </summary>
        public string AlgorithmName => "moead_gen";
        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["generation"] = Generation,
            ["function_evaluations"] = FunctionEvaluations,
            ["decomposed_fitness"] = DecomposedFitness,
            ["ideal_point"] = IdealPoint
        };
        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public string ToDisplayString() => $"gen={Generation}, fevals={FunctionEvaluations}, decomp_f={DecomposedFitness}";
    }

    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
    public IReadOnlyList<MoeadGenLogLine> GetTypedLogLines()
    {
        using var rawEntries = get_log_entries();
        var lines = new List<MoeadGenLogLine>(rawEntries.Count);
        for (var i = 0; i < rawEntries.Count; i++)
        {
            using var entry = rawEntries[i];
            using var ideal = entry.ideal_point;
            var values = new List<double>(ideal.Count);
            for (var j = 0; j < ideal.Count; j++) values.Add(ideal[j]);
            lines.Add(new MoeadGenLogLine(entry.gen, entry.fevals, entry.decomposed_f, values));
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

