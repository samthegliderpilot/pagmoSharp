package io.github.samthegliderpilot.pagmo4j.problems;

import io.github.samthegliderpilot.pagmo4j.*;
import java.util.Objects;

/**
 * Wraps a cloned {@link IProblem} and re-declares thread safety as
 * {@link ThreadSafety#Basic}, since the clone is exclusively owned by one island.
 */
public final class ExclusiveCloneAdapter extends ManagedProblemBase {

    private final IProblem inner;

    public ExclusiveCloneAdapter(IProblem inner) {
        this.inner = Objects.requireNonNull(inner, "inner");
    }

    @Override public DoubleVector fitness(DoubleVector x)              { return inner.fitness(x); }
    @Override public PairOfDoubleVectors get_bounds()                  { return inner.get_bounds(); }
    @Override public ThreadSafety get_thread_safety()                  { return ThreadSafety.Basic; }
    @Override public String get_name()                                 { return inner.get_name(); }
    @Override public String get_extra_info()                           { return inner.get_extra_info(); }
    @Override public long get_nobj()                                   { return inner.get_nobj(); }
    @Override public long get_nec()                                    { return inner.get_nec(); }
    @Override public long get_nic()                                    { return inner.get_nic(); }
    @Override public long get_nix()                                    { return inner.get_nix(); }
    @Override public boolean has_gradient()                            { return inner.has_gradient(); }
    @Override public DoubleVector gradient(DoubleVector x)             { return inner.gradient(x); }
    @Override public boolean has_gradient_sparsity()                   { return inner.has_gradient_sparsity(); }
    @Override public SparsityPattern gradient_sparsity()               { return inner.gradient_sparsity(); }
    @Override public boolean has_hessians()                            { return inner.has_hessians(); }
    @Override public VectorOfVectorOfDoubles hessians(DoubleVector x)  { return inner.hessians(x); }
    @Override public boolean has_hessians_sparsity()                   { return inner.has_hessians_sparsity(); }
    @Override public VectorOfSparsityPattern hessians_sparsity()       { return inner.hessians_sparsity(); }
    @Override public boolean has_batch_fitness()                       { return inner.has_batch_fitness(); }
    @Override public DoubleVector batch_fitness(DoubleVector dvs)      { return inner.batch_fitness(dvs); }
    @Override public boolean has_set_seed()                            { return inner.has_set_seed(); }
    @Override public void set_seed(long seed)                          { inner.set_seed(seed); }
    @Override public void close()                                      { inner.close(); }
}
