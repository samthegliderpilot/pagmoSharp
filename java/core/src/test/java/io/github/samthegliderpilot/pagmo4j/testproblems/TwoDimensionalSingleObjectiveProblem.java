package io.github.samthegliderpilot.pagmo4j.testproblems;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.problems.*;

/**
 * Canonical 2-D managed test problem: minimise x^2 + (y-3)^2, optimum at (0, 3).
 * ThreadSafety.Constant so it can be shared freely across threads.
 */
public class TwoDimensionalSingleObjectiveProblem extends ManagedProblemBase {

    public static final double OPTIMAL_X0 = 0.0;
    public static final double OPTIMAL_X1 = 3.0;
    public static final double OPTIMAL_F   = 0.0;

    @Override
    public DoubleVector fitness(DoubleVector x) {
        double dx = x.get(0);
        double dy = x.get(1) - 3.0;
        return vec(dx * dx + dy * dy);
    }

    @Override
    public PairOfDoubleVectors get_bounds() {
        return bounds(new double[]{-10.0, -10.0}, new double[]{10.0, 10.0});
    }

    @Override
    public boolean has_gradient() { return true; }

    @Override
    public DoubleVector gradient(DoubleVector x) {
        return vec(2.0 * x.get(0), 2.0 * x.get(1) - 6.0);
    }

    @Override
    public String get_name() { return "Simple 2-D Quadratic test problem"; }

    @Override
    public ThreadSafety get_thread_safety() { return ThreadSafety.Constant; }
}
