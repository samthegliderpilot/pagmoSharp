package io.github.samthegliderpilot.pagmo4j.testproblems;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.problems.*;

/** Simple 2-objective 2-D test problem for multi-objective algorithm validation. */
public class TwoDimensionalMultiObjectiveProblem extends ManagedProblemBase {

    @Override
    public DoubleVector fitness(DoubleVector x) {
        return vec(x.get(0) * x.get(0), (x.get(1) - 1.0) * (x.get(1) - 1.0));
    }

    @Override
    public PairOfDoubleVectors get_bounds() {
        return bounds(new double[]{-5.0, -5.0}, new double[]{5.0, 5.0});
    }

    @Override
    public long get_nobj() { return 2L; }

    @Override
    public String get_name() { return "2-D 2-Objective test problem"; }

    @Override
    public ThreadSafety get_thread_safety() { return ThreadSafety.Constant; }
}
