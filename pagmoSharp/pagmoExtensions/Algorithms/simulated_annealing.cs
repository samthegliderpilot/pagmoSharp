using System.Collections.Generic;

namespace pagmo;

public partial class simulated_annealing : IAlgorithm
{
    public readonly record struct SimulatedAnnealingLogLine(
        ulong FunctionEvaluations,
        double BestFitness,
        double CurrentFitness,
        double Temperature,
        double MoveRange) : IAlgorithmLogLine
    {
        public string AlgorithmName => "simulated_annealing";
        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["function_evaluations"] = FunctionEvaluations,
            ["best_fitness"] = BestFitness,
            ["current_fitness"] = CurrentFitness,
            ["temperature"] = Temperature,
            ["move_range"] = MoveRange
        };
        public string ToDisplayString() => $"fevals={FunctionEvaluations}, best={BestFitness}, temp={Temperature}";
    }

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

    public IReadOnlyList<IAlgorithmLogLine> GetLogLines()
    {
        var typedLines = GetTypedLogLines();
        var projected = new List<IAlgorithmLogLine>(typedLines.Count);
        foreach (var line in typedLines) projected.Add(line);
        return projected;
    }
}
