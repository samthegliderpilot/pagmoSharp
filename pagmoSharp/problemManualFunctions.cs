using System.Collections.Generic;

namespace pagmo
{
    /// <summary>
    /// Additional methods for the problem class.
    /// </summary>
    public partial class problemPagomWrapper
    {
        /// <summary>
        /// Evaluates the fitness of the problem.
        /// </summary>
        /// <param name="values">The independent variables to pass into the function.</param>
        /// <returns>The fitness values.</returns>
        public DoubleVector fitness(params double[] values)
        {
            return fitness(new DoubleVector(values));
        }

        /// <summary>
        /// Evaluates the fitness of the problem.
        /// </summary>
        /// <param name="values">The independent variables to pass into the function.</param>
        /// <returns>The fitness values.</returns>
        public DoubleVector fitness(IEnumerable<double> values)
        {
            return fitness(new DoubleVector(values));
        }

        /// <summary>
        /// Implicitly converts this problem to the problemBase.
        /// </summary>
        /// <param name="thisProblem">The problem to convert to a problemBase.</param>
        public static implicit operator problemBase(problemPagomWrapper thisProblem) => thisProblem.getBaseProblem();

        public static implicit operator problemPagomWrapper(problemBase thisProblem) => new problemPagomWrapper(thisProblem);
    }

    /// <summary>
    /// Additional methods for the problem class.
    /// </summary>
    public partial class problemBase : IProblem
    {
        /// <summary>
        /// Evaluates the fitness of the problem.
        /// </summary>
        /// <param name="values">The independent variables to pass into the function.</param>
        /// <returns>The fitness values.</returns>
        public DoubleVector fitness(params double[] values)
        {
            return fitness(new DoubleVector(values));
        }

        /// <summary>
        /// Evaluates the fitness of the problem.
        /// </summary>
        /// <param name="values">The independent variables to pass into the function.</param>
        /// <returns>The fitness values.</returns>
        public DoubleVector fitness(IEnumerable<double> values)
        {
            return fitness(new DoubleVector(values));
        }
    }
}
