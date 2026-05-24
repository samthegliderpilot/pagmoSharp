package io.github.samthegliderpilot.pagmo4j.algorithms;

import io.github.samthegliderpilot.pagmo4j.*;

/**
 * SWIG director adapter that forwards native algorithm callbacks to a managed
 * {@link IAlgorithm}.
 */
public final class AlgorithmCallbackAdapter extends AlgorithmCallback {

    private final IAlgorithm algorithm;
    private volatile Throwable deferredEx;

    public AlgorithmCallbackAdapter(IAlgorithm algorithm) {
        if (algorithm == null) throw new NullPointerException("algorithm");
        this.algorithm = algorithm;
    }

    @Override
    public population evolve(population pop) {
        try {
            population r = algorithm.evolve(pop);
            if (r == null) throw new NullPointerException("evolve() returned null");
            return r;
        } catch (Throwable ex) { if (deferredEx == null) deferredEx = ex; return pop; }
    }

    @Override
    public void set_seed(long seed) {
        try { algorithm.set_seed(seed); } catch (Throwable ex) { if (deferredEx == null) deferredEx = ex; }
    }

    @Override public boolean has_set_seed()      { return true; }

    @Override
    public void set_verbosity(long level) {
        try { algorithm.set_verbosity(level); } catch (Throwable ex) { if (deferredEx == null) deferredEx = ex; }
    }

    @Override public boolean has_set_verbosity() { return true; }

    @Override
    public String get_name() {
        try { return algorithm.get_name(); } catch (Throwable ex) { if (deferredEx == null) deferredEx = ex; return "Java algorithm"; }
    }

    @Override
    public String get_extra_info() {
        try { return algorithm.get_extra_info(); } catch (Throwable ex) { if (deferredEx == null) deferredEx = ex; return ""; }
    }

    @Override public ThreadSafety get_thread_safety() { return ThreadSafety.Basic; }

    /**
     * Called by the native bridge ({@code NativeInterop.createAlgorithmPointer}) after each
     * evolve round to retrieve any exception that was deferred during a director callback.
     * Returns the exception message string (empty string if none); the native side converts
     * this to a Java {@link RuntimeException}.
     */
    @Override
    public String consume_deferred_exception() {
        Throwable ex = deferredEx;
        deferredEx = null;
        return ex != null ? ex.toString() : "";
    }

    /**
     * Java-facing equivalent for use by {@code NativeInterop} — returns the raw
     * {@link Throwable} so the original exception type and stack trace are preserved,
     * rather than a stringified version.
     */
    public Throwable consumeDeferredException() {
        Throwable ex = deferredEx;
        deferredEx = null;
        return ex;
    }
}
