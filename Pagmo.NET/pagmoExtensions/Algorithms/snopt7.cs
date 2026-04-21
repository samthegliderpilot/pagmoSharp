#if PAGMO_WITH_SNOPT7
using System;
using System.Collections.Generic;

namespace pagmo;

/// <summary>
/// Represents snopt7. Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
/// </summary>
public partial class snopt7 : IAlgorithm
{
    /// <summary>
    /// Represents a typed algorithm log entry projected from pagmo runtime data.
    /// Log fields: major iterations, function evaluations, best objective,
    /// infeasibility norm, and feasibility flag.
    /// </summary>
    public readonly record struct Snopt7LogLine(
        ulong MajorIterations,
        ulong FunctionEvaluations,
        double Objective,
        double Infeasibility,
        bool Feasible) : IAlgorithmLogLine
    {
        /// <summary>
        /// Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
        /// </summary>
        public string AlgorithmName => "snopt7";

        /// <summary>
        /// Gets a generic field map for algorithm-agnostic log processing.
        /// </summary>
        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["major_iterations"]    = MajorIterations,
            ["function_evaluations"] = FunctionEvaluations,
            ["objective"]           = Objective,
            ["infeasibility"]       = Infeasibility,
            ["feasible"]            = Feasible
        };

        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public string ToDisplayString() =>
            $"major_iter={MajorIterations}, fevals={FunctionEvaluations}, objective={Objective}, feasible={Feasible}";
    }

    // pagmo::snopt7 does not expose a seed API in its wrapped surface.
    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
    public void set_seed(uint seed) => throw new NotSupportedException("snopt7 does not expose set_seed in pagmo.");

    // pagmo::snopt7 does not expose a seed API in its wrapped surface.
    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
    public uint get_seed() => throw new NotSupportedException("snopt7 does not expose get_seed in pagmo.");

    // pagmo::snopt7 exposes set_verbosity() but no getter in pagmo's public API.
    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
    public uint get_verbosity() => throw new NotSupportedException("snopt7 does not expose get_verbosity in pagmo.");

    /// <summary>
    /// Returns typed log entries for this algorithm.
    /// </summary>
    public IReadOnlyList<Snopt7LogLine> GetTypedLogLines()
    {
        using var rawEntries = get_log_entries();
        var lines = new List<Snopt7LogLine>(rawEntries.Count);
        for (var i = 0; i < rawEntries.Count; i++)
        {
            using var entry = rawEntries[i];
            lines.Add(new Snopt7LogLine(
                entry.major_iterations,
                entry.fevals,
                entry.objective,
                entry.infeasibility,
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
            projected.Add(line);

        return projected;
    }
}
#endif
