package io.github.samthegliderpilot.pagmo4j.migration;

import io.github.samthegliderpilot.pagmo4j.*;

/**
 * Bridges pagmo's native {@code fair_replace} policy to the managed {@link IRPolicy}
 * interface, allowing it to be used with
 * {@link io.github.samthegliderpilot.pagmo4j.archipelago#pushBackIsland}.
 *
 * <p>Delegates to the native C++ implementation for correct pagmo behaviour
 * (replaces a configurable fraction of the island population with the best
 * incoming migrants).
 */
public final class FairReplaceAdapter extends RPolicyCallbackAdapter {

    private final fair_replace inner;

    /** Constructs a fair_replace adapter with the default replacement rate (1.0). */
    public FairReplaceAdapter() {
        this.inner = new fair_replace();
    }

    @Override
    protected IndividualsGroup replaceManaged(
            IndividualsGroup incoming, long n_f, long n_ec, long n_ic,
            long n_obj, long pop_size, DoubleVector tol, IndividualsGroup current) {
        return inner.replace_wrapped(incoming, n_f, n_ec, n_ic, n_obj, pop_size, tol, current);
    }

    @Override public String get_name() { return inner.get_name(); }

    @Override
    public void close() {
        inner.close();
        super.close();
    }
}
