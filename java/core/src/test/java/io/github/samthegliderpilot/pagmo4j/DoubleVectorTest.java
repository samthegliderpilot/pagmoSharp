package io.github.samthegliderpilot.pagmo4j;

import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

/** Basic DoubleVector contract tests — mirrors Test_double_vector.cs. */
class DoubleVectorTest {

    @Test
    void defaultConstructorCreatesEmptyVector() {
        DoubleVector v = new DoubleVector();
        assertEquals(0, v.size());
        v.delete();
    }

    @Test
    void addAndGetRoundTrip() {
        DoubleVector v = new DoubleVector();
        v.add(1.0); v.add(2.0); v.add(3.0);
        assertEquals(3, v.size());
        assertEquals(1.0, v.get(0), 0.0);
        assertEquals(2.0, v.get(1), 0.0);
        assertEquals(3.0, v.get(2), 0.0);
        v.delete();
    }

    @Test
    void setOverwritesElement() {
        DoubleVector v = new DoubleVector();
        v.add(0.0); v.add(0.0);
        v.set(1, 42.0);
        assertEquals(42.0, v.get(1), 0.0);
        v.delete();
    }

    @Test
    void copyConstructorProducesIndependentCopy() {
        DoubleVector a = new DoubleVector();
        a.add(5.0);
        DoubleVector b = new DoubleVector(a);
        b.set(0, 99.0);
        assertEquals(5.0,  a.get(0), 0.0, "original must not be modified");
        assertEquals(99.0, b.get(0), 0.0);
        a.delete(); b.delete();
    }
}
