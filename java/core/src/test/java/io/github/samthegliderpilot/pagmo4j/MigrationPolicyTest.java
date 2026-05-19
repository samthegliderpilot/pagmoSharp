package io.github.samthegliderpilot.pagmo4j;

import io.github.samthegliderpilot.pagmo4j.algorithms.*;
import io.github.samthegliderpilot.pagmo4j.migration.*;
import io.github.samthegliderpilot.pagmo4j.problems.*;
import io.github.samthegliderpilot.pagmo4j.testproblems.*;
import org.junit.jupiter.api.Test;
import java.util.concurrent.atomic.AtomicBoolean;
import static org.junit.jupiter.api.Assertions.*;

class MigrationPolicyTest {

    // ── helpers ───────────────────────────────────────────────────────────────

    /** Replacement policy that records whether replace() was called. */
    private static class TrackingRPolicy extends RPolicyCallbackAdapter {
        final AtomicBoolean called = new AtomicBoolean(false);

        @Override
        public IndividualsGroup replace(
                IndividualsGroup incoming, long n_f, long n_ec, long n_ic, long n_obj,
                long pop_size, DoubleVector tol, IndividualsGroup current) {
            called.set(true);
            return incoming;
        }

        @Override public String get_name() { return "TrackingRPolicy"; }
    }

    /** Selection policy that records whether select() was called. */
    private static class TrackingSPolicy extends SPolicyCallbackAdapter {
        final AtomicBoolean called = new AtomicBoolean(false);

        @Override
        public IndividualsGroup select(
                IndividualsGroup population, long n_f, long n_ec, long n_ic, long n_obj,
                long pop_size, DoubleVector tol) {
            called.set(true);
            // Return first individual as the emigrant group.
            return population;
        }

        @Override public String get_name() { return "TrackingSPolicy"; }
    }

    // ── tests ─────────────────────────────────────────────────────────────────

    @Test
    void pushBackIslandWithCustomPoliciesDoesNotThrow() {
        TwoDimensionalSingleObjectiveProblem prob = new TwoDimensionalSingleObjectiveProblem();
        de algo = new de(10L);
        TrackingRPolicy rp = new TrackingRPolicy();
        TrackingSPolicy sp = new TrackingSPolicy();

        try (archipelago archi = new archipelago()) {
            long idx = archi.pushBackIsland(algo, prob, rp, sp, 16L, 42L);
            assertTrue(idx >= 0, "island index should be non-negative");
            assertEquals(1L, archi.size());
        }
    }

    @Test
    void customPoliciesAreInvokedDuringMigration() {
        TwoDimensionalSingleObjectiveProblem prob = new TwoDimensionalSingleObjectiveProblem();
        de algo = new de(10L);
        TrackingRPolicy rp1 = new TrackingRPolicy();
        TrackingSPolicy sp1 = new TrackingSPolicy();
        TrackingRPolicy rp2 = new TrackingRPolicy();
        TrackingSPolicy sp2 = new TrackingSPolicy();

        try (ring topo = new ring(); archipelago archi = new archipelago()) {
            archi.set_topology_ring(topo);
            archi.pushBackIsland(algo, prob, rp1, sp1, 24L, 1L);
            archi.pushBackIsland(algo, prob, rp2, sp2, 24L, 2L);

            // Evolve several rounds so that migration between the two islands occurs.
            archi.evolve(3L);
            archi.waitFor();

            // At least one island's selection policy must have been called.
            assertTrue(sp1.called.get() || sp2.called.get(),
                "select() should be called on at least one island during migration");
            // At least one island's replacement policy must have been called.
            assertTrue(rp1.called.get() || rp2.called.get(),
                "replace() should be called on at least one island during migration");
        }
    }

    @Test
    void defaultPolicyBehaviourIsPassThrough() {
        // Verify that a replacement policy returning `incoming` unmodified
        // and a selection policy returning the full population do not corrupt
        // the island or cause any exceptions.
        TwoDimensionalSingleObjectiveProblem prob = new TwoDimensionalSingleObjectiveProblem();
        de algo = new de(10L);

        RPolicyCallbackAdapter passThroughR = new RPolicyCallbackAdapter() {
            @Override
            public IndividualsGroup replace(
                    IndividualsGroup incoming, long n_f, long n_ec, long n_ic, long n_obj,
                    long pop_size, DoubleVector tol, IndividualsGroup current) {
                return incoming;
            }
        };

        SPolicyCallbackAdapter passThroughS = new SPolicyCallbackAdapter() {
            @Override
            public IndividualsGroup select(
                    IndividualsGroup population, long n_f, long n_ec, long n_ic, long n_obj,
                    long pop_size, DoubleVector tol) {
                return population;
            }
        };

        try (ring topo = new ring(); archipelago archi = new archipelago()) {
            archi.set_topology_ring(topo);
            archi.pushBackIsland(algo, prob, passThroughR, passThroughS, 16L, 7L);
            archi.pushBackIsland(algo, prob, passThroughR, passThroughS, 16L, 8L);
            archi.evolve(2L);
            archi.waitFor();
            assertEquals(2L, archi.size());
        }
    }

    @Test
    void customPoliciesWorkWithIAlgorithmPath() {
        // Verify the IAlgorithm overload (managed algorithm + custom policies).
        TwoDimensionalSingleObjectiveProblem prob = new TwoDimensionalSingleObjectiveProblem();
        IAlgorithm algo = new IAlgorithm() {
            @Override public population evolve(population pop) {
                try (de inner = new de(5L)) { return inner.evolve(pop); }
            }
            @Override public String get_name() { return "InlineDE"; }
        };
        TrackingRPolicy rp = new TrackingRPolicy();
        TrackingSPolicy sp = new TrackingSPolicy();

        try (ring topo = new ring(); archipelago archi = new archipelago()) {
            archi.set_topology_ring(topo);
            archi.pushBackIsland(algo, prob, rp, sp, 16L, 3L);
            archi.pushBackIsland(algo, prob, new TrackingRPolicy(), new TrackingSPolicy(), 16L, 4L);
            archi.evolve(2L);
            archi.waitFor();
            assertEquals(2L, archi.size());
        }
    }
}
