using System.Collections.Generic;

namespace pagmo;

public partial class xnes : IAlgorithm
{
    public readonly record struct XnesLogLine(
        uint Generation,
        ulong FunctionEvaluations,
        double BestFitness,
        double Dx,
        double Df,
        double Sigma) : IAlgorithmLogLine
    {
        public string AlgorithmName => "xnes";
        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["generation"] = Generation,
            ["function_evaluations"] = FunctionEvaluations,
            ["best_fitness"] = BestFitness,
            ["dx"] = Dx,
            ["df"] = Df,
            ["sigma"] = Sigma
        };
        public string ToDisplayString() => $"gen={Generation}, fevals={FunctionEvaluations}, best={BestFitness}";
    }

    public IReadOnlyList<XnesLogLine> GetTypedLogLines()
    {
        using var rawEntries = get_log_entries();
        var lines = new List<XnesLogLine>(rawEntries.Count);
        for (var i = 0; i < rawEntries.Count; i++)
        {
            using var entry = rawEntries[i];
            lines.Add(new XnesLogLine(entry.gen, entry.fevals, entry.best, entry.dx, entry.df, entry.sigma));
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
