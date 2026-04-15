using System.Collections.Generic;

namespace pagmo;

/// <summary>
/// Common projected algorithm log line contract used by <see cref="IAlgorithm.GetLogLines"/>.
/// </summary>
public interface IAlgorithmLogLine
{
    /// <summary>
    /// Gets the logical algorithm name associated with this log line.
    /// </summary>
    string AlgorithmName { get; }

    /// <summary>
    /// Gets raw field values keyed by field name for advanced/custom interpretation.
    /// </summary>
    IReadOnlyDictionary<string, object> RawFields { get; }

    /// <summary>
    /// Formats the log line into a human-readable display string.
    /// </summary>
    string ToDisplayString();
}
