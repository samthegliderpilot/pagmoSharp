using System;
using System.Collections.Generic;
using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp.Algorithms;

[TestFixture]
[Explicit("Quarantined: passing native SWIG algorithm wrappers through the managed IAlgorithm callback path currently causes an unmanaged host crash. Use type-erased algorithm inputs until native-wrapper normalization is fixed.")]
public class Test_algorithm_interop_runtime_paths
{
    private static IEnumerable<TestCaseData> RuntimeCases()
    {
        // Single-objective unconstrained algorithms.
        yield return new TestCaseData((Func<IAlgorithm>)(() => new bee_colony()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("BeeColony_IslandRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new cmaes(2u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Cmaes_IslandRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new compass_search(16u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("CompassSearch_IslandRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new de(2u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("De_IslandRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new de1220(2u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("De1220_IslandRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new gwo(2u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Gwo_IslandRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new ihs(3u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Ihs_IslandRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new pso(2u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Pso_IslandRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new pso_gen(2u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("PsoGen_IslandRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new sade(2u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Sade_IslandRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new sea(2u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Sea_IslandRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new sga(2u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Sga_IslandRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new simulated_annealing()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("SimulatedAnnealing_IslandRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new xnes(2u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Xnes_IslandRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new null_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("NullAlgorithm_IslandRuntimeBridge");

        // Constrained single-objective paths.
        yield return new TestCaseData((Func<IAlgorithm>)(() => new gaco(2u, 16u)), (Func<IProblem>)(() => new TwoDimensionalConstrainedProblem()), 1u, 3).SetName("Gaco_IslandRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new cstrs_self_adaptive(4u)), (Func<IProblem>)(() => new TwoDimensionalConstrainedProblem()), 1u, 3).SetName("CstrsSelfAdaptive_IslandRuntimeBridge");

        // Multi-objective paths.
        yield return new TestCaseData((Func<IAlgorithm>)(() => new nsga2(3u)), (Func<IProblem>)(() => new TwoDimensionalMultiObjectiveProblemWrapper()), 2u, 2).SetName("Nsga2_IslandRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new moead(3u)), (Func<IProblem>)(() => new TwoDimensionalMultiObjectiveProblemWrapper()), 2u, 2).SetName("Moead_IslandRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new moead_gen(3u)), (Func<IProblem>)(() => new TwoDimensionalMultiObjectiveProblemWrapper()), 2u, 2).SetName("MoeadGen_IslandRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new maco(3u, 16u)), (Func<IProblem>)(() => new TwoDimensionalMultiObjectiveProblemWrapper()), 2u, 2).SetName("Maco_IslandRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new nspso(3u)), (Func<IProblem>)(() => new TwoDimensionalMultiObjectiveProblemWrapper()), 2u, 2).SetName("Nspso_IslandRuntimeBridge");
    }

    [TestCaseSource(nameof(RuntimeCases))]
    public void ManagedAlgorithmInteropPathCreatesAndEvolvesIsland(
        Func<IAlgorithm> createAlgorithm,
        Func<IProblem> createProblem,
        uint expectedObjectiveCount,
        int expectedChampionFitnessCount)
    {
        using var algorithm = createAlgorithm();
        using var problem = createProblem();
        using var island = pagmo.island.Create(algorithm, problem, 24u, 2u);

        Assert.That(island.is_valid(), Is.True);
        using var configuredAlgorithm = island.get_algorithm();
        Assert.That(configuredAlgorithm.get_name(), Is.EqualTo(algorithm.get_name()));

        island.evolve(1);
        island.wait_check();
        Assert.That(island.status(), Is.EqualTo(evolve_status.idle));

        using var population = island.get_population();
        Assert.That(population.size(), Is.EqualTo(24u));
        using var populationProblem = population.get_problem();
        Assert.That(populationProblem.get_nobj(), Is.EqualTo(expectedObjectiveCount));

        if (expectedObjectiveCount > 1u)
        {
            var ex = Assert.Throws<ApplicationException>(() =>
            {
                using var _ = population.champion_f();
            });
            Assert.That(ex, Is.Not.Null);
        }
        else
        {
            using var championF = population.champion_f();
            Assert.That(championF.Count, Is.EqualTo(expectedChampionFitnessCount));
        }
    }
}
