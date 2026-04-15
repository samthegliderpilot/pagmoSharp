using System.Collections.Generic;

namespace pagmo;

/// <summary>
/// Represents nsga2. Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
/// </summary>
public partial class nsga2 : IAlgorithm
{
    public readonly record struct Nsga2LogLine(
        uint Generation,
        ulong FunctionEvaluations,
        IReadOnlyList<double> FitnessVector) : IAlgorithmLogLine
    {
        /// <summary>
        /// Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
        /// </summary>
        public string AlgorithmName => "nsga2";
        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["generation"] = Generation,
            ["function_evaluations"] = FunctionEvaluations,
            ["fitness_vector"] = FitnessVector
        };
        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public string ToDisplayString() => $"gen={Generation}, fevals={FunctionEvaluations}, objectives={FitnessVector.Count}";
    }

    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
    public IReadOnlyList<Nsga2LogLine> GetTypedLogLines()
    {
        using var rawEntries = get_log_entries();
        var lines = new List<Nsga2LogLine>(rawEntries.Count);
        for (var i = 0; i < rawEntries.Count; i++)
        {
            using var entry = rawEntries[i];
            using var fitness = entry.fitness;
            var values = new List<double>(fitness.Count);
            for (var j = 0; j < fitness.Count; j++) values.Add(fitness[j]);
            lines.Add(new Nsga2LogLine(entry.gen, entry.fevals, values));
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

