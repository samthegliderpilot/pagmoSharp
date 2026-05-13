package io.github.samthegliderpilot.pagmo4j.utils;

import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/** Mirrors Test_hypervolume.cs. */
class HypervolumeTest {

    private static VectorOfVectorOfDoubles makePoints(double[][] rows) {
        VectorOfVectorOfDoubles v = new VectorOfVectorOfDoubles();
        for (double[] row : rows) {
            DoubleVector dv = new DoubleVector();
            for (double d : row) dv.add(d);
            v.add(dv);
        }
        return v;
    }

    private static DoubleVector vec(double... values) {
        DoubleVector v = new DoubleVector();
        for (double d : values) v.add(d);
        return v;
    }

    @Test
    void computeReturnsExpectedValueForSimple2DFront() {
        try (hypervolume hv = new hypervolume(makePoints(new double[][]{{1.0, 3.0}, {2.0, 2.0}}))) {
            DoubleVector ref = vec(4.0, 4.0);
            assertEquals(5.0, hv.compute(ref), 1e-12);
            ref.delete();
        }
    }

    @Test
    void contributionsMatchPointCount() {
        try (hypervolume hv = new hypervolume(makePoints(new double[][]{{1.0, 3.0}, {2.0, 2.0}, {3.0, 1.0}}))) {
            DoubleVector ref = vec(4.0, 4.0);
            DoubleVector contribs = hv.contributions(ref);
            assertEquals(3, contribs.size());
            for (int i = 0; i < 3; i++) {
                double excl = hv.exclusive((long) i, ref);
                assertEquals(excl, contribs.get(i), 1e-12);
                assertTrue(contribs.get(i) > 0.0);
            }
            long least    = hv.least_contributor(ref).longValue();
            long greatest = hv.greatest_contributor(ref).longValue();
            assertTrue(least    < 3L);
            assertTrue(greatest < 3L);
            ref.delete(); contribs.delete();
        }
    }
}
