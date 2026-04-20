using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Algorithms;

[TestFixture]
public class Test_maco : TestAlgorithmBase
{
    public override IAlgorithm CreateAlgorithm()
    {
        return new maco(6u);
    }

    [Test]
    public override void TestNameIsCorrect()
    {
        using var algorithm = CreateAlgorithm();
        Assert.AreEqual("MHACO: Multi-objective Hypervolume-based Ant Colony Optimization", algorithm.get_name());
    }

    public override bool Constrained => false;
    public override bool Unconstrained => true;
    public override bool SingleObjective => false;
    public override bool MultiObjective => true;
    public override bool IntegerProgramming => false;
    public override bool Stochastic => false;
}

