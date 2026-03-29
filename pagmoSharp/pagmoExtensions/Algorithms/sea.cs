using System.Collections.Generic;

namespace pagmo;

public partial class sea : IAlgorithm
{
    public readonly record struct SeaLogLine(
        uint Generation,
        ulong FunctionEvaluations,
        double BestFitness,
        double Improvement,
        ulong OffspringEvaluations) : IAlgorithmLogLine
    {
        public string AlgorithmName => "sea";
        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["generation"] = Generation,
            ["function_evaluations"] = FunctionEvaluations,
            ["best_fitness"] = BestFitness,
            ["improvement"] = Improvement,
            ["offspring_evaluations"] = OffspringEvaluations
        };
        public string ToDisplayString() => $"gen={Generation}, fevals={FunctionEvaluations}, best={BestFitness}";
    }

    public IReadOnlyList<SeaLogLine> GetTypedLogLines()
    {
        using var rawEntries = get_log_entries();
        var lines = new List<SeaLogLine>(rawEntries.Count);
        for (var i = 0; i < rawEntries.Count; i++)
        {
            using var entry = rawEntries[i];
            lines.Add(new SeaLogLine(entry.gen, entry.fevals, entry.best, entry.improvement, entry.offspring_evals));
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
