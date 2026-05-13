package io.github.samthegliderpilot.pagmo4j.problems.cec;
import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.problems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class Cec2013Test extends ProblemTestBase {
    @Override public IProblem createStandardProblem() { return new cec2013(1L, 5L); }
    @Override @Test protected void boilerPlate() {
        try (cec2013 p = new cec2013(1L, 5L)) { assertFalse(p.get_name().isEmpty()); assertEquals(1L, p.get_nobj()); }
    }
    @Override @Test protected void optimizing() {
        try (cec2013 p = new cec2013(1L, 5L); de a = new de(20L); population pop = new population(p, 32L, 2L)) {
            a.set_seed(2L);
            try (population evolved = a.evolve(pop)) { assertNotNull(evolved); }
        }
    }
}
