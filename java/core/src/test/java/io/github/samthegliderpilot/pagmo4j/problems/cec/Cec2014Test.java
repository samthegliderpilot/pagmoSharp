package io.github.samthegliderpilot.pagmo4j.problems.cec;
import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.problems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class Cec2014Test extends ProblemTestBase {
    // CEC2014 only supports dim ∈ {2, 10, 20, 30, 50, 100}. Use 10 (smallest practical size).
    @Override public IProblem createStandardProblem() { return new cec2014(1L, 10L); }
    @Override @Test protected void boilerPlate() {
        try (cec2014 p = new cec2014(1L, 10L)) { assertFalse(p.get_name().isEmpty()); assertEquals(1L, p.get_nobj()); }
    }
    @Override @Test protected void optimizing() {
        try (cec2014 p = new cec2014(1L, 10L); de a = new de(20L); population pop = new population(p, 32L, 2L)) {
            a.set_seed(2L);
            try (population evolved = a.evolve(pop)) { assertNotNull(evolved); }
        }
    }
}
