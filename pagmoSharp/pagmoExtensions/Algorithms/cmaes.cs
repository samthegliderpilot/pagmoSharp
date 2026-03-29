using System.Collections.Generic;

namespace pagmo;

public partial class cmaes : IAlgorithm
{
    public readonly record struct CmaesLogLine(
        uint Generation,
        ulong FunctionEvaluations,
        double BestFitness,
        double Sigma,
        double MinVariance,
        double MaxVariance) : IAlgorithmLogLine
    {
        public string AlgorithmName => "cmaes";

        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["generation"] = Generation,
            ["function_evaluations"] = FunctionEvaluations,
            ["best_fitness"] = BestFitness,
            ["sigma"] = Sigma,
            ["min_variance"] = MinVariance,
            ["max_variance"] = MaxVariance
        };

        public string ToDisplayString() =>
            $"gen={Generation}, fevals={FunctionEvaluations}, best={BestFitness}, sigma={Sigma}, min_var={MinVariance}, max_var={MaxVariance}";
    }

    public IReadOnlyList<CmaesLogLine> GetTypedLogLines()
    {
        using var rawEntries = get_log_entries();
        var lines = new List<CmaesLogLine>(rawEntries.Count);

        for (var i = 0; i < rawEntries.Count; i++)
        {
            using var entry = rawEntries[i];
            lines.Add(new CmaesLogLine(
                entry.gen,
                entry.fevals,
                entry.best,
                entry.sigma,
                entry.min_variance,
                entry.max_variance));
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
