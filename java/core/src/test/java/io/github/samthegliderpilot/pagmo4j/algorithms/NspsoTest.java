package io.github.samthegliderpilot.pagmo4j.algorithms;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class NspsoTest extends AlgorithmTestBase {
    @Override public IAlgorithm createAlgorithm()      { return new nspso(10L); }
    @Override public boolean supportsUnconstrained()   { return true; }
    @Override public boolean supportsConstrained()     { return false; }
    @Override public boolean supportsSingleObjective() { return false; }
    @Override public boolean supportsMultiObjective()  { return true; }
    @Test void nameContainsNSPSO() { try (nspso a = new nspso(1L)) { assertTrue(a.get_name().contains("NSPSO") || a.get_name().contains("Particle Swarm")); } }
}
