package io.github.samthegliderpilot.pagmo4j.problems;

import io.github.samthegliderpilot.pagmo4j.*;

/**
 * Base class for managed Java problems (user-defined problems, UDPs).
 *
 * <p>Subclass this and implement {@link #fitness(DoubleVector)} and
 * {@link #get_bounds()}. All other methods have safe defaults.
 *
 * <p>Override {@link #clone()} to opt in to per-thread cloning for non-thread-safe
 * problems. The default returns {@code null}, preserving the thread-safety guard.
 */
public abstract class ManagedProblemBase implements IProblem, IThreadCloneableProblem {

    @Override public abstract DoubleVector fitness(DoubleVector x);
    @Override public abstract PairOfDoubleVectors get_bounds();

    @Override public String get_name()       { return getClass().getSimpleName(); }
    @Override public String get_extra_info() { return ""; }
    @Override public long get_nobj()         { return 1L; }
    @Override public long get_nec()          { return 0L; }
    @Override public long get_nic()          { return 0L; }
    @Override public long get_nix()          { return 0L; }
    @Override public boolean has_batch_fitness()     { return false; }
    @Override public ThreadSafety get_thread_safety() { return ThreadSafety.None; }

    @Override
    public IProblem clone() { return null; }

    @Override
    public DoubleVector batch_fitness(DoubleVector dvs) {
        throw new UnsupportedOperationException(get_name() + " does not provide batch_fitness().");
    }

    @Override public boolean has_gradient()          { return false; }
    @Override
    public DoubleVector gradient(DoubleVector x) {
        throw new UnsupportedOperationException(get_name() + " does not provide gradient().");
    }

    @Override public boolean has_gradient_sparsity() { return false; }
    @Override
    public SparsityPattern gradient_sparsity() {
        throw new UnsupportedOperationException(get_name() + " does not provide gradient_sparsity().");
    }

    @Override public boolean has_hessians() { return false; }
    @Override
    public VectorOfVectorOfDoubles hessians(DoubleVector x) {
        throw new UnsupportedOperationException(get_name() + " does not provide hessians().");
    }

    @Override public boolean has_hessians_sparsity() { return false; }
    @Override
    public VectorOfSparsityPattern hessians_sparsity() {
        throw new UnsupportedOperationException(get_name() + " does not provide hessians_sparsity().");
    }

    @Override
    public void set_seed(long seed) {
        throw new UnsupportedOperationException(get_name() + " does not provide set_seed().");
    }
    @Override public boolean has_set_seed() { return false; }

    @Override public void close() {}

    // ── Convenience helpers ───────────────────────────────────────────────────

    protected static DoubleVector vec(double... values) {
        DoubleVector v = new DoubleVector();
        for (double d : values) v.add(d);
        return v;
    }

    protected static PairOfDoubleVectors bounds(double[] lower, double[] upper) {
        DoubleVector l = new DoubleVector();
        DoubleVector u = new DoubleVector();
        for (double d : lower) l.add(d);
        for (double d : upper) u.add(d);
        return new PairOfDoubleVectors(l, u);
    }

    protected static PairOfDoubleVectors bounds(DoubleVector lower, DoubleVector upper) {
        return new PairOfDoubleVectors(lower, upper);
    }

    protected static SparsityPattern sparsity(long[]... entries) {
        SparsityPattern p = new SparsityPattern();
        for (long[] e : entries) p.add(new SizeTPair(e[0], e[1]));
        return p;
    }

    protected static VectorOfSparsityPattern hessiansSparsity(SparsityPattern... patterns) {
        VectorOfSparsityPattern r = new VectorOfSparsityPattern();
        for (SparsityPattern p : patterns) r.add(p);
        return r;
    }
}
