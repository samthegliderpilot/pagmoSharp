package io.github.samthegliderpilot.pagmo4j.algorithms;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class BeeColonyTest extends AlgorithmTestBase {
    @Override public IAlgorithm createAlgorithm()      { return new bee_colony(25L); }
    @Override public boolean supportsUnconstrained()   { return true; }
    @Override public boolean supportsConstrained()     { return false; }
    @Override public boolean supportsSingleObjective() { return true; }
    @Override public boolean supportsMultiObjective()  { return false; }
    @Test void nameIsCorrect() { try (bee_colony a = new bee_colony(1L)) { assertTrue(a.get_name().contains("Bee Colony") || a.get_name().contains("ABC")); } }
}
