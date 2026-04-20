using pagmo;

namespace Examples.Pagmo.NET;

internal static class Program
{
    private const ulong DefaultPopulationSize = 64u;
    private const int DefaultIslandCount = 8;

    private static void Main(string[] args)
    {
        var scenario = args.Length == 0 ? "all" : args[0].Trim().ToLowerInvariant();

        Console.WriteLine("Pagmo.NET runnable examples");
        Console.WriteLine("These examples teach API usage and why islands/archipelagos/policies can help search quality.");
        Console.WriteLine();

        switch (scenario)
        {
            case "single":
                RunSingleIslandBaseline();
                break;
            case "archipelago":
                RunArchipelagoTeachingScenario();
                break;
            case "policies":
                RunPolicyComparison();
                break;
            case "all":
                RunSingleIslandBaseline();
                Console.WriteLine();
                RunArchipelagoTeachingScenario();
                Console.WriteLine();
                RunPolicyComparison();
                break;
            default:
                Console.WriteLine($"Unknown scenario '{scenario}'.");
                Console.WriteLine("Use one of: single, archipelago, policies, all");
                break;
        }
    }

    // Baseline: one optimizer on one island.
    private static void RunSingleIslandBaseline()
    {
        Console.WriteLine("Scenario: single island baseline");
        var bestFitness = RunSingleIslandExperiment(seed: 42u, evolveCalls: 20u);
        Console.WriteLine($"  Best fitness after evolve: {bestFitness:F6}");
        Console.WriteLine("  Why it matters: this is the simplest production path and a useful baseline for comparison.");
    }

    // Teach topology intuition + show how multi-island search can improve exploration.
    private static void RunArchipelagoTeachingScenario()
    {
        Console.WriteLine("Scenario: archipelago and topology intuition");

        DescribeTopologyConnectivity();

        var singleIslandBest = RunSingleIslandExperiment(seed: 77u, evolveCalls: 15u * (uint)DefaultIslandCount);
        var archipelagoResult = RunArchipelagoExperiment(usePolicies: false);

        Console.WriteLine("  Single island (same total evolve rounds) vs archipelago multi-start:");
        Console.WriteLine($"    single-island best: {singleIslandBest:F6}");
        PrintExperiment("archipelago", archipelagoResult);
        Console.WriteLine("  Why it matters: islands create parallel search trajectories, which often helps avoid local minima lock-in.");
    }

    // Compare policy wiring on the same archipelago flow.
    private static void RunPolicyComparison()
    {
        Console.WriteLine("Scenario: policy impact (default vs fair_replace/select_best)");

        var defaultPolicyResult = RunArchipelagoExperiment(usePolicies: false);
        var explicitPolicyResult = RunArchipelagoExperiment(usePolicies: true);

        PrintExperiment("default policies", defaultPolicyResult);
        PrintExperiment("fair_replace/select_best", explicitPolicyResult);

        if (defaultPolicyResult.MigrationEvents == 0 && explicitPolicyResult.MigrationEvents == 0)
        {
            Console.WriteLine("  Note: this run had no migration events, so policy impact on migration was not observable.");
            Console.WriteLine("  Policies are still correctly wired and exercised through the archipelago API path.");
        }
        else
        {
            Console.WriteLine("  Why it matters: policies control which migrants are selected and replaced, shaping exploration vs exploitation.");
        }
    }

    private static void DescribeTopologyConnectivity()
    {
        using var ringTopology = new ring(8u, 0.7);
        using var unconnectedTopology = new unconnected();

        var ringConnections = ringTopology.GetConnectionsData(0u);
        var unconnectedConnections = unconnectedTopology.GetConnectionsData(0u);

        Console.WriteLine("  Topology intuition (vertex 0):");
        Console.WriteLine($"    ring neighbors:        {ringConnections.NeighborIds.Length}");
        Console.WriteLine($"    unconnected neighbors: {unconnectedConnections.NeighborIds.Length}");
    }

    private static ExperimentResult RunArchipelagoExperiment(bool usePolicies)
    {
        using var problem = new RastriginLikeProblem();
        using var archi = new archipelago();

        archi.set_migration_type(MigrationType.P2P);
        archi.set_migrant_handling(MigrantHandling.Preserve);

        using var replacementPolicy = usePolicies ? new fair_replace() : null;
        using var selectionPolicy = usePolicies ? new select_best() : null;

        for (var islandIndex = 0; islandIndex < DefaultIslandCount; islandIndex++)
        {
            using IAlgorithm algorithm = new de(60u, 0.8, 0.9, 2u, 1e-6, 1e-6, (uint)(101 + islandIndex));

            if (usePolicies)
            {
                archi.push_back_island(
                    algorithm,
                    problem,
                    DefaultPopulationSize,
                    replacementPolicy!,
                    selectionPolicy!,
                    seed: (uint)(201 + islandIndex));
            }
            else
            {
                archi.push_back_island(algorithm, problem, DefaultPopulationSize, seed: (uint)(201 + islandIndex));
            }
        }

        archi.evolve(15u);
        archi.wait_check();

        var bestFitness = GetArchipelagoBestFitness(archi, DefaultIslandCount);
        var migrationEvents = archi.MigrationLog.Count;
        var topologyName = archi.get_topology_name();

        return new ExperimentResult(topologyName, bestFitness, migrationEvents, usePolicies);
    }

    private static double RunSingleIslandExperiment(uint seed, uint evolveCalls)
    {
        using var problem = new RastriginLikeProblem();
        using IAlgorithm algorithm = new de(80u, 0.8, 0.9, 2u, 1e-6, 1e-6, seed);
        using var singleIsland = island.Create(algorithm, problem, popSize: DefaultPopulationSize, seed: seed);

        singleIsland.evolve(evolveCalls);
        singleIsland.wait_check();
        return GetIslandChampion(singleIsland);
    }

    private static double GetArchipelagoBestFitness(archipelago archi, int islandCount)
    {
        var best = double.PositiveInfinity;
        for (var i = 0; i < islandCount; i++)
        {
            using var islandCopy = archi.GetIslandCopy((ulong)i);
            best = Math.Min(best, GetIslandChampion(islandCopy));
        }

        return best;
    }

    private static double GetIslandChampion(island islandInstance)
    {
        using var population = islandInstance.get_population();
        using var champion = population.champion_f();
        return champion[0];
    }

    private static void PrintExperiment(string label, ExperimentResult result)
    {
        Console.WriteLine($"  {label}");
        Console.WriteLine($"    topology:        {result.TopologyName}");
        Console.WriteLine($"    best fitness:    {result.BestFitness:F6}");
        Console.WriteLine($"    migration events:{result.MigrationEvents}");
        Console.WriteLine($"    explicit policy: {result.UsedExplicitPolicies}");
    }

    private sealed record ExperimentResult(string TopologyName, double BestFitness, int MigrationEvents, bool UsedExplicitPolicies);
}

internal sealed class RastriginLikeProblem : ManagedProblemBase
{
    public override PairOfDoubleVectors get_bounds() => Bounds(new[] { -5.12, -5.12 }, new[] { 5.12, 5.12 });

    public override DoubleVector fitness(DoubleVector x)
    {
        const double a = 10.0;
        var f = a * 2
                + (x[0] * x[0] - a * Math.Cos(2.0 * Math.PI * x[0]))
                + (x[1] * x[1] - a * Math.Cos(2.0 * Math.PI * x[1]));
        return Vec(f);
    }

    public override string get_name() => "RastriginLikeProblem";

    public override ThreadSafety get_thread_safety() => ThreadSafety.Basic;
}
