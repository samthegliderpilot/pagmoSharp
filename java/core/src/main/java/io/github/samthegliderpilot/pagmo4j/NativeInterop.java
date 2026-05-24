package io.github.samthegliderpilot.pagmo4j;

import io.github.samthegliderpilot.pagmo4j.problems.*;
import io.github.samthegliderpilot.pagmo4j.algorithms.*;
import java.util.concurrent.ConcurrentHashMap;

/**
 * Internal bridge between managed {@link IProblem}/{@link IAlgorithm} implementations
 * and the pagmo4j JNI native layer.
 *
 * <p>Factory methods create SWIG director adapters and pin them against GC for the
 * lifetime of their owner, mirroring the {@code ConditionalWeakTable}/{@code GCHandle}
 * pattern from Pagmo.NET.
 */
public final class NativeInterop {

    private NativeInterop() {}

    private static final ConcurrentHashMap<Long, Object> problemRoots = new ConcurrentHashMap<>();
    private static final ConcurrentHashMap<Long, Object> algorithmRoots = new ConcurrentHashMap<>();

    private static void attachProblemRoot(long problemPtr, Object callback) {
        if (problemPtr != 0 && callback != null) {
            problemRoots.put(problemPtr, callback);
        }
    }

    private static void attachAlgorithmRoot(long algorithmPtr, Object callback) {
        if (algorithmPtr != 0 && callback != null) {
            algorithmRoots.put(algorithmPtr, callback);
        }
    }

    public static void releaseProblemRoot(long problemPtr) {
        if (problemPtr != 0) {
            problemRoots.remove(problemPtr);
        }
    }

    public static void releaseAlgorithmRoot(long algorithmPtr) {
        if (algorithmPtr != 0) {
            algorithmRoots.remove(algorithmPtr);
        }
    }

    /**
     * Creates a native {@code pagmo::problem*} pointer from a managed {@link IProblem}.
     *
     * <p>Wraps the problem in a {@link ProblemCallbackAdapter} (SWIG director), transfers
     * C++ ownership to native code via {@code pagmonet_problem_from_callback}, and pins
     * the adapter against GC for the owner's lifetime.
     */
    public static long createProblemPointer(IProblem problem) {
        if (problem == null) throw new NullPointerException("problem");

        ProblemCallbackAdapter adapter = new ProblemCallbackAdapter(problem);
        long cbPtr = ProblemCallback.getCPtr(adapter);

        adapter.swigReleaseOwnership();

        long problemPtr = pagmo4j.pagmonet_problem_from_callback(cbPtr);
        attachProblemRoot(problemPtr, adapter);

        // Surface any deferred exception from get_bounds() during construction.
        Throwable deferred = adapter.consumeDeferredException();
        if (deferred != null) {
            if (problemPtr != 0) {
                pagmo4j.pagmonet_problem_delete(problemPtr);
                releaseProblemRoot(problemPtr);
            }
            if (deferred instanceof RuntimeException) throw (RuntimeException) deferred;
            throw new RuntimeException(
                "IProblem callback threw during construction: " + deferred.getMessage(), deferred);
        }

        if (problemPtr == 0) {
            String nativeErr = pagmo4j.pagmonet_get_last_error();
            throw new RuntimeException(
                "Failed to build native pagmo::problem from IProblem callback." +
                (nativeErr != null && !nativeErr.isEmpty() ? " Native error: " + nativeErr : ""));
        }

        return problemPtr;
    }

    /** Wraps a raw pointer as a new owning {@link DoubleVector}. */
    public static DoubleVector wrapDoubleVectorPtr(long ptr) {
        return new DoubleVector(ptr, true);
    }

    /** Wraps a raw pointer as a new owning {@link SparsityPattern}. */
    public static SparsityPattern wrapSparsityPatternPtr(long ptr) {
        return new SparsityPattern(ptr, true);
    }

