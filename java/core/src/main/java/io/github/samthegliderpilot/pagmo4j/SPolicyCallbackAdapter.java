package io.github.samthegliderpilot.pagmo4j;

/**
 * SWIG director adapter for user-defined selection policies.
 *
 * <p>Extend this class to implement a custom selection policy. Override
 * {@link #select(ULongLongVector, VectorOfVectorOfDoubles, long, long, long, long)}
 * to choose which individuals emigrate from the island.
 *
 * <p>Wrap with {@code new s_policy(adapter)} to use in
 * {@code archipelago.pushBackIsland()} or {@code island.create()}.
 */
public abstract class SPolicyCallbackAdapter extends SPolicyCallback {

    @Override
    public abstract IndividualsGroup select(
        IndividualsGroup population,
        long n_f,
        long n_ec,
        long n_ic,
        long n_obj,
        long pop_size,
        DoubleVector tol);
}
