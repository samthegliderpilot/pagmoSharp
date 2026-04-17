using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp;

[TestFixture]
public class Test_archipelago_managed_policies
{
    // Managed problem fixture that supports batch_fitness so member_bfe and policy wiring
    // can be validated through the managed callback path.
    private sealed class ManagedBatchProblem : ManagedProblemBase
    {
        public override ThreadSafety get_thread_safety() => ThreadSafety.Basic;

        public override PairOfDoubleVectors get_bounds()
        {
            return new PairOfDoubleVectors(new DoubleVector(new[] { -5.0, -5.0 }), new DoubleVector(new[] { 5.0, 5.0 }));
        }

        public override DoubleVector fitness(DoubleVector x)
        {
            var x0 = x[0];
            var x1 = x[1];
            return new DoubleVector(new[] { x0 * x0 + x1 * x1 });
        }

        public override bool has_batch_fitness() => true;

        public override DoubleVector batch_fitness(DoubleVector dvs)
        {
            var result = new DoubleVector();
            for (var i = 0; i < dvs.Count; i += 2)
            {
                var x0 = dvs[i];
                var x1 = dvs[i + 1];
                result.Add(x0 * x0 + x1 * x1);
            }

            return result;
        }
    }

    // Managed replacement policy stub used to validate direct RPolicyCallback ingestion
    // by archipelago wrappers without requiring policy-specific optimization behavior.
    private sealed class ManagedReplacementPolicy : RPolicyCallback
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

    // Managed selection policy stub paired with ManagedReplacementPolicy for policy
    // callback lifetime and ownership-path coverage in archipelago entry points.
    private sealed class ManagedSelectionPolicy : SPolicyCallback
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
        Assert.That(archipelago.status(), Is.EqualTo(EvolveStatus.Idle));
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
        Assert.That(archipelago.status(), Is.EqualTo(EvolveStatus.Idle));
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
        Assert.That(archipelago.status(), Is.EqualTo(EvolveStatus.Idle));
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
        Assert.That(archipelago.status(), Is.EqualTo(EvolveStatus.Idle));
    }

    [Test]
    public void ArchipelagoCanPushBackIslandWithDefaultBfeFromManagedPolicyBases()
    {
        using var archipelago = new archipelago();
        using IAlgorithm algorithm = new bee_colony();
        using var evaluator = new default_bfe().to_bfe();
        using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
        using var replacementPolicy = new ManagedReplacementPolicy();
        using var selectionPolicy = new ManagedSelectionPolicy();

        var islandIndex = archipelago.push_back_island(algorithm, problem, evaluator, 16u, replacementPolicy, selectionPolicy, 2u);
        Assert.That(islandIndex, Is.EqualTo(0u));
        Assert.That(archipelago.size(), Is.EqualTo(1u));

        archipelago.evolve(1u);
        archipelago.wait_check();
        Assert.That(archipelago.status(), Is.EqualTo(EvolveStatus.Idle));
    }

    [Test]
    public void ArchipelagoCanPushBackIslandWithThreadBfeFromManagedPolicyBases()
    {
        using var archipelago = new archipelago();
        using IAlgorithm algorithm = new bee_colony();
        using var evaluator = new thread_bfe().to_bfe();
        using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();
        using var replacementPolicy = new ManagedReplacementPolicy();
        using var selectionPolicy = new ManagedSelectionPolicy();

        var islandIndex = archipelago.push_back_island(algorithm, problem, evaluator, 16u, replacementPolicy, selectionPolicy, 2u);
        Assert.That(islandIndex, Is.EqualTo(0u));
        Assert.That(archipelago.size(), Is.EqualTo(1u));

        archipelago.evolve(1u);
        archipelago.wait_check();
        Assert.That(archipelago.status(), Is.EqualTo(EvolveStatus.Idle));
    }

    [Test]
    public void ArchipelagoCanPushBackIslandWithMemberBfeFromManagedPolicyBases()
    {
        using var archipelago = new archipelago();
        using IAlgorithm algorithm = new bee_colony();
        using var evaluator = new member_bfe().to_bfe();
        using var problem = new ManagedBatchProblem();
        using var replacementPolicy = new ManagedReplacementPolicy();
        using var selectionPolicy = new ManagedSelectionPolicy();

        var islandIndex = archipelago.push_back_island(algorithm, problem, evaluator, 16u, replacementPolicy, selectionPolicy, 2u);
        Assert.That(islandIndex, Is.EqualTo(0u));
        Assert.That(archipelago.size(), Is.EqualTo(1u));

        archipelago.evolve(1u);
        archipelago.wait_check();
        Assert.That(archipelago.status(), Is.EqualTo(EvolveStatus.Idle));
    }
}
