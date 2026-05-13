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
    @Test void nameContainsSade() { try (sade a = new sade(1L)) { assertTrue(a.get_name().contains("sDE") || a.get_name().contains("Self-Adaptive")); } }
}
