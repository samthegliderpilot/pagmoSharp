using System.Collections.Generic;

namespace pagmo;

public partial class maco : IAlgorithm
{
    public readonly record struct MacoLogLine(
        uint Generation,
        ulong FunctionEvaluations,
        IReadOnlyList<double> FitnessVector) : IAlgorithmLogLine
    {
        public string AlgorithmName => "maco";
        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["generation"] = Generation,
            ["function_evaluations"] = FunctionEvaluations,
            ["fitness_vector"] = FitnessVector
        };
        public string ToDisplayString() => $"gen={Generation}, fevals={FunctionEvaluations}, objectives={FitnessVector.Count}";
    }

    public IReadOnlyList<MacoLogLine> GetTypedLogLines()
    {
        using var rawEntries = get_log_entries();
        var lines = new List<MacoLogLine>(rawEntries.Count);
        for (var i = 0; i < rawEntries.Count; i++)
        {
            using var entry = rawEntries[i];
            using var fitness = entry.fitness;
            var values = new List<double>(fitness.Count);
            for (var j = 0; j < fitness.Count; j++) values.Add(fitness[j]);
            lines.Add(new MacoLogLine(entry.gen, entry.fevals, values));
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
