package io.github.samthegliderpilot.pagmo4j.algorithms;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class Nsga2Test extends AlgorithmTestBase {
    @Override public IAlgorithm createAlgorithm()      { return new nsga2(8L); }
    @Override public boolean supportsUnconstrained()   { return true; }
    @Override public boolean supportsConstrained()     { return false; }
    @Override public boolean supportsSingleObjective() { return false; }
    @Override public boolean supportsMultiObjective()  { return true; }
    @Test void nameStartsWithNSGA() { try (nsga2 a = new nsga2(4L)) { assertTrue(a.get_name().contains("NSGA")); } }
}
