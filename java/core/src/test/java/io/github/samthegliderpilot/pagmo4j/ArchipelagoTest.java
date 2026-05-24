package io.github.samthegliderpilot.pagmo4j;

import io.github.samthegliderpilot.pagmo4j.problems.*;
import io.github.samthegliderpilot.pagmo4j.testproblems.*;
import org.junit.jupiter.api.Test;

import java.math.BigInteger;

import static org.junit.jupiter.api.Assertions.*;

/** Core archipelago smoke tests: multi-island construction and evolution. */
class ArchipelagoTest {

    @Test
    void archipelagoEvolvesMultipleIslands() {
        try (TwoDimensionalSingleObjectiveProblem prob = new TwoDimensionalSingleObjectiveProblem();
             de algo = new de(20L);
             archipelago archi = new archipelago()) {

            for (int i = 0; i < 4; i++) {
                archi.pushBackIsland(algo, prob, 32L, (long) i);
            }

            assertEquals(4L, archi.size());

            archi.evolve(1L);
            archi.wait_check();

            for (int i = 0; i < 4; i++) {
                try (island isl = archi.get_island_copy((long) i)) {
                    assertEquals(EvolveStatus.Idle, isl.status());
                    BigInteger fevals = isl.get_population().get_problem().get_fevals();
                    assertTrue(fevals.compareTo(BigInteger.ZERO) > 0,
                        "island " + i + " had no fitness evaluations after evolve");
                }
            }
        }
    }

    @Test
    void archipelagoPushBackRejectsNullProblem() {
        try (archipelago archi = new archipelago();
             de algo = new de(10L)) {
            assertThrows(NullPointerException.class,
                () -> archi.pushBackIsland(algo, null, 32L, 0L));
        }
    }

    @Test
    void archipelagoGetSizeReflectsIslandCount() {
        try (TwoDimensionalSingleObjectiveProblem prob = new TwoDimensionalSingleObjectiveProblem();
             de algo = new de(10L);
             archipelago archi = new archipelago()) {

            assertEquals(0L, archi.size());
            archi.pushBackIsland(algo, prob, 16L, 1L);
            assertEquals(1L, archi.size());
            archi.pushBackIsland(algo, prob, 16L, 2L);
            assertEquals(2L, archi.size());
        }
    }
}
