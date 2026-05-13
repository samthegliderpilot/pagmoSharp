package io.github.samthegliderpilot.pagmo4j.problems;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class DtlzTest extends ProblemTestBase {
    @Override public IProblem createStandardProblem() { return new dtlz(1L, 7L, 3L); }
    @Test protected void boilerPlate() {
        try (dtlz p = new dtlz(1L, 7L, 3L)) {
            assertFalse(p.get_name().isEmpty());
            assertEquals(3L, p.get_nobj());
        }
    }
    @Override @Test protected void optimizing() {
        try (dtlz p = new dtlz(1L, 5L, 2L); nsga2 algo = new nsga2(10L); population pop = new population(p, 64L, 2L)) {
            algo.set_seed(2L);
            try (population evolved = algo.evolve(pop)) { assertEquals(64L, evolved.size()); }
        }
    }
}
