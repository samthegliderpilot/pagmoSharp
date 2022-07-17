namespace pagmo
{
    /// <summary>
    /// Wraps a problem in pure .Net code.  This ends up being useful for
    /// calling existing C++ problems that are wrapped by swig.
    /// </summary>
    public class ProblemWrapper : problemBase
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
        public override DoubleVector fitness(DoubleVector arg0)
        {
            return WrappedProblem.fitness(arg0);
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

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
                base.Dispose(disposing);
                WrappedProblem.Dispose();
            }
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