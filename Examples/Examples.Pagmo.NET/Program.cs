using pagmo;
using Examples.Pagmo.NET.OrbitalManeuver;

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
            case "maneuver":
                RunOrbitalManeuverOptimization();
                break;
            case "all":
                RunSingleIslandBaseline();
                Console.WriteLine();
                RunArchipelagoTeachingScenario();
                Console.WriteLine();
                RunPolicyComparison();
                Console.WriteLine();
                RunOrbitalManeuverOptimization();
                break;
            default:
                Console.WriteLine($"Unknown scenario '{scenario}'.");
                Console.WriteLine("Use one of: single, archipelago, policies, maneuver, all");
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

    // Two-burn Hohmann-like transfer: circular LEO → higher circular orbit.
    // Demonstrates custom UDP with equality constraints solved via
    // cstrs_self_adaptive wrapping de.
    private static void RunOrbitalManeuverOptimization()
    {
        Console.WriteLine("Scenario: orbital maneuver optimisation (2-burn Hohmann-like transfer)");

        // Initial orbit: circular at 400 km altitude (Earth radii + altitude, km)
        const double earthRadiusKm = 6371.0;
        var initial = new KeplerianElements(
            SemiMajorAxis:     earthRadiusKm + 400.0,   // 6771 km
            Eccentricity:      0.001,                    // near-circular (e=0 is singular in Gauss eqs)
            Inclination:       0.5,                      // ~28.6 deg
            Raan:              0.0,
            ArgumentOfPeriapsis: 0.0,
            TrueAnomaly:       0.0);

        // Target: circular orbit at 1000 km altitude
        double targetSma = earthRadiusKm + 1000.0;
        var target = new ManeuverTarget(
            SemiMajorAxis: targetSma,
            Eccentricity:  0.001);

        // One orbital period ≈ upper bound for a single coast segment
        double period = 2.0 * Math.PI * Math.Sqrt(
            Math.Pow(initial.SemiMajorAxis, 3) / OrbitalMechanics.EarthGm);

        using var problem = new ManeuverOptimizationProblem(
            initial:          initial,
            t0:               0.0,
            numBurns:         2,
            target:           target,
            maxCoastDuration: period,
            maxDeltaV:        3.0);   // km/s

        Console.WriteLine($"  Initial SMA: {initial.SemiMajorAxis:F1} km  " +
                          $"Target SMA: {targetSma:F1} km");
        Console.WriteLine($"  Constraints: {problem.get_nec()} equality (Δa, Δe)");

        using var innerAlgorithm = new de(50u);
        using var erasedInner = innerAlgorithm.to_algorithm();
        using var algorithm = new cstrs_self_adaptive(200u, erasedInner, 42u);

        using var population = new population(problem, 200u, 1u);
        using var evolved = algorithm.evolve(population);

        // Find best feasible individual
        using var allF = evolved.get_f();
        using var allX = evolved.get_x();

        double bestDv = double.PositiveInfinity;
        int bestIdx = -1;
        for (int idx = 0; idx < (int)evolved.size(); idx++)
        {
            var fVec = allF[idx];
            bool feasible = true;
            for (int c = 1; c < fVec.Count; c++)
                if (Math.Abs(fVec[c]) > 1e-3) { feasible = false; break; }
            if (feasible && fVec[0] < bestDv) { bestDv = fVec[0]; bestIdx = idx; }
        }

        if (bestIdx < 0)
        {
            Console.WriteLine("  No feasible solution found in this run — try more iterations.");
            return;
        }

        var xBest = allX[bestIdx];
        Console.WriteLine($"  Best feasible total Δv: {bestDv * 1000.0:F1} m/s");
        for (int b = 0; b < 2; b++)
        {
            double coast = xBest[b * 4 + 0];
            double dv = Math.Sqrt(xBest[b*4+1]*xBest[b*4+1] +
                                  xBest[b*4+2]*xBest[b*4+2] +
                                  xBest[b*4+3]*xBest[b*4+3]);
            Console.WriteLine($"  Burn {b + 1}: coast {coast:F0} s, |Δv| {dv * 1000.0:F1} m/s");
        }

        // Show final state residuals
        var burns = new[]
        {
            new CoastAndBurn(xBest[0], xBest[1], xBest[2], xBest[3]),
            new CoastAndBurn(xBest[4], xBest[5], xBest[6], xBest[7])
        };
        var history = OrbitalMechanics.Propagate(initial, 0.0, burns, OrbitalMechanics.EarthGm);
        var finalEl = history[^1].Elements;
        Console.WriteLine($"  Final SMA:  {finalEl.SemiMajorAxis:F1} km  " +
                          $"(target {targetSma:F1} km, Δ {finalEl.SemiMajorAxis - targetSma:+0.0;-0.0} km)");
        Console.WriteLine($"  Final ecc:  {finalEl.Eccentricity:F4}  " +
                          $"(target {target.Eccentricity:F4})");
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
