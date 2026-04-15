using System;

namespace pagmo
{
    public partial class problem
    {
        public problem(IProblem managedProblem)
            : this(ProblemInterop.CreateProblemPointer(managedProblem), true)
        {
        }
    }
}
