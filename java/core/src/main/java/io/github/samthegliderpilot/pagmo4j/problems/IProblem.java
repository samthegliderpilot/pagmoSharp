package io.github.samthegliderpilot.pagmo4j.problems;

import io.github.samthegliderpilot.pagmo4j.*;

/**
 * Contract for user-defined problems (UDPs) in pagmo4j.
 *
 * <p>Mirrors pagmo's UDP concept. The two required methods are {@link #fitness(DoubleVector)}
 * and {@link #get_bounds()}; all others have defaults that declare the feature absent.
 *
 * <h3>Minimal implementation</h3>
 * <pre>{@code
 * class MyProblem extends ManagedProblemBase {
 *     public DoubleVector fitness(DoubleVector x) { return vec(x.get(0) * x.get(0)); }
 *     public PairOfDoubleVectors get_bounds() { return bounds(new double[]{-5}, new double[]{5}); }
 * }
 * }</pre>
 *
 * <h3>Thread safety</h3>
 * <p>Override {@link #get_thread_safety()} to return {@link ThreadSafety#Basic} if the
 * problem is safe to call concurrently. Alternatively, implement
 * {@link IThreadCloneableProblem} and override {@link ManagedProblemBase#clone()} to
 * return an independent copy — pagmo4j will create per-thread clones automatically.
 */
public interface IProblem extends AutoCloseable {

    /** Returns a human-readable problem name used in logging and diagnostics. */
    default String get_name() { return "Java problem"; }

    /** Returns extra diagnostic information (displayed by pagmo's pretty-printer). */
    default String get_extra_info() { return ""; }

    /**
     * Evaluates the problem at decision vector {@code x} and returns the combined
     * fitness vector {@code [f_0, …, f_{nobj-1}, c_eq_0, …, c_ineq_0, …]}.
     *
     * @param x decision vector; length must equal the number of decision variables
     * @return fitness + constraint values; length = nobj + nec + nic
     */
    DoubleVector fitness(DoubleVector x);

    /**
     * Returns the box bounds as a pair {@code (lower, upper)}, each of length {@code nx}.
     * The number of decision variables is inferred from {@code lower.size()}.
     */
    PairOfDoubleVectors get_bounds();

    /**
     * Evaluates a batch of decision vectors in one call (optional optimisation).
     * Override together with {@link #has_batch_fitness()} returning {@code true}.
     *
     * @param dvs concatenated decision vectors, length = nx × nPoints
     * @return concatenated fitness vectors, length = (nobj + nec + nic) × nPoints
     */
    default DoubleVector batch_fitness(DoubleVector dvs) {
        throw new UnsupportedOperationException("batch_fitness() is not implemented.");
    }

    /** Number of objectives (default 1 — single-objective). */
    default long get_nobj() { return 1L; }

    /** Number of equality constraints (default 0). */
    default long get_nec()  { return 0L; }

    /** Number of inequality constraints (default 0). */
    default long get_nic()  { return 0L; }

    /** Number of integer decision variables, counted from the end of the decision vector (default 0). */
    default long get_nix()  { return 0L; }

    /** Returns {@code true} if {@link #batch_fitness(DoubleVector)} is implemented. */
    default boolean has_batch_fitness()     { return false; }

    /** Returns {@code true} if {@link #gradient(DoubleVector)} is implemented. */
    default boolean has_gradient()          { return false; }

    /** Returns {@code true} if {@link #gradient_sparsity()} is implemented. */
    default boolean has_gradient_sparsity() { return false; }

    /** Returns {@code true} if {@link #hessians(DoubleVector)} is implemented. */
    default boolean has_hessians()          { return false; }

    /** Returns {@code true} if {@link #hessians_sparsity()} is implemented. */
    default boolean has_hessians_sparsity() { return false; }

    /** Returns {@code true} if {@link #set_seed(long)} is implemented. */
    default boolean has_set_seed()          { return false; }

    /**
     * Returns the gradient at {@code x} as a dense or sparse vector.
     * Override together with {@link #has_gradient()} returning {@code true}.
     */
    default DoubleVector gradient(DoubleVector x) {
        throw new UnsupportedOperationException("gradient() is not implemented.");
    }

    /** Returns the gradient sparsity pattern. Override with {@link #has_gradient_sparsity()}. */
    default SparsityPattern gradient_sparsity() {
        throw new UnsupportedOperationException("gradient_sparsity() is not implemented.");
    }

    /** Returns the Hessian matrices at {@code x}. Override with {@link #has_hessians()}. */
    default VectorOfVectorOfDoubles hessians(DoubleVector x) {
        throw new UnsupportedOperationException("hessians() is not implemented.");
    }

    /** Returns Hessian sparsity patterns. Override with {@link #has_hessians_sparsity()}. */
    default VectorOfSparsityPattern hessians_sparsity() {
        throw new UnsupportedOperationException("hessians_sparsity() is not implemented.");
    }

    /**
     * Sets the random seed used by stochastic problems.
     * Override together with {@link #has_set_seed()} returning {@code true}.
     */
    default void set_seed(long seed) {
        throw new UnsupportedOperationException("set_seed() is not implemented.");
    }

    /**
     * Returns the thread-safety level of this problem.
     * Default is {@link ThreadSafety#None}; override to declare concurrent safety
     * or implement {@link IThreadCloneableProblem} for per-thread cloning.
     */
    default ThreadSafety get_thread_safety() { return ThreadSafety.None; }

    default void throwIfNotThreadSafe() {
        if (get_thread_safety() == ThreadSafety.None) {
            String hint = (this instanceof IThreadCloneableProblem)
                ? " Alternatively, override clone() to return a non-null independent copy."
                : "";
            throw new IllegalStateException(
                "Managed problem '" + get_name() + "' must declare ThreadSafety.Basic or " +
                "ThreadSafety.Constant for this threaded path." + hint);
        }
    }

    @Override
    default void close() {}
}
