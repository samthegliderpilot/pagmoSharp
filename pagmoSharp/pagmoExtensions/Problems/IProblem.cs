using System;

namespace pagmo
{
    public interface IProblem : IDisposable
    {
        /// <summary>
        /// Gets the problem name.
        /// </summary>
        /// <returns></returns>
        string get_name();

        /// <summary>
        /// Evaluates the problems fitness.
        /// </summary>
        /// <param name="x">The values to evaluate the problem at.</param>
        /// <returns>The fitness of the problem.</returns>
        DoubleVector fitness(DoubleVector x);

        /// <summary>
        /// Gets the bounds of the problem. 
        /// </summary>
        /// <returns>The bounds of the problem.</returns>
        PairOfDoubleVectors get_bounds();

        /// <summary>
        /// Gets the number of objectives.
        /// </summary>
        /// <returns>The number of the objectives.</returns>
        uint get_nobj();

        /// <summary>
        /// Gets the number of equality constraints.
        /// </summary>
        /// <returns>The number of equality constraints.</returns>
        uint get_nec();

        /// <summary>
        /// Gets the number of inequality constraints.
        /// </summary>
        /// <returns>The number of inequality constraints.</returns>
        uint get_nic();

        /// <summary>
        /// Gets if this problem has a batch fitness.
        /// </summary>
        /// <returns>Does this problem have batch fitness?</returns>
        bool has_batch_fitness();

        /// <summary>
        /// Gets the dimension of the integer part of the problem.
        /// </summary>
        /// <returns>The dimension of the integer part of the problem.</returns>
        uint get_nix();

        /// <summary>
        /// Gets the thread safety of this problem.
        /// </summary>
        /// <returns>The thread safety of this problem.</returns>
        thread_safety get_thread_safety();
    }
}