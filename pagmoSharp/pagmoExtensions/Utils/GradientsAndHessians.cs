using System;

namespace pagmo;

/// <summary>
/// Native-backed wrappers over pagmo::utils::gradients_and_hessians helpers.
/// </summary>
public static class GradientsAndHessians
{
    public static SparsityPattern EstimateSparsity(problem prob, DoubleVector x, double dx = 1e-8)
    {
        var ptr = NativeInterop.estimate_sparsity_problem(problem.getCPtr(prob).Handle, DoubleVector.getCPtr(x).Handle, dx);
        if (ptr == IntPtr.Zero)
        {
            throw new InvalidOperationException(
                NativeInterop.TakeLastErrorOrDefault("Native estimate_sparsity() failed."));
        }

        return new SparsityPattern(ptr, true);
    }

    public static SparsityPattern EstimateSparsity(IProblem prob, DoubleVector x, double dx = 1e-8)
    {
        using var wrapped = new problem(prob);
        return EstimateSparsity(wrapped, x, dx);
    }

    public static DoubleVector EstimateGradient(problem prob, DoubleVector x, double dx = 1e-8)
    {
        var ptr = NativeInterop.estimate_gradient_problem(problem.getCPtr(prob).Handle, DoubleVector.getCPtr(x).Handle, dx);
        if (ptr == IntPtr.Zero)
        {
            throw new InvalidOperationException(
                NativeInterop.TakeLastErrorOrDefault("Native estimate_gradient() failed."));
        }

        return new DoubleVector(ptr, true);
    }

    public static DoubleVector EstimateGradient(IProblem prob, DoubleVector x, double dx = 1e-8)
    {
        using var wrapped = new problem(prob);
        return EstimateGradient(wrapped, x, dx);
    }

    public static DoubleVector EstimateGradientHighOrder(problem prob, DoubleVector x, double dx = 1e-2)
    {
        var ptr = NativeInterop.estimate_gradient_h_problem(problem.getCPtr(prob).Handle, DoubleVector.getCPtr(x).Handle, dx);
        if (ptr == IntPtr.Zero)
        {
            throw new InvalidOperationException(
                NativeInterop.TakeLastErrorOrDefault("Native estimate_gradient_h() failed."));
        }

        return new DoubleVector(ptr, true);
    }

    public static DoubleVector EstimateGradientHighOrder(IProblem prob, DoubleVector x, double dx = 1e-2)
    {
        using var wrapped = new problem(prob);
        return EstimateGradientHighOrder(wrapped, x, dx);
    }
}
