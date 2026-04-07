using System;

namespace pagmo
{
    public partial class population
    {
        public population(IProblem problem, ulong popSize)
            : this(problem, popSize, NewRandomSeed())
        {
        }

        public population(IProblem problem, ulong popSize, uint seed)
            : this(CreateFromManagedProblem(problem, popSize, seed), true)
        {
        }

        private static IntPtr CreateFromManagedProblem(IProblem problem, ulong popSize, uint seed)
        {
            var problemPtr = NativeInterop.CreateProblemPointer(problem, out var callbackAdapter);
            try
            {
                var nativePopulationSize = SizeTInterop.ToNativeUIntPtr(popSize, nameof(popSize));
                var populationPtr = NativeInterop.population_new(problemPtr, nativePopulationSize, seed);
                NativeInterop.ThrowIfSwigPendingException();
                NativeInterop.ThrowIfDeferredCallbackException(callbackAdapter, "native population construction");

                if (populationPtr == IntPtr.Zero)
                {
                    throw new InvalidOperationException(
                        NativeInterop.TakeLastErrorOrDefault("Failed to create native pagmo::population."));
                }

                return populationPtr;
            }
            finally
            {
                NativeInterop.problem_delete(problemPtr);
            }
        }

        private static uint NewRandomSeed()
        {
            using var rng = new random_device();
            return rng.next();
        }
    }
}
