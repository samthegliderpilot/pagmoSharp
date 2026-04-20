using System;

namespace pagmo
{
    /// <summary>
    /// Type-erased pagmo problem wrapper.
    /// </summary>
    /// <remarks>
    /// This constructor is the canonical bridge from managed <see cref="IProblem"/> implementations
    /// into a native <c>pagmo::problem</c>. Wrapped-native problem types are normalized through their
    /// native conversion path when available; custom managed problems use the callback adapter bridge.
    /// </remarks>
    public partial class problem
    {
        /// <summary>
        /// Builds a type-erased native problem from a managed problem instance.
        /// </summary>
        /// <param name="managedProblem">Managed problem to normalize into native type erasure.</param>
        public problem(IProblem managedProblem)
            : this(ProblemInterop.CreateProblemPointer(managedProblem), true)
        {
        }
    }
}

