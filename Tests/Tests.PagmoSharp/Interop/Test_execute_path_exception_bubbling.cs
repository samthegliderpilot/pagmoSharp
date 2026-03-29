using NUnit.Framework;
using pagmo;
using System;

namespace Tests.PagmoSharp.Interop;

[TestFixture]
public class Test_execute_path_exception_bubbling
{
    [Test]
    public void NativeRuntimeFailureBubblesThroughAlgorithmEvolveWithContext()
    {
        using var problem = new rosenbrock(2u);
        using var initialPopulation = new population(problem, 16u, 42u);
        using var constrainedMetaAlgorithm = new cstrs_self_adaptive(2u);
        using var algorithm = constrainedMetaAlgorithm.to_algorithm();

        var ex = Assert.Throws<ApplicationException>(() =>
        {
            using var _ = algorithm.evolve(initialPopulation);
        });

        AssertExecutePathException(ex, "algorithm.evolve failed");
    }

    [Test]
    public void NativeRuntimeFailureBubblesThroughIslandWaitCheckWithContext()
    {
        using var problem = new rosenbrock(2u);
        using var constrainedMetaAlgorithm = new cstrs_self_adaptive(2u);
        using var algorithm = constrainedMetaAlgorithm.to_algorithm();
        using var island = pagmo.island.Create(algorithm, problem, 16u, 42u);

        island.evolve(1u);
        var ex = Assert.Throws<ApplicationException>(() => island.wait_check());
        AssertExecutePathException(ex, "island.wait_check failed");
    }

    [Test]
    public void NativeRuntimeFailureBubblesThroughArchipelagoWaitCheckWithContext()
    {
        using var problem = new rosenbrock(2u);
        using var constrainedMetaAlgorithm = new cstrs_self_adaptive(2u);
        using var algorithm = constrainedMetaAlgorithm.to_algorithm();
        using var archipelago = new archipelago();

        _ = archipelago.push_back_island(algorithm, problem, 16u, 42u);

        archipelago.evolve(1u);
        var ex = Assert.Throws<ApplicationException>(() => archipelago.wait_check());
        AssertExecutePathException(ex, "archipelago.wait_check failed");
    }

    private static void AssertExecutePathException(ApplicationException ex, string expectedContext)
    {
        Assert.That(ex, Is.Not.Null);
        Assert.That(ex.Message, Does.Contain(expectedContext));
        Assert.That(ex.Message.ToLowerInvariant(), Does.Contain("constraint"));
    }
}
