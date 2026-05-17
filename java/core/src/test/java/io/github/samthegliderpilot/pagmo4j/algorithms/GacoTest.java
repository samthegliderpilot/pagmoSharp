package io.github.samthegliderpilot.pagmo4j.algorithms;
import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.testproblems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class GacoTest extends AlgorithmTestBase {
    @Override public IAlgorithm createAlgorithm()      { return new gaco(10L, 10L); }  // ker=10 keeps pop requirement <= 48 (log test pop size)
    @Override public boolean supportsUnconstrained()   { return true; }
    @Override public boolean supportsConstrained()     { return true; }
    @Override public boolean supportsSingleObjective() { return true; }
    @Override public boolean supportsMultiObjective()  { return false; }
    @Test void nameContainsGACO() { try (gaco a = new gaco(1L)) { assertTrue(a.get_name().contains("GACO") || a.get_name().contains("Ant Colony")); } }
}
