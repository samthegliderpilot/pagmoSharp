namespace pagmo
{
    /// <summary>
    /// Wraps a problem in pure .Net code.  This ends up being useful for
    /// calling existing C++ problems that are wrapped by swig.
    /// </summary>
    public class ProblemWrapper : ManagedProblemBase
    {
        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="wrappedProblem">The problem to wrap.</param>
        public ProblemWrapper(IProblem wrappedProblem)
        {
            WrappedProblem = wrappedProblem;
        }

        private bool _disposed = false;
        
        /// <summary>
        /// Gets the wrapped problem
        /// </summary>
        public IProblem WrappedProblem { get; }

        /// <inheritdoc />
        public override string get_name()
        {
            return WrappedProblem.get_name();
        }

        /// <inheritdoc />
        public override string get_extra_info()
        {
            return WrappedProblem.get_extra_info();
        }

        /// <inheritdoc />
        public override DoubleVector fitness(DoubleVector arg0)
        {
            return WrappedProblem.fitness(arg0);
        }

        /// <inheritdoc />
        public override DoubleVector batch_fitness(DoubleVector arg0)
        {
            return WrappedProblem.batch_fitness(arg0);
        }

        /// <inheritdoc />
        public override PairOfDoubleVectors get_bounds()
        {
            return WrappedProblem.get_bounds();
        }

        /// <inheritdoc />
        public override uint get_nobj()
        {
            return WrappedProblem.get_nobj();
        }

        /// <inheritdoc />
        public override uint get_nec()
        {
            return WrappedProblem.get_nec();
        }

        /// <inheritdoc />
        public override uint get_nic()
        {
            return WrappedProblem.get_nic();
        }

        /// <inheritdoc />
        public override bool has_batch_fitness()
        {
            return WrappedProblem.has_batch_fitness();
        }

        public override bool has_gradient()
        {
            return WrappedProblem.has_gradient();
        }

        public override DoubleVector gradient(DoubleVector arg0)
        {
            return WrappedProblem.gradient(arg0);
        }

        public override bool has_gradient_sparsity()
        {
            return WrappedProblem.has_gradient_sparsity();
        }

        public override SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t gradient_sparsity()
        {
            return WrappedProblem.gradient_sparsity();
        }

        public override bool has_hessians()
        {
            return WrappedProblem.has_hessians();
        }

        public override VectorOfVectorOfDoubles hessians(DoubleVector arg0)
        {
            return WrappedProblem.hessians(arg0);
        }

        public override bool has_hessians_sparsity()
        {
            return WrappedProblem.has_hessians_sparsity();
        }

        public override SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t hessians_sparsity()
        {
            return WrappedProblem.hessians_sparsity();
        }

        public override void set_seed(uint seed)
        {
            WrappedProblem.set_seed(seed);
        }

        public override bool has_set_seed()
        {
            return WrappedProblem.has_set_seed();
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            WrappedProblem.Dispose();
            base.Dispose();
        }

        /// <inheritdoc />
        public override uint get_nix()
        {
            return WrappedProblem.get_nix();
        }

        /// <inheritdoc />
        public override thread_safety get_thread_safety()
        {
            return WrappedProblem.get_thread_safety();
        }

        //public static implicit operator problem(ProblemWrapper thisProblem) => new problem(thisProblem);

    }
}
