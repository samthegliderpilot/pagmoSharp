package io.github.samthegliderpilot.pagmo4j;

/** Native pagmo selection policy: selects the best individuals for emigration. */
public class select_best implements AutoCloseable {
  private transient long swigCPtr;
  protected transient boolean swigCMemOwn;

  protected select_best(long cPtr, boolean cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }

  protected static long getCPtr(select_best obj) {
    return (obj == null) ? 0 : obj.swigCPtr;
  }

  protected static long swigRelease(select_best obj) {
    if (obj != null) {
      if (!obj.swigCMemOwn)
        throw new RuntimeException("Cannot release ownership of select_best that was not owned.");
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
        pagmo4jJNI.delete_select_best(swigCPtr);
      }
      swigCPtr = 0;
    }
  }

  @Override public void close() { delete(); }

  /** Constructs a select_best policy with the default emigrant rate (1.0 — select all best). */
  public select_best() {
    this(pagmo4jJNI.new_select_best(), true);
  }

  /** Returns the policy name. */
  public String get_name() {
    return pagmo4jJNI.select_best_get_name(swigCPtr, this);
  }

  /** Returns extra diagnostic information. */
  public String get_extra_info() {
    return pagmo4jJNI.select_best_get_extra_info(swigCPtr, this);
  }

  /**
   * Selects individuals for emigration using pagmo's native select_best algorithm.
   * This is the SWIG-safe bridge method (tuple types are converted internally).
   */
  public IndividualsGroup select(
      IndividualsGroup population, long n_f, long n_ec, long n_ic,
      long n_obj, long pop_size, DoubleVector tol) {
    return new IndividualsGroup(
        pagmo4jJNI.select_best_select(
            swigCPtr, this,
            IndividualsGroup.getCPtr(population), population,
            n_f, n_ec, n_ic, n_obj, pop_size,
            DoubleVector.getCPtr(tol), tol),
        true);
  }
}
