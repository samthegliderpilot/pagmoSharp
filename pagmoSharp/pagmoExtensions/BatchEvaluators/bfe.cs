using System;

namespace pagmo
{
    public partial class default_bfe
    {
        public DoubleVector Operator(IProblem problem, DoubleVector batchX)
        {
            return BfeBridge.BatchEvaluate(problem, batchX, NativeInterop.default_bfe_operator, getCPtr(this).Handle, requiresParallelSafety: false);
        }
    }

    public partial class thread_bfe
    {
        public DoubleVector Operator(IProblem problem, DoubleVector batchX)
        {
            return BfeBridge.BatchEvaluate(problem, batchX, NativeInterop.thread_bfe_operator, getCPtr(this).Handle, requiresParallelSafety: true);
        }
    }

    public partial class member_bfe
    {
        public DoubleVector Operator(IProblem problem, DoubleVector batchX)
        {
            return BfeBridge.BatchEvaluate(problem, batchX, NativeInterop.member_bfe_operator, getCPtr(this).Handle, requiresParallelSafety: false);
        }
    }

    internal delegate IntPtr BfeOperator(IntPtr bfePtr, IntPtr problemPtr, IntPtr batchXPtr);

    internal static class BfeBridge
    {
        internal static DoubleVector BatchEvaluate(IProblem problem, DoubleVector batchX, BfeOperator op, IntPtr bfePtr, bool requiresParallelSafety)
        {
            if (problem == null)
            {
                throw new ArgumentNullException(nameof(problem));
            }

            if (batchX == null)
            {
                throw new ArgumentNullException(nameof(batchX));
            }

            if (op == null)
            {
                throw new ArgumentNullException(nameof(op));
            }

            if (requiresParallelSafety)
            {
                problem.ThrowIfNotThreadSafe();
            }

            using var problemHandle = NativeInterop.CreateProblemHandle(problem, out var callbackAdapter);
            var resultPtr = op(bfePtr, problemHandle.DangerousGetHandle(), DoubleVector.getCPtr(batchX).Handle);
            return NativeInterop.GetVectorOrThrow(resultPtr, "Native batch evaluator returned null.", callbackAdapter);
        }
    }

}
