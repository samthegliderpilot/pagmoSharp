using System;
using System.Collections.Generic;

namespace pagmo;

public partial class algorithm
{
    public IReadOnlyList<IAlgorithmLogLine> GetLogLines()
    {
        return Array.Empty<IAlgorithmLogLine>();
    }
}
