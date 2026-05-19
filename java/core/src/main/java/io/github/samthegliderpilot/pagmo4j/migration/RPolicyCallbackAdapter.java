package io.github.samthegliderpilot.pagmo4j.migration;

import io.github.samthegliderpilot.pagmo4j.*;

/**
 * SWIG director adapter for user-defined replacement policies.
 *
 * <p>Extend this class and override {@link #replace} to implement a custom
 * replacement policy. Pass it to
 * {@code archipelago.pushBackIsland(..., IRPolicy, ISPolicy, ...)} or wrap it
 * directly: {@code new ManagedRPolicy(adapter)}.
 *
 * <p>Inheriting from {@link RPolicyCallback} wires SWIG director dispatch so
 * native pagmo calls back into this Java implementation during migration.
 */
public abstract class RPolicyCallbackAdapter extends RPolicyCallback implements IRPolicy {

    @Override
    public abstract IndividualsGroup replace(
        IndividualsGroup incoming,
        long n_f, long n_ec, long n_ic, long n_obj,
        long pop_size, DoubleVector tol,
        IndividualsGroup current);
}
