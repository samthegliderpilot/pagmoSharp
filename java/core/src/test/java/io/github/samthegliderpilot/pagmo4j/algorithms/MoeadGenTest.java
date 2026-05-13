package io.github.samthegliderpilot.pagmo4j.algorithms;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class MoeadGenTest extends AlgorithmTestBase {
    @Override public IAlgorithm createAlgorithm()      { return new moead_gen(10L); }
    @Override public boolean supportsUnconstrained()   { return true; }
    @Override public boolean supportsConstrained()     { return false; }
    @Override public boolean supportsSingleObjective() { return false; }
    @Override public boolean supportsMultiObjective()  { return true; }
    @Test void hasNonEmptyName() { try (moead_gen a = new moead_gen(1L)) { assertFalse(a.get_name().isEmpty()); } }
}
