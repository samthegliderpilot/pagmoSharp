package io.github.samthegliderpilot.pagmo4j.problems.cec;
import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.problems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class Cec2006Test extends ProblemTestBase {
    @Override public IProblem createStandardProblem() { return new cec2006(1L); }
    @Override @Test protected void boilerPlate() {
        try (cec2006 p = new cec2006(1L)) { assertFalse(p.get_name().isEmpty()); assertEquals(1L, p.get_nobj()); }
    }
    @Override @Test protected void optimizing() {
        try (cec2006 p = new cec2006(1L); gaco a = new gaco(5L); population pop = new population(p, 64L, 2L)) {
            a.set_seed(2L);
            try (population evolved = a.evolve(pop)) { assertNotNull(evolved); }
        }
    }
}
