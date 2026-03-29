using System.Collections.Generic;

namespace pagmo;

public interface IAlgorithmLogLine
{
    string AlgorithmName { get; }
    IReadOnlyDictionary<string, object> RawFields { get; }
    string ToDisplayString();
}
