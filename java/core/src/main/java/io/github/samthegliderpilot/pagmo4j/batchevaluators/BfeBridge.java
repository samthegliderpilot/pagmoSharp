package io.github.samthegliderpilot.pagmo4j.batchevaluators;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.problems.*;

/**
 * Internal bridge for batch fitness evaluation via native BFE operators.
 *
 * <p>Routes {@link IProblem} instances through either the managed
 * {@link ManagedThreadBfe} (for cloneable non-thread-safe problems) or the
 * native BFE path.
 */
public final class BfeBridge {

    private BfeBridge() {}

    public static DoubleVector batchEvaluate(
            IProblem problem,
            DoubleVector batchX,
            boolean requiresParallelSafety) {

        if (problem == null) throw new NullPointerException("problem");
        if (batchX == null)  throw new NullPointerException("batchX");

        if (requiresParallelSafety
                && problem.get_thread_safety() == ThreadSafety.None
                && problem instanceof IThreadCloneableProblem cloneable) {
            IProblem probe = cloneable.clone();
            if (probe != null) {
                probe.close();
                try (ManagedThreadBfe bfe = new ManagedThreadBfe()) {
                    return bfe.operator(cloneable, batchX);
                }
            }
        }

        if (requiresParallelSafety) {
            problem.throwIfNotThreadSafe();
        }

        // pagmo::default_bfe throws for thread_safety::none problems without batch_fitness().
        // Fall back to Java-side serial evaluation in that case.
        if (problem.get_thread_safety() == ThreadSafety.None && !problem.has_batch_fitness()) {
            return serialBatchEvaluate(problem, batchX);
        }

        long problemPtr = NativeInterop.createProblemPointer(problem);
        try {
            long ptr = pagmo4j.pagmonet_default_bfe_evaluate(
                    problemPtr, NativeInterop.getDoubleVectorPtr(batchX));
            if (ptr == 0) {
                throw new RuntimeException(
                    "pagmonet_default_bfe_evaluate failed: " + pagmo4j.pagmonet_get_last_error());
            }
            return NativeInterop.wrapDoubleVectorPtr(ptr);
        } finally {
            pagmo4j.pagmonet_problem_delete(problemPtr);
        }
    }

    private static DoubleVector serialBatchEvaluate(IProblem problem, DoubleVector batchX) {
        int nx = (int) problem.get_bounds().getFirst().size();
        int batchSize = (int) batchX.size();
        if (nx == 0 || batchSize % nx != 0) {
            throw new IllegalArgumentException(
                "batchX size " + batchSize + " is not a multiple of nx=" + nx);
        }
        int n = batchSize / nx;

        DoubleVector x = new DoubleVector(nx, 0.0);
        try {
            for (int j = 0; j < nx; j++) x.set(j, batchX.get(j));
            DoubleVector f0 = problem.fitness(x);
            int nf = (int) f0.size();
            DoubleVector result = new DoubleVector(n * nf, 0.0);
            try {
                for (int k = 0; k < nf; k++) result.set(k, f0.get(k));
                f0.delete();
                for (int i = 1; i < n; i++) {
                    for (int j = 0; j < nx; j++) x.set(j, batchX.get(i * nx + j));
                    DoubleVector fi = problem.fitness(x);
                    try {
                        for (int k = 0; k < nf; k++) result.set(i * nf + k, fi.get(k));
                    } finally { fi.delete(); }
                }
                return result;
            } catch (Throwable t) { result.delete(); throw t; }
        } finally { x.delete(); }
    }
}
