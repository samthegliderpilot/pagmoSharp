package io.github.samthegliderpilot.pagmo4j.problems;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class SchwefelTest extends ProblemTestBase {
    @Override public IProblem createStandardProblem() { return new schwefel(2L); }
    @Test protected void boilerPlate() {
        try (schwefel p = new schwefel(2L)) {
            assertFalse(p.get_name().isEmpty());
            assertEquals(1L, p.get_nobj());
        }
    }
    @Override @Test protected void optimizing() {
        try (schwefel p = new schwefel(2L); de algo = new de(100L); population pop = new population(p, 64L, 2L)) {
            algo.set_seed(2L);
            try (population evolved = algo.evolve(pop)) { assertNotNull(evolved); }
        }
    }
}
