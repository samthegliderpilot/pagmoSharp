package io.github.samthegliderpilot.pagmo4j;

/**
 * SWIG director adapter for user-defined replacement policies.
 *
 * <p>Override {@link #replace} to select which emigrants replace current
 * island residents. Wrap with {@code new r_policy(adapter)}.
 *
 * <p>Signature matches the generated {@link RPolicyCallback#replace}.
 */
public abstract class RPolicyCallbackAdapter extends RPolicyCallback {

    @Override
    public abstract IndividualsGroup replace(
        IndividualsGroup incoming,
        long n_f,
        long n_ec,
        long n_ic,
        long n_obj,
        long pop_size,
        DoubleVector tol,
        IndividualsGroup current);
}
