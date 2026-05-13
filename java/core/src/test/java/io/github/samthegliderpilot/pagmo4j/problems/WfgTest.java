package io.github.samthegliderpilot.pagmo4j.problems;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class WfgTest extends ProblemTestBase {
    @Override public IProblem createStandardProblem() { return new wfg(1L, 6L, 4L, 2L); }
    @Override @Test protected void boilerPlate() {
        try (wfg p = new wfg(1L, 6L, 4L, 2L)) {
            assertFalse(p.get_name().isEmpty());
            assertEquals(2L, p.get_nobj());
        }
    }
    @Override @Test protected void optimizing() {
        try (wfg p = new wfg(1L, 6L, 4L, 2L); nsga2 a = new nsga2(10L); population pop = new population(p, 64L, 2L)) {
            a.set_seed(2L);
            try (population evolved = a.evolve(pop)) { assertEquals(64L, evolved.size()); }
        }
    }
}
