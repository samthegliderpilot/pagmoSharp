package io.github.samthegliderpilot.pagmo4j.problems;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class MinlpRastriginTest extends ProblemTestBase {
    @Override public IProblem createStandardProblem() { return new minlp_rastrigin(2L, 2L); }
    @Test protected void boilerPlate() {
        try (minlp_rastrigin p = new minlp_rastrigin(2L, 2L)) {
            assertFalse(p.get_name().isEmpty());
            assertEquals(2L, p.get_nix(), "last 2 variables are integer");
        }
    }
    @Override @Test protected void optimizing() {
        try (minlp_rastrigin p = new minlp_rastrigin(2L, 2L); sga algo = new sga(50L, 0.5); population pop = new population(p, 64L, 2L)) {
            algo.set_seed(2L);
            try (population evolved = algo.evolve(pop)) { assertNotNull(evolved); }
        }
    }
}
