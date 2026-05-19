package io.github.samthegliderpilot.pagmo4j.algorithms;

import io.github.samthegliderpilot.pagmo4j.OptionalSolverAvailability;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/** Smoke tests: optional solver detection never throws, always returns a definite boolean. */
class OptionalSolverAvailabilityTest {

    @Test
    void nloptAvailabilityIsDetectable() {
        // Should never throw, always returns a definite value.
        boolean available = OptionalSolverAvailability.isNloptAvailable();
        assertTrue(available || !available, "isNloptAvailable() must return true or false without throwing");
    }

    @Test
    void ipoptAvailabilityIsDetectable() {
        boolean available = OptionalSolverAvailability.isIpoptAvailable();
        assertTrue(available || !available, "isIpoptAvailable() must return true or false without throwing");
    }

    @Test
    void multipleCallsReturnConsistentResult() {
        boolean first  = OptionalSolverAvailability.isNloptAvailable();
        boolean second = OptionalSolverAvailability.isNloptAvailable();
        assertEquals(first, second, "repeated calls must return the same value");
    }
}
