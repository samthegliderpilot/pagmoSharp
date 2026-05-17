package io.github.samthegliderpilot.pagmo4j.algorithms;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.problems.*;
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
        // IPOPT requires gradient info. hock_schittkowski_71 provides gradients natively but
        // pagmo detects this via C++ template traits, not a virtual method. Wrap it in a
        // ManagedProblemBase that explicitly exposes has_gradient()=true and delegates gradient().
        try (hock_schittkowski_71 native_hs71 = new hock_schittkowski_71()) {
            IProblem gradProb = new ManagedProblemBase() {
                @Override public DoubleVector fitness(DoubleVector x) { return native_hs71.fitness(x); }
                @Override public PairOfDoubleVectors get_bounds() { return native_hs71.get_bounds(); }
                @Override public long get_nec() { return native_hs71.get_nec(); }
                @Override public long get_nic() { return native_hs71.get_nic(); }
                @Override public boolean has_gradient() { return true; }
                @Override public DoubleVector gradient(DoubleVector x) { return native_hs71.gradient(x); }
            };
            try (ipopt algo = new ipopt();
                 population pop = new population(gradProb, 1L, 2L)) {
                try (population evolved = algo.evolve(pop)) {
                    assertNotNull(evolved);
                }
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
