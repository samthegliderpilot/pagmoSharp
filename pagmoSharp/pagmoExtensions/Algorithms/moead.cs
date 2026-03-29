using System.Collections.Generic;

namespace pagmo;

public partial class moead : IAlgorithm
{
    public readonly record struct MoeadLogLine(
        uint Generation,
        ulong FunctionEvaluations,
        double DecomposedFitness,
        IReadOnlyList<double> IdealPoint) : IAlgorithmLogLine
    {
        public string AlgorithmName => "moead";
        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["generation"] = Generation,
            ["function_evaluations"] = FunctionEvaluations,
            ["decomposed_fitness"] = DecomposedFitness,
            ["ideal_point"] = IdealPoint
        };
        public string ToDisplayString() => $"gen={Generation}, fevals={FunctionEvaluations}, decomp_f={DecomposedFitness}";
    }

    public IReadOnlyList<MoeadLogLine> GetTypedLogLines()
    {
        using var rawEntries = get_log_entries();
        var lines = new List<MoeadLogLine>(rawEntries.Count);
        for (var i = 0; i < rawEntries.Count; i++)
        {
            using var entry = rawEntries[i];
            using var ideal = entry.ideal_point;
            var values = new List<double>(ideal.Count);
            for (var j = 0; j < ideal.Count; j++) values.Add(ideal[j]);
            lines.Add(new MoeadLogLine(entry.gen, entry.fevals, entry.decomposed_f, values));
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
