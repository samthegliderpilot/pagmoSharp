using System;

namespace pagmo
{
    public interface IProblem : IDisposable
    {
        string get_name();
        DoubleVector fitness(DoubleVector x);
        PairOfDoubleVectors get_bounds();
        uint get_nobj();
        uint get_nec();
        uint get_nic();
        bool has_batch_fitness();
        uint get_nix();
        thread_safety get_thread_safety();
    }
}
