package io.github.samthegliderpilot.pagmo4j.algorithms;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class SadeTest extends AlgorithmTestBase {
    @Override public IAlgorithm createAlgorithm()      { return new sade(25L); }
    @Override public boolean supportsUnconstrained()   { return true; }
    @Override public boolean supportsConstrained()     { return false; }
    @Override public boolean supportsSingleObjective() { return true; }
    @Override public boolean supportsMultiObjective()  { return false; }
    @Test void nameContainsSade() { try (sade a = new sade(1L)) { assertTrue(a.get_name().toLowerCase().contains("sade") || a.get_name().toLowerCase().contains("self-adaptive") || a.get_name().contains("saDe")); } }
    @Test void getLogLinesReturnsTypedEntries() {
        try (sade a = new sade(5L)) {
            a.set_verbosity(1L);
            try (population pop = new population(new rastrigin(2L), 10L, 0L)) {
                try (population evolved = a.evolve(pop)) { assertNotNull(evolved); }
            }
            java.util.List<sade.SadeLogLine> lines = a.getTypedLogLines();
            assertFalse(lines.isEmpty(), "sade log should be non-empty after evolve with verbosity>0");
            sade.SadeLogLine first = lines.get(0);
            assertEquals("sade", first.getAlgorithmName());
            assertTrue(first.generation() >= 1);
            assertFalse(a.getLogLines().isEmpty());
        }
    }
}
