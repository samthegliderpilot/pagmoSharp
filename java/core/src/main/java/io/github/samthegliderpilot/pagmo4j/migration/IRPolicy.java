package io.github.samthegliderpilot.pagmo4j.migration;

import io.github.samthegliderpilot.pagmo4j.*;

/**
 * Managed replacement-policy contract for user-defined migration policies.
 *
 * <p>Implement this interface to control which incoming migrants replace
 * individuals in an island's population. Pass the implementation to
 * {@code archipelago.pushBackIsland(..., IRPolicy, ISPolicy, ...)} to use it.
 *
 * <p>Extend {@link RPolicyCallbackAdapter} for the simplest path — it
 * implements this interface and wires SWIG director dispatch automatically.
 */
public interface IRPolicy extends AutoCloseable {

    /**
     * Selects which individuals from {@code incoming} replace entries in {@code current}.
     *
     * @param incoming  migrants arriving at this island
     * @param n_f       number of fitness components
     * @param n_ec      number of equality constraints
     * @param n_ic      number of inequality constraints
     * @param n_obj     number of objectives
     * @param pop_size  island population size
     * @param tol       constraint tolerances
     * @param current   current island population
     * @return the replacement group to merge into the island
     */
    IndividualsGroup replace(
        IndividualsGroup incoming,
        long n_f, long n_ec, long n_ic, long n_obj,
        long pop_size, DoubleVector tol,
        IndividualsGroup current);

    default String get_name()       { return "Java r_policy"; }
    default String get_extra_info() { return ""; }
    default boolean is_valid()      { return true; }

    @Override default void close() {}
}
