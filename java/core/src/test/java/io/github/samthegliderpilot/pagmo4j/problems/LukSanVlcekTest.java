package io.github.samthegliderpilot.pagmo4j.problems;
import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;
class LukSanVlcekTest extends ProblemTestBase {
    @Override public IProblem createStandardProblem() { return new luksan_vlcek1(3L); }
    @Override @Test protected void boilerPlate() {
        try (luksan_vlcek1 p = new luksan_vlcek1(3L)) {
            assertFalse(p.get_name().isEmpty());
            assertTrue(p.get_nec() > 0L, "luksan_vlcek1 has equality constraints");
        }
    }
    @Override @Test protected void optimizing() {
        // de cannot handle non-linear constraints; compass_search can.
        try (luksan_vlcek1 p = new luksan_vlcek1(3L);
             compass_search algo = new compass_search(200L); population pop = new population(p, 1L, 2L)) {
            try (population evolved = algo.evolve(pop)) { assertNotNull(evolved); }
        }
    }
}
