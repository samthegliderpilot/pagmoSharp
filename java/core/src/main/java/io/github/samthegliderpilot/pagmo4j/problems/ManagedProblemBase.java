package io.github.samthegliderpilot.pagmo4j.problems;

import io.github.samthegliderpilot.pagmo4j.*;

/**
 * Convenient base class for user-defined pagmo4j problems (UDPs).
 *
 * <p>Subclass this and implement the two abstract methods; everything else has
 * sensible defaults (single-objective, unconstrained, no gradient, no seed support).
 *
 * <h3>Thread safety</h3>
 * <p>The default {@link #get_thread_safety()} returns {@link ThreadSafety#None}.
 * Override it to return {@link ThreadSafety#Basic} if the problem is concurrency-safe,
 * or override {@link #clone()} to return an independent copy so pagmo4j can create
 * per-thread instances automatically.
 *
 * <h3>Convenience helpers</h3>
 * <p>Use {@link #vec(double...)}, {@link #bounds(double[], double[])}, and
 * {@link #sparsity(long[]...)} to build return values compactly.
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

    /** Builds a {@link DoubleVector} from a varargs list of doubles. */
    protected static DoubleVector vec(double... values) {
        DoubleVector v = new DoubleVector();
        for (double d : values) v.add(d);
        return v;
    }

    /** Builds a bounds pair from parallel lower/upper primitive arrays. */
    protected static PairOfDoubleVectors bounds(double[] lower, double[] upper) {
        DoubleVector l = new DoubleVector();
        DoubleVector u = new DoubleVector();
        for (double d : lower) l.add(d);
        for (double d : upper) u.add(d);
        return new PairOfDoubleVectors(l, u);
    }

    /** Builds a bounds pair from pre-constructed {@link DoubleVector} instances. */
    protected static PairOfDoubleVectors bounds(DoubleVector lower, DoubleVector upper) {
        return new PairOfDoubleVectors(lower, upper);
    }

    /** Builds a {@link SparsityPattern} from {@code {row, col}} long-array pairs. */
    protected static SparsityPattern sparsity(long[]... entries) {
        SparsityPattern p = new SparsityPattern();
        for (long[] e : entries) p.add(new SizeTPair(e[0], e[1]));
        return p;
    }

    /** Builds a {@link VectorOfSparsityPattern} from per-objective sparsity patterns. */
    protected static VectorOfSparsityPattern hessiansSparsity(SparsityPattern... patterns) {
        VectorOfSparsityPattern r = new VectorOfSparsityPattern();
        for (SparsityPattern p : patterns) r.add(p);
        return r;
    }
}
