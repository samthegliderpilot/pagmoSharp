package io.github.samthegliderpilot.pagmo4j.algorithms;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class SgaTest extends AlgorithmTestBase {
    @Override public IAlgorithm createAlgorithm()      { return new sga(25L, 0.5); }
    @Override public boolean supportsUnconstrained()   { return true; }
    @Override public boolean supportsConstrained()     { return false; }
    @Override public boolean supportsSingleObjective() { return true; }
    @Override public boolean supportsMultiObjective()  { return false; }
    @Test void nameContainsSGA() { try (sga a = new sga(1L, 0.5)) { assertTrue(a.get_name().contains("SGA") || a.get_name().contains("Genetic")); } }
}
