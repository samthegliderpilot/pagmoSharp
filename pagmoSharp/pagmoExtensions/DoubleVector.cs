using System;
using System.Collections.Generic;

namespace pagmo
{
    public partial class DoubleVector
    {
        public DoubleVector(params double[] values)
            : this((IEnumerable<double>)(values ?? throw new ArgumentNullException(nameof(values))))
        {
        }
    }
}
