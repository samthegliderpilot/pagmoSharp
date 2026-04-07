using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp;

[TestFixture]
public class Test_archipelago_managed_policies
{
    private sealed class ManagedReplacementPolicy : r_policyBase
    {
        public override IndividualsGroup replace(
            IndividualsGroup incoming,
            uint migrationCount,
            uint populationSize,
            uint islandIndex,
            uint islandCount,
            uint migrationRound,
            DoubleVector migrationData,
            IndividualsGroup currentPopulation)
        {
            return incoming;
        }

        public override string get_name() => "ManagedReplacementPolicy";
        public override string get_extra_info() => "managed replacement policy";
        public override bool is_valid() => true;
    }

    private sealed class ManagedSelectionPolicy : s_policyBase
    {
        public override IndividualsGroup select(
            IndividualsGroup populationGroup,
            uint requested,
            uint populationSize,
            uint islandIndex,
            uint islandCount,
            uint migrationRound,
            DoubleVector migrationData)
        {
            return populationGroup;
        }

        public override string get_name() => "ManagedSelectionPolicy";
        public override string get_extra_info() => "managed selection policy";
        public override bool is_valid() => true;
    }

    [Test]
    public void ArchipelagoCanPushBackIslandDirectlyFromManagedPolicyBases()
    {
        using var archipelago = new archipelago();
        using var algorithm = new bee_colony().to_algorithm();
        using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
        using var replacementPolicy = new ManagedReplacementPolicy();
        using var selectionPolicy = new ManagedSelectionPolicy();

        var islandIndex = archipelago.push_back_island(algorithm, problem, 16u, replacementPolicy, selectionPolicy, 2u);
        Assert.That(islandIndex, Is.EqualTo(0u));
        Assert.That(archipelago.size(), Is.EqualTo(1u));

        archipelago.evolve(1u);
        archipelago.wait_check();
        Assert.That(archipelago.status(), Is.EqualTo(evolve_status.idle));
    }

    [Test]
    public void ArchipelagoPascalCaseAliasAcceptsManagedPolicyBases()
    {
        using var archipelago = new archipelago();
        using IAlgorithm algorithm = new bee_colony();
        using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
        using var replacementPolicy = new ManagedReplacementPolicy();
        using var selectionPolicy = new ManagedSelectionPolicy();

        var islandIndex = archipelago.PushBackIsland(algorithm, problem, 16u, replacementPolicy, selectionPolicy, 2u);
        Assert.That(islandIndex, Is.EqualTo(0u));
        Assert.That(archipelago.size(), Is.EqualTo(1u));

        archipelago.evolve(1u);
        archipelago.wait_check();
        Assert.That(archipelago.status(), Is.EqualTo(evolve_status.idle));
    }

    [Test]
    public void ArchipelagoCanPushBackIslandWithBfeFromManagedPolicyBases()
    {
        using var archipelago = new archipelago();
        using var algorithm = new bee_colony().to_algorithm();
        using var evaluator = new default_bfe().to_bfe();
        using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
        using var replacementPolicy = new ManagedReplacementPolicy();
        using var selectionPolicy = new ManagedSelectionPolicy();

        var islandIndex = archipelago.push_back_island(algorithm, problem, evaluator, 16u, replacementPolicy, selectionPolicy, 2u);
        Assert.That(islandIndex, Is.EqualTo(0u));
        Assert.That(archipelago.size(), Is.EqualTo(1u));

        archipelago.evolve(1u);
        archipelago.wait_check();
        Assert.That(archipelago.status(), Is.EqualTo(evolve_status.idle));
    }

    [Test]
    public void ArchipelagoPascalCaseAliasAcceptsManagedPolicyBasesWithBfe()
    {
        using var archipelago = new archipelago();
        using IAlgorithm algorithm = new bee_colony();
        using var evaluator = new default_bfe().to_bfe();
        using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
        using var replacementPolicy = new ManagedReplacementPolicy();
        using var selectionPolicy = new ManagedSelectionPolicy();

        var islandIndex = archipelago.PushBackIsland(algorithm, problem, evaluator, 16u, replacementPolicy, selectionPolicy, 2u);
        Assert.That(islandIndex, Is.EqualTo(0u));
        Assert.That(archipelago.size(), Is.EqualTo(1u));

        archipelago.evolve(1u);
        archipelago.wait_check();
        Assert.That(archipelago.status(), Is.EqualTo(evolve_status.idle));
    }
}
