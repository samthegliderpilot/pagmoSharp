package io.github.samthegliderpilot.pagmo4j.examples;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.algorithms.*;
import io.github.samthegliderpilot.pagmo4j.examples.problems.*;

/**
 * Runnable pagmo4j examples.
 *
 * <p>Teaches both how to use pagmo4j APIs and why optimization structures
 * (islands, archipelagos, topology, policies) matter in practice.
 *
 * <p>Run from the {@code java/} directory:
 * <pre>
 *   $env:PAGMO4J_NATIVE_DIR = "pagmoWrapper/win-build"
 *   .\gradlew :examples:run --args="all"
 * </pre>
 *
 * <p>Scenario options: {@code single}, {@code archipelago}, {@code policies},
 * {@code cloning}, {@code kotlin}, {@code all}.
 * Add {@code --verbose} (or {@code -v}) to print algorithm logs.
 */
public final class Main {

    static final int DEFAULT_POP_SIZE = 64;
    static final int DEFAULT_ISLAND_COUNT = 8;

    public static void main(String[] args) {
        String scenario = "all";
        boolean verbose = false;
        for (String arg : args) {
            String a = arg.trim().toLowerCase();
            if (a.equals("--verbose") || a.equals("-v")) verbose = true;
            else scenario = a;
        }

        System.out.println("pagmo4j runnable examples");
        System.out.println("These examples teach API usage and why islands/archipelagos/policies can help search quality.");
        if (verbose) System.out.println("(verbose: algorithm logs will be printed after each scenario)");
        System.out.println();

        switch (scenario) {
            case "single"      -> Scenarios.runSingleIslandBaseline(verbose);
            case "archipelago" -> Scenarios.runArchipelagoScenario(verbose);
            case "policies"    -> Scenarios.runPolicyComparison(verbose);
            case "cloning"     -> Scenarios.runCloneableProblemsScenario(verbose);
            case "kotlin"      -> KotlinExamples.INSTANCE.run(verbose);
            case "all" -> {
                Scenarios.runSingleIslandBaseline(verbose);
                System.out.println();
                Scenarios.runArchipelagoScenario(verbose);
                System.out.println();
                Scenarios.runPolicyComparison(verbose);
                System.out.println();
                Scenarios.runCloneableProblemsScenario(verbose);
                System.out.println();
                KotlinExamples.INSTANCE.run(verbose);
            }
            default -> {
                System.out.println("Unknown scenario '" + scenario + "'.");
                System.out.println("Use one of: single, archipelago, policies, cloning, kotlin, all");
                System.out.println("Add --verbose (or -v) to print algorithm logs after each scenario.");
            }
        }
    }

    static void printAlgorithmLog(String label, java.util.List<IAlgorithmLogLine> lines) {
        if (lines.isEmpty()) return;
        System.out.println("  [" + label + " — " + lines.size() + " log entries]");
        for (IAlgorithmLogLine line : lines)
            System.out.println("    " + line.toDisplayString());
    }

    static double getIslandChampionFitness(island isl) {
        try (population pop = isl.get_population()) {
            DoubleVector cf = pop.champion_f();
            try { return cf.get(0); }
            finally { cf.delete(); }
        }
    }

    static double getArchipelagoBestFitness(archipelago archi) {
        double best = Double.POSITIVE_INFINITY;
        for (long i = 0; i < archi.size(); i++) {
            try (island isl = archi.get_island_copy(i)) {
                best = Math.min(best, getIslandChampionFitness(isl));
            }
        }
        return best;
    }
}
