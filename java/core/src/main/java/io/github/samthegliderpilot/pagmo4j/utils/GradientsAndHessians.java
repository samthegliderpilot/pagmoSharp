package io.github.samthegliderpilot.pagmo4j.utils;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.problems.*;

/**
 * Native-backed wrappers over pagmo::utils::gradients_and_hessians helpers.
 * Mirrors {@code GradientsAndHessians.cs}.
 */
public final class GradientsAndHessians {

    private GradientsAndHessians() {}

    /**
     * Estimates gradient sparsity for a type-erased problem by finite differencing.
     *
     * @param prob the type-erased pagmo problem to analyse
     * @param x    decision vector at which sparsity is estimated
     * @param dx   finite-difference step size
     * @return sparsity pattern — a list of (row, column) index pairs
     */
    public static SparsityPattern estimateSparsity(problem prob, DoubleVector x, double dx) {
        if (prob == null) throw new NullPointerException("prob");
        if (x    == null) throw new NullPointerException("x");
        long ptr = pagmo4j.pagmonet_estimate_sparsity_problem(
            NativeInterop.getProblemPtr(prob), NativeInterop.getDoubleVectorPtr(x), dx);
        if (ptr == 0) throw new RuntimeException("Native estimate_sparsity() failed.");
        return NativeInterop.wrapSparsityPatternPtr(ptr);
    }

    /** Estimates gradient sparsity with default step dx = 1e-8. */
    public static SparsityPattern estimateSparsity(problem prob, DoubleVector x) {
        return estimateSparsity(prob, x, 1e-8);
    }

    /**
     * Estimates gradient sparsity for a managed problem.
     */
    public static SparsityPattern estimateSparsity(IProblem prob, DoubleVector x, double dx) {
        if (prob == null) throw new NullPointerException("prob");
        if (x    == null) throw new NullPointerException("x");
        long problemPtr = NativeInterop.createProblemPointer(prob);
        try {
            long ptr = pagmo4j.pagmonet_estimate_sparsity_problem(problemPtr, NativeInterop.getDoubleVectorPtr(x), dx);
            if (ptr == 0) throw new RuntimeException("Native estimate_sparsity() failed.");
            return NativeInterop.wrapSparsityPatternPtr(ptr);
        } finally {
            pagmo4j.pagmonet_problem_delete(problemPtr);
        }
    }

    /** Estimates gradient sparsity for a managed problem with default dx = 1e-8. */
    public static SparsityPattern estimateSparsity(IProblem prob, DoubleVector x) {
        return estimateSparsity(prob, x, 1e-8);
    }

    /**
     * Estimates first-order gradient for a type-erased problem by forward differences.
     */
    public static DoubleVector estimateGradient(problem prob, DoubleVector x, double dx) {
        if (prob == null) throw new NullPointerException("prob");
        if (x    == null) throw new NullPointerException("x");
        long ptr = pagmo4j.pagmonet_estimate_gradient_problem(
            NativeInterop.getProblemPtr(prob), NativeInterop.getDoubleVectorPtr(x), dx);
        if (ptr == 0) throw new RuntimeException("Native estimate_gradient() failed.");
        return NativeInterop.wrapDoubleVectorPtr(ptr);
    }

    /** Estimates gradient with default dx = 1e-8. */
    public static DoubleVector estimateGradient(problem prob, DoubleVector x) {
        return estimateGradient(prob, x, 1e-8);
    }

    /**
     * Estimates first-order gradient for a managed problem.
     */
    public static DoubleVector estimateGradient(IProblem prob, DoubleVector x, double dx) {
        if (prob == null) throw new NullPointerException("prob");
        if (x    == null) throw new NullPointerException("x");
        long problemPtr = NativeInterop.createProblemPointer(prob);
        try {
            long ptr = pagmo4j.pagmonet_estimate_gradient_problem(problemPtr, NativeInterop.getDoubleVectorPtr(x), dx);
            if (ptr == 0) throw new RuntimeException("Native estimate_gradient() failed.");
            return NativeInterop.wrapDoubleVectorPtr(ptr);
        } finally {
            pagmo4j.pagmonet_problem_delete(problemPtr);
        }
    }

    /** Estimates gradient for a managed problem with default dx = 1e-8. */
    public static DoubleVector estimateGradient(IProblem prob, DoubleVector x) {
        return estimateGradient(prob, x, 1e-8);
    }

    /**
     * Estimates gradient by higher-order finite differencing for a type-erased problem.
     */
    public static DoubleVector estimateGradientHighOrder(problem prob, DoubleVector x, double dx) {
        if (prob == null) throw new NullPointerException("prob");
        if (x    == null) throw new NullPointerException("x");
        long ptr = pagmo4j.pagmonet_estimate_gradient_h_problem(
            NativeInterop.getProblemPtr(prob), NativeInterop.getDoubleVectorPtr(x), dx);
        if (ptr == 0) throw new RuntimeException("Native estimate_gradient_h() failed.");
        return NativeInterop.wrapDoubleVectorPtr(ptr);
    }

    /** Higher-order gradient with default dx = 1e-2. */
    public static DoubleVector estimateGradientHighOrder(problem prob, DoubleVector x) {
        return estimateGradientHighOrder(prob, x, 1e-2);
    }

    /**
     * Estimates gradient by higher-order finite differencing for a managed problem.
     *
     * @param prob the managed problem to differentiate
     * @param x    decision vector at which the gradient is estimated
     * @param dx   finite-difference step size (typical: 1e-2)
     * @return gradient vector
     */
    public static DoubleVector estimateGradientHighOrder(IProblem prob, DoubleVector x, double dx) {
        if (prob == null) throw new NullPointerException("prob");
        if (x    == null) throw new NullPointerException("x");
        long problemPtr = NativeInterop.createProblemPointer(prob);
        try {
            long ptr = pagmo4j.pagmonet_estimate_gradient_h_problem(problemPtr, NativeInterop.getDoubleVectorPtr(x), dx);
            if (ptr == 0) throw new RuntimeException("Native estimate_gradient_h() failed.");
            return NativeInterop.wrapDoubleVectorPtr(ptr);
        } finally {
            pagmo4j.pagmonet_problem_delete(problemPtr);
        }
    }

    /** Higher-order gradient for a managed problem with default dx = 1e-2. */
    public static DoubleVector estimateGradientHighOrder(IProblem prob, DoubleVector x) {
        return estimateGradientHighOrder(prob, x, 1e-2);
    }
}
