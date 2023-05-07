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

public partial class compass_search : algorithm {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;

  internal compass_search(global::System.IntPtr cPtr, bool cMemoryOwn) : base(pagmoPINVOKE.compass_search_SWIGUpcast(cPtr), cMemoryOwn) {
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(compass_search obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  internal static global::System.Runtime.InteropServices.HandleRef swigRelease(compass_search obj) {
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
          pagmoPINVOKE.delete_compass_search(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      base.Dispose(disposing);
    }
  }

  public compass_search(uint max_fevals, double start_range, double stop_range, double reduction_coeff) : this(pagmoPINVOKE.new_compass_search__SWIG_0(max_fevals, start_range, stop_range, reduction_coeff), true) {
  }

  public compass_search(uint max_fevals, double start_range, double stop_range) : this(pagmoPINVOKE.new_compass_search__SWIG_1(max_fevals, start_range, stop_range), true) {
  }

  public compass_search(uint max_fevals, double start_range) : this(pagmoPINVOKE.new_compass_search__SWIG_2(max_fevals, start_range), true) {
  }

  public compass_search(uint max_fevals) : this(pagmoPINVOKE.new_compass_search__SWIG_3(max_fevals), true) {
  }

  public compass_search() : this(pagmoPINVOKE.new_compass_search__SWIG_4(), true) {
  }

  public population evolve(population arg0) {
    population ret = new population(pagmoPINVOKE.compass_search_evolve(swigCPtr, population.getCPtr(arg0)), true);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public new void set_verbosity(uint level) {
    pagmoPINVOKE.compass_search_set_verbosity(swigCPtr, level);
  }

  public uint get_verbosity() {
    uint ret = pagmoPINVOKE.compass_search_get_verbosity(swigCPtr);
    return ret;
  }

  public double get_max_fevals() {
    double ret = pagmoPINVOKE.compass_search_get_max_fevals(swigCPtr);
    return ret;
  }

  public double get_stop_range() {
    double ret = pagmoPINVOKE.compass_search_get_stop_range(swigCPtr);
    return ret;
  }

  public double get_start_range() {
    double ret = pagmoPINVOKE.compass_search_get_start_range(swigCPtr);
    return ret;
  }

  public double get_reduction_coeff() {
    double ret = pagmoPINVOKE.compass_search_get_reduction_coeff(swigCPtr);
    return ret;
  }

  public new string get_name() {
    string ret = pagmoPINVOKE.compass_search_get_name(swigCPtr);
    return ret;
  }

  public new string get_extra_info() {
    string ret = pagmoPINVOKE.compass_search_get_extra_info(swigCPtr);
    return ret;
  }

  public SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_long_long_double_size_t_double_double_t_t get_log() {
    SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_long_long_double_size_t_double_double_t_t ret = new SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_long_long_double_size_t_double_double_t_t(pagmoPINVOKE.compass_search_get_log(swigCPtr), false);
    return ret;
  }

  public void set_seed(uint arg0) {
    pagmoPINVOKE.compass_search_set_seed(swigCPtr, arg0);
  }

  public uint get_seed() {
    uint ret = pagmoPINVOKE.compass_search_get_seed(swigCPtr);
    return ret;
  }

  public uint get_gen() {
    uint ret = pagmoPINVOKE.compass_search_get_gen(swigCPtr);
    return ret;
  }

}

}
