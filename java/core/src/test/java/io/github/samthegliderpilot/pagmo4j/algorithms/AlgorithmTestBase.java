package io.github.samthegliderpilot.pagmo4j.algorithms;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.testproblems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
import static org.junit.jupiter.api.Assumptions.*;

/**
 * Shared abstract test harness for all pagmo algorithm wrappers.
 * Concrete subclasses implement {@link #createAlgorithm()} and declare capability flags.
 * All {@code @Test} methods here are inherited automatically by JUnit 5.
 */
public abstract class AlgorithmTestBase {

    public abstract IAlgorithm createAlgorithm();

    public abstract boolean supportsUnconstrained();
    public abstract boolean supportsConstrained();
    public abstract boolean supportsSingleObjective();
    public abstract boolean supportsMultiObjective();

    public boolean expectEvolutionIncreasesFevals() { return true; }
    public boolean supportsSeed()      { return true; }
    public boolean supportsVerbosity() { return true; }

    // ── Basic contract ────────────────────────────────────────────────────────

    @Test
    void algorithmHasNonEmptyName() {
        try (IAlgorithm algo = createAlgorithm()) {
            assertFalse(algo.get_name().isEmpty());
        }
    }

    @Test
    void seedRoundTrips() {
        assumeTrue(supportsSeed(), "Algorithm does not support set_seed()");
        try (IAlgorithm algo = createAlgorithm()) {
            algo.set_seed(42L);
            assertEquals(42L, algo.get_seed());
        }
    }

    @Test
    void verbosityRoundTrips() {
        assumeTrue(supportsVerbosity(), "Algorithm does not support set_verbosity()");
        try (IAlgorithm algo = createAlgorithm()) {
            algo.set_verbosity(1L);
            assertEquals(1L, algo.get_verbosity());
        }
    }

    // ── Evolution on 2-D unconstrained ───────────────────────────────────────

    @Test
    void evolvesOnTwoDimensionalProblem() {
        assumeTrue(supportsSingleObjective() && supportsUnconstrained(),
            "Not applicable: algorithm does not support single-objective unconstrained.");

        try (TwoDimensionalSingleObjectiveProblem prob = new TwoDimensionalSingleObjectiveProblem();
             problem wrapped = new problem(prob);
             IAlgorithm algo = createAlgorithm();
             population pop = new population(wrapped, 64L, 2L)) {

            long initialFevals = fevals(pop);
            try (population evolved = algo.evolve(pop)) {
                assertEquals(pop.size(), evolved.size(), "evolve() must preserve population size");
                if (expectEvolutionIncreasesFevals()) {
                    assertFevalsIncreased(evolved, initialFevals);
                }
            }
        }
    }

    // ── Evolution on 2-D constrained ─────────────────────────────────────────

    @Test
    void evolvesOnConstrainedProblem() {
        assumeTrue(supportsSingleObjective() && supportsConstrained(),
            "Not applicable: algorithm does not support constrained single-objective.");

        try (TwoDimensionalConstrainedProblem prob = new TwoDimensionalConstrainedProblem();
             problem wrapped = new problem(prob);
             IAlgorithm algo = createAlgorithm();
             population pop = new population(wrapped, 256L, 2L)) {

            algo.set_seed(2L);
            long initialFevals = fevals(pop);
            try (population evolved = algo.evolve(pop)) {
                assertEquals(pop.size(), evolved.size());
                if (expectEvolutionIncreasesFevals()) {
                    assertFevalsIncreased(evolved, initialFevals);
                }
            }
        }
    }

    // ── Multi-objective ───────────────────────────────────────────────────────

    @Test
    void evolvesOnMultiObjectiveProblem() {
        assumeTrue(supportsMultiObjective() && supportsUnconstrained(),
            "Not applicable: algorithm does not support unconstrained multi-objective.");

        try (TwoDimensionalMultiObjectiveProblem prob = new TwoDimensionalMultiObjectiveProblem();
             problem wrapped = new problem(prob);
             IAlgorithm algo = createAlgorithm();
             population pop = new population(wrapped, 128L, 2L)) {

            algo.set_seed(2L);
            long initialFevals = fevals(pop);
            try (population evolved = algo.evolve(pop)) {
                assertEquals(pop.size(), evolved.size());
                if (expectEvolutionIncreasesFevals()) {
                    assertFevalsIncreased(evolved, initialFevals);
                }
                // champion is unavailable for multi-objective
                assertThrows(RuntimeException.class, () -> evolved.champion_f());
            }
        }
    }

    // ── Log surface ───────────────────────────────────────────────────────────

    @Test
    void getLogLinesReturnsNonNull() {
        assumeTrue(supportsSingleObjective() && supportsUnconstrained());

        try (TwoDimensionalSingleObjectiveProblem prob = new TwoDimensionalSingleObjectiveProblem();
             problem wrapped = new problem(prob);
             IAlgorithm algo = createAlgorithm();
             population pop = new population(wrapped, 48L, 2L)) {

            algo.set_verbosity(1L);
            try (population evolved = algo.evolve(pop)) {
                var lines = algo.getLogLines();
                assertNotNull(lines, "getLogLines() must never return null");
            }
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    protected static long fevals(population pop) {
        try (problem p = pop.get_problem()) {
            return p.get_fevals().longValue();
        }
    }

    protected static void assertFevalsIncreased(population evolved, long initialFevals) {
        try (problem p = evolved.get_problem()) {
            assertTrue(p.get_fevals().longValue() > initialFevals,
                "evolve() should trigger additional function evaluations");
        }
    }
}
