using System;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Algorithms;

[TestFixture]
public class Test_algorithm_type_erasure_bridges
{
    private static void AssertBridgeProducesTypeErasedAlgorithm<TAlgorithm>(Func<TAlgorithm> create)
        where TAlgorithm : class, IDisposable
    {
        using var uda = create();
        dynamic dynamicUda = uda;
        using algorithm typeErased = dynamicUda.to_algorithm();
        Assert.AreEqual(dynamicUda.get_name(), typeErased.get_name());
    }

    [Test]
    public void IHSBridgeProducesTypeErasedAlgorithm()
    {
        AssertBridgeProducesTypeErasedAlgorithm(() => new ihs(3u));
    }

    [Test]
    public void NSGA2BridgeProducesTypeErasedAlgorithm()
    {
        AssertBridgeProducesTypeErasedAlgorithm(() => new nsga2(3u));
    }

    [Test]
    public void MOEADBridgeProducesTypeErasedAlgorithm()
    {
        AssertBridgeProducesTypeErasedAlgorithm(() => new moead(3u));
    }

    [Test]
    public void MOEADGenBridgeProducesTypeErasedAlgorithm()
    {
        AssertBridgeProducesTypeErasedAlgorithm(() => new moead_gen(3u));
    }

    [Test]
    public void MACOBridgeProducesTypeErasedAlgorithm()
    {
        AssertBridgeProducesTypeErasedAlgorithm(() => new maco(3u));
    }

    [Test]
    public void MBHBridgeProducesTypeErasedAlgorithm()
    {
        AssertBridgeProducesTypeErasedAlgorithm(() => new mbh());
    }

    [Test]
    public void CstrsSelfAdaptiveBridgeProducesTypeErasedAlgorithm()
    {
        AssertBridgeProducesTypeErasedAlgorithm(() => new cstrs_self_adaptive(3u));
    }

    [Test]
    public void BeeColonyBridgeProducesTypeErasedAlgorithm()
    {
        AssertBridgeProducesTypeErasedAlgorithm(() => new bee_colony());
    }

    [Test]
    public void CmaesBridgeProducesTypeErasedAlgorithm()
    {
        AssertBridgeProducesTypeErasedAlgorithm(() => new cmaes(2u));
    }

    [Test]
    public void CompassSearchBridgeProducesTypeErasedAlgorithm()
    {
        AssertBridgeProducesTypeErasedAlgorithm(() => new compass_search(2u));
    }

    [Test]
    public void DeBridgeProducesTypeErasedAlgorithm()
    {
        AssertBridgeProducesTypeErasedAlgorithm(() => new de(2u));
    }

    [Test]
    public void De1220BridgeProducesTypeErasedAlgorithm()
    {
        AssertBridgeProducesTypeErasedAlgorithm(() => new de1220(2u));
    }

    [Test]
    public void GacoBridgeProducesTypeErasedAlgorithm()
    {
        AssertBridgeProducesTypeErasedAlgorithm(() => new gaco(2u));
    }

    [Test]
    public void GwoBridgeProducesTypeErasedAlgorithm()
    {
        AssertBridgeProducesTypeErasedAlgorithm(() => new gwo(2u));
    }

    [Test]
    public void NspsoBridgeProducesTypeErasedAlgorithm()
    {
        AssertBridgeProducesTypeErasedAlgorithm(() => new nspso(2u));
    }

    [Test]
    public void NullAlgorithmBridgeProducesTypeErasedAlgorithm()
    {
        AssertBridgeProducesTypeErasedAlgorithm(() => new null_algorithm());
    }

    [Test]
    public void PsoBridgeProducesTypeErasedAlgorithm()
    {
        AssertBridgeProducesTypeErasedAlgorithm(() => new pso(2u));
    }

    [Test]
    public void PsoGenBridgeProducesTypeErasedAlgorithm()
    {
        AssertBridgeProducesTypeErasedAlgorithm(() => new pso_gen(2u));
    }

    [Test]
    public void SadeBridgeProducesTypeErasedAlgorithm()
    {
        AssertBridgeProducesTypeErasedAlgorithm(() => new sade(2u));
    }

    [Test]
    public void SeaBridgeProducesTypeErasedAlgorithm()
    {
        AssertBridgeProducesTypeErasedAlgorithm(() => new sea(2u));
    }

    [Test]
    public void SgaBridgeProducesTypeErasedAlgorithm()
    {
        AssertBridgeProducesTypeErasedAlgorithm(() => new sga(2u));
    }

    [Test]
    public void SimulatedAnnealingBridgeProducesTypeErasedAlgorithm()
    {
        AssertBridgeProducesTypeErasedAlgorithm(() => new simulated_annealing());
    }

    [Test]
    public void XnesBridgeProducesTypeErasedAlgorithm()
    {
        AssertBridgeProducesTypeErasedAlgorithm(() => new xnes(2u));
    }
}
