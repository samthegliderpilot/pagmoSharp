package io.github.samthegliderpilot.pagmo4j.algorithms;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class SeaTest extends AlgorithmTestBase {
    @Override public IAlgorithm createAlgorithm()      { return new sea(25L); }
    @Override public boolean supportsUnconstrained()   { return true; }
    @Override public boolean supportsConstrained()     { return false; }
    @Override public boolean supportsSingleObjective() { return true; }
    @Override public boolean supportsMultiObjective()  { return false; }
    @Test void nameContainsSEA() { try (sea a = new sea(1L)) { assertTrue(a.get_name().contains("SEA") || a.get_name().contains("Evolutionary")); } }
    @Test void getLogLinesReturnsTypedEntries() {
        try (sea a = new sea(5L)) {
            a.set_verbosity(1L);
            try (population pop = new population(new rastrigin(2L), 10L, 0L)) {
                try (population evolved = a.evolve(pop)) { assertNotNull(evolved); }
            }
            java.util.List<sea.SeaLogLine> lines = a.getTypedLogLines();
            assertFalse(lines.isEmpty(), "sea log should be non-empty after evolve with verbosity>0");
            assertEquals("sea", lines.get(0).getAlgorithmName());
            assertFalse(a.getLogLines().isEmpty());
        }
    }
}
