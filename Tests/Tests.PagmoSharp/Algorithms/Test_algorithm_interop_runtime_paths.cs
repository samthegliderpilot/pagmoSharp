using System;
using System.Collections.Generic;
using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp.Algorithms;

[TestFixture]
[Category("QuarantinedCrash")]
[Explicit("Quarantined: these runtime matrix cases still trigger unmanaged test-host crashes in current interop/native integration. Keep explicit until crash root cause is fixed.")]
public class Test_algorithm_interop_runtime_paths
{
    private static IEnumerable<TestCaseData> RuntimeCases()
    {
        // Single-objective unconstrained algorithms.
        yield return new TestCaseData((Func<algorithm>)(() => new bee_colony().to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("BeeColony_IslandRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new cmaes(2u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Cmaes_IslandRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new compass_search(16u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("CompassSearch_IslandRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new de(2u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("De_IslandRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new de1220(2u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("De1220_IslandRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new gwo(2u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Gwo_IslandRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new ihs(3u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Ihs_IslandRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new pso(2u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Pso_IslandRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new pso_gen(2u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("PsoGen_IslandRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new sade(2u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Sade_IslandRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new sea(2u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Sea_IslandRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new sga(2u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Sga_IslandRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new simulated_annealing().to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("SimulatedAnnealing_IslandRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new xnes(2u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Xnes_IslandRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new null_algorithm().to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("NullAlgorithm_IslandRuntimeBridge");

        // Constrained single-objective paths.
        yield return new TestCaseData((Func<algorithm>)(() => new gaco(2u, 16u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalConstrainedProblem()), 1u, 3).SetName("Gaco_IslandRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new cstrs_self_adaptive(4u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalConstrainedProblem()), 1u, 3).SetName("CstrsSelfAdaptive_IslandRuntimeBridge");

        // Multi-objective paths.
        yield return new TestCaseData((Func<algorithm>)(() => new nsga2(3u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalMultiObjectiveProblemWrapper()), 2u, 2).SetName("Nsga2_IslandRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new moead(3u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalMultiObjectiveProblemWrapper()), 2u, 2).SetName("Moead_IslandRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new moead_gen(3u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalMultiObjectiveProblemWrapper()), 2u, 2).SetName("MoeadGen_IslandRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new maco(3u, 16u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalMultiObjectiveProblemWrapper()), 2u, 2).SetName("Maco_IslandRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new nspso(3u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalMultiObjectiveProblemWrapper()), 2u, 2).SetName("Nspso_IslandRuntimeBridge");
    }

    [TestCaseSource(nameof(RuntimeCases))]
    public void ManagedAlgorithmInteropPathCreatesAndEvolvesIsland(
        Func<algorithm> createAlgorithm,
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
