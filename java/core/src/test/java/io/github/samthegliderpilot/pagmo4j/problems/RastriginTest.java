package io.github.samthegliderpilot.pagmo4j.problems;

import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

class RastriginTest extends ProblemTestBase {

    @Override
    public IProblem createStandardProblem() { return new rastrigin(3L); }

    @Override @Test
    protected void boilerPlate() {
        try (rastrigin p = new rastrigin(3L);
             PairOfDoubleVectors b = p.get_bounds()) {
            assertEquals("Rastrigin Function", p.get_name());
            assertEquals(1L, p.get_nobj());
            assertEquals(0L, p.get_nec());
            assertEquals(0L, p.get_nic());
            assertEquals(3, b.getFirst().size());
            assertEquals(-5.12, b.getFirst().get(0), 0.0);
            assertEquals(5.12,  b.getSecond().get(0), 0.0);
        }
    }

    @Test
    void optimumIsAtOrigin() {
        try (rastrigin p = new rastrigin(2L)) {
            DoubleVector zero = new DoubleVector();
            zero.add(0.0); zero.add(0.0);
            assertEquals(0.0, p.fitness(zero).get(0), 1e-10);
        }
    }

    @Override @Test
    protected void optimizing() {
        try (rastrigin p = new rastrigin(2L);
             de algo = new de(20L);
             population pop = new population(p, 64L, 7L)) {
            algo.set_seed(7L);
            try (population evolved = algo.evolve(pop)) {
                assertTrue(evolved.champion_f().get(0) < 1.0, "expected near-optimal rastrigin");
            }
        }
    }
}
