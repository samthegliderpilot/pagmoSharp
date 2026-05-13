package io.github.samthegliderpilot.pagmo4j.algorithms;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class De1220Test extends AlgorithmTestBase {
    @Override public IAlgorithm createAlgorithm()      { return new de1220(25L); }
    @Override public boolean supportsUnconstrained()   { return true; }
    @Override public boolean supportsConstrained()     { return false; }
    @Override public boolean supportsSingleObjective() { return true; }
    @Override public boolean supportsMultiObjective()  { return false; }
    @Test void nameContainsDe() { try (de1220 a = new de1220(1L)) { assertFalse(a.get_name().isEmpty()); } }
}
