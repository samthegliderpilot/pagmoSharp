package io.github.samthegliderpilot.pagmo4j;

import io.github.samthegliderpilot.pagmo4j.problems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/**
 * Validates that the managed problem pipeline exposes the full pagmo problem surface
 * end-to-end. Mirrors Test_de_managed_problem_pipeline.cs.
 */
class ManagedProblemPipelineTest {

    private static final class DeterministicBatchProblem extends ManagedProblemBase {
        @Override
        public String get_name()       { return "DeterministicBatchProblem"; }
        @Override
        public String get_extra_info() { return "Managed pipeline test UDP"; }

        @Override
        public PairOfDoubleVectors get_bounds() {
            return bounds(new double[]{-10.0, -10.0}, new double[]{10.0, 10.0});
        }

        @Override
        public ThreadSafety get_thread_safety() { return ThreadSafety.Constant; }

        @Override
        public DoubleVector fitness(DoubleVector x) {
            double dx = x.get(0), dy = x.get(1) - 3.0;
            return vec(dx * dx + dy * dy);
        }

        @Override public boolean has_batch_fitness() { return true; }

        @Override
        public DoubleVector batch_fitness(DoubleVector dvs) {
            DoubleVector out = new DoubleVector();
            for (int i = 0; i < (int) dvs.size(); i += 2) {
                double dx = dvs.get(i), dy = dvs.get(i + 1) - 3.0;
                out.add(dx * dx + dy * dy);
            }
            return out;
        }

        @Override public boolean has_gradient() { return true; }

        @Override
        public DoubleVector gradient(DoubleVector x) {
            return vec(2.0 * x.get(0), 2.0 * x.get(1) - 6.0);
        }

        @Override public boolean has_gradient_sparsity() { return true; }

        @Override
        public SparsityPattern gradient_sparsity() {
            return sparsity(new long[]{0, 0}, new long[]{0, 1});
        }
    }

    @Test
    void managedProblemExposesProblemSurface() {
        try (DeterministicBatchProblem managed = new DeterministicBatchProblem();
             problem prob = new problem(managed)) {

            assertTrue(prob.is_valid());
            assertEquals("DeterministicBatchProblem", prob.get_name());
            assertEquals("Managed pipeline test UDP", prob.get_extra_info());
            assertEquals(ThreadSafety.Constant, prob.get_thread_safety());

            assertEquals(1L, prob.get_nobj());
            assertEquals(2L, prob.get_nx());
            assertEquals(0L, prob.get_nix());
            assertEquals(0L, prob.get_nc());

            assertTrue(prob.has_batch_fitness());
            assertTrue(prob.has_gradient());
            assertTrue(prob.has_gradient_sparsity());
            assertFalse(prob.has_hessians());
            assertFalse(prob.has_set_seed());

            DoubleVector x = new DoubleVector();
            x.add(1.0); x.add(3.0);

            assertEquals(1.0, prob.fitness(x).get(0), 1e-12);
            assertEquals(1L, prob.get_fevals().longValue());

            DoubleVector g = prob.gradient(x);
            assertEquals(2.0, g.get(0), 1e-12);
            assertEquals(0.0, g.get(1), 1e-12);
            g.delete();

            DoubleVector batch = new DoubleVector();
            batch.add(1.0); batch.add(3.0); batch.add(2.0); batch.add(3.0);
            DoubleVector batchF = prob.batch_fitness(batch);
            assertEquals(2, batchF.size());
            assertEquals(1.0, batchF.get(0), 1e-12);
            assertEquals(4.0, batchF.get(1), 1e-12);
            batchF.delete();
        }
    }

    @Test
    void deEvolvesPopulationFromManagedProblem() {
        try (DeterministicBatchProblem managed = new DeterministicBatchProblem();
             problem prob = new problem(managed);
             de algo = new de(80L);
             population pop = new population(prob, 128L, 2L)) {

            algo.set_seed(2L);
            try (population final_ = algo.evolve(pop)) {
                assertTrue(final_.champion_f().get(0) < 0.5);
                assertTrue(Math.abs(final_.champion_x().get(0))       < 0.5);
                assertTrue(Math.abs(final_.champion_x().get(1) - 3.0) < 0.5);
            }
        }
    }

    @Test
    void managedProblemNameIsSimpleClassName() {
        try (DeterministicBatchProblem p = new DeterministicBatchProblem()) {
            assertEquals("DeterministicBatchProblem", p.get_name());
        }
    }
}
