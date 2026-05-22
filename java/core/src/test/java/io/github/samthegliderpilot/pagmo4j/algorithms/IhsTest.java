package io.github.samthegliderpilot.pagmo4j.algorithms;
import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.testproblems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class IhsTest extends AlgorithmTestBase {
    @Override public IAlgorithm createAlgorithm()      { return new ihs(100L); }
    @Override public boolean supportsUnconstrained()   { return true; }
    @Override public boolean supportsConstrained()     { return false; }
    @Override public boolean supportsSingleObjective() { return true; }
    @Override public boolean supportsMultiObjective()  { return false; }
    @Test void nameContainsIHS() { try (ihs a = new ihs(1L)) { assertTrue(a.get_name().contains("IHS") || a.get_name().contains("Harmony")); } }
    @Test void typedLogLinesExposed() {
        try (TwoDimensionalSingleObjectiveProblem p = new TwoDimensionalSingleObjectiveProblem();
             ihs a = new ihs(100L); population pop = new population(p, 48L, 2L)) {
            a.set_verbosity(1L);
            try (population ignored = a.evolve(pop)) {}
            var lines = a.getTypedLogLines();
            assertFalse(lines.isEmpty(), "no log lines produced with verbosity=1");
            for (var line : lines) {
                assertFalse(line.getRawFields().isEmpty(), "log entry has no fields");
                assertFalse(line.toDisplayString().isEmpty(), "toDisplayString() returned empty");
            }
        }
    }
}
