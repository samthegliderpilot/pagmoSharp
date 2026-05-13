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
}
