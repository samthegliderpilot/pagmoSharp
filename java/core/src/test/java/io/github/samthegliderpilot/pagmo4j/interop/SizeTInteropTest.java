package io.github.samthegliderpilot.pagmo4j.interop;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm;
import io.github.samthegliderpilot.pagmo4j.testproblems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/** Mirrors Test_size_t_interop.cs — validates overflow guards on size_t boundary. */
class SizeTInteropTest {

    // Must exceed uint32 max (4294967295). Integer.MAX_VALUE+1 = 2147483648 is still valid uint32.
    // .NET equivalent: (ulong)uint.MaxValue + 1 = 4294967296
    private static final long OVERSIZED = 0xFFFFFFFFL + 1L; // = 4294967296

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
             IAlgorithm algo = new de(1L)) {
            IllegalArgumentException ex = assertThrows(IllegalArgumentException.class,
                () -> { try (island isl = island.create(algo, prob, OVERSIZED)) {} });
            assertTrue(ex.getMessage().contains("popSize") || ex.getMessage().contains("32-bit"));
        }
    }

    @Test
    void archipelagoPushBackRejectsOversizedPopSize() {
        try (archipelago archi = new archipelago();
             TwoDimensionalSingleObjectiveProblem prob = new TwoDimensionalSingleObjectiveProblem();
             IAlgorithm algo = new de(1L)) {
            assertThrows(IllegalArgumentException.class,
                () -> archi.pushBackIsland(algo, prob, OVERSIZED, 0L));
        }
    }
}
