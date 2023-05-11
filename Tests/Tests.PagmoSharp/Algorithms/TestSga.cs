using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp.Algorithms;

[TestFixture]
public class TestSga : TestAlgorithmBase
{
    public override IAlgorithm CreateAlgorithm()
    {
        return new pagmo.sga(10u, 0.5);
    }

    [Test]
    public override void TestNameIsCorrect()
    {
        using (var problem = new TwoDimensionalSingleObjectiveProblemWrapper())
        using (var algorithm = CreateAlgorithm(problem))
        {
            Assert.AreEqual("SGA: Genetic Algorithm", algorithm.get_name());
        }
    }

    public override bool SupportsGeneration => false;

    /// <inheritdoc />
    public override bool Constrained => false;

    /// <inheritdoc />
    public override bool Unconstrained => true;

    /// <inheritdoc />
    public override bool SingleObjective => true;

    /// <inheritdoc />
    public override bool MultiObjective => false;

    /// <inheritdoc />
    public override bool IntegerPrograming => true;

    /// <inheritdoc />
    public override bool Stochastic => true;
}