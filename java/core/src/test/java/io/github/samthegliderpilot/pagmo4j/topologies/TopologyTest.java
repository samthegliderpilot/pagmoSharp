package io.github.samthegliderpilot.pagmo4j.topologies;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.testproblems.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/** Smoke tests for the three built-in topologies. */
class TopologyTest {

    @Test
    void ringTopologyEvolvesWithoutError() {
        TwoDimensionalSingleObjectiveProblem prob = new TwoDimensionalSingleObjectiveProblem();
        de algo = new de(10L);
        try (ring topo = new ring(); archipelago archi = new archipelago()) {
            archi.set_topology_ring(topo);
            for (int i = 0; i < 4; i++) archi.pushBackIsland(algo, prob, 16L, (long) i);
            archi.evolve(1L);
            archi.waitFor();
            assertEquals(4L, archi.size());
        }
    }

    @Test
    void fullyConnectedTopologyEvolvesWithoutError() {
        TwoDimensionalSingleObjectiveProblem prob = new TwoDimensionalSingleObjectiveProblem();
        de algo = new de(10L);
        try (fully_connected topo = new fully_connected(); archipelago archi = new archipelago()) {
            archi.set_topology_fully_connected(topo);
            for (int i = 0; i < 4; i++) archi.pushBackIsland(algo, prob, 16L, (long) i);
            archi.evolve(1L);
            archi.waitFor();
            assertEquals(4L, archi.size());
        }
    }

    @Test
    void freeFormTopologyConstructsCorrectly() {
        free_form topo = new free_form();
        assertNotNull(topo.get_name());
        topo.delete();
    }

    // ── Connection-data tests ─────────────────────────────────────────────────

    @Test
    void ringTopologyHasTwoNeighborsPerIsland() {
        // A ring with N islands: every island has exactly 2 neighbors
        // (the predecessor and the successor in the ring).
        final int N = 5;
        try (ring topo = new ring()) {
            for (int i = 0; i < N; i++) topo.push_back();
            for (int i = 0; i < N; i++) {
                TopologyConnections conn = topo.get_connections(i);
                assertEquals(2, conn.getFirst().size(),
                    "ring island " + i + " must have exactly 2 neighbors, got " + conn.getFirst().size());
            }
        }
    }

    @Test
    void fullyConnectedTopologyHasNMinusOneNeighborsPerIsland() {
        // A fully-connected topology with N islands: every island has N-1 neighbors.
        final int N = 4;
        try (fully_connected topo = new fully_connected()) {
            for (int i = 0; i < N; i++) topo.push_back();
            for (int i = 0; i < N; i++) {
                TopologyConnections conn = topo.get_connections(i);
                assertEquals(N - 1, conn.getFirst().size(),
                    "fully_connected island " + i + " must have " + (N - 1) + " neighbors");
            }
        }
    }

    @Test
    void ringConnectionWeightsArePositive() {
        try (ring topo = new ring()) {
            for (int i = 0; i < 3; i++) topo.push_back();
            TopologyConnections conn = topo.get_connections(0);
            for (int j = 0; j < conn.getSecond().size(); j++) {
                assertTrue(conn.getSecond().get(j) > 0.0,
                    "connection weight must be positive, got " + conn.getSecond().get(j));
            }
        }
    }
}
