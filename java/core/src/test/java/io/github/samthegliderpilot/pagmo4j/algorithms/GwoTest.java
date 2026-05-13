package io.github.samthegliderpilot.pagmo4j.algorithms;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class GwoTest extends AlgorithmTestBase {
    @Override public IAlgorithm createAlgorithm()      { return new gwo(25L); }
    @Override public boolean supportsUnconstrained()   { return true; }
    @Override public boolean supportsConstrained()     { return false; }
    @Override public boolean supportsSingleObjective() { return true; }
    @Override public boolean supportsMultiObjective()  { return false; }
    @Test void nameContainsGWO() { try (gwo a = new gwo(1L)) { assertTrue(a.get_name().contains("GWO") || a.get_name().contains("Grey Wolf")); } }
}
