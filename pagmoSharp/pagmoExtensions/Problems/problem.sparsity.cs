namespace pagmo
{
    /// <summary>
    /// Represents problem. Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
    /// </summary>
    public partial class problem
    {
        /// <summary>
        /// Returns gradient sparsity in a managed wrapper container.
        /// </summary>
        public SparsityPattern GradientSparsity()
        {
            var raw = gradient_sparsity();
            return new SparsityPattern(SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t.getCPtr(raw).Handle, true);
        }

        /// <summary>
        /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
        /// </summary>
        public VectorOfSparsityPattern HessiansSparsity()
        {
            var raw = hessians_sparsity();
            return new VectorOfSparsityPattern(
                SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t.getCPtr(raw).Handle,
                true);
        }
    }
}

