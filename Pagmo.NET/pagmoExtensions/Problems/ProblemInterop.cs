using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace pagmo
{
    internal static class ProblemInterop
    {
        private static readonly ConcurrentDictionary<Type, Func<IProblem, problem>> ToProblemFactories = new();

        internal static IntPtr CreateProblemPointer(IProblem source)
        {
            return CreateProblemPointer(source, out _);
        }

        internal static ProblemHandle CreateProblemHandle(IProblem source, out ProblemCallbackAdapter callbackAdapter)
        {
            var pointer = CreateProblemPointer(source, out callbackAdapter);
            return new ProblemHandle(pointer);
        }

        internal static IntPtr CreateProblemPointer(IProblem source, out ProblemCallbackAdapter callbackAdapter)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            // Managed custom problems must cross the callback bridge.
            if (source is ManagedProblemBase)
            {
                return NativeInterop.CreateProblemPointer(source, out callbackAdapter);
            }

            callbackAdapter = null;

            // Type-erased problem already present: copy directly on native side.
            if (source is problem typeErased)
            {
                var pointer = pagmoPINVOKE.new_problem__SWIG_1(problem.getCPtr(typeErased));
                NativeInterop.ThrowIfSwigPendingException();
                return pointer;
            }

            // Wrapped native UDPs expose to_problem(): prefer native conversion over callback wrapping.
            if (TryCreateViaToProblem(source, out var converted))
            {
                return converted;
            }

            // Unknown IProblem implementations default to callback bridge.
            return NativeInterop.CreateProblemPointer(source, out callbackAdapter);
        }

        private static bool TryCreateViaToProblem(IProblem source, out IntPtr pointer)
        {
            pointer = IntPtr.Zero;
            var factory = ToProblemFactories.GetOrAdd(source.GetType(), BuildToProblemFactory);
            if (factory == null)
            {
                return false;
            }

            var converted = factory(source);
            if (converted == null)
            {
                throw new InvalidOperationException(
                    $"Type '{source.GetType().FullName}' exposed to_problem() but returned null.");
            }

            pointer = problem.swigRelease(converted).Handle;
            NativeInterop.ThrowIfSwigPendingException();
            return true;
        }

        private static Func<IProblem, problem> BuildToProblemFactory(Type sourceType)
        {
            var method = sourceType.GetMethod("to_problem", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
            if (method == null || !typeof(problem).IsAssignableFrom(method.ReturnType))
            {
                return null;
            }

            var sourceParameter = Expression.Parameter(typeof(IProblem), "source");
            var typedSource = Expression.Convert(sourceParameter, sourceType);
            var call = Expression.Call(typedSource, method);
            var castResult = Expression.Convert(call, typeof(problem));
            var lambda = Expression.Lambda<Func<IProblem, problem>>(castResult, sourceParameter);
            return lambda.Compile();
        }
    }
}
