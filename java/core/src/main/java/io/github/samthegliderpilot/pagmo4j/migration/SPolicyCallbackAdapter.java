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

    @Override
    public abstract IndividualsGroup select(
        IndividualsGroup population,
        long n_f, long n_ec, long n_ic, long n_obj,
        long pop_size, DoubleVector tol);
}
