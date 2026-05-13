package io.github.samthegliderpilot.pagmo4j.ext

import io.github.samthegliderpilot.pagmo4j.*
import io.github.samthegliderpilot.pagmo4j.algorithms.*

// ── IAlgorithm helpers ────────────────────────────────────────────────────────

/** Returns the algorithm name. */
val IAlgorithm.name: String get() = get_name()

/** Returns extra diagnostic info. */
val IAlgorithm.extraInfo: String get() = get_extra_info()

/** Returns the current verbosity level. */
var IAlgorithm.verbosity: Long
    get() = get_verbosity()
    set(value) = set_verbosity(value)

/** Returns the current seed. */
var IAlgorithm.seed: Long
    get() = get_seed()
    set(value) = set_seed(value)

// ── population helpers ────────────────────────────────────────────────────────

/** Returns the champion decision vector as a Kotlin DoubleArray. */
fun population.championX(): DoubleArray {
    val v = champion_x()
    return try { v.toDoubleArray() } finally { v.delete() }
}

/** Returns the champion fitness vector as a Kotlin DoubleArray. */
fun population.championF(): DoubleArray {
    val v = champion_f()
    return try { v.toDoubleArray() } finally { v.delete() }
}

/** Returns the number of individuals in this population (long). */
val population.populationSize: Long get() = size()

// ── Convenience evolve + collect ─────────────────────────────────────────────

/**
 * Evolves the given population for {@code generations} and returns the champion
 * decision vector. The evolved population is disposed automatically.
 *
 * ```kotlin
 * val bestX = de(100).evolveAndGetBest(initialPop, generations = 50)
 * ```
 */
fun IAlgorithm.evolveAndGetBest(pop: population, generations: Int = 1): DoubleArray {
    var current = pop
    repeat(generations) {
        val next = evolve(current)
        if (current !== pop) current.close()
        current = next
    }
    val result = current.championX()
    if (current !== pop) current.close()
    return result
}
