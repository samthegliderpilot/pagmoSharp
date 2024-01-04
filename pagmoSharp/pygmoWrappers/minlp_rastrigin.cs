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

public partial class minlp_rastrigin : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal minlp_rastrigin(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(minlp_rastrigin obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  internal static global::System.Runtime.InteropServices.HandleRef swigRelease(minlp_rastrigin obj) {
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

  ~minlp_rastrigin() {
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
          pagmoPINVOKE.delete_minlp_rastrigin(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public minlp_rastrigin(uint dim_c, uint dim_i) : this(pagmoPINVOKE.new_minlp_rastrigin__SWIG_0(dim_c, dim_i), true) {
  }

  public minlp_rastrigin(uint dim_c) : this(pagmoPINVOKE.new_minlp_rastrigin__SWIG_1(dim_c), true) {
  }

  public minlp_rastrigin() : this(pagmoPINVOKE.new_minlp_rastrigin__SWIG_2(), true) {
  }

  public DoubleVector fitness(DoubleVector arg0) {
    DoubleVector ret = new DoubleVector(pagmoPINVOKE.minlp_rastrigin_fitness(swigCPtr, DoubleVector.getCPtr(arg0)), true);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public PairOfDoubleVectors get_bounds() {
    PairOfDoubleVectors ret = new PairOfDoubleVectors(pagmoPINVOKE.minlp_rastrigin_get_bounds(swigCPtr), true);
    return ret;
  }

  public uint get_nix() {
    uint ret = pagmoPINVOKE.minlp_rastrigin_get_nix(swigCPtr);
    return ret;
  }

  public DoubleVector gradient(DoubleVector arg0) {
    DoubleVector ret = new DoubleVector(pagmoPINVOKE.minlp_rastrigin_gradient(swigCPtr, DoubleVector.getCPtr(arg0)), true);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public VectorOfVectorOfDoubles hessians(DoubleVector arg0) {
    VectorOfVectorOfDoubles ret = new VectorOfVectorOfDoubles(pagmoPINVOKE.minlp_rastrigin_hessians(swigCPtr, DoubleVector.getCPtr(arg0)), true);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t hessians_sparsity() {
    SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t ret = new SWIGTYPE_p_std__vectorT_std__vectorT_std__pairT_size_t_size_t_t_t_t(pagmoPINVOKE.minlp_rastrigin_hessians_sparsity(swigCPtr), true);
    return ret;
  }

  public string get_name() {
    string ret = pagmoPINVOKE.minlp_rastrigin_get_name(swigCPtr);
    return ret;
  }

  public string get_extra_info() {
    string ret = pagmoPINVOKE.minlp_rastrigin_get_extra_info(swigCPtr);
    return ret;
  }

  public uint get_nic() {
    uint ret = pagmoPINVOKE.minlp_rastrigin_get_nic(swigCPtr);
    return ret;
  }

  public uint get_nec() {
    uint ret = pagmoPINVOKE.minlp_rastrigin_get_nec(swigCPtr);
    return ret;
  }

  public uint get_nobj() {
    uint ret = pagmoPINVOKE.minlp_rastrigin_get_nobj(swigCPtr);
    return ret;
  }

  public bool has_batch_fitness() {
    bool ret = pagmoPINVOKE.minlp_rastrigin_has_batch_fitness(swigCPtr);
    return ret;
  }

  public thread_safety get_thread_safety() {
    thread_safety ret = (thread_safety)pagmoPINVOKE.minlp_rastrigin_get_thread_safety(swigCPtr);
    return ret;
  }

}

}
