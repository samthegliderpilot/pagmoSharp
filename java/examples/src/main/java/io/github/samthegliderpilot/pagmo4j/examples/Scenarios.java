package io.github.samthegliderpilot.pagmo4j.examples;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.algorithms.*;
import io.github.samthegliderpilot.pagmo4j.migration.*;
import io.github.samthegliderpilot.pagmo4j.examples.problems.*;

/** Java-idiomatic scenario implementations. */
final class Scenarios {

    private Scenarios() {}

    // Scenario 1 ── single island baseline ───────────────────────────────────

    static void runSingleIslandBaseline(boolean verbose) {
        System.out.println("Scenario: single island baseline");
        double best = runSingleIsland(/*seed=*/42L, /*evolveCalls=*/20L, verbose);
        System.out.printf("  Best fitness after evolve: %.6f%n", best);
        System.out.println("  Why it matters: this is the simplest production path and a useful baseline.");
    }

    static double runSingleIsland(long seed, long evolveCalls, boolean verbose) {
        try (RastriginProblem prob = new RastriginProblem();
             de algo = new de(80L, 0.8, 0.9, 2L, 1e-6, 1e-6, seed)) {
            if (verbose) algo.set_verbosity(1L);
            try (island isl = island.create(algo, prob, Main.DEFAULT_POP_SIZE, seed)) {
                isl.evolve(evolveCalls);
                isl.waitCheck();
                // Log from the concrete algo object; the type-erased island.get_algorithm()
                // wrapper does not expose typed log lines.
                if (verbose) Main.printAlgorithmLog("de", algo.getLogLines());
                return Main.getIslandChampionFitness(isl);
            }
        }
    }

    // Scenario 2 ── archipelago topology ────────────────────────────────────

    static void runArchipelagoScenario(boolean verbose) {
        System.out.println("Scenario: archipelago and topology intuition");

        describeTopologyConnectivity();

        double singleBest = runSingleIsland(77L, (long) 15 * Main.DEFAULT_ISLAND_COUNT, verbose);
        double archiBest  = runArchipelago(false, verbose);

        System.out.printf("  Single island (same total evolve rounds) vs archipelago:%n");
        System.out.printf("    single-island best: %.6f%n", singleBest);
        System.out.printf("    archipelago best:   %.6f%n", archiBest);
        System.out.println("  Why it matters: parallel search trajectories help avoid local minima.");
    }

    private static void describeTopologyConnectivity() {
        // ring(n) connects each island to its two neighbours; default archipelago has no topology.
        try (ring ringTopo = new ring()) {
            // Get connections for vertex 0 once two islands are registered
            ringTopo.push_back(); ringTopo.push_back(); ringTopo.push_back();
            TopologyConnections conn = ringTopo.get_connections(0L);
            SizeTVector neighbours = conn.getFirst();
            System.out.println("  Topology intuition (ring, vertex 0):");
            System.out.println("    ring neighbours:      " + neighbours.size());
            System.out.println("    default (unconnected): 0  — islands evolve independently");
            neighbours.delete();
            conn.delete();
        }
    }

    // Scenario 3 ── policy comparison ────────────────────────────────────────

    static void runPolicyComparison(boolean verbose) {
        System.out.println("Scenario: migration policy impact");
        System.out.println("  Default: no custom policies (pagmo uses built-in fair_replace / select_best).");
        System.out.println("  Managed: IRPolicy / ISPolicy callbacks let you observe and customise migration.");

        double defaultBest = runArchipelago(false, verbose);
        double managedBest = runArchipelagoWithManagedPolicies(verbose);

        System.out.printf("  Default policies:   best=%.6f%n", defaultBest);
        System.out.printf("  Managed pass-through: best=%.6f%n", managedBest);
        System.out.println("  Why it matters: IRPolicy/ISPolicy let you log, filter, or customise migration.");
    }

