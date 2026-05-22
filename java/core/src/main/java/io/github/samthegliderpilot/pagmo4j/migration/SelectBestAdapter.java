package io.github.samthegliderpilot.pagmo4j.migration;

import io.github.samthegliderpilot.pagmo4j.*;

/**
 * Bridges pagmo's native {@code select_best} policy to the managed {@link ISPolicy}
 * interface, allowing it to be used with
 * {@link io.github.samthegliderpilot.pagmo4j.archipelago#pushBackIsland}.
 *
 * <p>Delegates to the native C++ implementation for correct pagmo behaviour
 * (selects the best-fitness individuals from the island as emigrants).
 */
public final class SelectBestAdapter extends SPolicyCallbackAdapter {

    private final select_best inner;

    /** Constructs a select_best adapter with the default emigrant rate (1.0). */
    public SelectBestAdapter() {
        this.inner = new select_best();
    }

    @Override
    public IndividualsGroup select(
            IndividualsGroup population, long n_f, long n_ec, long n_ic,
            long n_obj, long pop_size, DoubleVector tol) {
        return inner.select(population, n_f, n_ec, n_ic, n_obj, pop_size, tol);
    }

    @Override public String get_name() { return inner.get_name(); }

    @Override
    public void close() {
        inner.close();
        super.close();
    }
}
