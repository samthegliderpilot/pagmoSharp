package io.github.samthegliderpilot.pagmo4j.algorithms;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class PsoGenTest extends AlgorithmTestBase {
    @Override public IAlgorithm createAlgorithm()      { return new pso_gen(25L); }
    @Override public boolean supportsUnconstrained()   { return true; }
    @Override public boolean supportsConstrained()     { return false; }
    @Override public boolean supportsSingleObjective() { return true; }
    @Override public boolean supportsMultiObjective()  { return false; }
    @Test void nameContainsPSO() { try (pso_gen a = new pso_gen(1L)) { assertTrue(a.get_name().contains("PSO")); } }
}
