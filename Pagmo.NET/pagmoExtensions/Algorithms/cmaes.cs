using System.Collections.Generic;

namespace pagmo;

/// <summary>
/// Represents cmaes. Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
/// </summary>
public partial class cmaes : IAlgorithm
{
    /// <summary>
    /// Represents a typed algorithm log entry projected from pagmo runtime data.
    /// </summary>
    public readonly record struct CmaesLogLine(
        uint Generation,
        ulong FunctionEvaluations,
        double BestFitness,
        double Sigma,
        double MinVariance,
        double MaxVariance) : IAlgorithmLogLine
    {
        /// <summary>
        /// Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
        /// </summary>
        public string AlgorithmName => "cmaes";

        /// <summary>
        /// Gets a generic field map for algorithm-agnostic log processing.
        /// </summary>
        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["generation"] = Generation,
            ["function_evaluations"] = FunctionEvaluations,
            ["best_fitness"] = BestFitness,
            ["sigma"] = Sigma,
            ["min_variance"] = MinVariance,
            ["max_variance"] = MaxVariance
        };

        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public string ToDisplayString() =>
            $"gen={Generation}, fevals={FunctionEvaluations}, best={BestFitness}, sigma={Sigma}, min_var={MinVariance}, max_var={MaxVariance}";
    }

    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
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

