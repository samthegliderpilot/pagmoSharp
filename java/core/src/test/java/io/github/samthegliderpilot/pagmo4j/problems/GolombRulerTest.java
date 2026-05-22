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
        // golomb_ruler has constraints; compass_search handles constrained single-objective.
        try (golomb_ruler p = new golomb_ruler(3L); compass_search algo = new compass_search(100L); population pop = new population(p, 1L, 2L)) {
            try (population evolved = algo.evolve(pop)) {
                assertEquals(pop.size(), evolved.size(), "evolved population must preserve individual count");
                assertNotNull(evolved.champion_f(), "champion fitness must be non-null after evolve");
            }
        }
    }
}
