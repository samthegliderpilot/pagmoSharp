using System;
using System.Collections.Generic;
using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp;

[TestFixture]
public class Test_archipelago_algorithm_interop_matrix
{
    private static IEnumerable<TestCaseData> RuntimeCases()
    {
        // Single-objective unconstrained algorithms.
        yield return new TestCaseData((Func<algorithm>)(() => new bee_colony().to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("BeeColony_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new cmaes(2u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Cmaes_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new compass_search(16u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("CompassSearch_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new de(2u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("De_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new de1220(2u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("De1220_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new gwo(2u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Gwo_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new ihs(3u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Ihs_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new pso(2u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Pso_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new pso_gen(2u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("PsoGen_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new sade(2u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Sade_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new sea(2u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Sea_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new sga(2u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Sga_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new simulated_annealing().to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("SimulatedAnnealing_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new xnes(2u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Xnes_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new null_algorithm().to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("NullAlgorithm_ArchipelagoRuntimeBridge");

        // Constrained single-objective paths.
        yield return new TestCaseData((Func<algorithm>)(() => new gaco(2u, 16u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalConstrainedProblem()), 1u, 3).SetName("Gaco_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new cstrs_self_adaptive(4u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalConstrainedProblem()), 1u, 3).SetName("CstrsSelfAdaptive_ArchipelagoRuntimeBridge");

        // Multi-objective paths.
        yield return new TestCaseData((Func<algorithm>)(() => new nsga2(3u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalMultiObjectiveProblemWrapper()), 2u, 2).SetName("Nsga2_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new moead(3u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalMultiObjectiveProblemWrapper()), 2u, 2).SetName("Moead_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new moead_gen(3u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalMultiObjectiveProblemWrapper()), 2u, 2).SetName("MoeadGen_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new maco(3u, 16u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalMultiObjectiveProblemWrapper()), 2u, 2).SetName("Maco_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<algorithm>)(() => new nspso(3u).to_algorithm()), (Func<IProblem>)(() => new TwoDimensionalMultiObjectiveProblemWrapper()), 2u, 2).SetName("Nspso_ArchipelagoRuntimeBridge");
    }

    [TestCaseSource(nameof(RuntimeCases))]
    public void ManagedAlgorithmInteropPathCreatesAndEvolvesArchipelagoIsland(
        Func<algorithm> createAlgorithm,
        Func<IProblem> createProblem,
        uint expectedObjectiveCount,
        int expectedChampionFitnessCount)
    {
        using var archipelago = new archipelago();
        using var algorithm = createAlgorithm();
        using var problem = createProblem();

        archipelago.push_back_island(algorithm, problem, 24u, 2u);
        Assert.That(archipelago.size(), Is.EqualTo(1u));

        archipelago.evolve(1u);
        archipelago.wait_check();
        Assert.That(archipelago.status(), Is.EqualTo(EvolveStatus.Idle));

        using var islandCopy = archipelago.GetIslandCopy(0u);
        using var configuredAlgorithm = islandCopy.get_algorithm();
        Assert.That(configuredAlgorithm.get_name(), Is.EqualTo(algorithm.get_name()));

        using var population = islandCopy.get_population();
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
