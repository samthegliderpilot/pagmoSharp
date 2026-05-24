package io.github.samthegliderpilot.pagmo4j.ext

import io.github.samthegliderpilot.pagmo4j.*
import io.github.samthegliderpilot.pagmo4j.migration.*
import io.github.samthegliderpilot.pagmo4j.problems.*
import io.github.samthegliderpilot.pagmo4j.algorithms.*
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.Assertions.*

class TopologyBfeAndPolicyExtensionsTest {

    // ── helpers ───────────────────────────────────────────────────────────────

    private inner class SphereProblem : ManagedProblemBase() {
        override fun fitness(x: DoubleVector) = vec(x.get(0) * x.get(0) + x.get(1) * x.get(1))
        override fun get_bounds() = boundsOf(doubleArrayOf(-5.0, -5.0), doubleArrayOf(5.0, 5.0))
        override fun get_thread_safety() = ThreadSafety.Basic
    }

    // ── topology ─────────────────────────────────────────────────────────────

    @Test
    fun withTopologyRingConnectsIslands() {
        val archi = buildArchipelago {
            withTopology(ring())
            val prob = SphereProblem()
            val algo = de(10L)
            repeat(4) { i -> pushBackIsland(algo, prob, 16L, i.toLong()) }
        }
        archi.use {
            it.run(2L)
            assertEquals(4L, it.size())
        }
    }

    @Test
    fun withTopologyFullyConnectedConnectsIslands() {
        val archi = buildArchipelago {
            withTopology(fully_connected())
            val prob = SphereProblem()
            val algo = de(10L)
            repeat(3) { i -> pushBackIsland(algo, prob, 16L, i.toLong()) }
        }
        archi.use {
            it.run(2L)
            assertEquals(3L, it.size())
        }
    }

    @Test
    fun withTopologyFreeFormDoesNotThrow() {
        val archi = buildArchipelago {
            withTopology(free_form())
            val prob = SphereProblem()
            val algo = de(10L)
            repeat(2) { i -> pushBackIsland(algo, prob, 16L, i.toLong()) }
        }
        archi.use {
            it.run(1L)
            assertEquals(2L, it.size())
        }
    }

    @Test
    fun withTopologyIsChainable() {
        val archi = buildArchipelago {
            withTopology(ring()).let { _ ->
                pushBackIsland(de(10L), SphereProblem(), 16L, 0L)
                pushBackIsland(de(10L), SphereProblem(), 16L, 1L)
            }
        }
        archi.use { assertEquals(2L, it.size()) }
    }

    // ── migration policy with default seed ───────────────────────────────────

    @Test
    fun pushBackIslandWithPoliciesDefaultSeed() {
        val rp = object : RPolicyCallbackAdapter() {
            override fun replaceManaged(incoming: IndividualsGroup, nF: Long, nEc: Long, nIc: Long,
                                 nObj: Long, popSize: Long, tol: DoubleVector,
                                 current: IndividualsGroup) = incoming
        }
        val sp = object : SPolicyCallbackAdapter() {
            override fun selectManaged(population: IndividualsGroup, nF: Long, nEc: Long, nIc: Long,
                                nObj: Long, popSize: Long, tol: DoubleVector) = population
        }

        val archi = buildArchipelago {
            pushBackIslandWithPolicies(de(10L), SphereProblem(), rp, sp, 16L)
        }
        archi.use { assertEquals(1L, it.size()) }
    }

    @Test
    fun pushBackIslandWithPoliciesAndExplicitSeed() {
        val rp = object : RPolicyCallbackAdapter() {
            override fun replaceManaged(incoming: IndividualsGroup, nF: Long, nEc: Long, nIc: Long,
                                 nObj: Long, popSize: Long, tol: DoubleVector,
                                 current: IndividualsGroup) = incoming
        }
        val sp = object : SPolicyCallbackAdapter() {
            override fun selectManaged(population: IndividualsGroup, nF: Long, nEc: Long, nIc: Long,
                                nObj: Long, popSize: Long, tol: DoubleVector) = population
        }

        val archi = buildArchipelago {
            pushBackIslandWithPolicies(de(10L), SphereProblem(), rp, sp, 16L, seed = 42L)
        }
        archi.use { assertEquals(1L, it.size()) }
    }

    // ── BFE ──────────────────────────────────────────────────────────────────
    // bfe does not implement AutoCloseable, so manage lifecycle with delete().

    @Test
    fun islandOfWithDefaultBfe() {
        val rawBfe = default_bfe()
        val b: bfe = rawBfe.to_bfe()
        try {
            val isl = islandOf(de(10L), SphereProblem(), b, 16L, 7L)
            isl.use {
                it.run(1L)
                assertTrue(it.championX().isNotEmpty())
            }
        } finally {
            b.delete()
            rawBfe.delete()
        }
    }

    @Test
    fun islandOfWithDefaultBfeDefaultSeed() {
        val rawBfe = default_bfe()
        val b: bfe = rawBfe.to_bfe()
        try {
            islandOf(de(10L), SphereProblem(), b, 16L).use { it.run(1L) }
        } finally {
            b.delete()
            rawBfe.delete()
        }
    }

    @Test
    fun pushBackIslandWithBfe() {
        val rawBfe = default_bfe()
        val b: bfe = rawBfe.to_bfe()
        try {
            val archi = buildArchipelago {
                pushBackIslandWithBfe(de(10L), SphereProblem(), b, 16L, 3L)
                pushBackIslandWithBfe(de(10L), SphereProblem(), b, 16L, 4L)
            }
            archi.use {
                it.run(1L)
                assertEquals(2L, it.size())
            }
        } finally {
            b.delete()
            rawBfe.delete()
        }
    }

    @Test
    fun pushBackIslandWithBfeDefaultSeed() {
        val rawBfe = default_bfe()
        val b: bfe = rawBfe.to_bfe()
        try {
            val archi = buildArchipelago {
                pushBackIslandWithBfe(de(10L), SphereProblem(), b, 16L)
            }
            archi.use { assertEquals(1L, it.size()) }
        } finally {
            b.delete()
            rawBfe.delete()
        }
    }

    @Test
    fun buildArchipelagoWithTopologyAndBfe() {
        val rawBfe = default_bfe()
        val b: bfe = rawBfe.to_bfe()
        try {
            val archi = buildArchipelago {
                withTopology(ring())
                repeat(3) { i ->
                    pushBackIslandWithBfe(de(10L), SphereProblem(), b, 16L, i.toLong())
                }
            }
            archi.use {
                it.run(2L)
                assertEquals(3L, it.size())
            }
        } finally {
            b.delete()
            rawBfe.delete()
        }
    }
}
