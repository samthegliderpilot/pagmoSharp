package io.github.samthegliderpilot.pagmo4j.interop;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.algorithms.IAlgorithm;
import io.github.samthegliderpilot.pagmo4j.testproblems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/** Validates that null arguments are rejected with informative exceptions. */
class NullContractsTest {

    @Test
    void islandCreateRejectsNullAlgorithm() {
        try (TwoDimensionalSingleObjectiveProblem prob = new TwoDimensionalSingleObjectiveProblem()) {
            assertThrows(NullPointerException.class,
                () -> { try (island isl = island.create((IAlgorithm) null, prob, 32L)) {} });
        }
    }

    @Test
    void islandCreateRejectsNullProblem() {
        try (de algo = new de(10L)) {
            assertThrows(NullPointerException.class,
                () -> { try (island isl = island.create(algo, null, 32L)) {} });
        }
    }

    @Test
    void archipelagoPushBackRejectsNullAlgorithm() {
        try (archipelago archi = new archipelago();
             TwoDimensionalSingleObjectiveProblem prob = new TwoDimensionalSingleObjectiveProblem()) {
            assertThrows(NullPointerException.class,
                () -> archi.pushBackIsland((IAlgorithm) null, prob, 32L, 0L));
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
    void problemConstructorRejectsNullManagedProblem() {
        assertThrows(NullPointerException.class,
            () -> { try (problem p = new problem((io.github.samthegliderpilot.pagmo4j.problems.IProblem) null)) {} });
    }
}
