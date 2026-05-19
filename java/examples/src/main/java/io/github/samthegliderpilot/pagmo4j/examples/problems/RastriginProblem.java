package io.github.samthegliderpilot.pagmo4j.examples.problems;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.problems.*;

/**
 * 2-D Rastrigin function — a standard multimodal benchmark with many local minima.
 * Global minimum at (0,0) with fitness 0.
 */
public final class RastriginProblem extends ManagedProblemBase {

    private static final double A = 10.0;

    @Override
    public DoubleVector fitness(DoubleVector x) {
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

    @Override public String get_name() { return "RastriginProblem"; }
    @Override public ThreadSafety get_thread_safety() { return ThreadSafety.Basic; }
}
