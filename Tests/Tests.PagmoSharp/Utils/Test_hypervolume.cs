using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Utils
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
        }
    }
}
