using System;

namespace pagmo
{
    internal static class IProblemThreadingExtensions
    {
        internal static void ThrowIfNotThreadSafe(this IProblem problem)
        {
            if (problem.get_thread_safety() == thread_safety.none)
            {
                throw new InvalidOperationException("Managed problem must declare thread_safety.basic or thread_safety.constant for this threaded path.");
            }
        }
    }
}
