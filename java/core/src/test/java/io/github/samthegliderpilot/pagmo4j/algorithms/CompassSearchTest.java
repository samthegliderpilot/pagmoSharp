package io.github.samthegliderpilot.pagmo4j.algorithms;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class CompassSearchTest extends AlgorithmTestBase {
    @Override public IAlgorithm createAlgorithm()      { return new compass_search(200L); }
    @Override public boolean supportsUnconstrained()   { return true; }
    @Override public boolean supportsConstrained()     { return false; }
    @Override public boolean supportsSingleObjective() { return true; }
    @Override public boolean supportsMultiObjective()  { return false; }
    @Override public boolean expectEvolutionIncreasesFevals() { return true; }
    @Test void nameContainsCS() { try (compass_search a = new compass_search(10L)) { assertTrue(a.get_name().contains("CS") || a.get_name().contains("Compass")); } }
}
