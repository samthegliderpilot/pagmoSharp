package io.github.samthegliderpilot.pagmo4j.problems;

/**
 * Opt-in contract for managed problems that cannot be used concurrently
 * ({@link io.github.samthegliderpilot.pagmo4j.ThreadSafety#None}) but can produce
 * independent copies of themselves for per-thread or per-island use.
 */
public interface IThreadCloneableProblem extends IProblem {

    /**
     * Returns a fully independent copy of this problem for exclusive use on a single
     * thread or island. Must not return {@code null} or the same instance.
     */
    IProblem clone();
}
