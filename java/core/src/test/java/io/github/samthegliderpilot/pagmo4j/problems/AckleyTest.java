package io.github.samthegliderpilot.pagmo4j.problems;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class AckleyTest extends ProblemTestBase {
    @Override public IProblem createStandardProblem() { return new ackley(2L); }
    @Override @Test protected void boilerPlate() {
        try (ackley p = new ackley(2L)) {
            assertEquals("Ackley Function", p.get_name());
            assertEquals(1L, p.get_nobj()); assertEquals(0L, p.get_nec()); assertEquals(0L, p.get_nix());
        }
    }
    @Test void originYieldsZeroFitness() {
        try (ackley p = new ackley(2L)) {
            DoubleVector x = new DoubleVector(); x.add(0.0); x.add(0.0);
            assertEquals(0.0, p.fitness(x).get(0), 1e-10);
        }
    }
    @Override @Test protected void optimizing() {
        try (ackley p = new ackley(2L); gaco algo = new gaco(20L); population pop = new population(p, 256L, 2L)) {
            algo.set_seed(2L);
            try (population evolved = algo.evolve(pop)) {
                assertTrue(evolved.champion_f().get(0) < 5.0);
            }
        }
    }
}
