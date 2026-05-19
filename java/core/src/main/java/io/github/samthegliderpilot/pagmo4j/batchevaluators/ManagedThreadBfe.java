package io.github.samthegliderpilot.pagmo4j.batchevaluators;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.problems.*;
import java.util.*;
import java.util.concurrent.*;
import java.util.stream.IntStream;

/**
 * Managed parallel batch fitness evaluator for cloneable problems.
 *
 * <p>Each OS thread gets an exclusive clone of the problem, allowing problems that
 * declare {@link io.github.samthegliderpilot.pagmo4j.ThreadSafety#None} but implement
 * {@link IThreadCloneableProblem} to be evaluated in parallel.
 */
public final class ManagedThreadBfe implements AutoCloseable {

    public DoubleVector operator(IThreadCloneableProblem problem, DoubleVector batchX) {
        if (problem == null) throw new NullPointerException("problem");
        if (batchX == null)  throw new NullPointerException("batchX");

        PairOfDoubleVectors boundsVec = problem.get_bounds();
        try {
            int dim = (int) boundsVec.getFirst().size();
            if (dim <= 0)
                throw new IllegalStateException(
                    "'" + problem.get_name() + "' returned non-positive problem dimension.");

            int n = (int) batchX.size();
            if (n % dim != 0)
                throw new IllegalArgumentException(
                    "batchX length " + n + " is not a multiple of problem dimension " + dim + ".");

            int batchSize   = n / dim;
            int fitnessLen  = (int)(problem.get_nobj() + problem.get_nec() + problem.get_nic());
            double[] flat   = new double[batchSize * fitnessLen];

            ThreadLocal<IProblem> clones = ThreadLocal.withInitial(() -> {
                IProblem c = problem.clone();
                if (c == null)
                    throw new IllegalStateException(
                        "'" + problem.get_name() + ".clone()' returned null during batch evaluation.");
                return c;
            });

            // Plain list — the explicit synchronized block below provides the only locking needed.
            List<IProblem> allClones = new ArrayList<>();
            try {
                ForkJoinPool.commonPool().submit(() ->
                    IntStream.range(0, batchSize).parallel().forEach(i -> {
                        IProblem clone = clones.get();
                        synchronized (allClones) { if (!allClones.contains(clone)) allClones.add(clone); }
                        DoubleVector x = slice(batchX, i * dim, dim);
                        try {
                            DoubleVector f = clone.fitness(x);
                            try {
                                for (int j = 0; j < fitnessLen; j++)
                                    flat[i * fitnessLen + j] = f.get(j);
                            } finally { f.delete(); }
                        } finally { x.delete(); }
                    })
                ).get();
            } catch (ExecutionException e) {
                Throwable cause = e.getCause();
                if (cause instanceof RuntimeException) throw (RuntimeException) cause;
                throw new RuntimeException("Batch evaluation failed", cause);
            } catch (InterruptedException e) {
                Thread.currentThread().interrupt();
                throw new RuntimeException("Batch evaluation interrupted", e);
            } finally {
                allClones.forEach(IProblem::close);
                clones.remove();
            }

            DoubleVector result = new DoubleVector();
            for (double d : flat) result.add(d);
            return result;
        } finally { boundsVec.delete(); }
    }

    @Override public void close() {}

    private static DoubleVector slice(DoubleVector src, int offset, int len) {
        DoubleVector v = new DoubleVector();
        for (int i = 0; i < len; i++) v.add(src.get(offset + i));
        return v;
    }
}
