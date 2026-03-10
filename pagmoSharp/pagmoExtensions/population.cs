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
            var problemPtr = NativeInterop.CreateProblemPointer(problem);
            try
            {
                var populationPtr = NativeInterop.population_new(problemPtr, (UIntPtr)popSize, seed);
                if (populationPtr == IntPtr.Zero)
                {
                    throw new InvalidOperationException("Failed to create native pagmo::population.");
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
