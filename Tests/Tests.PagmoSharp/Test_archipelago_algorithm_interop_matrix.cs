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
        yield return new TestCaseData((Func<IAlgorithm>)(() => new bee_colony()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("BeeColony_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new cmaes(2u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Cmaes_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new compass_search(16u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("CompassSearch_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new de(2u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("De_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new de1220(2u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("De1220_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new gwo(2u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Gwo_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new ihs(3u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Ihs_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new pso(2u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Pso_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new pso_gen(2u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("PsoGen_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new sade(2u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Sade_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new sea(2u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Sea_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new sga(2u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Sga_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new simulated_annealing()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("SimulatedAnnealing_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new xnes(2u)), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("Xnes_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new null_algorithm()), (Func<IProblem>)(() => new TwoDimensionalSingleObjectiveProblemWrapper()), 1u, 1).SetName("NullAlgorithm_ArchipelagoRuntimeBridge");

        // Constrained single-objective paths.
        yield return new TestCaseData((Func<IAlgorithm>)(() => new gaco(2u, 16u)), (Func<IProblem>)(() => new TwoDimensionalConstrainedProblem()), 1u, 3).SetName("Gaco_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new cstrs_self_adaptive(4u)), (Func<IProblem>)(() => new TwoDimensionalConstrainedProblem()), 1u, 3).SetName("CstrsSelfAdaptive_ArchipelagoRuntimeBridge");

        // Multi-objective paths.
        yield return new TestCaseData((Func<IAlgorithm>)(() => new nsga2(3u)), (Func<IProblem>)(() => new TwoDimensionalMultiObjectiveProblemWrapper()), 2u, 2).SetName("Nsga2_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new moead(3u)), (Func<IProblem>)(() => new TwoDimensionalMultiObjectiveProblemWrapper()), 2u, 2).SetName("Moead_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new moead_gen(3u)), (Func<IProblem>)(() => new TwoDimensionalMultiObjectiveProblemWrapper()), 2u, 2).SetName("MoeadGen_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new maco(3u, 16u)), (Func<IProblem>)(() => new TwoDimensionalMultiObjectiveProblemWrapper()), 2u, 2).SetName("Maco_ArchipelagoRuntimeBridge");
        yield return new TestCaseData((Func<IAlgorithm>)(() => new nspso(3u)), (Func<IProblem>)(() => new TwoDimensionalMultiObjectiveProblemWrapper()), 2u, 2).SetName("Nspso_ArchipelagoRuntimeBridge");
    }

    [TestCaseSource(nameof(RuntimeCases))]
    public void ManagedAlgorithmInteropPathCreatesAndEvolvesArchipelagoIsland(
        Func<IAlgorithm> createAlgorithm,
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
        Assert.That(archipelago.status(), Is.EqualTo(evolve_status.idle));

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
