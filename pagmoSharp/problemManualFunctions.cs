using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace pagmo
{
    public partial class problem
    {
        public DoubleVector fitness(params double[] values)
        {
            return fitness(new DoubleVector(values));
        }

        public DoubleVector fitness(IEnumerable<double> values)
        {
            return fitness(new DoubleVector(values));
        }

        public static implicit operator problemBase(problem d) => d.getBaseProblem();
    }
}
