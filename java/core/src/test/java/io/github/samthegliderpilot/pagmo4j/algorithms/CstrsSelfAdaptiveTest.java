package io.github.samthegliderpilot.pagmo4j.algorithms;
import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.testproblems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class CstrsSelfAdaptiveTest extends AlgorithmTestBase {
    @Override public IAlgorithm createAlgorithm()      { return new cstrs_self_adaptive(6L); }
    @Override public boolean supportsUnconstrained()   { return false; }
    @Override public boolean supportsConstrained()     { return true; }
    @Override public boolean supportsSingleObjective() { return true; }
    @Override public boolean supportsMultiObjective()  { return false; }
    @Test void nameAndBasicPropertiesAccessible() {
        try (cstrs_self_adaptive a = new cstrs_self_adaptive(6L)) {
            assertTrue(a.get_name().contains("CNSTR") || a.get_name().contains("self-adaptive") || a.get_name().contains("sa-"));
            a.set_seed(3L); assertEquals(3L, a.get_seed());
            a.set_verbosity(2L); assertEquals(2L, a.get_verbosity());
            assertNotNull(a.get_extra_info());
        }
    }
    @Test void evolvesConstrainedProblem() {
        try (TwoDimensionalConstrainedProblem prob = new TwoDimensionalConstrainedProblem();
             cstrs_self_adaptive algo = new cstrs_self_adaptive(6L);
             population pop = new population(prob, 64L, 2L)) {
            algo.set_seed(2L);
            try (population evolved = algo.evolve(pop)) {
                assertEquals(pop.size(), evolved.size());
            }
        }
    }
}
