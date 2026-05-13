package io.github.samthegliderpilot.pagmo4j.problems;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class LennardJonesTest extends ProblemTestBase {
    @Override public IProblem createStandardProblem() { return new lennard_jones(3L); }
    @Test protected void boilerPlate() {
        try (lennard_jones p = new lennard_jones(3L)) {
            assertFalse(p.get_name().isEmpty());
            assertEquals(1L, p.get_nobj());
        }
    }
    @Override @Test protected void optimizing() {
        try (lennard_jones p = new lennard_jones(3L); de algo = new de(20L); population pop = new population(p, 32L, 2L)) {
            algo.set_seed(2L);
            try (population evolved = algo.evolve(pop)) { assertNotNull(evolved); }
        }
    }
}
