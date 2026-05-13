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

        long problemPtr = NativeInterop.createProblemPointer(problem);
        try {
            return NativeInterop.wrapDoubleVectorPtr(
                pagmo4j.pagmonet_default_bfe_evaluate(problemPtr, NativeInterop.getDoubleVectorPtr(batchX)));
        } finally {
            pagmo4j.pagmonet_problem_delete(problemPtr);
        }
    }
}
