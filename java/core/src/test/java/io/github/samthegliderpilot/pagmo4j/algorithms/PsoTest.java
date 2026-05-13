package io.github.samthegliderpilot.pagmo4j.algorithms;

import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

class PsoTest extends AlgorithmTestBase {

    @Override public IAlgorithm createAlgorithm()     { return new pso(25L); }
    @Override public boolean supportsUnconstrained()  { return true; }
    @Override public boolean supportsConstrained()    { return false; }
    @Override public boolean supportsSingleObjective(){ return true; }
    @Override public boolean supportsMultiObjective() { return false; }

    @Test
    void nameIsCorrect() {
        try (pso algo = new pso(10L)) {
            assertEquals("PSO: Particle Swarm Optimization", algo.get_name());
        }
    }

    @Test
    void typedLogLinesAreExposed() {
        try (pso algo = new pso(25L)) {
            var lines = algo.getTypedLogLines();
            assertNotNull(lines);
        }
    }
}
