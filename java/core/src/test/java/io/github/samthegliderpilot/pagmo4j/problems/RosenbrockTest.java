package io.github.samthegliderpilot.pagmo4j.problems;

import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

class RosenbrockTest extends ProblemTestBase {

    @Override
    public IProblem createStandardProblem() { return new rosenbrock(3L); }

    @Override @Test
    protected void boilerPlate() {
        try (rosenbrock p = new rosenbrock(3L);
             PairOfDoubleVectors b = p.get_bounds()) {
            assertEquals("Multidimensional Rosenbrock Function", p.get_name());
            assertEquals(1L, p.get_nobj());
            assertEquals(0L, p.get_nec());
            assertEquals(0L, p.get_nic());
            assertEquals(0L, p.get_nix());
            assertEquals(3, b.getFirst().size());
            for (int i = 0; i < 3; i++) {
                assertEquals(-5.0, b.getFirst().get(i),  0.0);
                assertEquals(10.0, b.getSecond().get(i), 0.0);
            }
        }
    }

    @Test
    void optimumYieldsZeroFitness() {
        try (rosenbrock p = new rosenbrock(4L)) {
            DoubleVector optimum = p.best_known();
            assertEquals(4, optimum.size());
            assertEquals(0.0, p.fitness(optimum).get(0), 1e-12);
        }
    }

    @Test
    void gradientHasExpectedSize() {
        try (rosenbrock p = new rosenbrock(4L)) {
            assertEquals(4, p.gradient(p.best_known()).size());
        }
    }

    @Override @Test
    protected void optimizing() {
        try (rosenbrock p = new rosenbrock(2L);
             de algo = new de(25L, 0.8, 0.9, 2L, 1e-6, 1e-6);
             population pop = new population(p, 24L, 99L)) {
            algo.set_seed(42L);
            try (population evolved = algo.evolve(pop)) {
                assertTrue(evolved.champion_f().get(0) <= 100.0,
                    "evolving rosenbrock should reduce champion fitness");
            }
        }
    }
}
