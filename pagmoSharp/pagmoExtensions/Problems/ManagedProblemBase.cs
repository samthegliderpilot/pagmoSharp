using System;

namespace pagmo
{
    /// <summary>
    /// Base class for managed C# problems.
    /// </summary>
    public abstract partial class ManagedProblemBase : IProblem
    {
        public abstract DoubleVector fitness(DoubleVector x);
        public abstract PairOfDoubleVectors get_bounds();

        public virtual string get_name() => GetType().Name;
        public virtual string get_extra_info() => string.Empty;
        public virtual uint get_nobj() => 1;
        public virtual uint get_nec() => 0;
        public virtual uint get_nic() => 0;
        public virtual uint get_nix() => 0;
        public virtual bool has_batch_fitness() => false;
        public virtual thread_safety get_thread_safety() => thread_safety.none;

        public virtual DoubleVector batch_fitness(DoubleVector dvs)
        {
            throw new NotSupportedException($"{get_name()} does not provide batch_fitness().");
        }

        public virtual bool has_gradient() => false;

        public virtual DoubleVector gradient(DoubleVector x)
        {
            throw new NotSupportedException($"{get_name()} does not provide gradient().");
        }

        public virtual bool has_gradient_sparsity() => false;

        public virtual SparsityPattern gradient_sparsity()
        {
            throw new NotSupportedException($"{get_name()} does not provide gradient_sparsity().");
        }

        public virtual bool has_hessians() => false;

        public virtual VectorOfVectorOfDoubles hessians(DoubleVector x)
        {
            throw new NotSupportedException($"{get_name()} does not provide hessians().");
        }

        public virtual bool has_hessians_sparsity() => false;

        public virtual VectorOfSparsityPattern hessians_sparsity()
        {
            throw new NotSupportedException($"{get_name()} does not provide hessians_sparsity().");
        }

        public virtual void set_seed(uint seed)
        {
            throw new NotSupportedException($"{get_name()} does not provide set_seed().");
        }

        public virtual bool has_set_seed() => false;

        public virtual void Dispose()
        {
        }

        /// <summary>
        /// Convenience helper for compact vector creation in managed UDPs.
        /// </summary>
        protected static DoubleVector Vec(params double[] values) => new(values);

        /// <summary>
        /// Convenience helper for compact bound creation in managed UDPs.
        /// </summary>
        protected static PairOfDoubleVectors Bounds(DoubleVector lower, DoubleVector upper) => new(lower, upper);

        /// <summary>
        /// Convenience helper for compact bound creation in managed UDPs.
        /// </summary>
        protected static PairOfDoubleVectors Bounds(double[] lower, double[] upper) => new(new DoubleVector(lower), new DoubleVector(upper));

        /// <summary>
        /// Convenience helper for compact gradient sparsity creation in managed UDPs.
        /// </summary>
        protected static SparsityPattern Sparsity(params (uint Row, uint Col)[] entries)
        {
            var result = new SparsityPattern();
            foreach (var (row, col) in entries)
            {
                result.Add(new SizeTPair(row, col));
            }

            return result;
        }

        /// <summary>
        /// Convenience helper for compact hessian sparsity creation in managed UDPs.
        /// </summary>
        protected static VectorOfSparsityPattern HessiansSparsity(params SparsityPattern[] patterns) => new(patterns);
    }
}
