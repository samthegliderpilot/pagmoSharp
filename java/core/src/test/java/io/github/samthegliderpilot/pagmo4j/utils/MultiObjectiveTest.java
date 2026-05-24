package io.github.samthegliderpilot.pagmo4j.utils;

import io.github.samthegliderpilot.pagmo4j.*;
import java.math.BigInteger;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/** Mirrors Test_multi_objective.cs and covers MultiObjectiveUtils projections. */
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
        // Points: (1,3), (2,2), (3,1) are non-dominated (Pareto front 0).
        // (4,4) is dominated by all three → goes to front 1.
        VectorOfVectorOfDoubles pts = vecs(
                new double[]{1.0, 3.0}, new double[]{2.0, 2.0},
                new double[]{3.0, 1.0}, new double[]{4.0, 4.0});
        FNDSResult result = pagmo4j.FastNonDominatedSorting(pts);
        assertNotNull(result);
        assertEquals(2, result.getFronts().size(), "should have exactly 2 fronts");
        assertEquals(3, result.getFronts().get(0).size(), "front 0 must have the 3 non-dominated points");
        assertEquals(1, result.getFronts().get(1).size(), "front 1 must have the 1 dominated point (4,4)");
        // Verify the dominated point index is present in front 1
        assertTrue(result.getFronts().get(1).contains(BigInteger.valueOf(3)), "(4,4) is at index 3 and must be in front 1");
        pts.delete();
    }

    @Test
    void paretoDominanceIsCorrect() {
        DoubleVector lhs = vec(1.0, 1.0);
        DoubleVector rhs = vec(2.0, 3.0);
        assertTrue(pagmo4j.pareto_dominance(lhs, rhs),  "lhs dominates rhs");
        assertFalse(pagmo4j.pareto_dominance(rhs, lhs), "rhs does not dominate lhs");
        assertFalse(pagmo4j.pareto_dominance(lhs, lhs), "identical points do not dominate each other");
        lhs.delete(); rhs.delete();
    }

    @Test
    void crowdingDistanceReturnsOneValuePerPoint() {
        // Boundary points get infinite distance; interior point gets a finite value.
        VectorOfVectorOfDoubles pts = vecs(
                new double[]{1.0, 3.0}, new double[]{2.0, 2.0}, new double[]{3.0, 1.0});
        DoubleVector dist = pagmo4j.crowding_distance(pts);
        assertEquals(3, dist.size());
        assertTrue(Double.isInfinite(dist.get(0)),
            "first boundary point must have infinite crowding distance");
        assertTrue(Double.isInfinite(dist.get(2)),
            "last boundary point must have infinite crowding distance");
        assertTrue(dist.get(1) > 0 && Double.isFinite(dist.get(1)),
            "interior point must have finite positive crowding distance");
        pts.delete(); dist.delete();
    }

    // ── MultiObjectiveUtils projections ───────────────────────────────────────

    @Test
    void idealValuesReturnsComponentWiseMinimum() {
        VectorOfVectorOfDoubles pts = vecs(new double[]{1.0, 3.0}, new double[]{2.0, 2.0}, new double[]{4.0, 1.0});
        double[] ideal = MultiObjectiveUtils.idealValues(pts);
        assertEquals(2, ideal.length);
        assertEquals(1.0, ideal[0], 1e-9, "ideal[0] should be min of first objectives");
        assertEquals(1.0, ideal[1], 1e-9, "ideal[1] should be min of second objectives");
        pts.delete();
    }

    @Test
    void nadirValuesReturnsMaxOfNonDominatedFront() {
        VectorOfVectorOfDoubles pts = vecs(new double[]{1.0, 3.0}, new double[]{2.0, 2.0}, new double[]{3.0, 1.0});
        double[] nadir = MultiObjectiveUtils.nadirValues(pts);
        assertEquals(2, nadir.length);
        // All 3 points are non-dominated; nadir is the component-wise max of the front.
        assertEquals(3.0, nadir[0], 1e-9, "nadir[0] should be max of first objectives on front");
        assertEquals(3.0, nadir[1], 1e-9, "nadir[1] should be max of second objectives on front");
        pts.delete();
    }

    @Test
    void nonDominatedFront2DIndicesReturnsCorrectIndices() {
        // (1,3), (2,2), (3,1) are non-dominated; (4,4) is dominated
        VectorOfVectorOfDoubles pts = vecs(
                new double[]{1.0, 3.0}, new double[]{2.0, 2.0},
                new double[]{3.0, 1.0}, new double[]{4.0, 4.0});
        long[] front = MultiObjectiveUtils.nonDominatedFront2DIndices(pts);
        assertEquals(3, front.length, "exactly 3 points are on the 2D non-dominated front");
        pts.delete();
    }

    @Test
    void sortPopulationMoIndicesReturnsAllIndices() {
        VectorOfVectorOfDoubles pts = vecs(
                new double[]{1.0, 3.0}, new double[]{2.0, 2.0}, new double[]{3.0, 1.0});
        long[] sorted = MultiObjectiveUtils.sortPopulationMoIndices(pts);
        assertEquals(3, sorted.length, "sorted array must contain all individuals");
        pts.delete();
    }

    @Test
    void multiObjectiveUtilsNullChecks() {
        assertThrows(NullPointerException.class, () -> MultiObjectiveUtils.idealValues(null));
        assertThrows(NullPointerException.class, () -> MultiObjectiveUtils.nadirValues(null));
        assertThrows(NullPointerException.class, () -> MultiObjectiveUtils.nonDominatedFront2DIndices(null));
        assertThrows(NullPointerException.class, () -> MultiObjectiveUtils.sortPopulationMoIndices(null));
        DoubleVector v = new DoubleVector();
        assertThrows(NullPointerException.class, () -> MultiObjectiveUtils.decomposeObjectiveValues(null, v, v, "weighted"));
        assertThrows(NullPointerException.class, () -> MultiObjectiveUtils.decomposeObjectiveValues(v, null, v, "weighted"));
        assertThrows(NullPointerException.class, () -> MultiObjectiveUtils.decomposeObjectiveValues(v, v, null, "weighted"));
        assertThrows(NullPointerException.class, () -> MultiObjectiveUtils.decomposeObjectiveValues(v, v, v, null));
    }
}
