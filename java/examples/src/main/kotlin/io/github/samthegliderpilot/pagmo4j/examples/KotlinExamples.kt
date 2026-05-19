package io.github.samthegliderpilot.pagmo4j.examples

import io.github.samthegliderpilot.pagmo4j.*
import io.github.samthegliderpilot.pagmo4j.algorithms.*
import io.github.samthegliderpilot.pagmo4j.migration.*
import io.github.samthegliderpilot.pagmo4j.examples.problems.CloneableRastriginProblem
import io.github.samthegliderpilot.pagmo4j.examples.problems.RastriginProblem
import io.github.samthegliderpilot.pagmo4j.ext.*

/**
 * Kotlin-idiomatic examples using the kotlin-ext DSL.
 *
 * Shows how [buildArchipelago], [withTopology], [islandOf], and
 * extension properties reduce boilerplate vs the raw Java API.
 */
object KotlinExamples {

    fun run(verbose: Boolean) {
        println("Scenario: Kotlin-idiomatic examples (kotlin-ext DSL)")
        println()

        runKotlinSingleIsland(verbose)
        println()
        runKotlinArchipelago(verbose)
        println()
        runKotlinPolicies(verbose)
        println()
        runKotlinCloneableProblem(verbose)
    }

    // ── 1. single island via islandOf ────────────────────────────────────────

    private fun runKotlinSingleIsland(verbose: Boolean) {
        println("  Kotlin: single island with islandOf()")

        val algo = de(80L, 0.8, 0.9, 2L, 1e-6, 1e-6, 42L)
        if (verbose) algo.set_verbosity(1L)
        val best = islandOf(algo, RastriginProblem(), 64L, 42L).use { isl ->
            isl.run(20L)
            if (verbose) Main.printAlgorithmLog("de", algo.getLogLines())
            isl.championF()[0]
        }

        println("  Best fitness: %.6f".format(best))
        println("  Key API: islandOf(algo, prob, popSize, seed) creates and manages an island.")
    }

    // ── 2. archipelago with topology via buildArchipelago + withTopology ─────

    private fun runKotlinArchipelago(verbose: Boolean) {
        println("  Kotlin: archipelago with ring topology via buildArchipelago { withTopology(ring()) }")

        val archi = buildArchipelago {
            withTopology(ring())
            val prob = RastriginProblem()
            repeat(Main.DEFAULT_ISLAND_COUNT) { i ->
                pushBackIsland(de(60L, 0.8, 0.9, 2L, 1e-6, 1e-6, 101L + i), prob,
                    Main.DEFAULT_POP_SIZE.toLong(), 201L + i)
            }
        }

        archi.use {
            it.run(10L)
            val best = it.bestChampionF()[0]
            println("  Best fitness across all islands: %.6f".format(best))
        }

        println("  Key API: buildArchipelago { withTopology(...) } — topology as part of the DSL block.")
    }

    // ── 3. managed policies with default seed ────────────────────────────────

    private fun runKotlinPolicies(verbose: Boolean) {
        println("  Kotlin: custom migration policies with default-seed sugar")

        var selectCalls = 0
        var replaceCalls = 0

        val rp = object : RPolicyCallbackAdapter() {
            override fun replace(incoming: IndividualsGroup, nF: Long, nEc: Long, nIc: Long,
                                 nObj: Long, popSize: Long, tol: DoubleVector, current: IndividualsGroup): IndividualsGroup {
                replaceCalls++
                return incoming
            }
        }
        val sp = object : SPolicyCallbackAdapter() {
            override fun select(population: IndividualsGroup, nF: Long, nEc: Long, nIc: Long,
                                nObj: Long, popSize: Long, tol: DoubleVector): IndividualsGroup {
                selectCalls++
                return population
            }
        }

        val archi = buildArchipelago {
            withTopology(ring())
            repeat(4) { i ->
                // Default seed — no explicit seed argument needed
                pushBackIslandWithPolicies(
                    de(60L), RastriginProblem(), rp, sp, Main.DEFAULT_POP_SIZE.toLong()
                )
            }
        }

        archi.use {
            it.run(3L)
            println("  replace() called: $replaceCalls  select() called: $selectCalls")
        }

        println("  Key API: pushBackIslandWithPolicies(algo, prob, rp, sp, popSize) — seed is optional.")
    }

    // ── 4. cloneable problem in Kotlin ────────────────────────────────────────

    private fun runKotlinCloneableProblem(verbose: Boolean) {
        println("  Kotlin: cloneable non-thread-safe problem")

        CloneableRastriginProblem.cloneCount.set(0)
        CloneableRastriginProblem.totalEvaluations.set(0)

        val archi = buildArchipelago {
            val prob = CloneableRastriginProblem()
            repeat(Main.DEFAULT_ISLAND_COUNT) { i ->
                pushBackIsland(de(60L), prob, Main.DEFAULT_POP_SIZE.toLong(), 301L + i)
            }
        }

        archi.use {
            it.run(10L)
            val best = it.bestChampionF()[0]
            println("  Clones created: ${CloneableRastriginProblem.cloneCount.get()}")
            println("  Best fitness: %.6f".format(best))
            println("  Total evaluations: ${CloneableRastriginProblem.totalEvaluations.get()}")
        }

        println("  Key API: problems implementing IThreadCloneableProblem.clone() are safe for archipelago.")
    }
}

