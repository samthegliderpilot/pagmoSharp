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

public class thread_bfe : bfe {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;

  internal thread_bfe(global::System.IntPtr cPtr, bool cMemoryOwn) : base(pagmoPINVOKE.thread_bfe_SWIGUpcast(cPtr), cMemoryOwn) {
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(thread_bfe obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  protected override void Dispose(bool disposing) {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          pagmoPINVOKE.delete_thread_bfe(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      base.Dispose(disposing);
    }
  }

  public thread_bfe() : this(pagmoPINVOKE.new_thread_bfe__SWIG_0(), true) {
  }

  public thread_bfe(thread_bfe arg0) : this(pagmoPINVOKE.new_thread_bfe__SWIG_1(thread_bfe.getCPtr(arg0)), true) {
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  public virtual string get_name() {
    string ret = pagmoPINVOKE.thread_bfe_get_name(swigCPtr);
    return ret;
  }

  public DoubleVector Operator(problem theProblem, DoubleVector values) {
    DoubleVector ret = new DoubleVector(pagmoPINVOKE.thread_bfe_Operator(swigCPtr, problem.getCPtr(theProblem), DoubleVector.getCPtr(values)), true);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

}

}
