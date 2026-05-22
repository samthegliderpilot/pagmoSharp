package io.github.samthegliderpilot.pagmo4j.algorithms;

import io.github.samthegliderpilot.pagmo4j.population;
import java.util.Collections;
import java.util.List;

/**
 * Contract for user-defined and native pagmo algorithms (UDAs).
 *
 * <p>Implement this interface to create a custom algorithm that can be used with
 * {@link io.github.samthegliderpilot.pagmo4j.island} and
 * {@link io.github.samthegliderpilot.pagmo4j.archipelago}.  All native pagmo
 * algorithm classes ({@code de}, {@code sade}, {@code pso}, etc.) also implement
 * this interface.
 *
 * <h3>Minimal implementation</h3>
 * <pre>{@code
 * IAlgorithm myAlgo = new IAlgorithm() {
 *     public population evolve(population pop) {
 *         try (de inner = new de(100L)) { return inner.evolve(pop); }
 *     }
 *     public String get_name() { return "MyAlgo"; }
 * };
 * }</pre>
 */
public interface IAlgorithm extends AutoCloseable {

    /**
     * Runs one evolution step on {@code pop} and returns the improved population.
     *
     * @param pop the input population (not modified in-place; a new population is returned)
     * @return the evolved population
     */
    population evolve(population pop);

    /** Returns a human-readable algorithm name used in logging and diagnostics. */
    String get_name();

    /** Returns extra diagnostic information (displayed by pagmo's pretty-printer). */
    default String get_extra_info() { return ""; }

    /** Sets the random seed (no-op if the algorithm doesn't support it). */
    default void set_seed(long seed) {}

    /** Returns the current random seed (0 if not applicable). */
    default long get_seed() { return 0L; }

    /** Returns the current verbosity level (0 = silent). */
    default long get_verbosity() { return 0L; }

    /**
     * Sets the verbosity level for logging ({@code 0} = silent, {@code 1} = every generation).
     * Has no effect if the algorithm doesn't support verbosity.
     */
    default void set_verbosity(long level) {}

    /**
     * Returns structured log lines produced during the last {@link #evolve(population)} call.
     * Returns an empty list for algorithms that do not produce log output.
     */
    default List<IAlgorithmLogLine> getLogLines() { return Collections.emptyList(); }

    @Override
    default void close() {}
}
