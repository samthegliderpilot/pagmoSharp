using System.Collections.Generic;

namespace pagmo;

public partial class bee_colony : IAlgorithm
{
    public readonly record struct BeeColonyLogLine(
        uint Generation,
        ulong FunctionEvaluations,
        double BestFitness,
        double CurrentBestFitness) : IAlgorithmLogLine
    {
        public string AlgorithmName => "bee_colony";
        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["generation"] = Generation,
            ["function_evaluations"] = FunctionEvaluations,
            ["best_fitness"] = BestFitness,
            ["current_best_fitness"] = CurrentBestFitness
        };
        public string ToDisplayString() => $"gen={Generation}, fevals={FunctionEvaluations}, best={BestFitness}, cur_best={CurrentBestFitness}";
    }

    public IReadOnlyList<BeeColonyLogLine> GetTypedLogLines()
    {
        using var rawEntries = get_log_lines();
        var lines = new List<BeeColonyLogLine>(rawEntries.Count);
        for (var i = 0; i < rawEntries.Count; i++)
        {
            using var entry = rawEntries[i];
            lines.Add(new BeeColonyLogLine(entry.gen, entry.fevals, entry.best, entry.cur_best));
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
