package io.github.samthegliderpilot.pagmo4j.interop;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm;
import io.github.samthegliderpilot.pagmo4j.testproblems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/** Mirrors Test_size_t_interop.cs — validates overflow guards on size_t boundary. */
class SizeTInteropTest {

    private static final long OVERSIZED = (long) Integer.MAX_VALUE + 1L;

    @Test
    void populationConstructorRejectsOversizedSize() {
        try (TwoDimensionalSingleObjectiveProblem prob = new TwoDimensionalSingleObjectiveProblem()) {
            IllegalArgumentException ex = assertThrows(IllegalArgumentException.class,
                () -> { try (population p = new population(prob, OVERSIZED, 2L)) {} });
            assertTrue(ex.getMessage().contains("popSize") || ex.getMessage().contains("32-bit"),
                "error should identify the parameter: " + ex.getMessage());
        }
    }

    @Test
    void islandCreateRejectsOversizedPopSize() {
        try (TwoDimensionalSingleObjectiveProblem prob = new TwoDimensionalSingleObjectiveProblem();
             IAlgorithm algo = new bee_colony()) {
            IllegalArgumentException ex = assertThrows(IllegalArgumentException.class,
                () -> { try (island isl = island.create(algo, prob, OVERSIZED)) {} });
            assertTrue(ex.getMessage().contains("popSize") || ex.getMessage().contains("32-bit"));
        }
    }

    @Test
    void archipelagoPushBackRejectsOversizedPopSize() {
        try (archipelago archi = new archipelago();
             TwoDimensionalSingleObjectiveProblem prob = new TwoDimensionalSingleObjectiveProblem();
             IAlgorithm algo = new bee_colony()) {
            assertThrows(IllegalArgumentException.class,
                () -> archi.pushBackIsland(algo, prob, OVERSIZED, 0L));
        }
    }
}
