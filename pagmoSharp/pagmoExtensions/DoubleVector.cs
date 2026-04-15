using System;
using System.Collections.Generic;

namespace pagmo
{
    /// <summary>
    /// Represents DoubleVector. Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
    /// </summary>
    public partial class DoubleVector
    {
        public DoubleVector(params double[] values)
            : this((IEnumerable<double>)(values ?? throw new ArgumentNullException(nameof(values))))
        {
        }
    }
}

