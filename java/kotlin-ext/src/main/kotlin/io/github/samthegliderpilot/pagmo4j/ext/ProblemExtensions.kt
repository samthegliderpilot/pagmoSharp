package io.github.samthegliderpilot.pagmo4j.ext

import io.github.samthegliderpilot.pagmo4j.*
import io.github.samthegliderpilot.pagmo4j.batchevaluators.BfeBridge
import io.github.samthegliderpilot.pagmo4j.problems.*

// ── IProblem helpers ──────────────────────────────────────────────────────────

/** Returns the problem name. Kotlin-idiomatic alias for {@code get_name()}. */
val IProblem.name: String get() = get_name()

/** Returns thread safety level. */
val IProblem.threadSafety: ThreadSafety get() = get_thread_safety()

/** Returns extra diagnostic info. */
val IProblem.extraInfo: String get() = get_extra_info()

/** Returns number of objectives. */
val IProblem.nobj: Long get() = get_nobj()

/** Returns number of equality constraints. */
val IProblem.nec: Long get() = get_nec()

/** Returns number of inequality constraints. */
val IProblem.nic: Long get() = get_nic()

/** Returns number of integer decision variables. */
val IProblem.nix: Long get() = get_nix()

/**
 * Evaluates a batch of decision vectors using the BFE bridge.
 *
 * @param batchX concatenated decision vectors (length = nDecisionVars × nPoints)
 * @param parallel when true, requires thread-safe or cloneable problem
 */
fun IProblem.batchFitness(batchX: DoubleVector, parallel: Boolean = true): DoubleVector =
    BfeBridge.batchEvaluate(this, batchX, parallel)

/**
 * Throws {@link IllegalStateException} if this problem declares
 * {@link ThreadSafety#None} and has no clone support.
 */
fun IProblem.throwIfNotThreadSafe() {
    if (get_thread_safety() == ThreadSafety.None) {
        val cloneable = this as? IThreadCloneableProblem
        if (cloneable?.clone() == null) {
            throw IllegalStateException(
                "'${get_name()}' declares ThreadSafety.None and does not implement " +
                "clone(). Declare ThreadSafety.Basic or override clone() in " +
                "ManagedProblemBase to use this problem on threaded execution paths."
            )
        }
    }
}

// ── DoubleVector helpers ──────────────────────────────────────────────────────

/** Converts a Kotlin {@code DoubleArray} to a {@link DoubleVector}. */
fun DoubleArray.toDoubleVector(): DoubleVector {
    val v = DoubleVector(this.size, 0.0)
    for (i in this.indices) v.set(i, this[i])
    return v
}

/** Converts a Kotlin {@code List<Double>} to a {@link DoubleVector}. */
fun List<Double>.toDoubleVector(): DoubleVector {
    val v = DoubleVector(this.size, 0.0)
    for (i in this.indices) v.set(i, this[i])
    return v
}

/** Converts a {@link DoubleVector} to a Kotlin {@code DoubleArray}. */
fun DoubleVector.toDoubleArray(): DoubleArray {
    val arr = DoubleArray(size)
    for (i in arr.indices) arr[i] = get(i)
    return arr
}

/** Indexed get access — lets Kotlin use {@code vec[i]} syntax. */
operator fun DoubleVector.get(index: Int): Double = get(index)

/** Indexed set access. */
operator fun DoubleVector.set(index: Int, value: Double) = set(index, value)

// ── Compact problem construction helpers ──────────────────────────────────────

/** Compact fitness vector construction: {@code fitnessOf(1.0, 2.0, 3.0)}. */
fun fitnessOf(vararg values: Double): DoubleVector = values.toDoubleVector()

/** Compact decision vector construction. */
fun decisionVec(vararg values: Double): DoubleVector = values.toDoubleVector()

/** Compact bounds construction from parallel arrays. */
fun boundsOf(lower: DoubleArray, upper: DoubleArray): PairOfDoubleVectors =
    PairOfDoubleVectors(lower.toDoubleVector(), upper.toDoubleVector())

/** Compact bounds construction from parallel lists. */
fun boundsOf(lower: List<Double>, upper: List<Double>): PairOfDoubleVectors =
    PairOfDoubleVectors(lower.toDoubleVector(), upper.toDoubleVector())
