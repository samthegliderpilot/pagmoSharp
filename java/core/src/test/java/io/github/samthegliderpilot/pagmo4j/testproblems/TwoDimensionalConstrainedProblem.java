package io.github.samthegliderpilot.pagmo4j.testproblems;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.problems.*;

/**
 * 2-D constrained test problem: minimise x + y subject to x^2 + y^2 <= 1.
 * Optimum at approximately (-0.707, -0.707), f ≈ -1.414.
 */
public class TwoDimensionalConstrainedProblem extends ManagedProblemBase {

    public static final double OPTIMAL_X0 = -0.707;
    public static final double OPTIMAL_X1 = -0.707;
    public static final double OPTIMAL_F  = -1.414;

    @Override
    public DoubleVector fitness(DoubleVector x) {
        double obj = x.get(0) + x.get(1);
        double constraint = x.get(0) * x.get(0) + x.get(1) * x.get(1) - 1.0;
        return vec(obj, constraint);
    }

    @Override
    public PairOfDoubleVectors get_bounds() {
        return bounds(new double[]{-2.0, -2.0}, new double[]{2.0, 2.0});
    }

    @Override
    public long get_nobj() { return 1L; }

    @Override
    public long get_nic() { return 1L; }

    @Override
    public String get_name() { return "2-D Constrained test problem"; }

    @Override
    public ThreadSafety get_thread_safety() { return ThreadSafety.Constant; }
}
