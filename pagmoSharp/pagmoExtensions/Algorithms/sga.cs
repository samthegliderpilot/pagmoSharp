using System.Collections.Generic;

namespace pagmo;

public partial class sga : IAlgorithm
{
    public readonly record struct SgaLogLine(
        uint Generation,
        ulong FunctionEvaluations,
        double BestFitness,
        double Improvement) : IAlgorithmLogLine
    {
        public string AlgorithmName => "sga";
        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["generation"] = Generation,
            ["function_evaluations"] = FunctionEvaluations,
            ["best_fitness"] = BestFitness,
            ["improvement"] = Improvement
        };
        public string ToDisplayString() => $"gen={Generation}, fevals={FunctionEvaluations}, best={BestFitness}";
    }

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

    public IReadOnlyList<IAlgorithmLogLine> GetLogLines()
    {
        var typedLines = GetTypedLogLines();
        var projected = new List<IAlgorithmLogLine>(typedLines.Count);
        foreach (var line in typedLines) projected.Add(line);
        return projected;
    }
}
