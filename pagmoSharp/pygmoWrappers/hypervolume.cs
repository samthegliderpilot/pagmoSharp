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

public partial class hypervolume : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal hypervolume(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(hypervolume obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  internal static global::System.Runtime.InteropServices.HandleRef swigRelease(hypervolume obj) {
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

  ~hypervolume() {
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
          pagmoPINVOKE.delete_hypervolume(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public hypervolume() : this(pagmoPINVOKE.new_hypervolume__SWIG_0(), true) {
  }

  public hypervolume(population pop, bool verify) : this(pagmoPINVOKE.new_hypervolume__SWIG_1(population.getCPtr(pop), verify), true) {
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  public hypervolume(population pop) : this(pagmoPINVOKE.new_hypervolume__SWIG_2(population.getCPtr(pop)), true) {
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  public hypervolume(VectorDoubleVector points, bool verify) : this(pagmoPINVOKE.new_hypervolume__SWIG_3(VectorDoubleVector.getCPtr(points), verify), true) {
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  public hypervolume(VectorDoubleVector points) : this(pagmoPINVOKE.new_hypervolume__SWIG_4(VectorDoubleVector.getCPtr(points)), true) {
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  public hypervolume(hypervolume arg0) : this(pagmoPINVOKE.new_hypervolume__SWIG_5(hypervolume.getCPtr(arg0)), true) {
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  public void set_copy_points(bool arg0) {
    pagmoPINVOKE.hypervolume_set_copy_points(swigCPtr, arg0);
  }

  public bool get_copy_points() {
    bool ret = pagmoPINVOKE.hypervolume_get_copy_points(swigCPtr);
    return ret;
  }

  public void set_verify(bool arg0) {
    pagmoPINVOKE.hypervolume_set_verify(swigCPtr, arg0);
  }

  public bool get_verify() {
    bool ret = pagmoPINVOKE.hypervolume_get_verify(swigCPtr);
    return ret;
  }

  public DoubleVector refpoint(double offset) {
    DoubleVector ret = new DoubleVector(pagmoPINVOKE.hypervolume_refpoint__SWIG_0(swigCPtr, offset), true);
    return ret;
  }

  public DoubleVector refpoint() {
    DoubleVector ret = new DoubleVector(pagmoPINVOKE.hypervolume_refpoint__SWIG_1(swigCPtr), true);
    return ret;
  }

  public VectorDoubleVector get_points() {
    VectorDoubleVector ret = new VectorDoubleVector(pagmoPINVOKE.hypervolume_get_points(swigCPtr), false);
    return ret;
  }

  public SWIGTYPE_p_std__shared_ptrT_pagmo__hv_algorithm_t get_best_compute(DoubleVector arg0) {
    SWIGTYPE_p_std__shared_ptrT_pagmo__hv_algorithm_t ret = new SWIGTYPE_p_std__shared_ptrT_pagmo__hv_algorithm_t(pagmoPINVOKE.hypervolume_get_best_compute(swigCPtr, DoubleVector.getCPtr(arg0)), true);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public SWIGTYPE_p_std__shared_ptrT_pagmo__hv_algorithm_t get_best_exclusive(uint arg0, DoubleVector arg1) {
    SWIGTYPE_p_std__shared_ptrT_pagmo__hv_algorithm_t ret = new SWIGTYPE_p_std__shared_ptrT_pagmo__hv_algorithm_t(pagmoPINVOKE.hypervolume_get_best_exclusive(swigCPtr, arg0, DoubleVector.getCPtr(arg1)), true);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public SWIGTYPE_p_std__shared_ptrT_pagmo__hv_algorithm_t get_best_contributions(DoubleVector arg0) {
    SWIGTYPE_p_std__shared_ptrT_pagmo__hv_algorithm_t ret = new SWIGTYPE_p_std__shared_ptrT_pagmo__hv_algorithm_t(pagmoPINVOKE.hypervolume_get_best_contributions(swigCPtr, DoubleVector.getCPtr(arg0)), true);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public double compute(DoubleVector arg0) {
    double ret = pagmoPINVOKE.hypervolume_compute__SWIG_0(swigCPtr, DoubleVector.getCPtr(arg0));
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public double compute(DoubleVector arg0, hv_algorithm arg1) {
    double ret = pagmoPINVOKE.hypervolume_compute__SWIG_1(swigCPtr, DoubleVector.getCPtr(arg0), hv_algorithm.getCPtr(arg1));
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public double exclusive(uint arg0, DoubleVector arg1, hv_algorithm arg2) {
    double ret = pagmoPINVOKE.hypervolume_exclusive__SWIG_0(swigCPtr, arg0, DoubleVector.getCPtr(arg1), hv_algorithm.getCPtr(arg2));
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public double exclusive(uint arg0, DoubleVector arg1) {
    double ret = pagmoPINVOKE.hypervolume_exclusive__SWIG_1(swigCPtr, arg0, DoubleVector.getCPtr(arg1));
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public DoubleVector contributions(DoubleVector arg0, hv_algorithm arg1) {
    DoubleVector ret = new DoubleVector(pagmoPINVOKE.hypervolume_contributions__SWIG_0(swigCPtr, DoubleVector.getCPtr(arg0), hv_algorithm.getCPtr(arg1)), true);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public DoubleVector contributions(DoubleVector arg0) {
    DoubleVector ret = new DoubleVector(pagmoPINVOKE.hypervolume_contributions__SWIG_1(swigCPtr, DoubleVector.getCPtr(arg0)), true);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public ulong least_contributor(DoubleVector arg0, hv_algorithm arg1) {
    ulong ret = pagmoPINVOKE.hypervolume_least_contributor__SWIG_0(swigCPtr, DoubleVector.getCPtr(arg0), hv_algorithm.getCPtr(arg1));
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public ulong least_contributor(DoubleVector arg0) {
    ulong ret = pagmoPINVOKE.hypervolume_least_contributor__SWIG_1(swigCPtr, DoubleVector.getCPtr(arg0));
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public ulong greatest_contributor(DoubleVector arg0, hv_algorithm arg1) {
    ulong ret = pagmoPINVOKE.hypervolume_greatest_contributor__SWIG_0(swigCPtr, DoubleVector.getCPtr(arg0), hv_algorithm.getCPtr(arg1));
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public ulong greatest_contributor(DoubleVector arg0) {
    ulong ret = pagmoPINVOKE.hypervolume_greatest_contributor__SWIG_1(swigCPtr, DoubleVector.getCPtr(arg0));
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

}

}