    /** Shows managed policy wiring — a pass-through that counts calls. */
    static double runArchipelagoWithManagedPolicies(boolean verbose) {
        int[] replaceCalls = {0};
        int[] selectCalls  = {0};

        IRPolicy rPolicy = new RPolicyCallbackAdapter() {
            @Override
            public IndividualsGroup replace(IndividualsGroup incoming, long nF, long nEc,
                    long nIc, long nObj, long popSize, DoubleVector tol,
                    IndividualsGroup current) {
                replaceCalls[0]++;
                return incoming;  // accept all incoming migrants
            }
            @Override public String get_name() { return "CountingRPolicy"; }
        };

        ISPolicy sPolicy = new SPolicyCallbackAdapter() {
            @Override
            public IndividualsGroup select(IndividualsGroup population, long nF, long nEc,
                    long nIc, long nObj, long popSize, DoubleVector tol) {
                selectCalls[0]++;
                return population;  // send full population as emigrants
            }
            @Override public String get_name() { return "CountingSPolicy"; }
        };

        try (RastriginProblem prob = new RastriginProblem();
             archipelago archi = new archipelago()) {
            try (ring topo = new ring()) { archi.set_topology_ring(topo); }

            for (int i = 0; i < Main.DEFAULT_ISLAND_COUNT; i++) {
                try (de algo = new de(60L, 0.8, 0.9, 2L, 1e-6, 1e-6, 101L + i)) {
                    archi.pushBackIsland(algo, prob, rPolicy, sPolicy,
                        Main.DEFAULT_POP_SIZE, 201L + i);
                }
            }

            archi.evolve(5L);
            archi.waitCheck();

            System.out.printf("    replace() called: %d  select() called: %d%n",
                replaceCalls[0], selectCalls[0]);

            return Main.getArchipelagoBestFitness(archi);
        }
    }

    static double runArchipelago(boolean usePolicies, boolean verbose) {
        try (RastriginProblem prob = new RastriginProblem();
             archipelago archi = new archipelago()) {

            for (int i = 0; i < Main.DEFAULT_ISLAND_COUNT; i++) {
                try (de algo = new de(60L, 0.8, 0.9, 2L, 1e-6, 1e-6, 101L + i)) {
                    if (verbose) algo.set_verbosity(1L);
                    archi.pushBackIsland(algo, prob, Main.DEFAULT_POP_SIZE, 201L + i);
                }
            }

            archi.evolve(15L);
            archi.waitCheck();

            return Main.getArchipelagoBestFitness(archi);
        }
    }

    // Scenario 4 ── cloneable non-thread-safe problem ────────────────────────

    static void runCloneableProblemsScenario(boolean verbose) {
        System.out.println("Scenario: cloneable non-thread-safe problem in archipelago");
        System.out.println("  CloneableRastriginProblem declares ThreadSafety.None but implements clone().");
        System.out.println("  Each island receives its own exclusive copy — no concurrency required.");

        CloneableRastriginProblem.cloneCount.set(0);
        CloneableRastriginProblem.totalEvaluations.set(0);

        try (CloneableRastriginProblem prob = new CloneableRastriginProblem();
             archipelago archi = new archipelago()) {

            for (int i = 0; i < Main.DEFAULT_ISLAND_COUNT; i++) {
                try (de algo = new de(60L, 0.8, 0.9, 2L, 1e-6, 1e-6, 301L + i)) {
                    archi.pushBackIsland(algo, prob, Main.DEFAULT_POP_SIZE, 401L + i);
                }
            }

            System.out.println("  Clones created for " + Main.DEFAULT_ISLAND_COUNT +
                " islands: " + CloneableRastriginProblem.cloneCount.get());

            archi.evolve(15L);
            archi.waitCheck();

            System.out.printf("  Best fitness: %.6f%n", Main.getArchipelagoBestFitness(archi));
            System.out.println("  Total fitness evaluations across all clones: " +
                CloneableRastriginProblem.totalEvaluations.get());
        }
    }
}
