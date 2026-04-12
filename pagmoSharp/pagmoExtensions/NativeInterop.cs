using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace pagmo
{
    internal static class NativeInterop
    {
        private const string NativeLib = "pagmoWrapper";

        [DllImport(NativeLib, EntryPoint = "pagmosharp_problem_from_callback", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr problem_from_callback(IntPtr callbackPtr);

        [DllImport(NativeLib, EntryPoint = "pagmosharp_algorithm_from_callback", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr algorithm_from_callback(IntPtr callbackPtr);

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

        [DllImport(NativeLib, EntryPoint = "pagmosharp_estimate_gradient_problem", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr estimate_gradient_problem(IntPtr problemPtr, IntPtr xPtr, double dx);

        [DllImport(NativeLib, EntryPoint = "pagmosharp_estimate_gradient_h_problem", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr estimate_gradient_h_problem(IntPtr problemPtr, IntPtr xPtr, double dx);

        [DllImport(NativeLib, EntryPoint = "pagmosharp_estimate_sparsity_problem", CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr estimate_sparsity_problem(IntPtr problemPtr, IntPtr xPtr, double dx);

        [DllImport(NativeLib, EntryPoint = "pagmosharp_get_last_error", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr get_last_error();

        [DllImport(NativeLib, EntryPoint = "pagmosharp_clear_last_error", CallingConvention = CallingConvention.Cdecl)]
        private static extern void clear_last_error();

        private sealed class CallbackRootBucket
        {
            private readonly List<GCHandle> _handles = new();
            private readonly object _gate = new();

            public void Add(object callback)
            {
                lock (_gate)
                {
                    _handles.Add(GCHandle.Alloc(callback));
                }
            }

            ~CallbackRootBucket()
            {
                lock (_gate)
                {
                    for (var i = 0; i < _handles.Count; i++)
                    {
                        if (_handles[i].IsAllocated)
                        {
                            _handles[i].Free();
                        }
                    }

                    _handles.Clear();
                }
            }
        }

        private static readonly ConditionalWeakTable<object, CallbackRootBucket> ProblemCallbackRoots = new();

        private static void AttachCallbackRoot(object owner, object callback)
        {
            if (owner == null || callback == null)
            {
                return;
            }

            var bucket = ProblemCallbackRoots.GetOrCreateValue(owner);
            bucket.Add(callback);
        }

        internal static void AttachProblemCallbackRoot(object owner, problem_callback callback)
        {
            AttachCallbackRoot(owner, callback);
        }

        internal static void AttachAlgorithmCallbackRoot(object owner, algorithm_callback callback)
        {
            AttachCallbackRoot(owner, callback);
        }

        internal static string TakeLastErrorOrDefault(string fallbackMessage)
        {
            var errorPtr = get_last_error();
            var message = errorPtr == IntPtr.Zero ? null : Marshal.PtrToStringAnsi(errorPtr);
            clear_last_error();
            return string.IsNullOrWhiteSpace(message) ? fallbackMessage : message!;
        }

        internal static void ThrowIfSwigPendingException()
        {
            if (pagmoPINVOKE.SWIGPendingException.Pending)
            {
                throw pagmoPINVOKE.SWIGPendingException.Retrieve();
            }
        }

        internal static void ThrowIfDeferredCallbackException(ProblemCallbackAdapter callbackAdapter, string operationContext)
        {
            var deferredManagedException = callbackAdapter?.ConsumeDeferredManagedException();
            if (deferredManagedException != null)
            {
                throw new InvalidOperationException(
                    $"Managed problem callback threw during {operationContext}: {deferredManagedException.Message}",
                    deferredManagedException);
            }
        }

        internal static DoubleVector GetVectorOrThrow(IntPtr resultPtr, string fallbackMessage, ProblemCallbackAdapter callbackAdapter = null)
        {
            if (pagmoPINVOKE.SWIGPendingException.Pending)
            {
                // Native code may still have produced an output object before the
                // callback exception was surfaced. Dispose to avoid leaking it.
                if (resultPtr != IntPtr.Zero)
                {
                    using var _ = new DoubleVector(resultPtr, true);
                }

                throw pagmoPINVOKE.SWIGPendingException.Retrieve();
            }

            try
            {
                ThrowIfDeferredCallbackException(callbackAdapter, "native evaluation");
            }
            catch
            {
                if (resultPtr != IntPtr.Zero)
                {
                    using var _ = new DoubleVector(resultPtr, true);
                }
                throw;
            }

            if (resultPtr == IntPtr.Zero)
            {
                throw new InvalidOperationException(TakeLastErrorOrDefault(fallbackMessage));
            }

            return new DoubleVector(resultPtr, true);
        }

        internal static IntPtr CreateProblemPointer(IProblem problem)
        {
            return CreateProblemPointer(problem, out _);
        }

        internal static ProblemHandle CreateProblemHandle(IProblem problem, out ProblemCallbackAdapter callbackAdapter)
        {
            var pointer = CreateProblemPointer(problem, out callbackAdapter);
            return new ProblemHandle(pointer);
        }

        internal static IntPtr CreateProblemPointer(
            IProblem problem,
            out ProblemCallbackAdapter callbackAdapter)
        {
            if (problem == null)
            {
                throw new ArgumentNullException(nameof(problem));
            }

            callbackAdapter = new ProblemCallbackAdapter(problem);
            var callback = callbackAdapter;
            var callbackPtr = problem_callback.swigRelease(callback).Handle;
            var problemPtr = problem_from_callback(callbackPtr);
            if (problemPtr == IntPtr.Zero)
            {
                // Prefer SWIG's canonical pending-exception channel when available.
                ThrowIfSwigPendingException();
                throw new InvalidOperationException(
                    TakeLastErrorOrDefault("Failed to build native pagmo::problem from IProblem callback."));
            }

            // Keep director delegates alive for the same managed owner lifetime to avoid callback GC crashes.
            AttachProblemCallbackRoot(problem, callback);
            return problemPtr;
        }

        internal static IntPtr CreateAlgorithmPointer(IAlgorithm algorithm)
        {
            if (algorithm == null)
            {
                throw new ArgumentNullException(nameof(algorithm));
            }

            var callbackAdapter = new AlgorithmCallbackAdapter(algorithm);
            var callback = callbackAdapter;
            var callbackPtr = algorithm_callback.swigRelease(callback).Handle;
            var algorithmPtr = algorithm_from_callback(callbackPtr);
            if (algorithmPtr == IntPtr.Zero)
            {
                ThrowIfSwigPendingException();
                throw new InvalidOperationException(
                    TakeLastErrorOrDefault("Failed to build native pagmo::algorithm from IAlgorithm callback."));
            }

            AttachAlgorithmCallbackRoot(algorithm, callback);
            return algorithmPtr;
        }

    }
}
