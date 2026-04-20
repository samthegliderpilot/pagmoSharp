using System;

namespace pagmo
{
    /// <summary>
    /// Managed convenience constructors for creating <c>pagmo::population</c> from <see cref="IProblem"/>.
    /// </summary>
    /// <remarks>
    /// Construction normalizes managed problems into native type-erased problems and applies centralized
    /// callback-boundary exception handling for managed callback failures.
    /// </remarks>
    public partial class population
    {
        /// <summary>
            /// Creates a population for the provided managed problem using a random seed.
        /// </summary>
        /// <param name="problem">Managed problem used to initialize the population.</param>
        /// <param name="popSize">Number of individuals in the initial population.</param>
        public population(IProblem problem, ulong popSize)
            : this(problem, popSize, NewRandomSeed())
        {
        }

        /// <summary>
            /// Creates a population for the provided managed problem using an explicit seed.
        /// </summary>
        /// <param name="problem">Managed problem used to initialize the population.</param>
        /// <param name="popSize">Number of individuals in the initial population.</param>
        /// <param name="seed">Random seed used by pagmo population initialization.</param>
        public population(IProblem problem, ulong popSize, uint seed)
            : this(CreateFromManagedProblem(problem, popSize, seed), true)
        {
        }

        private static IntPtr CreateFromManagedProblem(IProblem problem, ulong popSize, uint seed)
        {
            using var problemHandle = ProblemInterop.CreateProblemHandle(problem, out var callbackAdapter);
            var problemPtr = problemHandle.DangerousGetHandle();
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

        private static uint NewRandomSeed()
        {
            using var rng = new random_device();
            return rng.next();
        }
    }
}
