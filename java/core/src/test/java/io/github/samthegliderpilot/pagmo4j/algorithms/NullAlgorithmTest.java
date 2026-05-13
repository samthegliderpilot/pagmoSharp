package io.github.samthegliderpilot.pagmo4j.algorithms;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class NullAlgorithmTest extends AlgorithmTestBase {
    @Override public IAlgorithm createAlgorithm()      { return new null_algorithm(); }
    @Override public boolean supportsUnconstrained()   { return true; }
    @Override public boolean supportsConstrained()     { return false; }
    @Override public boolean supportsSingleObjective() { return true; }
    @Override public boolean supportsMultiObjective()  { return false; }
    @Override public boolean expectEvolutionIncreasesFevals() { return false; }
    @Test void nameContainsNull() { try (null_algorithm a = new null_algorithm()) { assertTrue(a.get_name().toLowerCase().contains("null")); } }
}
