package io.github.samthegliderpilot.pagmo4j.problems;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class GriewankTest extends ProblemTestBase {
    @Override public IProblem createStandardProblem() { return new griewank(2L); }
    @Override @Test protected void boilerPlate() {
        try (griewank p = new griewank(2L)) {
            assertFalse(p.get_name().isEmpty());
            assertEquals(1L, p.get_nobj());
        }
    }
    @Override @Test protected void optimizing() {
        try (griewank p = new griewank(2L); de algo = new de(50L); population pop = new population(p, 64L, 2L)) {
            algo.set_seed(2L);
            try (population evolved = algo.evolve(pop)) {
                assertTrue(evolved.champion_f().get(0) < 5.0);
            }
        }
    }
}
