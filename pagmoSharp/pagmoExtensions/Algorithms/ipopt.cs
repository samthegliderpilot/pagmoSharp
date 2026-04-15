using System;
using System.Collections.Generic;

namespace pagmo;

/// <summary>
/// Represents ipopt. Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
/// </summary>
public partial class ipopt : IAlgorithm
{
    // IPOPT's return status is exposed as a numeric code to avoid leaking native pointer-style enums.
    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
    public int GetLastOptimizationResultCode() => get_last_opt_result_code();

    // Keep pagmo-style naming for option setters while exposing C#-friendly primitives.
    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
    public void set_integer_option(string name, ulong value) => set_integer_option_u64(name, value);

    public readonly record struct IpoptLogLine(
        ulong ObjectiveEvaluations,
        double Objective,
        ulong ViolatedConstraints,
        double ViolationNorm,
        bool Feasible) : IAlgorithmLogLine
    {
        /// <summary>
        /// Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
        /// </summary>
        public string AlgorithmName => "ipopt";

        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["objective_evaluations"] = ObjectiveEvaluations,
            ["objective"] = Objective,
            ["violated_constraints"] = ViolatedConstraints,
            ["violation_norm"] = ViolationNorm,
            ["feasible"] = Feasible
        };

        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public string ToDisplayString()
            => $"obj_eval={ObjectiveEvaluations}, objective={Objective}, feasible={Feasible}";
    }

    // pagmo::ipopt does not expose a seed API in its wrapped surface.
    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
    public void set_seed(uint seed) => throw new NotSupportedException("ipopt does not expose set_seed in pagmo.");

    // pagmo::ipopt does not expose a seed API in its wrapped surface.
    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
    public uint get_seed() => throw new NotSupportedException("ipopt does not expose get_seed in pagmo.");

    // pagmo::ipopt exposes set_verbosity() but no getter in pagmo's public API.
    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
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

    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
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

