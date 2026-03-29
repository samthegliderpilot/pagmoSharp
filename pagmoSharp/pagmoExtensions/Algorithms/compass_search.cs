using System.Collections.Generic;

namespace pagmo;

public partial class compass_search : IAlgorithm
{
    public readonly record struct CompassSearchLogLine(
        ulong FunctionEvaluations,
        double BestFitness,
        ulong ViolatedConstraints,
        double ViolationNorm,
        double Range) : IAlgorithmLogLine
    {
        public string AlgorithmName => "compass_search";
        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["function_evaluations"] = FunctionEvaluations,
            ["best_fitness"] = BestFitness,
            ["violated_constraints"] = ViolatedConstraints,
            ["violation_norm"] = ViolationNorm,
            ["range"] = Range
        };
        public string ToDisplayString() => $"fevals={FunctionEvaluations}, best={BestFitness}, range={Range}";
    }

    public IReadOnlyList<CompassSearchLogLine> GetTypedLogLines()
    {
        using var rawEntries = get_log_entries();
        var lines = new List<CompassSearchLogLine>(rawEntries.Count);
        for (var i = 0; i < rawEntries.Count; i++)
        {
            using var entry = rawEntries[i];
            lines.Add(new CompassSearchLogLine(entry.fevals, entry.best, entry.violated, entry.violation_norm, entry.range));
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
