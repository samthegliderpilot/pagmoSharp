using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp.Utils
{
    [TestFixture]
    public class Test_gradients_and_hessians
    {
        [Test]
        public void EstimateGradientFromManagedProblemMatchesExpectedShape()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var x = new DoubleVector(new[] { 1.0, 3.0 });
            using var grad = GradientsAndHessians.EstimateGradient((IProblem)managed, x, 1e-6);

            Assert.AreEqual(2, grad.Count);
            Assert.AreEqual(2.0, grad[0], 1e-3);
            Assert.AreEqual(0.0, grad[1], 1e-3);
        }

        [Test]
        public void EstimateGradientFromProblemWrapperMatchesExpectedShape()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var prob = new problem(managed);
            using var x = new DoubleVector(new[] { 1.0, 3.0 });
            using var grad = GradientsAndHessians.EstimateGradient(prob, x, 1e-6);

            Assert.AreEqual(2, grad.Count);
            Assert.AreEqual(2.0, grad[0], 1e-3);
            Assert.AreEqual(0.0, grad[1], 1e-3);
        }

        [Test]
        public void EstimateSparsityFromManagedProblemReturnsDenseGradientPattern()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var x = new DoubleVector(new[] { 1.0, 3.0 });
            using var sparsity = GradientsAndHessians.EstimateSparsity((IProblem)managed, x, 1e-8);

            Assert.AreEqual(2, sparsity.Count);
            Assert.AreEqual((uint)0, sparsity[0].first);
            Assert.AreEqual((uint)0, sparsity[0].second);
            Assert.AreEqual((uint)0, sparsity[1].first);
            Assert.AreEqual((uint)1, sparsity[1].second);
        }

        [Test]
        public void EstimateSparsityEntriesFromManagedProblemReturnsProjectedIndices()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var x = new DoubleVector(new[] { 1.0, 3.0 });

            var entries = GradientsAndHessians.EstimateSparsityEntries((IProblem)managed, x, 1e-8);

            Assert.AreEqual(2, entries.Length);
            Assert.AreEqual(new SparsityIndex(0u, 0u), entries[0]);
            Assert.AreEqual(new SparsityIndex(0u, 1u), entries[1]);
        }

        [Test]
        public void EstimateSparsityEntriesFromProblemWrapperReturnsProjectedIndices()
        {
            using var managed = new TwoDimensionalSingleObjectiveProblemWrapper();
            using var prob = new problem(managed);
            using var x = new DoubleVector(new[] { 1.0, 3.0 });

            var entries = GradientsAndHessians.EstimateSparsityEntries(prob, x, 1e-8);

            Assert.AreEqual(2, entries.Length);
            Assert.AreEqual(new SparsityIndex(0u, 0u), entries[0]);
            Assert.AreEqual(new SparsityIndex(0u, 1u), entries[1]);
        }
    }
}
