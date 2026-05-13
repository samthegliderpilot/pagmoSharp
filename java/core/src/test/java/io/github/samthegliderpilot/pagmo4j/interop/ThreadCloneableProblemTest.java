package io.github.samthegliderpilot.pagmo4j.interop;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.problems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/** Mirrors Test_thread_cloneable_problem.cs — per-island cloning behaviour. */
class ThreadCloneableProblemTest {

    private static final class CloneableRastrigin extends ManagedProblemBase {
        private static volatile int cloneCount = 0;

        @Override
        public DoubleVector fitness(DoubleVector x) {
            double sum = 0.0;
            for (int i = 0; i < (int) x.size(); i++)
                sum += x.get(i) * x.get(i) - 10.0 * Math.cos(2 * Math.PI * x.get(i)) + 10.0;
            return vec(sum);
        }

        @Override
        public PairOfDoubleVectors get_bounds() {
            return bounds(new double[]{-5.12, -5.12}, new double[]{5.12, 5.12});
        }

        @Override
        public ThreadSafety get_thread_safety() { return ThreadSafety.None; }

        @Override
        public IProblem clone() {
            cloneCount++;
            return new CloneableRastrigin();
        }
    }

    @Test
    void cloneableNonThreadSafeProblemCanBeAddedToArchipelago() {
        CloneableRastrigin.cloneCount = 0;

        try (CloneableRastrigin prob = new CloneableRastrigin();
             de algo = new de(20L);
             archipelago archi = new archipelago()) {

            // push_back_island clones the problem once per island
            archi.pushBackIsland(algo, prob, 32L, 1L);
            archi.pushBackIsland(algo, prob, 32L, 2L);

            assertTrue(CloneableRastrigin.cloneCount >= 2,
                "Each island should receive its own clone");

            archi.evolve(1L);
            archi.wait_check();
        }
    }

    @Test
    void nonCloneableNonThreadSafeProblemIsRejectedOnThreadedPath() {
        ManagedProblemBase noClone = new ManagedProblemBase() {
            @Override public DoubleVector fitness(DoubleVector x) { return vec(x.get(0)); }
            @Override public PairOfDoubleVectors get_bounds() { return bounds(new double[]{0.0}, new double[]{1.0}); }
            @Override public ThreadSafety get_thread_safety() { return ThreadSafety.None; }
            @Override public IProblem clone() { return null; } // no clone support
        };

        try (de algo = new de(10L);
             archipelago archi = new archipelago()) {
            assertThrows(IllegalStateException.class,
                () -> archi.pushBackIsland(algo, noClone, 16L, 0L),
                "ThreadSafety.None without clone() must be rejected");
        }
    }

    @Test
    void cloneReturningSameInstanceIsRejected() {
        ManagedProblemBase selfClone = new ManagedProblemBase() {
            @Override public DoubleVector fitness(DoubleVector x) { return vec(x.get(0)); }
            @Override public PairOfDoubleVectors get_bounds() { return bounds(new double[]{0.0}, new double[]{1.0}); }
            @Override public ThreadSafety get_thread_safety() { return ThreadSafety.None; }
            @Override public IProblem clone() { return this; } // same instance — forbidden
        };

        try (de algo = new de(10L);
             archipelago archi = new archipelago()) {
            assertThrows(IllegalStateException.class,
                () -> archi.pushBackIsland(algo, selfClone, 16L, 0L));
        }
    }
}
