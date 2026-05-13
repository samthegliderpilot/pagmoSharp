package io.github.samthegliderpilot.pagmo4j.algorithms;

import io.github.samthegliderpilot.pagmo4j.population;
import java.util.Collections;
import java.util.List;

/**
 * Common managed algorithm contract across generated and handwritten algorithm wrappers.
 */
public interface IAlgorithm extends AutoCloseable {

    population evolve(population pop);
    String get_name();
    default String get_extra_info() { return ""; }

    // Optional methods — not all native algorithms implement these.
    default void set_seed(long seed) {}
    default long get_seed() { return 0L; }
    default long get_verbosity() { return 0L; }
    default void set_verbosity(long level) {}

    default List<IAlgorithmLogLine> getLogLines() { return Collections.emptyList(); }

    @Override
    default void close() {}
}
