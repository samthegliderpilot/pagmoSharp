using NUnit.Framework;
using pagmo;

namespace Tests.Pagmo.NET.Utils
{
    [TestFixture]
    public class Test_hypervolume
    {
        [Test]
        public void ComputeReturnsExpectedValueForSimple2DFront()
        {
            using var points = new VectorOfVectorOfDoubles(new[]
            {
                new DoubleVector(1.0, 3.0),
                new DoubleVector(2.0, 2.0)
            });
            using var hv = new hypervolume(points);
            using var reference = new DoubleVector(4.0, 4.0);

            var value = hv.compute(reference);
            Assert.AreEqual(5.0, value, 1e-12);
        }

        [Test]
        public void ContributionsMatchPointCount()
        {
            using var points = new VectorOfVectorOfDoubles(new[]
            {
                new DoubleVector(1.0, 3.0),
                new DoubleVector(2.0, 2.0),
                new DoubleVector(3.0, 1.0)
            });
            using var hv = new hypervolume(points);
            using var reference = new DoubleVector(4.0, 4.0);

            using var contributions = hv.contributions(reference);
            Assert.AreEqual(3, contributions.Count);

            for (uint i = 0; i < contributions.Count; i++)
            {
                var exclusiveContribution = hv.exclusive(i, reference);
                Assert.That(contributions[(int)i], Is.EqualTo(exclusiveContribution).Within(1e-12));
                Assert.That(contributions[(int)i], Is.GreaterThan(0.0));
            }

            var leastContributorIndex = hv.least_contributor(reference);
            var greatestContributorIndex = hv.greatest_contributor(reference);
            Assert.That(leastContributorIndex, Is.LessThan((ulong)contributions.Count));
            Assert.That(greatestContributorIndex, Is.LessThan((ulong)contributions.Count));
        }

        [Test]
        public void BestSelectorMethodsMatchDefaultBehavior()
        {
            using var points = new VectorOfVectorOfDoubles(new[]
            {
                new DoubleVector(1.0, 3.0),
                new DoubleVector(2.0, 2.0),
                new DoubleVector(3.0, 1.0)
            });
            using var hv = new hypervolume(points);
            using var reference = new DoubleVector(4.0, 4.0);

            var defaultCompute = hv.compute(reference);
            var selectedCompute = hv.compute_via_best_compute(reference);
            Assert.That(selectedCompute, Is.EqualTo(defaultCompute).Within(1e-12));

            var defaultExclusive = hv.exclusive(0, reference);
            var selectedExclusive = hv.exclusive_via_best_exclusive(0, reference);
            Assert.That(selectedExclusive, Is.EqualTo(defaultExclusive).Within(1e-12));

            using var defaultContributions = hv.contributions(reference);
            using var selectedContributions = hv.contributions_via_best_contributions(reference);
            Assert.That(selectedContributions.Count, Is.EqualTo(defaultContributions.Count));
            for (var i = 0; i < defaultContributions.Count; i++)
            {
                Assert.That(selectedContributions[i], Is.EqualTo(defaultContributions[i]).Within(1e-12));
            }
        }
    }
}
