package io.github.samthegliderpilot.pagmo4j;

/** Native pagmo replacement policy: replaces migrants into the current island population. */
public class fair_replace implements AutoCloseable {
  private transient long swigCPtr;
  protected transient boolean swigCMemOwn;

  protected fair_replace(long cPtr, boolean cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }

  protected static long getCPtr(fair_replace obj) {
    return (obj == null) ? 0 : obj.swigCPtr;
  }

  protected static long swigRelease(fair_replace obj) {
    if (obj != null) {
      if (!obj.swigCMemOwn)
        throw new RuntimeException("Cannot release ownership of fair_replace that was not owned.");
      long ptr = obj.swigCPtr;
      obj.swigCMemOwn = false;
      obj.delete();
      return ptr;
    }
    return 0;
  }

  public synchronized void delete() {
    if (swigCPtr != 0) {
      if (swigCMemOwn) {
        swigCMemOwn = false;
        pagmo4jJNI.delete_fair_replace(swigCPtr);
      }
      swigCPtr = 0;
    }
  }

  @Override public void close() { delete(); }

  /** Constructs a fair_replace policy with the default replacement rate (1.0 — replace all). */
  public fair_replace() {
    this(pagmo4jJNI.new_fair_replace(), true);
  }

  /** Returns the policy name. */
  public String get_name() {
    return pagmo4jJNI.fair_replace_get_name(swigCPtr, this);
  }

  /** Returns extra diagnostic information. */
  public String get_extra_info() {
    return pagmo4jJNI.fair_replace_get_extra_info(swigCPtr, this);
  }

  /**
   * Computes the replacement group using pagmo's native fair_replace algorithm.
   * This is the SWIG-safe bridge method (tuple types are converted internally).
   */
  public IndividualsGroup replace_wrapped(
      IndividualsGroup incoming, long n_f, long n_ec, long n_ic,
      long n_obj, long pop_size, DoubleVector tol, IndividualsGroup current) {
    return new IndividualsGroup(
        pagmo4jJNI.fair_replace_replace_wrapped(
            swigCPtr, this,
            IndividualsGroup.getCPtr(incoming), incoming,
            n_f, n_ec, n_ic, n_obj, pop_size,
            DoubleVector.getCPtr(tol), tol,
            IndividualsGroup.getCPtr(current), current),
        true);
  }
}
