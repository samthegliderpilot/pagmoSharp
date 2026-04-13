namespace pagmo
{
    public partial class problem
    {
        public problem(IProblem source)
            : this(ProblemInterop.CreateProblemPointer(source), true)
        {
        }
    }
}
