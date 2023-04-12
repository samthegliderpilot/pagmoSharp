//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (https://www.swig.org).
// Version 4.1.1
//
// Do not make changes to this file unless you know what you are doing - modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace pagmo {

public partial class ackley : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal ackley(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(ackley obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  internal static global::System.Runtime.InteropServices.HandleRef swigRelease(ackley obj) {
    if (obj != null) {
      if (!obj.swigCMemOwn)
        throw new global::System.ApplicationException("Cannot release ownership as memory is not owned");
      global::System.Runtime.InteropServices.HandleRef ptr = obj.swigCPtr;
      obj.swigCMemOwn = false;
      obj.Dispose();
      return ptr;
    } else {
      return new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
    }
  }

  ~ackley() {
    Dispose(false);
  }

  public void Dispose() {
    Dispose(true);
    global::System.GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing) {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          pagmoPINVOKE.delete_ackley(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public ackley(uint dim) : this(pagmoPINVOKE.new_ackley__SWIG_0(dim), true) {
  }

  public ackley() : this(pagmoPINVOKE.new_ackley__SWIG_1(), true) {
  }

  public DoubleVector fitness(DoubleVector arg0) {
    DoubleVector ret = new DoubleVector(pagmoPINVOKE.ackley_fitness(swigCPtr, DoubleVector.getCPtr(arg0)), true);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public PairOfDoubleVectors get_bounds() {
    PairOfDoubleVectors ret = new PairOfDoubleVectors(pagmoPINVOKE.ackley_get_bounds(swigCPtr), true);
    return ret;
  }

  public string get_name() {
    string ret = pagmoPINVOKE.ackley_get_name(swigCPtr);
    return ret;
  }

  public DoubleVector best_known() {
    DoubleVector ret = new DoubleVector(pagmoPINVOKE.ackley_best_known(swigCPtr), true);
    return ret;
  }

  public uint m_dim {
    set {
      pagmoPINVOKE.ackley_m_dim_set(swigCPtr, value);
    } 
    get {
      uint ret = pagmoPINVOKE.ackley_m_dim_get(swigCPtr);
      return ret;
    } 
  }

  public uint get_nic() {
    uint ret = pagmoPINVOKE.ackley_get_nic(swigCPtr);
    return ret;
  }

  public uint get_nec() {
    uint ret = pagmoPINVOKE.ackley_get_nec(swigCPtr);
    return ret;
  }

  public uint get_nix() {
    uint ret = pagmoPINVOKE.ackley_get_nix(swigCPtr);
    return ret;
  }

  public uint get_nobj() {
    uint ret = pagmoPINVOKE.ackley_get_nobj(swigCPtr);
    return ret;
  }

  public bool has_batch_fitness() {
    bool ret = pagmoPINVOKE.ackley_has_batch_fitness(swigCPtr);
    return ret;
  }

  public thread_safety get_thread_safety() {
    thread_safety ret = (thread_safety)pagmoPINVOKE.ackley_get_thread_safety(swigCPtr);
    return ret;
  }

}

}
