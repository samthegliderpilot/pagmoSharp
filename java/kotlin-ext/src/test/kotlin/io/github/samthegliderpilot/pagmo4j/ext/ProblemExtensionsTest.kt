package io.github.samthegliderpilot.pagmo4j.ext

import io.github.samthegliderpilot.pagmo4j.*
import io.github.samthegliderpilot.pagmo4j.problems.*
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.Assertions.*

class ProblemExtensionsTest {

    private inner class SphereProblem : ManagedProblemBase() {
        override fun fitness(x: DoubleVector): DoubleVector {
            val sum = x.toDoubleArray().sumOf { it * it }
            return vec(sum)
        }
        override fun get_bounds() = boundsOf(doubleArrayOf(-5.0, -5.0), doubleArrayOf(5.0, 5.0))
        override fun get_thread_safety() = ThreadSafety.Basic
    }

    @Test
    fun namePropertyDelegates() {
        SphereProblem().use { p ->
            assertEquals(p.get_name(), p.name)
        }
    }

    @Test
    fun threadSafetyPropertyDelegates() {
        SphereProblem().use { p ->
            assertEquals(ThreadSafety.Basic, p.threadSafety)
        }
    }

    @Test
    fun throwIfNotThreadSafePassesForBasicSafety() {
        SphereProblem().use { p ->
            assertDoesNotThrow { p.throwIfNotThreadSafe() }
        }
    }

    @Test
    fun throwIfNotThreadSafeFailsForNoneSafety() {
        val noneSafe = object : ManagedProblemBase() {
            override fun fitness(x: DoubleVector) = vec(x.get(0))
            override fun get_bounds() = boundsOf(doubleArrayOf(0.0), doubleArrayOf(1.0))
            override fun get_thread_safety() = ThreadSafety.None
            override fun clone(): IProblem? = null
        }
        assertThrows(IllegalStateException::class.java) { noneSafe.throwIfNotThreadSafe() }
    }

    @Test
    fun throwIfNotThreadSafeDoesNotCallCloneJustToCheck() {
        var cloneCalls = 0
        val noneSafeCloneable = object : ManagedProblemBase(), IThreadCloneableProblem {
            override fun fitness(x: DoubleVector) = vec(x.get(0))
            override fun get_bounds() = boundsOf(doubleArrayOf(0.0), doubleArrayOf(1.0))
            override fun get_thread_safety() = ThreadSafety.None
            override fun clone(): IProblem {
                cloneCalls += 1
                return this
            }
        }

        assertThrows(IllegalStateException::class.java) { noneSafeCloneable.throwIfNotThreadSafe() }
        assertEquals(0, cloneCalls, "thread-safety guard should not invoke clone() as a probe")
    }

    @Test
    fun doubleArrayToDoubleVectorRoundTrips() {
        val arr = doubleArrayOf(1.0, 2.0, 3.0)
        val v = arr.toDoubleVector()
        assertArrayEquals(arr, v.toDoubleArray(), 0.0)
        v.delete()
    }

    @Test
    fun fitnessOfHelperBuildsVector() {
        val v = fitnessOf(1.0, 2.0, 3.0)
        assertEquals(3, v.size)
        assertEquals(2.0, v.get(1), 0.0)
        v.delete()
    }

    @Test
    fun boundsOfHelperBuildsCorrectPair() {
        boundsOf(doubleArrayOf(-1.0, -2.0), doubleArrayOf(1.0, 2.0)).use { b ->
            assertEquals(2, b.first.size)
            assertEquals(-1.0, b.first.get(0), 0.0)
            assertEquals( 1.0, b.second.get(0), 0.0)
        }
    }
}
