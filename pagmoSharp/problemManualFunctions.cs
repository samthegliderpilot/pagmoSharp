using System.Collections.Generic;

namespace pagmo
{
    public static class ProblemManualFunctions
    {
        public static DoubleVector fitness(this IProblem problem, params double[] values)
        {
            return problem.fitness(new DoubleVector(values));
        }

        public static DoubleVector fitness(this IProblem problem, IEnumerable<double> values)
        {
            return problem.fitness(new DoubleVector(values));
        }
    }
}
