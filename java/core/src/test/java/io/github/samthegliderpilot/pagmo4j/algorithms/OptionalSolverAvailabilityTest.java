package io.github.samthegliderpilot.pagmo4j.algorithms;

import io.github.samthegliderpilot.pagmo4j.OptionalSolverAvailability;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/** Smoke tests: optional solver detection never throws, always returns a definite boolean. */
class OptionalSolverAvailabilityTest {

    @Test
    void nloptAvailabilityIsDetectable() {
        assertDoesNotThrow(OptionalSolverAvailability::isNloptAvailable,
            "isNloptAvailable() must not throw");
    }

    @Test
    void ipoptAvailabilityIsDetectable() {
        assertDoesNotThrow(OptionalSolverAvailability::isIpoptAvailable,
            "isIpoptAvailable() must not throw");
    }

    @Test
    void multipleCallsReturnConsistentResult() {
        boolean first  = OptionalSolverAvailability.isNloptAvailable();
        boolean second = OptionalSolverAvailability.isNloptAvailable();
        assertEquals(first, second, "repeated calls must return the same value");
    }
}
