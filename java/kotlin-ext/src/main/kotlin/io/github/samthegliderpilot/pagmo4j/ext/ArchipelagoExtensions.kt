package io.github.samthegliderpilot.pagmo4j.ext

import io.github.samthegliderpilot.pagmo4j.*
import io.github.samthegliderpilot.pagmo4j.migration.*
import io.github.samthegliderpilot.pagmo4j.problems.*
import io.github.samthegliderpilot.pagmo4j.algorithms.*

// ── archipelago helpers ───────────────────────────────────────────────────────

/** Evolves all islands for the given number of rounds and blocks until complete. */
fun archipelago.run(rounds: Long = 1L) {
    evolve(rounds)
    waitFor()
}

/**
 * Returns the champion decision vector from every island, in island-index order.
 * Each element corresponds to [get_island_copy] at that index.
 */
fun archipelago.allChampions(): List<DoubleArray> {
    val out = mutableListOf<DoubleArray>()
    val n = size()
    for (i in 0L until n) {
        out += get_island_copy(i).use { it.championX() }
    }
    return out
}

/**
 * Returns the champion fitness vector that has the smallest L2-norm across all islands.
 *
 * **Note:** ranking is by L2-norm (sum of squares of all fitness components), not by
 * first objective alone.  For single-objective problems this is equivalent to the minimum
 * fitness value.  For multi-objective problems use [allChampions] and apply your own
 * ranking criterion.
 *
 * @return the best champion fitness array, or an empty array if the archipelago has no islands
 */
fun archipelago.bestChampionF(): DoubleArray {
    var best: DoubleArray? = null
    var bestNorm = Double.MAX_VALUE
    val n = size()
    for (i in 0L until n) {
        val f = get_island_copy(i).use { it.championF() }
        val norm = f.sumOf { it * it }
        if (best == null || norm < bestNorm) {
            best = f
            bestNorm = norm
        }
    }
    return best ?: doubleArrayOf()
}

// ── Topology helpers ──────────────────────────────────────────────────────────

/**
 * Sets a ring topology on this archipelago and returns it for chaining.
 * Works naturally inside [buildArchipelago]:
 * ```kotlin
 * val archi = buildArchipelago {
 *     withTopology(ring())
 *     repeat(4) { i -> pushBackIsland(de(100), prob, 64, i.toLong()) }
 * }
 * ```
 */
fun archipelago.withTopology(t: ring): archipelago = also { set_topology_ring(t) }

/** Sets a fully-connected topology on this archipelago and returns it for chaining. */
fun archipelago.withTopology(t: fully_connected): archipelago = also { set_topology_fully_connected(t) }

/** Sets a free-form topology on this archipelago and returns it for chaining. */
fun archipelago.withTopology(t: free_form): archipelago = also { set_topology_free_form(t) }

// ── Migration policy with default seed ───────────────────────────────────────
// Note: the Java member already accepts an explicit seed; this overload adds the
// convenience of omitting it (Kotlin extensions add the no-seed form).

/**
 * Adds an island with custom replacement and selection policies, using a random seed.
 * ```kotlin
 * pushBackIsland(de(100), prob, myRPolicy, mySPolicy, popSize = 64)
 * ```
 */
fun archipelago.pushBackIslandWithPolicies(
    algo: IAlgorithm,
    problem: IProblem,
    rPolicy: IRPolicy,
    sPolicy: ISPolicy,
    popSize: Long,
    seed: Long = random_device().next(),
): Long = pushBackIsland(algo, problem, rPolicy, sPolicy, popSize, seed)

// ── BFE overload with default seed ───────────────────────────────────────────

/**
 * Adds an island that uses the given batch fitness evaluator, using a random seed.
 * ```kotlin
 * pushBackIslandWithBfe(de(100), prob, default_bfe().to_bfe(), popSize = 64)
 * ```
 */
fun archipelago.pushBackIslandWithBfe(
    algo: IAlgorithm,
    problem: IProblem,
    bfe: bfe,
    popSize: Long,
    seed: Long = random_device().next(),
): Long = pushBackIsland(algo, problem, bfe, popSize, seed)

// ── DSL builder ───────────────────────────────────────────────────────────────

/**
 * Builds an archipelago from the given block.
 *
 * ```kotlin
 * val archi = buildArchipelago {
 *     withTopology(ring())
 *     repeat(8) {
 *         pushBackIsland(de(100), myProblem, popSize = 64, seed = it.toLong())
 *     }
 * }
 * archi.run(rounds = 20)
 * val best = archi.bestChampionF()
 * ```
 */
fun buildArchipelago(block: archipelago.() -> Unit): archipelago {
    val archi = archipelago()
    archi.block()
    return archi
}
