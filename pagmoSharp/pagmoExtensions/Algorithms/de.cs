using System.Collections.Generic;

namespace pagmo;

public partial class de : IAlgorithm
{
    public readonly record struct DeLogLine(
        uint Generation,
        ulong FunctionEvaluations,
        double BestFitness,
        double FunctionEvaluationDifference,
        double Dx) : IAlgorithmLogLine
    {
        public string AlgorithmName => "de";

        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["generation"] = Generation,
            ["function_evaluations"] = FunctionEvaluations,
            ["best_fitness"] = BestFitness,
            ["function_evaluation_difference"] = FunctionEvaluationDifference,
            ["dx"] = Dx
        };

        public string ToDisplayString() =>
            $"gen={Generation}, fevals={FunctionEvaluations}, best={BestFitness}, df={FunctionEvaluationDifference}, dx={Dx}";
    }

    public IReadOnlyList<DeLogLine> GetTypedLogLines()
    {
        using var rawEntries = get_log_entries();
        var lines = new List<DeLogLine>(rawEntries.Count);

        for (var i = 0; i < rawEntries.Count; i++)
        {
            using var entry = rawEntries[i];
            lines.Add(new DeLogLine(
                entry.gen,
                entry.fevals,
                entry.best,
                entry.feval_difference,
                entry.dx));
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
