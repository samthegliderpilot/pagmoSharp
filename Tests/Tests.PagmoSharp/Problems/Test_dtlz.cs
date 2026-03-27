using System.Collections.Generic;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Problems;

[TestFixture]
public class Test_dtlz : TestProblemBase
{
    public override IProblem CreateStandardProblem(uint problemIndex = 0)
    {
        var id = problemIndex == 0 ? 1u : problemIndex;
        return new dtlz(id, 7u, 3u);
    }

    [Test]
    public override void TestBoilerPlate()
    {
        using var problem = CreateStandardProblem(1u);
        Assert.IsNotEmpty(problem.get_name());
        Assert.AreEqual(3u, problem.get_nobj());
        Assert.AreEqual(0u, problem.get_nec());
        Assert.AreEqual(0u, problem.get_nic());
        Assert.AreEqual(0u, problem.get_nix());
        Assert.AreEqual(thread_safety.none, problem.get_thread_safety());
        Assert.IsFalse(problem.has_batch_fitness());

        using var bounds = problem.get_bounds();
        Assert.AreEqual(7, bounds.first.Count);
        Assert.AreEqual(7, bounds.second.Count);
        for (var i = 0; i < bounds.first.Count; i++)
        {
            Assert.LessOrEqual(bounds.first[i], bounds.second[i]);
        }
    }

    [Test]
    public override void TestOptimizing()
    {
        using var problem = CreateStandardProblem(1u);
        using var algorithm = new nspso(20, 1);
        algorithm.set_seed(211u);

        using var initialPopulation = new population(problem, 64u, 37u);
        using var evolvedPopulation = algorithm.evolve(initialPopulation);
        using var allFitness = evolvedPopulation.get_f();
        using var allDecisionVectors = evolvedPopulation.get_x();

        Assert.AreEqual(64, allFitness.Count);
        Assert.AreEqual(64, allDecisionVectors.Count);
        Assert.AreEqual(3, allFitness[0].Count);

        using var bounds = problem.get_bounds();
        for (var individualIdx = 0; individualIdx < allDecisionVectors.Count; individualIdx++)
        {
            Assert.AreEqual(bounds.first.Count, allDecisionVectors[individualIdx].Count);
            for (var dimensionIdx = 0; dimensionIdx < allDecisionVectors[individualIdx].Count; dimensionIdx++)
            {
                var value = allDecisionVectors[individualIdx][dimensionIdx];
                Assert.GreaterOrEqual(value, bounds.first[dimensionIdx]);
                Assert.LessOrEqual(value, bounds.second[dimensionIdx]);
            }
        }

        using var concreteProblem = new dtlz(1u, 7u, 3u);
        using var firstDecisionVector = allDecisionVectors[0];
        var pointDistance = concreteProblem.p_distance(firstDecisionVector);
        var populationDistance = concreteProblem.p_distance(evolvedPopulation);
        Assert.GreaterOrEqual(pointDistance, 0d);
        Assert.GreaterOrEqual(populationDistance, 0d);
    }

    [Test]
    public void TestFitnessVectorLengthForKnownValidInput()
    {
        using var problem = CreateStandardProblem(1u);
        using var x = new DoubleVector(new[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7 });
        using var fitness = problem.fitness(x);
        Assert.AreEqual(problem.get_nobj(), (uint)fitness.Count);
    }

    protected override bool SupportsMidpointFitnessProbe()
    {
        return false;
    }

    protected override IEnumerable<ProblemTestData> GetRegressionData()
    {
        return new[]
        {
            new ProblemTestData("dtlz", "DTLZ1_regression", new[] { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7 }, new[] { 0.10999999999999965, 0.4399999999999986, 4.949999999999984 }, 1u),
        };
    }
}
