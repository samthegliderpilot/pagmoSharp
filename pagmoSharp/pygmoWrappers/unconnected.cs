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

public partial class unconnected : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal unconnected(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(unconnected obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  internal static global::System.Runtime.InteropServices.HandleRef swigRelease(unconnected obj) {
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

  ~unconnected() {
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
          pagmoPINVOKE.delete_unconnected(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public SWIGTYPE_p_std__pairT_std__vectorT_std__size_t_t_std__vectorT_double_t_t get_connections(uint arg0) {
    SWIGTYPE_p_std__pairT_std__vectorT_std__size_t_t_std__vectorT_double_t_t ret = new SWIGTYPE_p_std__pairT_std__vectorT_std__size_t_t_std__vectorT_double_t_t(pagmoPINVOKE.unconnected_get_connections(swigCPtr, arg0), true);
    return ret;
  }

  public void push_back() {
    pagmoPINVOKE.unconnected_push_back(swigCPtr);
  }

  public string get_name() {
    string ret = pagmoPINVOKE.unconnected_get_name(swigCPtr);
    return ret;
  }

  public unconnected() : this(pagmoPINVOKE.new_unconnected(), true) {
  }

}

}
