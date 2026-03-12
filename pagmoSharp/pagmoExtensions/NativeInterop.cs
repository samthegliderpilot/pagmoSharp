using System;
using System.Runtime.InteropServices;

namespace pagmo
{
    internal static class NativeInterop
    {
        private const string NativeLib = "pagmoWrapper";

        [DllImport(NativeLib, EntryPoint = "pagmosharp_problem_from_callback", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr problem_from_callback(IntPtr callbackPtr);

        [DllImport(NativeLib, EntryPoint = "pagmosharp_problem_delete", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void problem_delete(IntPtr problemPtr);

        [DllImport(NativeLib, EntryPoint = "pagmosharp_population_new", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr population_new(IntPtr problemPtr, UIntPtr popSize, uint seed);

        [DllImport(NativeLib, EntryPoint = "pagmosharp_default_bfe_operator", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr default_bfe_operator(IntPtr bfePtr, IntPtr problemPtr, IntPtr batchXPtr);

        [DllImport(NativeLib, EntryPoint = "pagmosharp_thread_bfe_operator", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr thread_bfe_operator(IntPtr bfePtr, IntPtr problemPtr, IntPtr batchXPtr);

        [DllImport(NativeLib, EntryPoint = "pagmosharp_member_bfe_operator", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr member_bfe_operator(IntPtr bfePtr, IntPtr problemPtr, IntPtr batchXPtr);

        internal static IntPtr CreateProblemPointer(IProblem problem)
        {
            var callback = new ProblemCallbackAdapter(problem);
            var callbackPtr = problem_callback.swigRelease(callback).Handle;
            var problemPtr = problem_from_callback(callbackPtr);
            if (problemPtr == IntPtr.Zero)
            {
                throw new InvalidOperationException("Failed to build native pagmo::problem from IProblem callback.");
            }

            return problemPtr;
        }

    }
}
