using System;

namespace pagmo;

/// <summary>
/// Native-backed wrappers over pagmo::utils::gradients_and_hessians helpers.
/// </summary>
public static class GradientsAndHessians
{
    private static T RequireNotNull<T>(T value, string parameterName) where T : class
    {
        return value ?? throw new ArgumentNullException(parameterName);
    }

    public static SparsityPattern EstimateSparsity(problem prob, DoubleVector x, double dx = 1e-8)
    {
        var ptr = NativeInterop.estimate_sparsity_problem(
            problem.getCPtr(RequireNotNull(prob, nameof(prob))).Handle,
            DoubleVector.getCPtr(RequireNotNull(x, nameof(x))).Handle,
            dx);
        if (ptr == IntPtr.Zero)
        {
            throw new InvalidOperationException(
                NativeInterop.TakeLastErrorOrDefault("Native estimate_sparsity() failed."));
        }

        return new SparsityPattern(ptr, true);
    }

    public static SparsityIndex[] EstimateSparsityEntries(problem prob, DoubleVector x, double dx = 1e-8)
    {
        using var sparsity = EstimateSparsity(prob, x, dx);
        return SparsityProjection.ToEntries(sparsity);
    }

    public static SparsityPattern EstimateSparsity(IProblem prob, DoubleVector x, double dx = 1e-8)
    {
        using var problemHandle = NativeInterop.CreateProblemHandle(RequireNotNull(prob, nameof(prob)), out var callbackAdapter);
        var ptr = NativeInterop.estimate_sparsity_problem(problemHandle.DangerousGetHandle(), DoubleVector.getCPtr(RequireNotNull(x, nameof(x))).Handle, dx);
        NativeInterop.ThrowIfSwigPendingException();
        NativeInterop.ThrowIfDeferredCallbackException(callbackAdapter, "native sparsity estimation");
        if (ptr == IntPtr.Zero)
        {
            throw new InvalidOperationException(
                NativeInterop.TakeLastErrorOrDefault("Native estimate_sparsity() failed."));
        }

        return new SparsityPattern(ptr, true);
    }

    public static SparsityIndex[] EstimateSparsityEntries(IProblem prob, DoubleVector x, double dx = 1e-8)
    {
        using var sparsity = EstimateSparsity(prob, x, dx);
        return SparsityProjection.ToEntries(sparsity);
    }

    public static DoubleVector EstimateGradient(problem prob, DoubleVector x, double dx = 1e-8)
    {
        var ptr = NativeInterop.estimate_gradient_problem(
            problem.getCPtr(RequireNotNull(prob, nameof(prob))).Handle,
            DoubleVector.getCPtr(RequireNotNull(x, nameof(x))).Handle,
            dx);
        if (ptr == IntPtr.Zero)
        {
            throw new InvalidOperationException(
                NativeInterop.TakeLastErrorOrDefault("Native estimate_gradient() failed."));
        }

        return new DoubleVector(ptr, true);
    }

    public static DoubleVector EstimateGradient(IProblem prob, DoubleVector x, double dx = 1e-8)
    {
        using var problemHandle = NativeInterop.CreateProblemHandle(RequireNotNull(prob, nameof(prob)), out var callbackAdapter);
        var ptr = NativeInterop.estimate_gradient_problem(problemHandle.DangerousGetHandle(), DoubleVector.getCPtr(RequireNotNull(x, nameof(x))).Handle, dx);
        return NativeInterop.GetVectorOrThrow(ptr, "Native estimate_gradient() failed.", callbackAdapter);
    }

    public static DoubleVector EstimateGradientHighOrder(problem prob, DoubleVector x, double dx = 1e-2)
    {
        var ptr = NativeInterop.estimate_gradient_h_problem(
            problem.getCPtr(RequireNotNull(prob, nameof(prob))).Handle,
            DoubleVector.getCPtr(RequireNotNull(x, nameof(x))).Handle,
            dx);
        if (ptr == IntPtr.Zero)
        {
            throw new InvalidOperationException(
                NativeInterop.TakeLastErrorOrDefault("Native estimate_gradient_h() failed."));
        }

        return new DoubleVector(ptr, true);
    }

    public static DoubleVector EstimateGradientHighOrder(IProblem prob, DoubleVector x, double dx = 1e-2)
    {
        using var problemHandle = NativeInterop.CreateProblemHandle(RequireNotNull(prob, nameof(prob)), out var callbackAdapter);
        var ptr = NativeInterop.estimate_gradient_h_problem(problemHandle.DangerousGetHandle(), DoubleVector.getCPtr(RequireNotNull(x, nameof(x))).Handle, dx);
        return NativeInterop.GetVectorOrThrow(ptr, "Native estimate_gradient_h() failed.", callbackAdapter);
    }
}
