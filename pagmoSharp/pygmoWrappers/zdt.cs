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

public partial class zdt : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal zdt(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(zdt obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  internal static global::System.Runtime.InteropServices.HandleRef swigRelease(zdt obj) {
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

  ~zdt() {
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
          pagmoPINVOKE.delete_zdt(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public zdt(uint prob_id, uint param) : this(pagmoPINVOKE.new_zdt__SWIG_0(prob_id, param), true) {
  }

  public zdt(uint prob_id) : this(pagmoPINVOKE.new_zdt__SWIG_1(prob_id), true) {
  }

  public zdt() : this(pagmoPINVOKE.new_zdt__SWIG_2(), true) {
  }

  public DoubleVector fitness(DoubleVector arg0) {
    DoubleVector ret = new DoubleVector(pagmoPINVOKE.zdt_fitness(swigCPtr, DoubleVector.getCPtr(arg0)), true);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public uint get_nobj() {
    uint ret = pagmoPINVOKE.zdt_get_nobj(swigCPtr);
    return ret;
  }

  public PairOfDoubleVectors get_bounds() {
    PairOfDoubleVectors ret = new PairOfDoubleVectors(pagmoPINVOKE.zdt_get_bounds(swigCPtr), true);
    return ret;
  }

  public uint get_nix() {
    uint ret = pagmoPINVOKE.zdt_get_nix(swigCPtr);
    return ret;
  }

  public string get_name() {
    string ret = pagmoPINVOKE.zdt_get_name(swigCPtr);
    return ret;
  }

  public double p_distance(population arg0) {
    double ret = pagmoPINVOKE.zdt_p_distance__SWIG_0(swigCPtr, population.getCPtr(arg0));
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public double p_distance(DoubleVector arg0) {
    double ret = pagmoPINVOKE.zdt_p_distance__SWIG_1(swigCPtr, DoubleVector.getCPtr(arg0));
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public uint get_nic() {
    uint ret = pagmoPINVOKE.zdt_get_nic(swigCPtr);
    return ret;
  }

  public uint get_nec() {
    uint ret = pagmoPINVOKE.zdt_get_nec(swigCPtr);
    return ret;
  }

  public bool has_batch_fitness() {
    bool ret = pagmoPINVOKE.zdt_has_batch_fitness(swigCPtr);
    return ret;
  }

  public thread_safety get_thread_safety() {
    thread_safety ret = (thread_safety)pagmoPINVOKE.zdt_get_thread_safety(swigCPtr);
    return ret;
  }

}

}
