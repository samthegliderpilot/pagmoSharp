using System.Collections.Generic;

namespace pagmo;

public partial class ihs : IAlgorithm
{
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
        public string AlgorithmName => "ihs";

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

        public string ToDisplayString() =>
            $"fevals={FunctionEvaluations}, ppar={PitchAdjustmentRate}, bw={Bandwidth}, violated={ViolatedConstraints}, viol_norm={ViolationNorm}";
    }

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
