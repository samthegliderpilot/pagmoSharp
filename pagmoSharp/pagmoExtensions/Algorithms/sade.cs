using System.Collections.Generic;

namespace pagmo;

public partial class sade : IAlgorithm
{
    public readonly record struct SadeLogLine(
        uint Generation,
        ulong FunctionEvaluations,
        double BestFitness,
        double F,
        double Cr,
        double Dx,
        double Df) : IAlgorithmLogLine
    {
        public string AlgorithmName => "sade";
        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["generation"] = Generation,
            ["function_evaluations"] = FunctionEvaluations,
            ["best_fitness"] = BestFitness,
            ["f"] = F,
            ["cr"] = Cr,
            ["dx"] = Dx,
            ["df"] = Df
        };
        public string ToDisplayString() => $"gen={Generation}, fevals={FunctionEvaluations}, best={BestFitness}";
    }

    public IReadOnlyList<SadeLogLine> GetTypedLogLines()
    {
        using var rawEntries = get_log_entries();
        var lines = new List<SadeLogLine>(rawEntries.Count);
        for (var i = 0; i < rawEntries.Count; i++)
        {
            using var entry = rawEntries[i];
            lines.Add(new SadeLogLine(entry.gen, entry.fevals, entry.best, entry.f, entry.cr, entry.dx, entry.df));
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
