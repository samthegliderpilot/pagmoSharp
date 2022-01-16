//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 4.0.2
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace pagmo {

public partial class problem : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal problem(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(problem obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~problem() {
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
          pagmoPINVOKE.delete_problem(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public problem() : this(pagmoPINVOKE.new_problem__SWIG_0(), true) {
  }

  public problem(problemBase base_) : this(pagmoPINVOKE.new_problem__SWIG_1(problemBase.getCPtr(base_)), true) {
  }

  public problem(problem old) : this(pagmoPINVOKE.new_problem__SWIG_2(problem.getCPtr(old)), true) {
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  public void setBaseProblem(problemBase b) {
    pagmoPINVOKE.problem_setBaseProblem(swigCPtr, problemBase.getCPtr(b));
  }

  public problemBase getBaseProblem() {
    global::System.IntPtr cPtr = pagmoPINVOKE.problem_getBaseProblem(swigCPtr);
    problemBase ret = (cPtr == global::System.IntPtr.Zero) ? null : new problemBase(cPtr, false);
    return ret;
  }

  public DoubleVector fitness(DoubleVector x) {
    DoubleVector ret = new DoubleVector(pagmoPINVOKE.problem_fitness(swigCPtr, DoubleVector.getCPtr(x)), true);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public PairOfDoubleVectors get_bounds() {
    PairOfDoubleVectors ret = new PairOfDoubleVectors(pagmoPINVOKE.problem_get_bounds(swigCPtr), true);
    return ret;
  }

  public bool has_batch_fitness() {
    bool ret = pagmoPINVOKE.problem_has_batch_fitness(swigCPtr);
    return ret;
  }

  public string get_name() {
    string ret = pagmoPINVOKE.problem_get_name(swigCPtr);
    return ret;
  }

  public uint get_nobj() {
    uint ret = pagmoPINVOKE.problem_get_nobj(swigCPtr);
    return ret;
  }

  public uint get_nec() {
    uint ret = pagmoPINVOKE.problem_get_nec(swigCPtr);
    return ret;
  }

  public uint get_nic() {
    uint ret = pagmoPINVOKE.problem_get_nic(swigCPtr);
    return ret;
  }

  public uint get_nix() {
    uint ret = pagmoPINVOKE.problem_get_nix(swigCPtr);
    return ret;
  }

}

}
