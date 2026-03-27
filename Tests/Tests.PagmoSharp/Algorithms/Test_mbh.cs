using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Algorithms;

[TestFixture]
public class Test_mbh
{
    [Test]
    public void NameAndBasicPropertiesAreAccessible()
    {
        using var algorithm = new mbh();
        Assert.AreEqual("MBH: Monotonic Basin Hopping - Generalized", algorithm.get_name());

        algorithm.set_seed(7u);
        Assert.AreEqual(7u, algorithm.get_seed());

        algorithm.set_verbosity(2u);
        Assert.AreEqual(2u, algorithm.get_verbosity());
        Assert.IsNotNull(algorithm.get_extra_info());
    }

    [Test]
    public void PerturbationCanBeUpdated()
    {
        using var algorithm = new mbh();
        using var perturb = new DoubleVector(new[] { 0.15, 0.25 });

        algorithm.set_perturb(perturb);
        using var configured = algorithm.get_perturb();

        Assert.AreEqual(2, configured.Count);
        Assert.AreEqual(0.15, configured[0], 1e-12);
        Assert.AreEqual(0.25, configured[1], 1e-12);
    }
}
