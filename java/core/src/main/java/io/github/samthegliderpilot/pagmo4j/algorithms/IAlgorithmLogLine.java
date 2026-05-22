package io.github.samthegliderpilot.pagmo4j.algorithms;

import java.util.Map;

/**
 * Algorithm-agnostic view of a single structured log entry emitted during
 * {@link IAlgorithm#evolve(io.github.samthegliderpilot.pagmo4j.population)}.
 *
 * <p>Each entry represents one generation or iteration of a pagmo algorithm.
 * The fields vary by algorithm (e.g. DE logs "fevals", "best", "dx", "df";
 * CMA-ES logs "fevals", "gbest", "coeff. var", "std"), but are always accessible
 * via {@link #getRawFields()} for generic handling.
 */
public interface IAlgorithmLogLine {

    /** Returns the name of the algorithm that produced this log entry. */
    String getAlgorithmName();

    /**
     * Returns the raw log fields as an unmodifiable name-to-value map.
     * Keys are algorithm-specific field names; values are typically {@link Double} or {@link Long}.
     */
    Map<String, Object> getRawFields();

    /** Returns a human-readable summary of this log entry for display or debugging. */
    String toDisplayString();
}
