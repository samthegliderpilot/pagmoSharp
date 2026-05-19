package io.github.samthegliderpilot.pagmo4j.batchevaluators;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.problems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/** Tests for BfeBridge routing logic. */
class BfeBridgeTest {

    // Thread-safe problem — should route through native BFE path.
    private static final class ThreadSafeProblem extends ManagedProblemBase {
        @Override public DoubleVector fitness(DoubleVector x) { return vec(x.get(0) * x.get(0)); }
        @Override public PairOfDoubleVectors get_bounds() { return bounds(new double[]{-1.0}, new double[]{1.0}); }
        @Override public ThreadSafety get_thread_safety() { return ThreadSafety.Basic; }
    }

    // Non-thread-safe cloneable problem — should route through ManagedThreadBfe.
    private static final class CloneableProblem extends ManagedProblemBase implements IThreadCloneableProblem {
        @Override public DoubleVector fitness(DoubleVector x) { return vec(x.get(0) * x.get(0)); }
        @Override public PairOfDoubleVectors get_bounds() { return bounds(new double[]{-1.0}, new double[]{1.0}); }
        @Override public ThreadSafety get_thread_safety() { return ThreadSafety.None; }
        @Override public IProblem clone() {
            return new CloneableProblem();
        }
    }

    // Non-thread-safe non-cloneable — should throw when parallel safety required.
    private static final class UnsafeProblem extends ManagedProblemBase {
        @Override public DoubleVector fitness(DoubleVector x) { return vec(x.get(0) * x.get(0)); }
        @Override public PairOfDoubleVectors get_bounds() { return bounds(new double[]{-1.0}, new double[]{1.0}); }
        @Override public ThreadSafety get_thread_safety() { return ThreadSafety.None; }
    }

    @Test
    void threadSafeProblemEvaluatesCorrectly() {
        try (ThreadSafeProblem prob = new ThreadSafeProblem()) {
            DoubleVector batch = new DoubleVector();
            batch.add(2.0); batch.add(3.0);
            DoubleVector result = BfeBridge.batchEvaluate(prob, batch, false);
            try {
                assertEquals(2, result.size());
                assertEquals(4.0, result.get(0), 1e-12);
                assertEquals(9.0, result.get(1), 1e-12);
            } finally { result.delete(); }
        }
    }

    @Test
    void cloneableProblemEvaluatesCorrectlyWithParallelSafety() {
        try (CloneableProblem prob = new CloneableProblem()) {
            DoubleVector batch = new DoubleVector();
            batch.add(2.0); batch.add(3.0);
            DoubleVector result = BfeBridge.batchEvaluate(prob, batch, true);
            try {
                assertEquals(2, result.size());
                assertEquals(4.0, result.get(0), 1e-12);
                assertEquals(9.0, result.get(1), 1e-12);
            } finally { result.delete(); }
        }
    }

    @Test
    void unsafeProblemThrowsWhenParallelSafetyRequired() {
        try (UnsafeProblem prob = new UnsafeProblem()) {
            DoubleVector batch = new DoubleVector();
            batch.add(1.0);
            assertThrows(IllegalStateException.class,
                () -> BfeBridge.batchEvaluate(prob, batch, true));
        }
    }

    @Test
    void unsafeProblemSucceedsWhenParallelSafetyNotRequired() {
        try (UnsafeProblem prob = new UnsafeProblem()) {
            DoubleVector batch = new DoubleVector();
            batch.add(3.0);
            DoubleVector result = BfeBridge.batchEvaluate(prob, batch, false);
            try {
                assertEquals(1, result.size());
                assertEquals(9.0, result.get(0), 1e-12);
            } finally { result.delete(); }
        }
    }

    @Test
    void nullProblemThrows() {
        DoubleVector batch = new DoubleVector();
        batch.add(1.0);
        assertThrows(NullPointerException.class, () -> BfeBridge.batchEvaluate(null, batch, false));
    }

    @Test
    void nullBatchThrows() {
        try (ThreadSafeProblem prob = new ThreadSafeProblem()) {
            assertThrows(NullPointerException.class, () -> BfeBridge.batchEvaluate(prob, null, false));
        }
    }
}
