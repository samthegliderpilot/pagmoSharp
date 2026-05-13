package io.github.samthegliderpilot.pagmo4j.problems;

import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
import static org.junit.jupiter.api.Assumptions.*;

/**
 * Shared abstract test harness for wrapped pagmo problem types.
 * Enforces common wrapper invariants; concrete subclasses only provide a factory.
 */
public abstract class ProblemTestBase {

    /** Creates a standard instance of the problem under test. */
    public abstract IProblem createStandardProblem();

    /** Problem-specific boilerplate checks (name, bounds, objective counts, thread safety). */
    @Test protected abstract void boilerPlate();

    /** Verify evolving with a suitable algorithm produces a non-worse champion. */
    @Test protected abstract void optimizing();

    /** Concrete tests may override to provide regression scenarios. */
    protected boolean supportsMidpointFitnessProbe() { return true; }

    @Test
    void nameIsNonEmpty() {
        try (IProblem p = createStandardProblem()) {
            assertFalse(p.get_name().isEmpty(), "problem name must not be empty");
        }
    }

    @Test
    void nObjIsAtLeastOne() {
        try (IProblem p = createStandardProblem()) {
            assertTrue(p.get_nobj() >= 1L, "nobj must be >= 1");
        }
    }

    @Test
    void boundsAreWellFormed() {
        try (IProblem p = createStandardProblem();
             PairOfDoubleVectors b = p.get_bounds()) {
            int n = b.getFirst().size();
            assertEquals(n, b.getSecond().size(), "lower/upper bound lengths must match");
            assertTrue(n > 0, "problem dimension must be > 0");
            for (int i = 0; i < n; i++) {
                assertTrue(b.getFirst().get(i) <= b.getSecond().get(i),
                    "lower bound > upper bound at index " + i);
            }
        }
    }

    @Test
    void fitnessVectorSizeMatchesDeclaredCounts() {
        assumeTrue(supportsMidpointFitnessProbe(),
            "Midpoint fitness probe disabled for this problem.");

        try (IProblem p = createStandardProblem();
             PairOfDoubleVectors b = p.get_bounds()) {
            int dim = b.getFirst().size();
            DoubleVector midpoint = new DoubleVector();
            for (int i = 0; i < dim; i++) {
                midpoint.add(0.5 * (b.getFirst().get(i) + b.getSecond().get(i)));
            }
            DoubleVector f = p.fitness(midpoint);
            try {
                long expected = p.get_nobj() + p.get_nec() + p.get_nic();
                assertEquals(expected, (long) f.size(),
                    "fitness output length should equal nobj + nec + nic");
            } finally { f.delete(); }
        }
    }

    @Test
    void threadSafetyIsNotNull() {
        try (IProblem p = createStandardProblem()) {
            assertNotNull(p.get_thread_safety());
        }
    }
}
