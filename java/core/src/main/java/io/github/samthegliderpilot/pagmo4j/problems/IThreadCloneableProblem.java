package io.github.samthegliderpilot.pagmo4j.problems;

/**
 * Opt-in contract for managed problems that cannot be used concurrently
 * ({@link io.github.samthegliderpilot.pagmo4j.ThreadSafety#None}) but can produce
 * independent copies of themselves for per-thread or per-island use.
 */
public interface IThreadCloneableProblem extends IProblem {

    /**
     * Returns a fully independent copy of this problem for exclusive use on a single
     * thread or island.
     *
     * <p>Returning {@code null} opts out of per-thread cloning for this call — the
     * caller will fall back to requiring {@link io.github.samthegliderpilot.pagmo4j.ThreadSafety#Basic}
     * instead. Never return the same instance ({@code this}); that would defeat the
     * purpose of isolation.
     *
     * @return an independent copy of this problem, or {@code null} to opt out of cloning
     */
    IProblem clone();
}
