package io.github.samthegliderpilot.pagmo4j.problems.cec;
import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.problems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class Cec2009Test extends ProblemTestBase {
    @Override public IProblem createStandardProblem() { return new cec2009(1L, false, 10L); }
    @Override @Test protected void boilerPlate() {
        try (cec2009 p = new cec2009(1L, false, 10L)) { assertFalse(p.get_name().isEmpty()); assertTrue(p.get_nobj() >= 1L); }
    }
    @Override @Test protected void optimizing() {
        try (cec2009 p = new cec2009(1L, false, 10L); nsga2 a = new nsga2(5L); population pop = new population(p, 32L, 2L)) {
            a.set_seed(2L);
            try (population evolved = a.evolve(pop)) { assertNotNull(evolved); }
        }
    }
}
