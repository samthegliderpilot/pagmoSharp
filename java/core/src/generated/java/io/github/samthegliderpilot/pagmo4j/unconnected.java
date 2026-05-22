package io.github.samthegliderpilot.pagmo4j;

/** Native pagmo topology: every island is isolated (no migration connections). */
public class unconnected implements AutoCloseable {
  private transient long swigCPtr;
  protected transient boolean swigCMemOwn;

  protected unconnected(long cPtr, boolean cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = cPtr;
  }

  protected static long getCPtr(unconnected obj) {
    return (obj == null) ? 0 : obj.swigCPtr;
  }

  protected static long swigRelease(unconnected obj) {
    if (obj != null) {
      if (!obj.swigCMemOwn)
        throw new RuntimeException("Cannot release ownership of unconnected that was not owned.");
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
        pagmo4jJNI.delete_unconnected(swigCPtr);
      }
      swigCPtr = 0;
    }
  }

  @Override public void close() { delete(); }

  /** Constructs the unconnected topology (no migration paths). */
  public unconnected() {
    this(pagmo4jJNI.new_unconnected(), true);
  }

  /** Returns the topology name. */
  public String get_name() {
    return pagmo4jJNI.unconnected_get_name(swigCPtr, this);
  }

  /**
   * Returns the connection weight/index pairs for island {@code n}.
   * Always empty for the unconnected topology.
   */
  public TopologyConnections get_connections(long n) {
    return new TopologyConnections(
        pagmo4jJNI.unconnected_get_connections(swigCPtr, this, n), true);
  }

  /** Registers a new island vertex (no-op for unconnected). */
  public void push_back() {
    pagmo4jJNI.unconnected_push_back(swigCPtr, this);
  }

  /** Wraps this topology in a type-erased {@link topology} for use with archipelago. */
  public topology to_topology() {
    return new topology(pagmo4jJNI.unconnected_to_topology(swigCPtr, this), true);
  }
}
