using System.Collections.Generic;

namespace pagmo
{
    /// <summary>
    /// Represents struct .
    /// </summary>
    public readonly record struct GacoLogLine(
        uint Generation,
        ulong FunctionEvaluations,
        double BestFitness,
        uint KernelSize,
        double OracleValue,
        double Dx,
        double Dp) : IAlgorithmLogLine
    {
        /// <summary>
        /// Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
        /// </summary>
        public string AlgorithmName => "gaco";

        /// <summary>
        /// Gets a generic field map for algorithm-agnostic log processing.
        /// </summary>
        public IReadOnlyDictionary<string, object> RawFields => new Dictionary<string, object>
        {
            ["generation"] = Generation,
            ["function_evaluations"] = FunctionEvaluations,
            ["best_fitness"] = BestFitness,
            ["kernel_size"] = KernelSize,
            ["oracle_value"] = OracleValue,
            ["dx"] = Dx,
            ["dp"] = Dp
        };

        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public string ToDisplayString() =>
            $"gen={Generation}, fevals={FunctionEvaluations}, best={BestFitness}, ker={KernelSize}, oracle={OracleValue}, dx={Dx}, dp={Dp}";
    }

    /// <summary>
    /// Represents gaco. Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
    /// </summary>
    public partial class gaco : IAlgorithm
    {
        /// <summary>
        /// Returns typed log entries for this algorithm.
        /// </summary>
        public IReadOnlyList<GacoLogLine> GetTypedLogLines()
        {
            using var rawEntries = get_log_entries();
            var lines = new List<GacoLogLine>(rawEntries.Count);

            for (var i = 0; i < rawEntries.Count; i++)
            {
                using var entry = rawEntries[i];
                lines.Add(new GacoLogLine(
                    entry.gen,
                    entry.fevals,
                    entry.best_fit,
                    entry.kernel,
                    entry.oracle,
                    entry.dx,
                    entry.dp));
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
}

