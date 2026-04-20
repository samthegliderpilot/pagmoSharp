using NUnit.Framework;
using pagmo;

namespace Tests.Pagmo.NET.Algorithms;

[TestFixture]
public class Test_moead : TestAlgorithmBase
{
    public override IAlgorithm CreateAlgorithm()
    {
        return new moead(6u);
    }

    [Test]
    public override void TestNameIsCorrect()
    {
        using var algorithm = CreateAlgorithm();
        Assert.AreEqual("MOEAD: MOEA/D - DE", algorithm.get_name());
    }

    public override bool Constrained => false;
    public override bool Unconstrained => true;
    public override bool SingleObjective => false;
    public override bool MultiObjective => true;
    public override bool IntegerProgramming => false;
    public override bool Stochastic => false;
}

