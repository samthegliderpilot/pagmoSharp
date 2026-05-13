package io.github.samthegliderpilot.pagmo4j;

/**
 * Runtime detection of optional pagmo solvers that may or may not be compiled
 * into the native library.
 *
 * <p>Use these flags to guard solver construction in code that should work both
 * with and without the optional solvers available.
 */
public final class OptionalSolverAvailability {

    private OptionalSolverAvailability() {}

    private static final boolean nloptAvailable;
    private static final boolean ipoptAvailable;

    static {
        boolean nl = false, ip = false;
        try {
            // Attempt to construct the algorithm; if the native build lacks it, pagmo throws.
            try (nlopt probe = new nlopt("cobyla")) {
                nl = true;
            } catch (RuntimeException ignored) {}
        } catch (Throwable ignored) {}
        try {
            try (ipopt probe = new ipopt()) {
                ip = true;
            } catch (RuntimeException ignored) {}
        } catch (Throwable ignored) {}
        nloptAvailable = nl;
        ipoptAvailable = ip;
    }

    /** Returns {@code true} if the native library was built with NLopt support. */
    public static boolean isNloptAvailable() { return nloptAvailable; }

    /** Returns {@code true} if the native library was built with IPOPT support. */
    public static boolean isIpoptAvailable() { return ipoptAvailable; }
}
