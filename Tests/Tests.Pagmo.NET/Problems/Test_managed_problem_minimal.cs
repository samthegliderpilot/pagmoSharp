using System;
using NUnit.Framework;
using pagmo;

namespace Tests.Pagmo.NET.Problems
{
    [TestFixture]
    public class Test_ManagedProblem_minimal
    {
        // Minimal IProblem implementation used to assert default optional-member behavior
        // (metadata and NotSupportedException defaults) for managed problem callbacks.
        private sealed class MinimalProblem : IProblem
        {
            private readonly DoubleVector _lb = new(new[] { -5.0, -5.0 });
            private readonly DoubleVector _ub = new(new[] { 5.0, 5.0 });

            public DoubleVector fitness(DoubleVector x)
            {
                return new DoubleVector(new[] { x[0] * x[0] + x[1] * x[1] });
            }

            public PairOfDoubleVectors get_bounds()
            {
                return new PairOfDoubleVectors(_lb, _ub);
            }

            public void Dispose()
            {
            }
        }

        [Test]
        public void MinimalManagedProblemUsesDefaultOptionalMembers()
        {
            using var managed = new MinimalProblem();
            using var prob = new problem(managed);
            using var x = new DoubleVector(new[] { 3.0, 4.0 });
            using var f = prob.fitness(x);

            Assert.AreEqual("C# problem", prob.get_name());
            Assert.AreEqual(string.Empty, prob.get_extra_info());
            Assert.AreEqual(1u, prob.get_nobj());
            Assert.AreEqual(0u, prob.get_nec());
            Assert.AreEqual(0u, prob.get_nic());
            Assert.AreEqual(0u, prob.get_nix());
            Assert.IsFalse(prob.has_batch_fitness());
            Assert.IsFalse(prob.has_set_seed());
            Assert.AreEqual(ThreadSafety.None, prob.get_thread_safety());
            Assert.AreEqual(25.0, f[0], 1e-12);

            var problemInterface = (IProblem)managed;
            Assert.Throws<NotSupportedException>(() => problemInterface.batch_fitness(x));
            Assert.Throws<NotSupportedException>(() => problemInterface.gradient(x));
            Assert.Throws<NotSupportedException>(() => problemInterface.gradient_sparsity());
            Assert.Throws<NotSupportedException>(() => problemInterface.hessians(x));
            Assert.Throws<NotSupportedException>(() => problemInterface.hessians_sparsity());
            Assert.Throws<NotSupportedException>(() => problemInterface.set_seed(2u));
        }
    }
}
