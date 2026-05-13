package io.github.samthegliderpilot.pagmo4j.algorithms;
import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.testproblems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class SimulatedAnnealingTest extends AlgorithmTestBase {
    @Override public IAlgorithm createAlgorithm()      { return new simulated_annealing(); }
    @Override public boolean supportsUnconstrained()   { return true; }
    @Override public boolean supportsConstrained()     { return false; }
    @Override public boolean supportsSingleObjective() { return true; }
    @Override public boolean supportsMultiObjective()  { return false; }
    @Test void nameContainsSA() { try (simulated_annealing a = new simulated_annealing()) { assertTrue(a.get_name().contains("SA") || a.get_name().contains("Annealing")); } }
    @Test void typedLogLinesExposed() {
        try (TwoDimensionalSingleObjectiveProblem p = new TwoDimensionalSingleObjectiveProblem();
             simulated_annealing a = new simulated_annealing(); population pop = new population(p, 48L, 2L)) {
            a.set_verbosity(1L);
            try (population ignored = a.evolve(pop)) {}
            assertNotNull(a.getTypedLogLines());
        }
    }
}
