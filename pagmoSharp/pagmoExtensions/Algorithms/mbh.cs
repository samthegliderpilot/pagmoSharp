using System.Collections.Generic;

namespace pagmo;

public partial class mbh : IAlgorithm
{
    public readonly record struct MbhLogLine(
        ulong FunctionEvaluations,
        double BestFitness,
        uint ViolatedConstraints,
        double ViolationNorm,
        uint Trial) : IAlgorithmLogLine
    {
        public string AlgorithmName => "mbh";

        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["function_evaluations"] = FunctionEvaluations,
            ["best_fitness"] = BestFitness,
            ["violated_constraints"] = ViolatedConstraints,
            ["violation_norm"] = ViolationNorm,
            ["trial"] = Trial
        };

        public string ToDisplayString() =>
            $"fevals={FunctionEvaluations}, best={BestFitness}, violated={ViolatedConstraints}, viol_norm={ViolationNorm}, trial={Trial}";
    }

    public IReadOnlyList<MbhLogLine> GetTypedLogLines()
    {
        using var rawEntries = get_log_entries();
        var lines = new List<MbhLogLine>(rawEntries.Count);

        for (var i = 0; i < rawEntries.Count; i++)
        {
            using var entry = rawEntries[i];
            lines.Add(new MbhLogLine(
                entry.fevals,
                entry.best,
                entry.violated,
                entry.violation_norm,
                entry.trial));
        }

        return lines;
    }

    public IReadOnlyList<IAlgorithmLogLine> GetLogLines()
    {
        var typedLines = GetTypedLogLines();
        var projected = new List<IAlgorithmLogLine>(typedLines.Count);
        foreach (var line in typedLines)
        {
            projected.Add(line);
        }

        return projected;
    }
}
