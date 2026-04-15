using System;

namespace pagmo
{
    /// <summary>
    /// Managed problem contract for user-defined and wrapped pagmo problems.
    /// </summary>
    /// <remarks>
    /// This mirrors pagmo's UDP concept. Implementers should return consistent fitness/bounds dimensions and
    /// accurately report capability flags (<c>has_*</c>) and <see cref="thread_safety"/> for runtime safety.
    /// </remarks>
    public interface IProblem : IDisposable
    {
        /// <summary>
        /// Returns a human-readable problem name.
        /// </summary>
        string get_name() => "C# problem";

        /// <summary>
        /// Returns additional problem metadata for diagnostics/display.
        /// </summary>
        string get_extra_info() => string.Empty;

        /// <summary>
        /// Evaluates the fitness vector at the provided decision vector.
        /// </summary>
        /// <param name="x">Decision vector.</param>
        /// <returns>Fitness vector.</returns>
        DoubleVector fitness(DoubleVector x);

        /// <summary>
        /// Evaluates fitness for a flattened batch of decision vectors when supported.
        /// </summary>
        /// <param name="dvs">Flattened batch decision vector input.</param>
        /// <returns>Flattened batch fitness output.</returns>
        DoubleVector batch_fitness(DoubleVector dvs) => throw new NotSupportedException("batch_fitness() is not implemented.");

        /// <summary>
        /// Returns lower/upper bounds for decision variables.
        /// </summary>
        PairOfDoubleVectors get_bounds();

        /// <summary>
        /// Returns objective-count (<c>f</c>) dimension.
        /// </summary>
        uint get_nobj() => 1;

        /// <summary>
        /// Returns equality-constraint count.
        /// </summary>
        uint get_nec() => 0;

        /// <summary>
        /// Returns inequality-constraint count.
        /// </summary>
        uint get_nic() => 0;

        /// <summary>
        /// Returns number of integer decision variables.
        /// </summary>
        uint get_nix() => 0;

        /// <summary>
        /// Reports whether <see cref="batch_fitness"/> is implemented.
        /// </summary>
        bool has_batch_fitness() => false;

        /// <summary>
        /// Reports whether <see cref="gradient"/> is implemented.
        /// </summary>
        bool has_gradient() => false;

        /// <summary>
        /// Returns gradient values at the provided decision vector when supported.
        /// </summary>
        DoubleVector gradient(DoubleVector x) => throw new NotSupportedException("gradient() is not implemented.");

        /// <summary>
        /// Reports whether <see cref="gradient_sparsity"/> is implemented.
        /// </summary>
        bool has_gradient_sparsity() => false;

        /// <summary>
        /// Returns gradient sparsity pattern when supported.
        /// </summary>
        SparsityPattern gradient_sparsity() => throw new NotSupportedException("gradient_sparsity() is not implemented.");

        /// <summary>
        /// Reports whether <see cref="hessians"/> is implemented.
        /// </summary>
        bool has_hessians() => false;

        /// <summary>
        /// Returns hessian matrices at the provided decision vector when supported.
        /// </summary>
        VectorOfVectorOfDoubles hessians(DoubleVector x) => throw new NotSupportedException("hessians() is not implemented.");

        /// <summary>
        /// Reports whether <see cref="hessians_sparsity"/> is implemented.
        /// </summary>
        bool has_hessians_sparsity() => false;

        /// <summary>
        /// Returns hessian sparsity patterns when supported.
        /// </summary>
        VectorOfSparsityPattern hessians_sparsity() => throw new NotSupportedException("hessians_sparsity() is not implemented.");

        /// <summary>
        /// Sets the random seed when stochastic behavior supports explicit seeding.
        /// </summary>
        void set_seed(uint seed) => throw new NotSupportedException("set_seed() is not implemented.");

        /// <summary>
        /// Reports whether <see cref="set_seed"/> is implemented.
        /// </summary>
        bool has_set_seed() => false;

        /// <summary>
        /// Returns thread-safety capability used by threaded pagmo execution paths.
        /// </summary>
        thread_safety get_thread_safety() => thread_safety.none;
    }
}
