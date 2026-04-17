using System.Collections.Generic;

namespace pagmo
{
    /// <summary>
    /// Convenience extension methods for <see cref="IProblem"/> that accept plain .NET arrays and
    /// enumerables, sparing callers from constructing <see cref="DoubleVector"/> manually.
    /// </summary>
    public static class ProblemExtensions
    {
        /// <summary>
        /// Evaluates the fitness function using a params array of decision-variable values.
        /// </summary>
        public static DoubleVector fitness(this IProblem problem, params double[] values)
        {
            return problem.fitness(new DoubleVector(values));
        }

        /// <summary>
        /// Evaluates the fitness function using an enumerable sequence of decision-variable values.
        /// </summary>
        public static DoubleVector fitness(this IProblem problem, IEnumerable<double> values)
        {
            return problem.fitness(new DoubleVector(values));
        }
    }
}
