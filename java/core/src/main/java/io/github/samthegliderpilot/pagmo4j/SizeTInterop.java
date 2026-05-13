package io.github.samthegliderpilot.pagmo4j;

/**
 * Helpers for converting between Java {@code long} and native {@code size_t} / unsigned types.
 *
 * <p>pagmo uses {@code unsigned int} and {@code size_t} throughout its API. Java has no
 * unsigned integer types; we represent them as {@code long} and validate at the boundary.
 */
public final class SizeTInterop {

    private SizeTInterop() {}

    /**
     * Validates that {@code value} fits in a 32-bit unsigned integer and returns it as {@code long}.
     * Use for pagmo parameters typed {@code unsigned int} (e.g. population size, generations, seed).
     *
     * @throws IllegalArgumentException if value is negative or exceeds 0xFFFFFFFFL
     */
    public static long toNativeUInt32(long value, String paramName) {
        if (value < 0 || value > 0xFFFFFFFFL) {
            throw new IllegalArgumentException(
                paramName + " must be in [0, 4294967295] (unsigned 32-bit); got " + value);
        }
        return value;
    }

    /**
     * Validates that {@code value} is non-negative (fits in {@code size_t} on 64-bit systems).
     * Use for pagmo parameters typed {@code size_t} or {@code unsigned long long}.
     *
     * @throws IllegalArgumentException if value is negative
     */
    public static long toNativeSizeT(long value, String paramName) {
        if (value < 0) {
            throw new IllegalArgumentException(
                paramName + " must be non-negative (size_t); got " + value);
        }
        return value;
    }
}
