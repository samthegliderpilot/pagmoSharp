package io.github.samthegliderpilot.pagmo4j.ext

import io.github.samthegliderpilot.pagmo4j.*
import io.github.samthegliderpilot.pagmo4j.problems.*
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.Assertions.*

class ArchipelagoExtensionsTest {

    private inner class SphereProblem : ManagedProblemBase() {
        override fun fitness(x: DoubleVector) = vec(x.get(0) * x.get(0) + x.get(1) * x.get(1))
        override fun get_bounds() = boundsOf(doubleArrayOf(-5.0, -5.0), doubleArrayOf(5.0, 5.0))
        override fun get_thread_safety() = ThreadSafety.Basic
    }

    @Test
    fun buildArchipelagoCreatesIslands() {
        val archi = buildArchipelago {
            val prob = SphereProblem()
            val algo = de(10L)
            repeat(4) { i -> pushBackIsland(algo, prob, 16L, i.toLong()) }
        }
        archi.use {
            assertEquals(4L, it.size())
        }
    }

    @Test
    fun runEvolvesAllIslands() {
        val archi = buildArchipelago {
            val prob = SphereProblem()
            val algo = de(10L)
            repeat(4) { i -> pushBackIsland(algo, prob, 16L, i.toLong()) }
        }
        archi.use {
            it.run(rounds = 1L)
            assertEquals(4L, it.size())
        }
    }

    @Test
    fun bestChampionFReturnsDoubleArray() {
        val archi = buildArchipelago {
            val prob = SphereProblem()
            val algo = de(20L)
            repeat(4) { i -> pushBackIsland(algo, prob, 32L, i.toLong()) }
        }
        archi.use {
            it.run(1L)
            val best = it.bestChampionF()
            assertEquals(1, best.size)
            assertTrue(best[0] >= 0.0)
        }
    }

    @Test
    fun allChampionsReturnsOnePerIsland() {
        val archi = buildArchipelago {
            val prob = SphereProblem()
            val algo = de(10L)
            repeat(3) { i -> pushBackIsland(algo, prob, 16L, i.toLong()) }
        }
        archi.use {
            it.run(1L)
            val champions = it.allChampions()
            assertEquals(3, champions.size)
        }
    }
}
