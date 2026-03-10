namespace pagmo
{
    /// <summary>
    /// SWIG director adapter that forwards native callbacks to a managed IProblem.
    /// </summary>
    internal sealed class ProblemCallbackAdapter : problem_callback
    {
        private readonly IProblem _problem;

        public ProblemCallbackAdapter(IProblem problem)
        {
            _problem = problem;
        }

        public override DoubleVector fitness(DoubleVector x) => _problem.fitness(x);
        public override PairOfDoubleVectors get_bounds() => _problem.get_bounds();
        public override string get_name() => _problem.get_name();
        public override uint get_nobj() => _problem.get_nobj();
        public override uint get_nec() => _problem.get_nec();
        public override uint get_nic() => _problem.get_nic();
        public override uint get_nix() => _problem.get_nix();
        public override thread_safety get_thread_safety() => _problem.get_thread_safety();
    }
}
