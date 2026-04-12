using System;
using System.Collections.Generic;

namespace pagmo;

public partial class ipopt : IAlgorithm
{
    public readonly record struct IpoptLogLine(
        ulong ObjectiveEvaluations,
        double Objective,
        ulong ViolatedConstraints,
        double ViolationNorm,
        bool Feasible) : IAlgorithmLogLine
    {
        public string AlgorithmName => "ipopt";

        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["objective_evaluations"] = ObjectiveEvaluations,
            ["objective"] = Objective,
            ["violated_constraints"] = ViolatedConstraints,
            ["violation_norm"] = ViolationNorm,
            ["feasible"] = Feasible
        };

        public string ToDisplayString()
            => $"obj_eval={ObjectiveEvaluations}, objective={Objective}, feasible={Feasible}";
    }

    // pagmo::ipopt does not expose a seed API in its wrapped surface.
    public void set_seed(uint seed) => throw new NotSupportedException("ipopt does not expose set_seed in pagmo.");

    // pagmo::ipopt does not expose a seed API in its wrapped surface.
    public uint get_seed() => throw new NotSupportedException("ipopt does not expose get_seed in pagmo.");

    // pagmo::ipopt exposes set_verbosity() but no getter in pagmo's public API.
    public uint get_verbosity() => throw new NotSupportedException("ipopt does not expose get_verbosity in pagmo.");

    public IReadOnlyList<IpoptLogLine> GetTypedLogLines()
    {
        using var rawEntries = get_log_entries();
        var lines = new List<IpoptLogLine>(rawEntries.Count);
        for (var i = 0; i < rawEntries.Count; i++)
        {
            using var entry = rawEntries[i];
            lines.Add(new IpoptLogLine(
                entry.objective_evaluations,
                entry.objective,
                entry.violated,
                entry.violation_norm,
                entry.feasible));
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
