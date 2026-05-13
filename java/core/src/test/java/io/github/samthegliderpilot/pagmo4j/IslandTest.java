package io.github.samthegliderpilot.pagmo4j;

import io.github.samthegliderpilot.pagmo4j.algorithms.*;
import io.github.samthegliderpilot.pagmo4j.problems.*;
import io.github.samthegliderpilot.pagmo4j.testproblems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/** Mirrors Test_island.cs — island creation, evolution and exception propagation. */
class IslandTest {

    private static final class MinimalManagedProblem extends ManagedProblemBase {
        @Override
        public PairOfDoubleVectors get_bounds() {
            return bounds(new double[]{-1.0, -1.0}, new double[]{1.0, 1.0});
        }
        @Override
        public DoubleVector fitness(DoubleVector x) {
            return vec(x.get(0) * x.get(0) + x.get(1) * x.get(1));
        }
        @Override public ThreadSafety get_thread_safety() { return ThreadSafety.Constant; }
        @Override public String get_name() { return "MinimalManagedProblem"; }
    }

    private static final class ThrowingManagedAlgorithm implements IAlgorithm {
        @Override public population evolve(population pop) {
            throw new IllegalStateException("Managed algorithm failure.");
        }
        @Override public void set_seed(long seed) {}
        @Override public long get_seed() { return 0L; }
        @Override public long get_verbosity() { return 0L; }
        @Override public void set_verbosity(long level) {}
        @Override public String get_name() { return "ThrowingManagedAlgorithm"; }
        @Override public String get_extra_info() { return ""; }
    }

    @Test
    void islandCanBeCreatedFromManagedProblemAndEvolved() {
        try (MinimalManagedProblem prob = new MinimalManagedProblem();
             IAlgorithm algo = new bee_colony();
             island isl = island.create(algo, prob, 32L, 2L)) {

            assertTrue(isl.is_valid());
            assertFalse(isl.get_name().isEmpty());

            isl.evolve(1L);
            isl.wait_check();
            assertEquals(EvolveStatus.Idle, isl.status());

            try (population pop = isl.get_population()) {
                assertEquals(32L, pop.size());
            }
        }
    }

    @Test
    void islandCreatedFromManagedAlgorithmEvolvesCorrectly() {
        try (TwoDimensionalSingleObjectiveProblem prob = new TwoDimensionalSingleObjectiveProblem();
             de algo = new de(20L);
             island isl = island.create(algo, prob, 64L, 42L)) {

            isl.evolve(1L);
            isl.wait_check();
            assertEquals(EvolveStatus.Idle, isl.status());
        }
    }

    @Test
    void managedProblemConstructionDoesNotCorruptSubsequentAlgorithmCalls() {
        try (MinimalManagedProblem managed = new MinimalManagedProblem();
             problem wrapped = new problem(managed);
             algorithm algo = new bee_colony().to_algorithm()) {

            assertEquals("ABC: Artificial Bee Colony", algo.get_name());
            assertEquals("MinimalManagedProblem", wrapped.get_name());
        }
    }

    @Test
    void managedAlgorithmExceptionSurfacesAsRuntimeException() {
        try (TwoDimensionalSingleObjectiveProblem prob = new TwoDimensionalSingleObjectiveProblem();
             ThrowingManagedAlgorithm throwing = new ThrowingManagedAlgorithm();
             island isl = island.create(throwing, prob, 32L, 2L)) {

            isl.evolve(1L);
            assertThrows(RuntimeException.class, isl::wait_check,
                "Exception from managed evolve() must surface via wait_check()");
        }
    }
}
