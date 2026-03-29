using System.Collections.Generic;

namespace pagmo;

public partial class pso : IAlgorithm
{
    public readonly record struct PsoLogLine(
        uint Generation,
        ulong FunctionEvaluations,
        double BestFitness,
        double Inertia,
        double Cognitive,
        double Social) : IAlgorithmLogLine
    {
        public string AlgorithmName => "pso";
        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["generation"] = Generation,
            ["function_evaluations"] = FunctionEvaluations,
            ["best_fitness"] = BestFitness,
            ["inertia"] = Inertia,
            ["cognitive"] = Cognitive,
            ["social"] = Social
        };
        public string ToDisplayString() => $"gen={Generation}, fevals={FunctionEvaluations}, best={BestFitness}";
    }

    public IReadOnlyList<PsoLogLine> GetTypedLogLines()
    {
        using var rawEntries = get_log_entries();
        var lines = new List<PsoLogLine>(rawEntries.Count);
        for (var i = 0; i < rawEntries.Count; i++)
        {
            using var entry = rawEntries[i];
            lines.Add(new PsoLogLine(entry.gen, entry.fevals, entry.best, entry.inertia, entry.cognitive, entry.social));
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
