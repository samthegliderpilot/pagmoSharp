using System;

namespace pagmo
{
    /// <summary>
    /// Base class for managed C# problems.
    /// </summary>
    public abstract class problemBase : IProblem
    {
        public abstract DoubleVector fitness(DoubleVector x);
        public abstract PairOfDoubleVectors get_bounds();

        public virtual string get_name() => GetType().Name;
        public virtual uint get_nobj() => 1;
        public virtual uint get_nec() => 0;
        public virtual uint get_nic() => 0;
        public virtual bool has_batch_fitness() => false;
        public virtual uint get_nix() => 0;
        public virtual thread_safety get_thread_safety() => thread_safety.none;

        public virtual bool has_gradient() => false;

        public virtual DoubleVector gradient(DoubleVector x)
        {
            throw new NotSupportedException($"{get_name()} does not provide gradient().");
        }

        public virtual void Dispose()
        {
        }
    }
}
