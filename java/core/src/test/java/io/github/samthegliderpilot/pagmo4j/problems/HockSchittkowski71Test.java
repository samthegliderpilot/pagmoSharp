package io.github.samthegliderpilot.pagmo4j.problems;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class HockSchittkowski71Test extends ProblemTestBase {
    @Override public IProblem createStandardProblem() { return new hock_schittkowski_71(); }
    @Override @Test protected void boilerPlate() {
        try (hock_schittkowski_71 p = new hock_schittkowski_71()) {
            assertFalse(p.get_name().isEmpty());
            assertEquals(1L, p.get_nobj());
            assertTrue(p.get_nec() + p.get_nic() > 0L, "HS71 is constrained");
        }
    }
    @Override @Test protected void optimizing() {
        // de cannot handle non-linear constraints; compass_search can.
        try (hock_schittkowski_71 p = new hock_schittkowski_71();
             compass_search algo = new compass_search(200L); population pop = new population(p, 1L, 2L)) {
            try (population evolved = algo.evolve(pop)) { assertNotNull(evolved); }
        }
    }
}
