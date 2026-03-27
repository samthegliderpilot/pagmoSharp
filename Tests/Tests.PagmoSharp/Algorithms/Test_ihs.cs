using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp.Algorithms;

[TestFixture]
public class Test_ihs : TestAlgorithmBase
{
    public override IAlgorithm CreateAlgorithm()
    {
        return new ihs(100u);
    }

    [Test]
    public override void TestNameIsCorrect()
    {
        using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
        using var algorithm = CreateAlgorithm(problem);
        Assert.AreEqual("IHS: Improved Harmony Search", algorithm.get_name());
    }

    public override bool SupportsGeneration => false;
    public override bool Constrained => false;
    public override bool Unconstrained => true;
    public override bool SingleObjective => true;
    public override bool MultiObjective => false;
    public override bool IntegerPrograming => false;
    public override bool Stochastic => true;
}
