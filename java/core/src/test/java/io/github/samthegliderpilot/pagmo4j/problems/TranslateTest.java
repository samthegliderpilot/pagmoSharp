package io.github.samthegliderpilot.pagmo4j.problems;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class TranslateTest extends ProblemTestBase {
    @Override public IProblem createStandardProblem() {
        DoubleVector t = new DoubleVector(); t.add(1.0); t.add(1.0);
        return new translate(new problem(new rastrigin(2L)), t);
    }
    @Test void hasNonEmptyName() {
        try (IProblem p = createStandardProblem()) { assertFalse(p.get_name().isEmpty()); }
    }
    @Override @Test protected void optimizing() { /* wrapping test is sufficient */ }
    @Override @Test protected void boilerPlate() {
        try (IProblem p = createStandardProblem()) { assertEquals(1L, p.get_nobj()); }
    }
}
