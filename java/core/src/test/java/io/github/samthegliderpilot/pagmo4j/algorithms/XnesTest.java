package io.github.samthegliderpilot.pagmo4j.algorithms;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class XnesTest extends AlgorithmTestBase {
    @Override public IAlgorithm createAlgorithm()      { return new xnes(25L); }
    @Override public boolean supportsUnconstrained()   { return true; }
    @Override public boolean supportsConstrained()     { return false; }
    @Override public boolean supportsSingleObjective() { return true; }
    @Override public boolean supportsMultiObjective()  { return false; }
    @Test void nameContainsNES() { try (xnes a = new xnes(1L)) { assertTrue(a.get_name().contains("NES") || a.get_name().contains("xNES")); } }
}
