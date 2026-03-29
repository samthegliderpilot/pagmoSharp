using System.Collections.Generic;

namespace pagmo
{
    public readonly record struct GacoLogLine(
        uint Generation,
        ulong FunctionEvaluations,
        double BestFitness,
        uint KernelSize,
        double OracleValue,
        double Dx,
        double Dp) : IAlgorithmLogLine
    {
        public string AlgorithmName => "gaco";

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

        public string ToDisplayString() =>
            $"gen={Generation}, fevals={FunctionEvaluations}, best={BestFitness}, ker={KernelSize}, oracle={OracleValue}, dx={Dx}, dp={Dp}";
    }

    public partial class gaco : IAlgorithm
    {
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
