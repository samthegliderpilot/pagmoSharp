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

public partial class sea : algorithm {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;

  internal sea(global::System.IntPtr cPtr, bool cMemoryOwn) : base(pagmoPINVOKE.sea_SWIGUpcast(cPtr), cMemoryOwn) {
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(sea obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  internal static global::System.Runtime.InteropServices.HandleRef swigRelease(sea obj) {
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

  protected override void Dispose(bool disposing) {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          pagmoPINVOKE.delete_sea(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      base.Dispose(disposing);
    }
  }

  public sea(uint gen, uint seed) : this(pagmoPINVOKE.new_sea__SWIG_0(gen, seed), true) {
  }

  public sea(uint gen) : this(pagmoPINVOKE.new_sea__SWIG_1(gen), true) {
  }

  public sea() : this(pagmoPINVOKE.new_sea__SWIG_2(), true) {
  }

  public population evolve(population arg0) {
    population ret = new population(pagmoPINVOKE.sea_evolve(swigCPtr, population.getCPtr(arg0)), true);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public new void set_verbosity(uint level) {
    pagmoPINVOKE.sea_set_verbosity(swigCPtr, level);
  }

  public uint get_verbosity() {
    uint ret = pagmoPINVOKE.sea_get_verbosity(swigCPtr);
    return ret;
  }

  public void set_seed(uint arg0) {
    pagmoPINVOKE.sea_set_seed(swigCPtr, arg0);
  }

  public uint get_seed() {
    uint ret = pagmoPINVOKE.sea_get_seed(swigCPtr);
    return ret;
  }

  public new string get_name() {
    string ret = pagmoPINVOKE.sea_get_name(swigCPtr);
    return ret;
  }

  public new string get_extra_info() {
    string ret = pagmoPINVOKE.sea_get_extra_info(swigCPtr);
    return ret;
  }

  public SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_int_unsigned_long_long_double_double_size_t_t_t get_log() {
    SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_int_unsigned_long_long_double_double_size_t_t_t ret = new SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_int_unsigned_long_long_double_double_size_t_t_t(pagmoPINVOKE.sea_get_log(swigCPtr), false);
    return ret;
  }

}

}