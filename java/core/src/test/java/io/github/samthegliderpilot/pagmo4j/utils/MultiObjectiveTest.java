package io.github.samthegliderpilot.pagmo4j.utils;

import io.github.samthegliderpilot.pagmo4j.*;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/** Mirrors Test_multi_objective.cs. */
class MultiObjectiveTest {

    private static DoubleVector vec(double... v) {
        DoubleVector dv = new DoubleVector();
        for (double d : v) dv.add(d);
        return dv;
    }

    private static VectorOfVectorOfDoubles vecs(double[]... rows) {
        VectorOfVectorOfDoubles v = new VectorOfVectorOfDoubles();
        for (double[] row : rows) v.add(vec(row));
        return v;
    }

    @Test
    void fastNonDominatedSortingReturnsExpectedFrontShape() {
        VectorOfVectorOfDoubles pts = vecs(
                new double[]{1.0, 3.0}, new double[]{2.0, 2.0},
                new double[]{3.0, 1.0}, new double[]{4.0, 4.0});
        FNDSResult result = pagmo4j.FastNonDominatedSorting(pts);
        assertNotNull(result);
        assertTrue(result.getFronts().size() >= 1);
        // first front should contain the 3 non-dominated points
        assertEquals(3, result.getFronts().get(0).size());
        pts.delete();
    }

    @Test
    void paretoDominanceIsCorrect() {
        DoubleVector lhs = vec(1.0, 1.0);
        DoubleVector rhs = vec(2.0, 3.0);
        assertTrue(pagmo4j.pareto_dominance(lhs, rhs),  "lhs dominates rhs");
        assertFalse(pagmo4j.pareto_dominance(rhs, lhs), "rhs does not dominate lhs");
        lhs.delete(); rhs.delete();
    }

    @Test
    void crowdingDistanceReturnsOneValuePerPoint() {
        VectorOfVectorOfDoubles pts = vecs(
                new double[]{1.0, 3.0}, new double[]{2.0, 2.0}, new double[]{3.0, 1.0});
        DoubleVector dist = pagmo4j.crowding_distance(pts);
        assertEquals(3, dist.size());
        pts.delete(); dist.delete();
    }
}
