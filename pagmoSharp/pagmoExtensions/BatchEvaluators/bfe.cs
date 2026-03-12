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
            if (requiresParallelSafety)
            {
                problem.throwIfNotThreadSafe();
            }

            var problemPtr = NativeInterop.CreateProblemPointer(problem);
            try
            {
                var resultPtr = op(bfePtr, problemPtr, DoubleVector.getCPtr(batchX).Handle);
                if (resultPtr == IntPtr.Zero)
                {
                    throw new InvalidOperationException("Native batch evaluator returned null.");
                }

                return new DoubleVector(resultPtr, true);
            }
            finally
            {
                NativeInterop.problem_delete(problemPtr);
            }
        }
    }

}
