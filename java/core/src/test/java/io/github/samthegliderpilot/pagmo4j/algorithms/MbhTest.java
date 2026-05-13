package io.github.samthegliderpilot.pagmo4j.algorithms;
import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.testproblems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class MbhTest extends AlgorithmTestBase {
    @Override public IAlgorithm createAlgorithm() {
        try (de inner = new de(5L); algorithm a = new algorithm(inner)) { return new mbh(a, 5L, 0.1); }
    }
    @Override public boolean supportsUnconstrained()   { return true; }
    @Override public boolean supportsConstrained()     { return true; }
    @Override public boolean supportsSingleObjective() { return true; }
    @Override public boolean supportsMultiObjective()  { return false; }
    @Test void nameContainsMBH() {
        try (de inner = new de(1L); algorithm a = new algorithm(inner); mbh m = new mbh(a, 1L, 0.1)) {
            assertTrue(m.get_name().contains("MBH") || m.get_name().contains("Basin"));
        }
    }
    @Test void typedLogLinesExposed() {
        try (de inner = new de(5L); algorithm a = new algorithm(inner);
             TwoDimensionalSingleObjectiveProblem p = new TwoDimensionalSingleObjectiveProblem();
             mbh m = new mbh(a, 5L, 0.1); population pop = new population(p, 48L, 2L)) {
            m.set_verbosity(1L);
            try (population ignored = m.evolve(pop)) {}
            assertNotNull(m.getTypedLogLines());
        }
    }
}
