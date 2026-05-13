package io.github.samthegliderpilot.pagmo4j.problems;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class ZdtTest extends ProblemTestBase {
    @Override public IProblem createStandardProblem() { return new zdt(1L, 5L); }
    @Test protected void boilerPlate() {
        try (zdt p = new zdt(1L, 5L)) {
            assertFalse(p.get_name().isEmpty());
            assertEquals(2L, p.get_nobj());
        }
    }
    @Override @Test protected void optimizing() {
        try (zdt p = new zdt(1L, 5L); nsga2 algo = new nsga2(10L); population pop = new population(p, 64L, 2L)) {
            algo.set_seed(2L);
            try (population evolved = algo.evolve(pop)) { assertEquals(64L, evolved.size()); }
        }
    }
}
