package io.github.samthegliderpilot.pagmo4j.problems;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class NullProblemTest extends ProblemTestBase {
    @Override public IProblem createStandardProblem() { return new null_problem(1L); }
    @Override protected boolean supportsMidpointFitnessProbe() { return false; }
    @Test void hasNonEmptyName() {
        try (null_problem p = new null_problem(1L)) { assertFalse(p.get_name().isEmpty()); }
    }
    @Override @Test protected void optimizing() { /* null problem has no sensible optimisation test */ }
    @Override @Test protected void boilerPlate() {
        try (null_problem p = new null_problem(2L)) {
            assertEquals(2L, p.get_nobj());
        }
    }
}
