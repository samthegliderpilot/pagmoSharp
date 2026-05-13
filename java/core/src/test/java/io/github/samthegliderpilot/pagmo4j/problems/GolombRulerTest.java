package io.github.samthegliderpilot.pagmo4j.problems;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class GolombRulerTest extends ProblemTestBase {
    @Override public IProblem createStandardProblem() { return new golomb_ruler(3L); }
    @Test protected void boilerPlate() {
        try (golomb_ruler p = new golomb_ruler(3L)) {
            assertFalse(p.get_name().isEmpty());
            assertTrue(p.get_nix() > 0L, "Golomb ruler is an integer problem");
        }
    }
    @Override @Test protected void optimizing() {
        try (golomb_ruler p = new golomb_ruler(3L); sga algo = new sga(100L, 0.5); population pop = new population(p, 64L, 2L)) {
            algo.set_seed(2L);
            try (population evolved = algo.evolve(pop)) { assertNotNull(evolved); }
        }
    }
}
