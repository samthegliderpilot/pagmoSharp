package io.github.samthegliderpilot.pagmo4j.ext

import io.github.samthegliderpilot.pagmo4j.*
import io.github.samthegliderpilot.pagmo4j.problems.*
import io.github.samthegliderpilot.pagmo4j.algorithms.*

// ── archipelago helpers ───────────────────────────────────────────────────────

/** Evolves all islands for the given number of rounds and blocks until complete. */
fun archipelago.run(rounds: Long = 1L) {
    evolve(rounds)
    waitFor()
}

/** Returns the champion decision vectors from all islands. */
fun archipelago.allChampions(): List<DoubleArray> {
    val out = mutableListOf<DoubleArray>()
    val n = size()
    for (i in 0L until n) {
        out += get_island_copy(i).use { it.championX() }
    }
    return out
}

/** Returns the best champion fitness found across all islands. */
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

// ── DSL builder ───────────────────────────────────────────────────────────────

/**
 * Builds an archipelago from the given block.
 *
 * ```kotlin
 * val archi = buildArchipelago {
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