    /** Extracts the native pointer from a {@link DoubleVector} (accessible within this package). */
    public static long getDoubleVectorPtr(DoubleVector v) {
        return DoubleVector.getCPtr(v);
    }

    /** Extracts the native pointer from a {@link problem} wrapper. */
    public static long getProblemPtr(problem p) {
        return problem.getCPtr(p);
    }

    /** Wraps a raw sparsity-pattern pointer in a non-owning {@link SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t}. */
    public static SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t toSwigSparsityPattern(SparsityPattern sp) {
        return new SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t(SparsityPattern.getCPtr(sp), false);
    }

    /** Wraps a raw hessians-sparsity pointer in a non-owning {@link SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t}. */
    public static SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t toSwigVectorOfSparsityPattern(VectorOfSparsityPattern vsp) {
        return new SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t(VectorOfSparsityPattern.getCPtr(vsp), false);
    }

    /** Wraps a raw population pointer in a non-owning Java {@link population}. */
    public static population wrapPopulationPtr(long ptr) {
        return new population(ptr, false);
    }

    /** Extracts the native pointer from a {@link population} wrapper. */
    public static long getPopulationPtr(population pop) {
        return population.getCPtr(pop);
    }

    /** Wraps a raw pointer in a non-owning {@link SWIGTYPE_p_pagmo__population}. */
    public static SWIGTYPE_p_pagmo__population wrapSwigPopulationPtr(long ptr) {
        return new SWIGTYPE_p_pagmo__population(ptr, false);
    }

    /** Extracts the raw pointer from a {@link SWIGTYPE_p_pagmo__population}. */
    public static long getSwigPopulationPtr(SWIGTYPE_p_pagmo__population swig) {
        return SWIGTYPE_p_pagmo__population.getCPtr(swig);
    }

    public static long[] consumeOwnedSizeTVector(SWIGTYPE_p_std__vectorT_pagmo__pop_size_t_t opaque) {
        if (opaque == null) throw new NullPointerException("opaque");
        long ptr = SWIGTYPE_p_std__vectorT_pagmo__pop_size_t_t.getCPtr(opaque);
        SizeTVector v = new SizeTVector(ptr, true);
        try {
            long[] arr = new long[(int) v.size()];
            for (int i = 0; i < arr.length; i++) arr[i] = v.get(i);
            return arr;
        } finally {
            v.delete();
        }
    }

    public static long createAlgorithmPointer(IAlgorithm algorithm) {
        if (algorithm == null) throw new NullPointerException("algorithm");

        AlgorithmCallbackAdapter adapter = new AlgorithmCallbackAdapter(algorithm);
        long cbPtr = AlgorithmCallback.getCPtr(adapter);

        adapter.swigReleaseOwnership();

        if (cbPtr == 0) {
            throw new RuntimeException(
                "AlgorithmCallback native pointer is null — SWIG director construction failed");
        }

        long algorithmPtr = pagmo4j.pagmonet_algorithm_from_callback_java(cbPtr);
        attachAlgorithmRoot(algorithmPtr, adapter);

        // Mirror the problem path: surface any exception deferred during construction.
        // (No Java callbacks are invoked during managed_algorithm construction, so this
        // is precautionary, matching createProblemPointer() for consistency.)
        Throwable deferred = adapter.consumeDeferredException();
        if (deferred != null) {
            if (algorithmPtr != 0) {
                pagmo4jJNI.delete_algorithm(algorithmPtr);
                releaseAlgorithmRoot(algorithmPtr);
            }
            if (deferred instanceof RuntimeException re) throw re;
            throw new RuntimeException(
                "IAlgorithm callback threw during construction: " + deferred.getMessage(), deferred);
        }

        if (algorithmPtr == 0) {
            String nativeErr = pagmo4j.pagmonet_get_last_error();
            throw new RuntimeException(
                "Failed to build native pagmo::algorithm from IAlgorithm callback." +
                (nativeErr != null && !nativeErr.isEmpty() ? " Native error: " + nativeErr : ""));
        }

        return algorithmPtr;
    }
}
