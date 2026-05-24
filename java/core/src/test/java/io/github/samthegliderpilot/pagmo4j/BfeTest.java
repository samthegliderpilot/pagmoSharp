package io.github.samthegliderpilot.pagmo4j;

import io.github.samthegliderpilot.pagmo4j.batchevaluators.*;
import io.github.samthegliderpilot.pagmo4j.problems.*;
import io.github.samthegliderpilot.pagmo4j.testproblems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/** Mirrors Test_bfe.cs — batch fitness evaluator routing. */
class BfeTest {

    private static final class BatchCapableProblem extends ManagedProblemBase {
        @Override
        public DoubleVector fitness(DoubleVector x) { return vec(x.get(0) * x.get(0) + x.get(1) * x.get(1)); }
        @Override
        public PairOfDoubleVectors get_bounds() { return bounds(new double[]{-5.0, -5.0}, new double[]{5.0, 5.0}); }
        @Override public ThreadSafety get_thread_safety() { return ThreadSafety.Basic; }
        @Override public boolean has_batch_fitness() { return true; }
        @Override
        public DoubleVector batch_fitness(DoubleVector dvs) {
            DoubleVector out = new DoubleVector();
            for (int i = 0; i < (int) dvs.size(); i += 2)
                out.add(dvs.get(i) * dvs.get(i) + dvs.get(i + 1) * dvs.get(i + 1));
            return out;
        }
    }

    @Test
    void defaultBfeEvaluatesBatchCorrectly() {
        try (BatchCapableProblem prob = new BatchCapableProblem();
             default_bfe bfe = new default_bfe()) {
            DoubleVector batch = new DoubleVector();
            batch.add(1.0); batch.add(2.0);
            batch.add(3.0); batch.add(4.0);
            DoubleVector result = bfe.operator(prob, batch);
            assertEquals(2, result.size());
            assertEquals(5.0,  result.get(0), 1e-12);
            assertEquals(25.0, result.get(1), 1e-12);
            result.delete();
        }
    }

    @Test
    void threadBfeWorksWithThreadSafeProblem() {
        try (TwoDimensionalSingleObjectiveProblem prob = new TwoDimensionalSingleObjectiveProblem();
             thread_bfe bfe = new thread_bfe()) {
            DoubleVector batch = new DoubleVector();
            batch.add(0.0); batch.add(3.0);
            batch.add(1.0); batch.add(3.0);
            DoubleVector result = bfe.operator(prob, batch);
            assertEquals(2, result.size());
            assertEquals(0.0, result.get(0), 1e-12);
            assertEquals(1.0, result.get(1), 1e-12);
            result.delete();
        }
    }

    @Test
    void threadBfeRejectsManagedProblemWithThreadSafetyNone() {
        ManagedProblemBase noThreadSafety = new ManagedProblemBase() {
            @Override public DoubleVector fitness(DoubleVector x) { return vec(x.get(0)); }
            @Override public PairOfDoubleVectors get_bounds() { return bounds(new double[]{0.0}, new double[]{1.0}); }
            @Override public ThreadSafety get_thread_safety() { return ThreadSafety.None; }
            @Override public IProblem clone() { return null; }
        };
        try (thread_bfe bfe = new thread_bfe()) {
            DoubleVector batch = new DoubleVector(); batch.add(0.5);
            assertThrows(IllegalStateException.class, () -> bfe.operator(noThreadSafety, batch));
        }
    }

    @Test
    void managedThreadBfeUsesCloneForNonThreadSafeProblem() {
        class CloneableProblem extends ManagedProblemBase implements IThreadCloneableProblem {
            @Override public DoubleVector fitness(DoubleVector x) { return vec(x.get(0) * x.get(0)); }
            @Override public PairOfDoubleVectors get_bounds() { return bounds(new double[]{-1.0}, new double[]{1.0}); }
            @Override public ThreadSafety get_thread_safety() { return ThreadSafety.None; }
            @Override public IProblem clone() { return new CloneableProblem(); }
        }
        IThreadCloneableProblem cloneable = new CloneableProblem();
        try (ManagedThreadBfe bfe = new ManagedThreadBfe()) {
            DoubleVector batch = new DoubleVector(); batch.add(2.0); batch.add(3.0);
            DoubleVector result = bfe.operator(cloneable, batch);
            assertEquals(2, result.size());
            assertEquals(4.0, result.get(0), 1e-12);
            assertEquals(9.0, result.get(1), 1e-12);
            result.delete();
        }
    }

    @Test
    void managedThreadBfeRejectsCloneReturningSameInstance() {
        class SelfCloneProblem extends ManagedProblemBase implements IThreadCloneableProblem {
            @Override public DoubleVector fitness(DoubleVector x) { return vec(x.get(0) * x.get(0)); }
            @Override public PairOfDoubleVectors get_bounds() { return bounds(new double[]{-1.0}, new double[]{1.0}); }
            @Override public ThreadSafety get_thread_safety() { return ThreadSafety.None; }
            @Override public IProblem clone() { return this; }
        }
        IThreadCloneableProblem selfClone = new SelfCloneProblem();
        try (ManagedThreadBfe bfe = new ManagedThreadBfe()) {
            DoubleVector batch = new DoubleVector();
            batch.add(2.0);
            IllegalStateException ex = assertThrows(IllegalStateException.class,
                () -> bfe.operator(selfClone, batch));
            assertTrue(ex.getMessage().contains("same instance"));
        }
    }
}
