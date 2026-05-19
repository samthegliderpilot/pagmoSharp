package io.github.samthegliderpilot.pagmo4j.algorithms;

import io.github.samthegliderpilot.pagmo4j.*;
import io.github.samthegliderpilot.pagmo4j.NativeInterop;

/**
 * Normalizes any {@link IAlgorithm} to a type-erased {@link algorithm} for
 * pagmo's island/archipelago machinery.
 *
 * <p>Concrete pagmo algorithm classes are passed through their native
 * {@code to_algorithm()} conversion path. Custom managed algorithms are
 * wrapped via the callback bridge.
 */
public final class AlgorithmInterop {

    private AlgorithmInterop() {}

    /**
     * Normalizes any {@link IAlgorithm} to a type-erased {@link algorithm}.
     *
     * <p>Concrete pagmo algorithm classes (e.g. {@code de}, {@code sade}) are converted through
     * their native {@code to_algorithm()} path for zero-copy wrapping. Custom managed
     * {@link IAlgorithm} implementations are wrapped via the director callback bridge.
     *
     * <p><b>NOTE:</b> This method must be updated whenever a new concrete algorithm class is
     * added to the pagmo4j binding. Each concrete type needs an explicit {@code instanceof}
     * branch so it uses the native conversion path rather than the slower director path.
     */
    public static algorithm normalizeToTypeErased(IAlgorithm source) {
        if (source == null) throw new NullPointerException("source");

        if (source instanceof algorithm)            return (algorithm) source;
        if (source instanceof bee_colony)           return ((bee_colony) source).to_algorithm();
        if (source instanceof cmaes)                return ((cmaes) source).to_algorithm();
        if (source instanceof compass_search)       return ((compass_search) source).to_algorithm();
        if (source instanceof ihs)                  return ((ihs) source).to_algorithm();
        if (source instanceof ipopt)                return ((ipopt) source).to_algorithm();
        if (source instanceof nsga2)                return ((nsga2) source).to_algorithm();
        if (source instanceof nlopt)                return ((nlopt) source).to_algorithm();
        if (source instanceof moead)                return ((moead) source).to_algorithm();
        if (source instanceof moead_gen)            return ((moead_gen) source).to_algorithm();
        if (source instanceof maco)                 return ((maco) source).to_algorithm();
        if (source instanceof mbh)                  return ((mbh) source).to_algorithm();
        if (source instanceof cstrs_self_adaptive)  return ((cstrs_self_adaptive) source).to_algorithm();
        if (source instanceof de)                   return ((de) source).to_algorithm();
        if (source instanceof de1220)               return ((de1220) source).to_algorithm();
        if (source instanceof gaco)                 return ((gaco) source).to_algorithm();
        if (source instanceof gwo)                  return ((gwo) source).to_algorithm();
        if (source instanceof nspso)                return ((nspso) source).to_algorithm();
        if (source instanceof null_algorithm)       return ((null_algorithm) source).to_algorithm();
        if (source instanceof pso)                  return ((pso) source).to_algorithm();
        if (source instanceof pso_gen)              return ((pso_gen) source).to_algorithm();
        if (source instanceof sade)                 return ((sade) source).to_algorithm();
        if (source instanceof sea)                  return ((sea) source).to_algorithm();
        if (source instanceof sga)                  return ((sga) source).to_algorithm();
        if (source instanceof simulated_annealing)  return ((simulated_annealing) source).to_algorithm();
        if (source instanceof xnes)                 return ((xnes) source).to_algorithm();

        return new algorithm(source);
    }
}
