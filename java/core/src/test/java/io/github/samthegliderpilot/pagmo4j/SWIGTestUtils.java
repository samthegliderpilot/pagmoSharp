package io.github.samthegliderpilot.pagmo4j;

/**
 * Test utility for SWIG-generated types that cannot implement AutoCloseable.
 *
 * <p>SWIG's std_vector.i sets javainterfaces via an unexpandable macro, preventing
 * AutoCloseable from being added to vector types. Use {@link #close} as a drop-in
 * for try-with-resources on {@link DoubleVector}, {@link ULongLongVector}, etc.
 */
final class SWIGTestUtils {

    private SWIGTestUtils() {}

    public static AutoCloseable close(DoubleVector v) { return v::delete; }
    public static AutoCloseable close(ULongLongVector v) { return v::delete; }
    public static AutoCloseable close(SizeTVector v) { return v::delete; }
    public static AutoCloseable close(VectorOfVectorOfDoubles v) { return v::delete; }
}
