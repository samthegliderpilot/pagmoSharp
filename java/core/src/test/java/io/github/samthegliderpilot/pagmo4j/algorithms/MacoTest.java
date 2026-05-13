package io.github.samthegliderpilot.pagmo4j.algorithms;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class MacoTest extends AlgorithmTestBase {
    @Override public IAlgorithm createAlgorithm()      { return new maco(10L); }
    @Override public boolean supportsUnconstrained()   { return true; }
    @Override public boolean supportsConstrained()     { return false; }
    @Override public boolean supportsSingleObjective() { return false; }
    @Override public boolean supportsMultiObjective()  { return true; }
    @Test void nameContainsMACO() { try (maco a = new maco(1L)) { assertTrue(a.get_name().contains("MACO") || a.get_name().contains("Ant Colony")); } }
}
