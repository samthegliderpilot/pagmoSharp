package io.github.samthegliderpilot.pagmo4j.problems;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class DecomposeTest extends ProblemTestBase {
    @Override public IProblem createStandardProblem() {
        DoubleVector w = new DoubleVector(); w.add(0.5); w.add(0.5);
        DoubleVector z = new DoubleVector(); z.add(0.0); z.add(0.0);
        // decompose requires a type-erased problem wrapper
        return new decompose(new problem(new zdt(1L, 5L)), w, z);
    }
    @Override protected boolean supportsMidpointFitnessProbe() { return false; }
    @Test void hasNonEmptyName() {
        try (IProblem p = createStandardProblem()) { assertFalse(p.get_name().isEmpty()); }
    }
    @Override @Test protected void optimizing() { /* decompose is a wrapper; component tested separately */ }
    @Override @Test protected void boilerPlate() {
        try (IProblem p = createStandardProblem()) {
            assertEquals(1L, p.get_nobj(), "decompose reduces to single-objective");
        }
    }
}
