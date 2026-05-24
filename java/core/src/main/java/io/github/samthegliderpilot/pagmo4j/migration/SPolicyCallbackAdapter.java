package io.github.samthegliderpilot.pagmo4j.migration;

import io.github.samthegliderpilot.pagmo4j.*;

/**
 * SWIG director adapter for user-defined selection policies.
 *
 * <p>Extend this class and override {@link #select} to implement a custom
 * selection policy. Pass it to
 * {@code archipelago.pushBackIsland(..., IRPolicy, ISPolicy, ...)} or wrap it
 * directly: {@code new ManagedSPolicy(adapter)}.
 *
 * <p>Inheriting from {@link SPolicyCallback} wires SWIG director dispatch so
 * native pagmo calls back into this Java implementation during migration.
 */
public abstract class SPolicyCallbackAdapter extends SPolicyCallback implements ISPolicy {
    private volatile Throwable deferredEx;

    @Override
    public final IndividualsGroup select(
        IndividualsGroup population,
        long n_f, long n_ec, long n_ic, long n_obj,
        long pop_size, DoubleVector tol) {
        try {
            IndividualsGroup result = selectManaged(population, n_f, n_ec, n_ic, n_obj, pop_size, tol);
            if (result == null) throw new NullPointerException("select() returned null");
            return result;
        } catch (Throwable ex) {
            if (deferredEx == null) deferredEx = ex;
            return new IndividualsGroup();
        }
    }

    protected abstract IndividualsGroup selectManaged(
        IndividualsGroup population,
        long n_f, long n_ec, long n_ic, long n_obj,
        long pop_size, DoubleVector tol);

    @Override
    public String get_name() {
        try { return ISPolicy.super.get_name(); } catch (Throwable ex) { if (deferredEx == null) deferredEx = ex; return "Java s_policy"; }
    }

    @Override
    public String get_extra_info() {
        try { return ISPolicy.super.get_extra_info(); } catch (Throwable ex) { if (deferredEx == null) deferredEx = ex; return ""; }
    }

    @Override
    public boolean is_valid() {
        try { return ISPolicy.super.is_valid(); } catch (Throwable ex) { if (deferredEx == null) deferredEx = ex; return false; }
    }

    public Throwable consumeDeferredException() {
        Throwable ex = deferredEx;
        deferredEx = null;
        return ex;
    }
}
