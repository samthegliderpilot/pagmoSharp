package io.github.samthegliderpilot.pagmo4j;

import io.github.samthegliderpilot.pagmo4j.problems.*;
import io.github.samthegliderpilot.pagmo4j.algorithms.*;
import java.util.WeakHashMap;
import java.util.ArrayList;
import java.util.List;

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

    // WeakHashMap ties each callback list's lifetime to the owner key.
    // When the owner is GC'd the entry is removed automatically.
    private static final WeakHashMap<Object, List<Object>> callbackRoots = new WeakHashMap<>();

    private static synchronized void attachCallbackRoot(Object owner, Object callback) {
        if (owner == null || callback == null) return;
        callbackRoots.computeIfAbsent(owner, k -> new ArrayList<>()).add(callback);
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

        // Pin adapter BEFORE releasing ownership so there is no GC window between
        // swigReleaseOwnership() (which makes the director's Java ref weak) and the
        // native call that may invoke Java callbacks during construction.
        attachCallbackRoot(problem, adapter);
        adapter.swigReleaseOwnership();

        long problemPtr = pagmo4j.pagmonet_problem_from_callback(cbPtr);

        // Surface any deferred exception from get_bounds() during construction.
        Throwable deferred = adapter.consumeDeferredException();
        if (deferred != null) {
            if (problemPtr != 0) pagmo4j.pagmonet_problem_delete(problemPtr);
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

    public static long createAlgorithmPointer(IAlgorithm algorithm) {
        if (algorithm == null) throw new NullPointerException("algorithm");

        AlgorithmCallbackAdapter adapter = new AlgorithmCallbackAdapter(algorithm);
        long cbPtr = AlgorithmCallback.getCPtr(adapter);

        // Pin adapter in callbackRoots BEFORE releasing Java ownership and before
        // the native call.  swigReleaseOwnership() makes the director's Java reference
        // weak; without the root pinned first, a GC window between that call and the
        // native return could collect adapter, causing a crash inside the director
        // callback (premature finalization).
        attachCallbackRoot(algorithm, adapter);
        adapter.swigReleaseOwnership();

        if (cbPtr == 0) {
            throw new RuntimeException(
                "AlgorithmCallback native pointer is null — SWIG director construction failed");
        }

        long algorithmPtr = pagmo4j.pagmonet_algorithm_from_callback_java(cbPtr);

        if (algorithmPtr == 0) {
            String nativeErr = pagmo4j.pagmonet_get_last_error();
            throw new RuntimeException(
                "Failed to build native pagmo::algorithm from IAlgorithm callback." +
                (nativeErr != null && !nativeErr.isEmpty() ? " Native error: " + nativeErr : ""));
        }

        return algorithmPtr;
    }
}
