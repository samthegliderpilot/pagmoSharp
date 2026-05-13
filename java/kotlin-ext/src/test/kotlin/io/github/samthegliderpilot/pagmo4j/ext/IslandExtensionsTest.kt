package io.github.samthegliderpilot.pagmo4j.ext

import io.github.samthegliderpilot.pagmo4j.*
import io.github.samthegliderpilot.pagmo4j.problems.*
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.Assertions.*

class IslandExtensionsTest {

    private inner class QuadraticProblem : ManagedProblemBase() {
        override fun fitness(x: DoubleVector) = vec(x.get(0) * x.get(0) + (x.get(1) - 3.0).let { it * it })
        override fun get_bounds() = boundsOf(doubleArrayOf(-10.0, -10.0), doubleArrayOf(10.0, 10.0))
        override fun get_thread_safety() = ThreadSafety.Basic
    }

    @Test
    fun islandOfCreatesIsland() {
        QuadraticProblem().use { prob ->
            de(20L).use { algo ->
                islandOf(algo, prob, 32L, 42L).use { isl ->
                    assertNotNull(isl)
                    assertTrue(isl.is_valid())
                }
            }
        }
    }

    @Test
    fun runEvolvesThenWaits() {
        QuadraticProblem().use { prob ->
            de(20L).use { algo ->
                islandOf(algo, prob, 32L, 42L).use { isl ->
                    isl.run(rounds = 1L)
                    assertEquals(EvolveStatus.Idle, isl.status())
                }
            }
        }
    }

    @Test
    fun championFReturnsDoubleArray() {
        QuadraticProblem().use { prob ->
            de(20L).use { algo ->
                islandOf(algo, prob, 32L, 42L).use { isl ->
                    isl.run(1L)
                    val f = isl.championF()
                    assertEquals(1, f.size)
                    assertTrue(f[0] >= 0.0)
                }
            }
        }
    }
}
