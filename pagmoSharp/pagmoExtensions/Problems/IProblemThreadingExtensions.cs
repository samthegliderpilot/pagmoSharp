using System;

namespace pagmo
{
    internal static class IProblemThreadingExtensions
    {
        internal static void ThrowIfNotThreadSafe(this IProblem problem)
        {
            if (problem == null)
            {
                throw new ArgumentNullException(nameof(problem));
            }

            var declaredThreadSafety = problem.get_thread_safety();
            if (declaredThreadSafety == thread_safety.none)
            {
                throw new InvalidOperationException(
                    $"Managed problem '{problem.get_name()}' must declare thread_safety.basic or thread_safety.constant for this threaded path.");
            }
        }
    }
}
