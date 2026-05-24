package io.github.samthegliderpilot.pagmo4j.interop;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.problems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/** Mirrors Test_thread_cloneable_problem.cs: per-island clone behaviour. */
class ThreadCloneableProblemTest {

    private static final class CloneableRastrigin extends ManagedProblemBase implements IThreadCloneableProblem {
        private static volatile int cloneCount = 0;

        @Override
        public DoubleVector fitness(DoubleVector x) {
            double sum = 0.0;
            for (int i = 0; i < (int) x.size(); i++) {
                sum += x.get(i) * x.get(i) - 10.0 * Math.cos(2 * Math.PI * x.get(i)) + 10.0;
            }
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

            archi.pushBackIsland(algo, prob, 32L, 1L);
            archi.pushBackIsland(algo, prob, 32L, 2L);

            assertTrue(CloneableRastrigin.cloneCount >= 2,
                "each island should receive its own clone");

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
            @Override public IProblem clone() { return null; }
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
        class SelfCloneProblem extends ManagedProblemBase implements IThreadCloneableProblem {
            @Override public DoubleVector fitness(DoubleVector x) { return vec(x.get(0)); }
            @Override public PairOfDoubleVectors get_bounds() { return bounds(new double[]{0.0}, new double[]{1.0}); }
            @Override public ThreadSafety get_thread_safety() { return ThreadSafety.None; }
            @Override public IProblem clone() { return this; }
        }

        try (de algo = new de(10L);
             archipelago archi = new archipelago()) {
            IllegalStateException ex = assertThrows(IllegalStateException.class,
                () -> archi.pushBackIsland(algo, new SelfCloneProblem(), 16L, 0L));
            assertTrue(ex.getMessage().contains("same instance"));
        }
    }
}
