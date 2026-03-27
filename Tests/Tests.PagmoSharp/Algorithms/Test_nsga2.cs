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

    [Test]
    public void TestMultiobjectiveUnconstrainedBehavior()
    {
        using var problemBase = new zdt(1u);
        using var algorithm = CreateAlgorithm();
        using var pop = new population(problemBase, 64u, 2u);

        var originalSize = pop.size();
        var finalPop = algorithm.evolve(pop);

        Assert.AreEqual(originalSize, finalPop.size(), "Population size should be preserved.");
        using var finalProblem = finalPop.get_problem();
        Assert.AreEqual(2u, finalProblem.get_nobj(), "ZDT1 should expose two objectives.");
    }

    public override bool SupportsGeneration => false;
    public override bool Constrained => false;
    public override bool Unconstrained => true;
    public override bool SingleObjective => false;
    public override bool MultiObjective => false;
    public override bool IntegerPrograming => false;
    public override bool Stochastic => false;
}
