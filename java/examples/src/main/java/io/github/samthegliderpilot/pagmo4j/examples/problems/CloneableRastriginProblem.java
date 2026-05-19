package io.github.samthegliderpilot.pagmo4j.examples.problems;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.problems.*;
import java.util.concurrent.atomic.AtomicInteger;
import java.util.concurrent.atomic.AtomicLong;

/**
 * Rastrigin problem with per-instance mutable state that is NOT thread-safe.
 *
 * <p>Implements {@link IThreadCloneableProblem} so the archipelago machinery
 * can give each island its own exclusive clone rather than sharing one instance.
 * This demonstrates how problems that cannot be evaluated concurrently can still
 * participate in parallel island search.
 */
public final class CloneableRastriginProblem extends ManagedProblemBase
        implements IThreadCloneableProblem {

    public static final AtomicInteger cloneCount = new AtomicInteger(0);
    public static final AtomicLong totalEvaluations = new AtomicLong(0);

    // Per-instance mutable state — safe ONLY because each island has its own clone.
    private int instanceEvaluations = 0;

    private static final double A = 10.0;

    @Override
    public DoubleVector fitness(DoubleVector x) {
        instanceEvaluations++;
        totalEvaluations.incrementAndGet();
        double f = A * 2;
        for (int i = 0; i < 2; i++) {
            double xi = x.get(i);
            f += xi * xi - A * Math.cos(2.0 * Math.PI * xi);
        }
        return vec(f);
    }

    @Override
    public PairOfDoubleVectors get_bounds() {
        return bounds(new double[]{-5.12, -5.12}, new double[]{5.12, 5.12});
    }

    @Override public String get_name() { return "CloneableRastriginProblem"; }

    @Override
    public String get_extra_info() {
        return "instance_evaluations=" + instanceEvaluations;
    }

    // Declares that this instance is not safe for concurrent access.
    @Override public ThreadSafety get_thread_safety() { return ThreadSafety.None; }

    // Each island receives an independent clone with its own counter state.
    @Override
    public IProblem clone() {
        cloneCount.incrementAndGet();
        return new CloneableRastriginProblem();
    }
}
