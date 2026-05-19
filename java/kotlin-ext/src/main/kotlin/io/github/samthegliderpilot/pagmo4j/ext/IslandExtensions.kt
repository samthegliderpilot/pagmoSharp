package io.github.samthegliderpilot.pagmo4j.ext

import io.github.samthegliderpilot.pagmo4j.*
import io.github.samthegliderpilot.pagmo4j.problems.*
import io.github.samthegliderpilot.pagmo4j.algorithms.*

// ── island helpers ────────────────────────────────────────────────────────────

/** Evolves the island for the given number of rounds and blocks until complete. */
fun island.run(rounds: Long = 1L) {
    evolve(rounds)
    wait_check()
}

/** Returns the champion decision vector from the island's population. */
fun island.championX(): DoubleArray = get_population().use { it.championX() }

/** Returns the champion fitness from the island's population. */
fun island.championF(): DoubleArray = get_population().use { it.championF() }

// ── DSL-style island creation ─────────────────────────────────────────────────

/**
 * Creates an island and immediately evolves it for the given rounds.
 *
 * ```kotlin
 * val best = islandOf(de(100), problem, popSize = 64)
 *     .also { it.run(rounds = 10) }
 *     .championX()
 * ```
 */
fun islandOf(
    algo: IAlgorithm,
    problem: IProblem,
    popSize: Long,
    seed: Long = random_device().next(),
): island = island.create(algo, problem, popSize, seed)

/**
 * Creates an island that uses the given batch fitness evaluator.
 * ```kotlin
 * val isl = islandOf(de(100), prob, default_bfe().to_bfe(), popSize = 64)
 * ```
 */
fun islandOf(
    algo: IAlgorithm,
    problem: IProblem,
    bfe: bfe,
    popSize: Long,
    seed: Long = random_device().next(),
): island = island.create(algo, problem, bfe, popSize, seed)
