package io.github.samthegliderpilot.pagmo4j.utils;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.problems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/** Mirrors Test_gradients_and_hessians.cs. */
class GradientsAndHessiansTest {

    private static final class SphereProblem extends ManagedProblemBase {
        @Override public DoubleVector fitness(DoubleVector x) {
            double sum = 0; for (int i = 0; i < x.size(); i++) sum += x.get(i) * x.get(i);
            DoubleVector v = new DoubleVector(); v.add(sum); return v;
        }
        @Override public PairOfDoubleVectors get_bounds() {
            return bounds(new double[]{-5.0, -5.0}, new double[]{5.0, 5.0});
        }
        @Override public ThreadSafety get_thread_safety() { return ThreadSafety.Basic; }
    }

    // ── type-erased (problem) path ────────────────────────────────────────────

    @Test
    void estimateGradientReturnsCorrectDimension() {
        try (SphereProblem prob = new SphereProblem();
             problem wrapped = new problem(prob)) {
            DoubleVector x = new DoubleVector(); x.add(1.0); x.add(1.0);
            DoubleVector g = GradientsAndHessians.estimateGradient(wrapped, x);
            assertEquals(2, g.size());
            assertEquals(2.0, g.get(0), 0.1);
            assertEquals(2.0, g.get(1), 0.1);
            g.delete();
        }
    }

    @Test
    void estimateSparsityReturnsNonEmptyPattern() {
        try (SphereProblem prob = new SphereProblem();
             problem wrapped = new problem(prob)) {
            DoubleVector x = new DoubleVector(); x.add(1.0); x.add(1.0);
            SparsityPattern sp = GradientsAndHessians.estimateSparsity(wrapped, x);
            assertTrue(sp.size() > 0);
            sp.delete();
        }
    }

    @Test
    void estimateGradientHighOrderIsSimilarToForwardDifferences() {
        try (SphereProblem prob = new SphereProblem();
             problem wrapped = new problem(prob)) {
            DoubleVector x = new DoubleVector(); x.add(1.0); x.add(1.0);
            DoubleVector g1 = GradientsAndHessians.estimateGradient(wrapped, x);
            DoubleVector g2 = GradientsAndHessians.estimateGradientHighOrder(wrapped, x);
            assertEquals(g1.size(), g2.size());
            for (int i = 0; i < g1.size(); i++) {
                assertEquals(g1.get(i), g2.get(i), 0.1);
            }
            g1.delete(); g2.delete();
        }
    }

    // ── managed (IProblem) path ───────────────────────────────────────────────

    @Test
    void estimateGradientIProblemMatchesTypeErased() {
        try (SphereProblem prob = new SphereProblem()) {
            DoubleVector x = new DoubleVector(); x.add(1.0); x.add(1.0);
            DoubleVector g = GradientsAndHessians.estimateGradient(prob, x);
            assertEquals(2, g.size(), "gradient dimension must equal problem dimension");
            assertEquals(2.0, g.get(0), 0.1, "df/dx0 at (1,1) should be ~2");
            assertEquals(2.0, g.get(1), 0.1, "df/dx1 at (1,1) should be ~2");
            g.delete();
        }
    }

    @Test
    void estimateSparsityIProblemReturnsNonEmptyPattern() {
        try (SphereProblem prob = new SphereProblem()) {
            DoubleVector x = new DoubleVector(); x.add(1.0); x.add(1.0);
            SparsityPattern sp = GradientsAndHessians.estimateSparsity(prob, x);
            assertTrue(sp.size() > 0, "sparsity pattern must be non-empty for dense sphere");
            sp.delete();
        }
    }

    @Test
    void estimateGradientHighOrderIProblemMatchesForwardDifferences() {
        try (SphereProblem prob = new SphereProblem()) {
            DoubleVector x = new DoubleVector(); x.add(1.0); x.add(1.0);
            DoubleVector g = GradientsAndHessians.estimateGradientHighOrder(prob, x);
            assertEquals(2, g.size());
            assertEquals(2.0, g.get(0), 0.1);
            assertEquals(2.0, g.get(1), 0.1);
            g.delete();
        }
    }

    // ── null-contract tests ───────────────────────────────────────────────────

    @Test
    void estimateGradientProblemNullThrows() {
        DoubleVector x = new DoubleVector(); x.add(1.0);
        assertThrows(NullPointerException.class,
            () -> GradientsAndHessians.estimateGradient((problem) null, x));
    }

    @Test
    void estimateGradientIProblemNullThrows() {
        DoubleVector x = new DoubleVector(); x.add(1.0);
        assertThrows(NullPointerException.class,
            () -> GradientsAndHessians.estimateGradient((IProblem) null, x));
    }

    @Test
    void estimateSparsityIProblemNullThrows() {
        DoubleVector x = new DoubleVector(); x.add(1.0);
        assertThrows(NullPointerException.class,
            () -> GradientsAndHessians.estimateSparsity((IProblem) null, x));
    }

    @Test
    void estimateGradientHighOrderIProblemNullThrows() {
        DoubleVector x = new DoubleVector(); x.add(1.0);
        assertThrows(NullPointerException.class,
            () -> GradientsAndHessians.estimateGradientHighOrder((IProblem) null, x));
    }
}
