using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp.Algorithms;

[TestFixture]
public class Test_cstrs_self_adaptive
{
    [Test]
    public void NameAndBasicPropertiesAreAccessible()
    {
        using var algorithm = new cstrs_self_adaptive(6u);
        Assert.AreEqual("sa-CNSTR: Self-adaptive constraints handling", algorithm.get_name());

        algorithm.set_seed(3u);
        Assert.AreEqual(3u, algorithm.get_seed());

        algorithm.set_verbosity(2u);
        Assert.AreEqual(2u, algorithm.get_verbosity());
        Assert.IsNotNull(algorithm.get_extra_info());
    }

    [Test]
    public void EvolvesConstrainedProblemAndPreservesPopulationSize()
    {
        using var problem = new TwoDimensionalConstrainedProblem();
        using var algorithm = new cstrs_self_adaptive(10u);
        using var pop = new population(problem, 64u, 2u);

        var originalSize = pop.size();
        var evolved = algorithm.evolve(pop);

        Assert.AreEqual(originalSize, evolved.size());
        using var evolvedProblem = evolved.get_problem();
        Assert.AreEqual(problem.get_nobj(), evolvedProblem.get_nobj());
    }
}
