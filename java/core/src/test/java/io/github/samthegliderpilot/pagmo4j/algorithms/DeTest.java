package io.github.samthegliderpilot.pagmo4j.algorithms;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.testproblems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

class DeTest extends AlgorithmTestBase {

    @Override public IAlgorithm createAlgorithm()     { return new de(25L); }
    @Override public boolean supportsUnconstrained()  { return true; }
    @Override public boolean supportsConstrained()    { return false; }
    @Override public boolean supportsSingleObjective(){ return true; }
    @Override public boolean supportsMultiObjective() { return false; }

    @Test
    void nameIsCorrect() {
        try (de algo = new de(10L)) {
            assertEquals("DE: Differential Evolution", algo.get_name());
        }
    }

    @Test
    void typedAndGenericLogsAreExposed() {
        try (TwoDimensionalSingleObjectiveProblem prob = new TwoDimensionalSingleObjectiveProblem();
             problem wrapped = new problem(prob);
             de algo = new de(25L);
             population pop = new population(wrapped, 64L, 2L)) {

            algo.set_seed(2L);
            algo.set_verbosity(1L);
            try (population ignored = algo.evolve(pop)) {}

            var typed = algo.getTypedLogLines();
            assertFalse(typed.isEmpty(), "verbosity should produce at least one log line");

            IAlgorithm iface = algo;
            var generic = iface.getLogLines();
            assertEquals(typed.size(), generic.size());

            var first = generic.get(0);
            assertEquals("de", first.getAlgorithmName());
            assertTrue(first.getRawFields().containsKey("generation"));
            assertTrue(first.getRawFields().containsKey("function_evaluations"));
            assertTrue(first.getRawFields().containsKey("best_fitness"));
            assertTrue(first.getRawFields().containsKey("dx"));
            assertTrue(first.toDisplayString().contains("gen="));

            assertEquals(typed.get(0).generation(), (long) first.getRawFields().get("generation"));
            assertEquals(typed.get(0).functionEvaluations(), (long) first.getRawFields().get("function_evaluations"));
        }
    }
}
