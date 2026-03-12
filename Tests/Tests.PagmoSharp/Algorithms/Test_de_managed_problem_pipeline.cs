using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Algorithms
{
    [TestFixture]
    public class Test_de_managed_problem_pipeline
    {
        private sealed class SeedableBatchProblem : problemBase
        {
            private readonly DoubleVector _lb = new(new[] { -10.0, -10.0 });
            private readonly DoubleVector _ub = new(new[] { 10.0, 10.0 });

            public uint LastSeed { get; private set; }

            public override string get_name() => "SeedableBatchProblem";
            public override string get_extra_info() => "Managed pipeline test UDP";
            public override PairOfDoubleVectors get_bounds() => new(_lb, _ub);
            public override thread_safety get_thread_safety() => thread_safety.constant;

            public override DoubleVector fitness(DoubleVector x)
            {
                var dx = x[0];
                var dy = x[1] - 3.0;
                return new DoubleVector(new[] { dx * dx + dy * dy });
            }

            public override bool has_batch_fitness() => true;

            public override DoubleVector batch_fitness(DoubleVector dvs)
            {
                var nx = 2;
                var outVals = new DoubleVector();
                for (var i = 0; i < dvs.Count; i += nx)
                {
                    var x0 = dvs[i];
                    var x1 = dvs[i + 1];
                    var dy = x1 - 3.0;
                    outVals.Add(x0 * x0 + dy * dy);
                }

                return outVals;
            }

            public override bool has_gradient() => true;

            public override DoubleVector gradient(DoubleVector x)
            {
                return new DoubleVector(new[] { 2.0 * x[0], 2.0 * x[1] - 6.0 });
            }

            public override bool has_set_seed() => true;

            public override void set_seed(uint seed)
            {
                LastSeed = seed;
            }
        }

        [Test]
        public void ManagedProblemExposesProblemSurface()
        {
            using var managed = new SeedableBatchProblem();
            using var prob = new problem(managed);

            Assert.IsTrue(prob.is_valid());
            Assert.AreEqual("SeedableBatchProblem", prob.get_name());
            Assert.AreEqual("Managed pipeline test UDP", prob.get_extra_info());
            Assert.AreEqual(thread_safety.constant, prob.get_thread_safety());

            Assert.AreEqual(1u, prob.get_nobj());
            Assert.AreEqual(2u, prob.get_nx());
            Assert.AreEqual(2u, prob.get_ncx());
            Assert.AreEqual(0u, prob.get_nix());
            Assert.AreEqual(1u, prob.get_nf());
            Assert.AreEqual(0u, prob.get_nc());

            Assert.IsTrue(prob.has_batch_fitness());
            Assert.IsTrue(prob.has_gradient());
            Assert.IsFalse(prob.has_hessians());
            Assert.IsTrue(prob.has_set_seed());

            using var x = new DoubleVector(new[] { 1.0, 3.0 });
            using var f = prob.fitness(x);
            Assert.AreEqual(1.0, f[0], 1e-12);
            Assert.AreEqual(1ul, prob.get_fevals());

            using var g = prob.gradient(x);
            Assert.AreEqual(2.0, g[0], 1e-12);
            Assert.AreEqual(0.0, g[1], 1e-12);
            Assert.AreEqual(1ul, prob.get_gevals());

            using var batch = new DoubleVector(new[] { 1.0, 3.0, 2.0, 3.0 });
            using var batchF = prob.batch_fitness(batch);
            Assert.AreEqual(2, batchF.Count);
            Assert.AreEqual(1.0, batchF[0], 1e-12);
            Assert.AreEqual(4.0, batchF[1], 1e-12);

            prob.set_seed(123u);
            Assert.AreEqual(123u, managed.LastSeed);

            prob.set_c_tol(1e-6);
            using var cTol = prob.get_c_tol();
            Assert.AreEqual(0, cTol.Count);

            using var feasibleX = new DoubleVector(new[] { 0.0, 0.0 });
            using var feasibleF = new DoubleVector(new[] { 0.0 });
            Assert.IsTrue(prob.feasibility_x(feasibleX));
            Assert.IsTrue(prob.feasibility_f(feasibleF));
        }

        [Test]
        public void DeEvolvesPopulationFromManagedProblemWrapper()
        {
            using var managed = new SeedableBatchProblem();
            using var prob = new problem(managed);
            using var algo = new de(80);
            using var pop = new population(prob, 128u, 2u);

            algo.set_seed(2u);
            using var finalPop = algo.evolve(pop);

            Assert.Less(finalPop.champion_f()[0], 0.2);
            Assert.Less(System.Math.Abs(finalPop.champion_x()[0]), 0.5);
            Assert.Less(System.Math.Abs(finalPop.champion_x()[1] - 3.0), 0.5);
        }
    }
}
