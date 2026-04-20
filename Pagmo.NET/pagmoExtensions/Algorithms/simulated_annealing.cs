using System.Collections.Generic;

namespace pagmo;

/// <summary>
/// Represents simulated_annealing. Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
/// </summary>
public partial class simulated_annealing : IAlgorithm
{
    /// <summary>
    /// Represents a typed algorithm log entry projected from pagmo runtime data.
    /// </summary>
    public readonly record struct SimulatedAnnealingLogLine(
        ulong FunctionEvaluations,
        double BestFitness,
        double CurrentFitness,
        double Temperature,
        double MoveRange) : IAlgorithmLogLine
    {
        /// <summary>
        /// Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
        /// </summary>
        public string AlgorithmName => "simulated_annealing";
        /// <summary>
        /// Gets a generic field map for algorithm-agnostic log processing.
        /// </summary>
        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["function_evaluations"] = FunctionEvaluations,
            ["best_fitness"] = BestFitness,
            ["current_fitness"] = CurrentFitness,
            ["temperature"] = Temperature,
            ["move_range"] = MoveRange
        };
        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public string ToDisplayString() => $"fevals={FunctionEvaluations}, best={BestFitness}, temp={Temperature}";
    }

    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
    public IReadOnlyList<SimulatedAnnealingLogLine> GetTypedLogLines()
    {
        using var rawEntries = get_log_entries();
        var lines = new List<SimulatedAnnealingLogLine>(rawEntries.Count);
        for (var i = 0; i < rawEntries.Count; i++)
        {
            using var entry = rawEntries[i];
            lines.Add(new SimulatedAnnealingLogLine(entry.fevals, entry.best, entry.current, entry.temperature, entry.move_range));
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

