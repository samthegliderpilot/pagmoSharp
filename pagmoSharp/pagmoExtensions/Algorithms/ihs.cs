using System.Collections.Generic;

namespace pagmo;

/// <summary>
/// Represents ihs. Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
/// </summary>
public partial class ihs : IAlgorithm
{
    /// <summary>
    /// Represents a typed algorithm log entry projected from pagmo runtime data.
    /// </summary>
    public readonly record struct IhsLogLine(
        ulong FunctionEvaluations,
        double PitchAdjustmentRate,
        double Bandwidth,
        double DecisionFlatness,
        double FitnessFlatness,
        ulong ViolatedConstraints,
        double ViolationNorm,
        IReadOnlyList<double> IdealPoint) : IAlgorithmLogLine
    {
        /// <summary>
        /// Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
        /// </summary>
        public string AlgorithmName => "ihs";

        /// <summary>
        /// Gets a generic field map for algorithm-agnostic log processing.
        /// </summary>
        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["function_evaluations"] = FunctionEvaluations,
            ["pitch_adjustment_rate"] = PitchAdjustmentRate,
            ["bandwidth"] = Bandwidth,
            ["decision_flatness"] = DecisionFlatness,
            ["fitness_flatness"] = FitnessFlatness,
            ["violated_constraints"] = ViolatedConstraints,
            ["violation_norm"] = ViolationNorm,
            ["ideal_point"] = IdealPoint
        };

        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public string ToDisplayString() =>
            $"fevals={FunctionEvaluations}, ppar={PitchAdjustmentRate}, bw={Bandwidth}, violated={ViolatedConstraints}, viol_norm={ViolationNorm}";
    }

    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
    public IReadOnlyList<IhsLogLine> GetTypedLogLines()
    {
        using var rawEntries = get_log_entries();
        var lines = new List<IhsLogLine>(rawEntries.Count);

        for (var i = 0; i < rawEntries.Count; i++)
        {
            using var entry = rawEntries[i];
            using var ideal = entry.ideal;
            var idealPoint = new List<double>(ideal.Count);
            for (var j = 0; j < ideal.Count; j++)
            {
                idealPoint.Add(ideal[j]);
            }

            lines.Add(new IhsLogLine(
                entry.fevals,
                entry.ppar,
                entry.bw,
                entry.dx,
                entry.df,
                entry.violated,
                entry.violation_norm,
                idealPoint));
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

