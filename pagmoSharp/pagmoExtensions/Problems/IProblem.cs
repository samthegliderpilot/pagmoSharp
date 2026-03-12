using System;

namespace pagmo
{
    public interface IProblem : IDisposable
    {
        string get_name();
        string get_extra_info() => string.Empty;
        DoubleVector fitness(DoubleVector x);
        DoubleVector batch_fitness(DoubleVector dvs) => throw new NotSupportedException("batch_fitness() is not implemented.");
        PairOfDoubleVectors get_bounds();
        uint get_nobj();
        uint get_nec();
        uint get_nic();
        uint get_nix();
        bool has_batch_fitness();
        bool has_gradient() => false;
        DoubleVector gradient(DoubleVector x) => throw new NotSupportedException("gradient() is not implemented.");
        bool has_gradient_sparsity() => false;
        SWIGTYPE_p_std__vectorT_std__pairT_size_t_size_t_t_t gradient_sparsity() => throw new NotSupportedException("gradient_sparsity() is not implemented.");
        bool has_hessians() => false;
        VectorOfVectorOfDoubles hessians(DoubleVector x) => throw new NotSupportedException("hessians() is not implemented.");
        bool has_hessians_sparsity() => false;
        SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t hessians_sparsity() => throw new NotSupportedException("hessians_sparsity() is not implemented.");
        void set_seed(uint seed) => throw new NotSupportedException("set_seed() is not implemented.");
        bool has_set_seed() => false;
        thread_safety get_thread_safety();
    }
}
