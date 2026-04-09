using System;
using System.Collections.Generic;

namespace pagmo;

public partial class nlopt : IAlgorithm
{
    public readonly record struct NloptLogLine(
        ulong FunctionEvaluations,
        double Objective,
        ulong ViolatedConstraints,
        double ViolationNorm,
        bool Feasible) : IAlgorithmLogLine
    {
        public string AlgorithmName => "nlopt";

        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["function_evaluations"] = FunctionEvaluations,
            ["objective"] = Objective,
            ["violated_constraints"] = ViolatedConstraints,
            ["violation_norm"] = ViolationNorm,
            ["feasible"] = Feasible
        };

        public string ToDisplayString() => $"fevals={FunctionEvaluations}, objective={Objective}, feasible={Feasible}";
    }

    // pagmo::nlopt does not expose a seed API in its wrapped surface.
    public void set_seed(uint seed) => throw new NotSupportedException("nlopt does not expose set_seed in pagmo.");

    // pagmo::nlopt does not expose a seed API in its wrapped surface.
    public uint get_seed() => throw new NotSupportedException("nlopt does not expose get_seed in pagmo.");

    // pagmo::nlopt exposes set_verbosity() but no getter in pagmo's public API.
    public uint get_verbosity() => throw new NotSupportedException("nlopt does not expose get_verbosity in pagmo.");

    public IReadOnlyList<NloptLogLine> GetTypedLogLines()
    {
        using var rawEntries = get_log_entries();
        var lines = new List<NloptLogLine>(rawEntries.Count);
        for (var i = 0; i < rawEntries.Count; i++)
        {
            using var entry = rawEntries[i];
            lines.Add(new NloptLogLine(entry.fevals, entry.objective, entry.violated, entry.violation_norm, entry.feasible));
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
