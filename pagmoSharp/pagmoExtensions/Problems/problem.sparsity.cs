namespace pagmo
{
    public partial class problem
    {
        public SparsityPattern GradientSparsity()
        {
            var raw = gradient_sparsity();
            return new SparsityPattern(SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t.getCPtr(raw).Handle, true);
        }

        public VectorOfSparsityPattern HessiansSparsity()
        {
            var raw = hessians_sparsity();
            return new VectorOfSparsityPattern(
                SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t.getCPtr(raw).Handle,
                true);
        }
    }
}
