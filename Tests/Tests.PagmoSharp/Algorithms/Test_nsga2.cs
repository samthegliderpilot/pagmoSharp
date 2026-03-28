using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Algorithms;

[TestFixture]
public class Test_nsga2 : TestAlgorithmBase
{
    public override IAlgorithm CreateAlgorithm()
    {
        return new nsga2(8u);
    }

    [Test]
    public override void TestNameIsCorrect()
    {
        using var algorithm = CreateAlgorithm();
        Assert.AreEqual("NSGA-II:", algorithm.get_name());
    }

    public override bool SupportsGeneration => false;
    public override bool Constrained => false;
    public override bool Unconstrained => true;
    public override bool SingleObjective => false;
    public override bool MultiObjective => true;
    public override bool IntegerPrograming => false;
    public override bool Stochastic => false;
}
