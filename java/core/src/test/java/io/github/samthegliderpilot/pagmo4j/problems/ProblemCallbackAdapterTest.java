package io.github.samthegliderpilot.pagmo4j.problems;

import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/**
 * Tests that {@link ProblemCallbackAdapter} correctly defers exceptions and returns
 * safe fallback values rather than letting them escape the JNI boundary.
 */
class ProblemCallbackAdapterTest {

    private static DoubleVector vec(double... values) {
        DoubleVector v = new DoubleVector();
        for (double d : values) v.add(d);
        return v;
    }

    // ── fitness exception deferral ────────────────────────────────────────────

    @Test
    void fitnessExceptionIsDeferredAndFallbackReturned() {
        RuntimeException boom = new RuntimeException("fitness boom");
        IProblem throwing = new ManagedProblemBase() {
            @Override public DoubleVector fitness(DoubleVector x) { throw boom; }
            @Override public PairOfDoubleVectors get_bounds() { return bounds(new double[]{-1}, new double[]{1}); }
        };

        ProblemCallbackAdapter adapter = new ProblemCallbackAdapter(throwing);
        DoubleVector x = vec(0.5);
        DoubleVector result = adapter.fitness(x);

        assertNotNull(result, "fitness() must return a non-null fallback even on exception");
        Throwable deferred = adapter.consumeDeferredException();
        assertSame(boom, deferred, "the original exception must be deferred");
        assertNull(adapter.consumeDeferredException(), "deferred exception must be cleared after consume");
    }

    @Test
    void fitnessNullReturnDeferredAsNullPointerException() {
        IProblem nullFitness = new ManagedProblemBase() {
            @Override public DoubleVector fitness(DoubleVector x) { return null; }
            @Override public PairOfDoubleVectors get_bounds() { return bounds(new double[]{-1}, new double[]{1}); }
        };

        ProblemCallbackAdapter adapter = new ProblemCallbackAdapter(nullFitness);
        DoubleVector result = adapter.fitness(vec(0.0));

        assertNotNull(result, "must return a non-null fallback for null fitness return");
        assertNotNull(adapter.consumeDeferredException(), "null fitness return must be deferred as NPE");
    }

    // ── first exception wins ──────────────────────────────────────────────────

    @Test
    void onlyFirstExceptionIsRetained() {
        RuntimeException first  = new RuntimeException("first");
        RuntimeException second = new RuntimeException("second");
        IProblem doubleThrow = new ManagedProblemBase() {
            private int calls = 0;
            @Override public DoubleVector fitness(DoubleVector x) { throw (calls++ == 0) ? first : second; }
            @Override public PairOfDoubleVectors get_bounds() { return bounds(new double[]{-1}, new double[]{1}); }
        };

        ProblemCallbackAdapter adapter = new ProblemCallbackAdapter(doubleThrow);
        adapter.fitness(vec(0.0));
        adapter.fitness(vec(0.0));
        assertSame(first, adapter.consumeDeferredException(), "only the first exception must be retained");
    }

    // ── get_bounds exception deferral ─────────────────────────────────────────

    @Test
    void boundsExceptionIsDeferredAndFallbackReturned() {
        RuntimeException boom = new RuntimeException("bounds boom");
        IProblem throwing = new ManagedProblemBase() {
            @Override public DoubleVector fitness(DoubleVector x) { return vec(0); }
            @Override public PairOfDoubleVectors get_bounds() { throw boom; }
        };

        ProblemCallbackAdapter adapter = new ProblemCallbackAdapter(throwing);
        PairOfDoubleVectors result = adapter.get_bounds();

        assertNotNull(result, "get_bounds() must return a non-null fallback on exception");
        assertSame(boom, adapter.consumeDeferredException());
    }

    // ── null constructor argument ─────────────────────────────────────────────

    @Test
    void nullProblemThrows() {
        assertThrows(NullPointerException.class, () -> new ProblemCallbackAdapter(null));
    }
}
