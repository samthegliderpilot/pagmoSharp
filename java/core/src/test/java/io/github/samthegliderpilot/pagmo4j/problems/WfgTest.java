package io.github.samthegliderpilot.pagmo4j.problems;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class WfgTest extends ProblemTestBase {
    // pagmo requires dim_k mod(dim_obj-1) == 0. With dim_obj=3, dim_k=4: 4 mod 2 == 0. ✓
    @Override public IProblem createStandardProblem() { return new wfg(1L, 5L, 3L, 4L); }
    @Override @Test protected void boilerPlate() {
        try (wfg p = new wfg(1L, 5L, 3L, 4L)) {
            assertFalse(p.get_name().isEmpty());
            assertEquals(3L, p.get_nobj());
        }
    }
    @Override @Test protected void optimizing() {
        try (wfg p = new wfg(1L, 5L, 3L, 4L); nsga2 a = new nsga2(10L); population pop = new population(p, 64L, 2L)) {
            a.set_seed(2L);
            try (population evolved = a.evolve(pop)) { assertEquals(64L, evolved.size()); }
        }
    }
}
