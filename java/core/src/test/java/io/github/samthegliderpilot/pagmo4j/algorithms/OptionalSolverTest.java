package io.github.samthegliderpilot.pagmo4j.algorithms;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.testproblems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
import static org.junit.jupiter.api.Assumptions.*;

/** Guards optional-solver tests behind availability checks. */
class OptionalSolverTest {

    @Test
    void nloptCanEvolveIfAvailable() {
        assumeTrue(OptionalSolverAvailability.isNloptAvailable(), "NLopt not available in this build.");
        try (TwoDimensionalSingleObjectiveProblem prob = new TwoDimensionalSingleObjectiveProblem();
             nlopt algo = new nlopt("cobyla");
             population pop = new population(prob, 1L, 2L)) {
            try (population evolved = algo.evolve(pop)) {
                assertNotNull(evolved);
            }
        }
    }

    @Test
    void ipoptCanEvolveIfAvailable() {
        assumeTrue(OptionalSolverAvailability.isIpoptAvailable(), "IPOPT not available in this build.");
        try (TwoDimensionalConstrainedProblem prob = new TwoDimensionalConstrainedProblem();
             ipopt algo = new ipopt();
             population pop = new population(prob, 1L, 2L)) {
            try (population evolved = algo.evolve(pop)) {
                assertNotNull(evolved);
            }
        }
    }

    @Test
    void optionalSolverAvailabilityDoesNotThrow() {
        assertDoesNotThrow(() -> {
            boolean nl = OptionalSolverAvailability.isNloptAvailable();
            boolean ip = OptionalSolverAvailability.isIpoptAvailable();
        });
    }
}
