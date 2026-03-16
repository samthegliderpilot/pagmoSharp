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
        public override DoubleVector batch_fitness(DoubleVector x) => _problem.batch_fitness(x);
        public override PairOfDoubleVectors get_bounds() => _problem.get_bounds();
        public override string get_name() => _problem.get_name();
        public override string get_extra_info() => _problem.get_extra_info();
        public override uint get_nobj() => _problem.get_nobj();
        public override uint get_nec() => _problem.get_nec();
        public override uint get_nic() => _problem.get_nic();
        public override uint get_nix() => _problem.get_nix();
        public override bool has_batch_fitness() => _problem.has_batch_fitness();
        public override bool has_gradient() => _problem.has_gradient();
        public override DoubleVector gradient(DoubleVector x) => _problem.gradient(x);
        public override bool has_gradient_sparsity() => _problem.has_gradient_sparsity();
        public override SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t gradient_sparsity()
        {
            var sparsity = _problem.gradient_sparsity();
            return new SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t(SparsityPattern.swigRelease(sparsity).Handle, true);
        }

        public override bool has_hessians() => _problem.has_hessians();
        public override VectorOfVectorOfDoubles hessians(DoubleVector x) => _problem.hessians(x);
        public override bool has_hessians_sparsity() => _problem.has_hessians_sparsity();
        public override SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t hessians_sparsity()
        {
            var sparsity = _problem.hessians_sparsity();
            return new SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t(VectorOfSparsityPattern.swigRelease(sparsity).Handle, true);
        }

        public override void set_seed(uint seed) => _problem.set_seed(seed);
        public override bool has_set_seed() => _problem.has_set_seed();
        public override thread_safety get_thread_safety() => _problem.get_thread_safety();
    }
}
