using System.Collections.Generic;

namespace pagmo;

public partial class pso_gen : IAlgorithm
{
    public readonly record struct PsoGenLogLine(
        uint Generation,
        ulong FunctionEvaluations,
        double BestFitness,
        double Inertia,
        double Cognitive,
        double Social) : IAlgorithmLogLine
    {
        public string AlgorithmName => "pso_gen";
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

    public IReadOnlyList<PsoGenLogLine> GetTypedLogLines()
    {
        using var rawEntries = get_log_entries();
        var lines = new List<PsoGenLogLine>(rawEntries.Count);
        for (var i = 0; i < rawEntries.Count; i++)
        {
            using var entry = rawEntries[i];
            lines.Add(new PsoGenLogLine(entry.gen, entry.fevals, entry.best, entry.inertia, entry.cognitive, entry.social));
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
