using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Algorithms;

[TestFixture]
public class Test_algorithm_type_erasure_bridges
{
    [Test]
    public void IHSBridgeProducesTypeErasedAlgorithm()
    {
        using var uda = new ihs(3u);
        using var typeErased = uda.to_algorithm();
        Assert.AreEqual(uda.get_name(), typeErased.get_name());
    }

    [Test]
    public void NSGA2BridgeProducesTypeErasedAlgorithm()
    {
        using var uda = new nsga2(3u);
        using var typeErased = uda.to_algorithm();
        Assert.AreEqual(uda.get_name(), typeErased.get_name());
    }

    [Test]
    public void MOEADBridgeProducesTypeErasedAlgorithm()
    {
        using var uda = new moead(3u);
        using var typeErased = uda.to_algorithm();
        Assert.AreEqual(uda.get_name(), typeErased.get_name());
    }

    [Test]
    public void MOEADGenBridgeProducesTypeErasedAlgorithm()
    {
        using var uda = new moead_gen(3u);
        using var typeErased = uda.to_algorithm();
        Assert.AreEqual(uda.get_name(), typeErased.get_name());
    }

    [Test]
    public void MACOBridgeProducesTypeErasedAlgorithm()
    {
        using var uda = new maco(3u);
        using var typeErased = uda.to_algorithm();
        Assert.AreEqual(uda.get_name(), typeErased.get_name());
    }

    [Test]
    public void MBHBridgeProducesTypeErasedAlgorithm()
    {
        using var uda = new mbh();
        using var typeErased = uda.to_algorithm();
        Assert.AreEqual(uda.get_name(), typeErased.get_name());
    }

    [Test]
    public void CstrsSelfAdaptiveBridgeProducesTypeErasedAlgorithm()
    {
        using var uda = new cstrs_self_adaptive(3u);
        using var typeErased = uda.to_algorithm();
        Assert.AreEqual(uda.get_name(), typeErased.get_name());
    }
}
