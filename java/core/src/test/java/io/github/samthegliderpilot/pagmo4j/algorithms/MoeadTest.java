package io.github.samthegliderpilot.pagmo4j.algorithms;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class MoeadTest extends AlgorithmTestBase {
    @Override public IAlgorithm createAlgorithm()      { return new moead(10L); }
    @Override public boolean supportsUnconstrained()   { return true; }
    @Override public boolean supportsConstrained()     { return false; }
    @Override public boolean supportsSingleObjective() { return false; }
    @Override public boolean supportsMultiObjective()  { return true; }
    @Test void nameContainsMoead() { try (moead a = new moead(1L)) { assertTrue(a.get_name().contains("MOEA") || a.get_name().contains("Decomposition")); } }
}
