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

    private static volatile Boolean nloptAvailable;
    private static volatile Boolean ipoptAvailable;

    private static boolean detectNloptAvailability() {
        try {
            try (nlopt probe = new nlopt("cobyla")) {
                return true;
            } catch (RuntimeException ignored) {
                return false;
            }
        } catch (Throwable ignored) {
            return false;
        }
    }

    private static boolean detectIpoptAvailability() {
        try {
            try (ipopt probe = new ipopt()) {
                return true;
            } catch (RuntimeException ignored) {
                return false;
            }
        } catch (Throwable ignored) {
            return false;
        }
    }

    /** Returns {@code true} if the native library was built with NLopt support. */
    public static boolean isNloptAvailable() {
        Boolean cached = nloptAvailable;
        if (cached != null) return cached;
        synchronized (OptionalSolverAvailability.class) {
            if (nloptAvailable == null) {
                nloptAvailable = detectNloptAvailability();
            }
            return nloptAvailable;
        }
    }

    /** Returns {@code true} if the native library was built with IPOPT support. */
    public static boolean isIpoptAvailable() {
        Boolean cached = ipoptAvailable;
        if (cached != null) return cached;
        synchronized (OptionalSolverAvailability.class) {
            if (ipoptAvailable == null) {
                ipoptAvailable = detectIpoptAvailability();
            }
            return ipoptAvailable;
        }
    }
}
