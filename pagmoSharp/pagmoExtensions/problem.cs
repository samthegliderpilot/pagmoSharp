using System;

namespace pagmo
{
    public partial class problem
    {
        public problem(IProblem managedProblem)
            : this(CreateFromManagedProblem(managedProblem), true)
        {
        }

        private static IntPtr CreateFromManagedProblem(IProblem managedProblem)
        {
            if (managedProblem == null)
            {
                throw new ArgumentNullException(nameof(managedProblem));
            }

            return NativeInterop.CreateProblemPointer(managedProblem);
        }
    }
}
