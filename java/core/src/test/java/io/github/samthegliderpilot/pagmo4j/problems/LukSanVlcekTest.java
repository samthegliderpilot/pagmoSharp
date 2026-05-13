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
        try (luksan_vlcek1 p = new luksan_vlcek1(3L);
             de inner = new de(5L); algorithm a = new algorithm(inner);
             mbh algo = new mbh(a, 5L, 0.1); population pop = new population(p, 32L, 2L)) {
            algo.set_seed(2L);
            try (population evolved = algo.evolve(pop)) { assertNotNull(evolved); }
        }
    }
}
