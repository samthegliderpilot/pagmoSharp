package io.github.samthegliderpilot.pagmo4j.problems;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class UnconstrainTest extends ProblemTestBase {
    @Override public IProblem createStandardProblem() {
        return new unconstrain(new problem(new hock_schittkowski_71()), "kuri", new DoubleVector());
    }
    @Override @Test protected void boilerPlate() {
        try (unconstrain p = new unconstrain(new problem(new hock_schittkowski_71()), "kuri", new DoubleVector())) {
            assertFalse(p.get_name().isEmpty());
            assertEquals(1L, p.get_nobj());
            assertEquals(0L, p.get_nec() + p.get_nic(), "unconstrain removes constraints");
        }
    }
    @Override @Test protected void optimizing() {
        try (unconstrain p = new unconstrain(new problem(new hock_schittkowski_71()), "kuri", new DoubleVector());
             de a = new de(20L); population pop = new population(p, 64L, 2L)) {
            a.set_seed(2L);
            double initialBest = pop.champion_f().get(0);
            try (population evolved = a.evolve(pop)) {
                assertEquals(pop.size(), evolved.size(), "evolved population must preserve individual count");
                double evolvedBest = evolved.champion_f().get(0);
                assertTrue(evolvedBest <= initialBest,
                    "DE must not worsen the champion fitness (initial=" + initialBest + ", evolved=" + evolvedBest + ")");
            }
        }
    }
}
