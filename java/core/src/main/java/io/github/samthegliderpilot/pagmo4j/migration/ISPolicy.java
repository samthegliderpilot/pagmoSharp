package io.github.samthegliderpilot.pagmo4j.migration;

import io.github.samthegliderpilot.pagmo4j.*;

/**
 * Managed selection-policy contract for user-defined migration policies.
 *
 * <p>Implement this interface to control which individuals emigrate from an
 * island during migration. Pass the implementation to
 * {@code archipelago.pushBackIsland(..., IRPolicy, ISPolicy, ...)} to use it.
 *
 * <p>Extend {@link SPolicyCallbackAdapter} for the simplest path — it
 * implements this interface and wires SWIG director dispatch automatically.
 */
public interface ISPolicy extends AutoCloseable {

    /**
     * Selects emigrants from {@code population} to send to neighbouring islands.
     *
     * @param population source island's individuals
     * @param n_f        number of fitness components
     * @param n_ec       number of equality constraints
     * @param n_ic       number of inequality constraints
     * @param n_obj      number of objectives
     * @param pop_size   island population size
     * @param tol        constraint tolerances
     * @return the group of individuals chosen to emigrate
     */
    IndividualsGroup select(
        IndividualsGroup population,
        long n_f, long n_ec, long n_ic, long n_obj,
        long pop_size, DoubleVector tol);

    default String get_name()       { return "Java s_policy"; }
    default String get_extra_info() { return ""; }
    default boolean is_valid()      { return true; }

    @Override default void close() {}
}
