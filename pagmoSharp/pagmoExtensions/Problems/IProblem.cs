using System;

namespace pagmo
{
    public interface IProblem : IDisposable
    {
        string get_name() => "C# problem";
        string get_extra_info() => string.Empty;
        DoubleVector fitness(DoubleVector x);
        DoubleVector batch_fitness(DoubleVector dvs) => throw new NotSupportedException("batch_fitness() is not implemented.");
        PairOfDoubleVectors get_bounds();
        uint get_nobj() => 1;
        uint get_nec() => 0;
        uint get_nic() => 0;
        uint get_nix() => 0;
        bool has_batch_fitness() => false;
        bool has_gradient() => false;
        DoubleVector gradient(DoubleVector x) => throw new NotSupportedException("gradient() is not implemented.");
        bool has_gradient_sparsity() => false;
        SparsityPattern gradient_sparsity() => throw new NotSupportedException("gradient_sparsity() is not implemented.");
        bool has_hessians() => false;
        VectorOfVectorOfDoubles hessians(DoubleVector x) => throw new NotSupportedException("hessians() is not implemented.");
        bool has_hessians_sparsity() => false;
        VectorOfSparsityPattern hessians_sparsity() => throw new NotSupportedException("hessians_sparsity() is not implemented.");
        void set_seed(uint seed) => throw new NotSupportedException("set_seed() is not implemented.");
        bool has_set_seed() => false;
        thread_safety get_thread_safety() => thread_safety.none;
    }
}
