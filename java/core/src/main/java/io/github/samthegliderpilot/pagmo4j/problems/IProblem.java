package io.github.samthegliderpilot.pagmo4j.problems;

import io.github.samthegliderpilot.pagmo4j.*;

/**
 * Managed problem contract for user-defined and wrapped pagmo problems.
 *
 * <p>Mirrors pagmo's UDP concept. Implement {@link IThreadCloneableProblem} and
 * override {@link ManagedProblemBase#clone()} to opt in to per-thread cloning for
 * problems that declare {@link ThreadSafety#None}.
 */
public interface IProblem extends AutoCloseable {

    default String get_name() { return "Java problem"; }
    default String get_extra_info() { return ""; }

    DoubleVector fitness(DoubleVector x);
    PairOfDoubleVectors get_bounds();

    default DoubleVector batch_fitness(DoubleVector dvs) {
        throw new UnsupportedOperationException("batch_fitness() is not implemented.");
    }

    default long get_nobj() { return 1L; }
    default long get_nec()  { return 0L; }
    default long get_nic()  { return 0L; }
    default long get_nix()  { return 0L; }

    default boolean has_batch_fitness()     { return false; }
    default boolean has_gradient()          { return false; }
    default boolean has_gradient_sparsity() { return false; }
    default boolean has_hessians()          { return false; }
    default boolean has_hessians_sparsity() { return false; }
    default boolean has_set_seed()          { return false; }

    default DoubleVector gradient(DoubleVector x) {
        throw new UnsupportedOperationException("gradient() is not implemented.");
    }
    default SparsityPattern gradient_sparsity() {
        throw new UnsupportedOperationException("gradient_sparsity() is not implemented.");
    }
    default VectorOfVectorOfDoubles hessians(DoubleVector x) {
        throw new UnsupportedOperationException("hessians() is not implemented.");
    }
    default VectorOfSparsityPattern hessians_sparsity() {
        throw new UnsupportedOperationException("hessians_sparsity() is not implemented.");
    }
    default void set_seed(long seed) {
        throw new UnsupportedOperationException("set_seed() is not implemented.");
    }

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
