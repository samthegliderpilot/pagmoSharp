package io.github.samthegliderpilot.pagmo4j.algorithms;
import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.testproblems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class CmaesTest extends AlgorithmTestBase {
    @Override public IAlgorithm createAlgorithm()      { return new cmaes(50L); }
    @Override public boolean supportsUnconstrained()   { return true; }
    @Override public boolean supportsConstrained()     { return false; }
    @Override public boolean supportsSingleObjective() { return true; }
    @Override public boolean supportsMultiObjective()  { return false; }
    @Test void nameContainsCmaes() { try (cmaes a = new cmaes(1L)) { assertTrue(a.get_name().contains("CMA")); } }
    @Test void typedLogLinesExposed() {
        try (TwoDimensionalSingleObjectiveProblem p = new TwoDimensionalSingleObjectiveProblem();
             cmaes a = new cmaes(10L); population pop = new population(p, 48L, 2L)) {
            a.set_verbosity(1L);
            try (population ignored = a.evolve(pop)) {}
            assertNotNull(a.getTypedLogLines());
        }
    }
}
