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

public partial class sade : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal sade(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(sade obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  internal static global::System.Runtime.InteropServices.HandleRef swigRelease(sade obj) {
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

  ~sade() {
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
          pagmoPINVOKE.delete_sade(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public sade(uint gen, uint variant, uint variant_adptv, double ftol, double xtol, bool memory, uint seed) : this(pagmoPINVOKE.new_sade__SWIG_0(gen, variant, variant_adptv, ftol, xtol, memory, seed), true) {
  }

  public sade(uint gen, uint variant, uint variant_adptv, double ftol, double xtol, bool memory) : this(pagmoPINVOKE.new_sade__SWIG_1(gen, variant, variant_adptv, ftol, xtol, memory), true) {
  }

  public sade(uint gen, uint variant, uint variant_adptv, double ftol, double xtol) : this(pagmoPINVOKE.new_sade__SWIG_2(gen, variant, variant_adptv, ftol, xtol), true) {
  }

  public sade(uint gen, uint variant, uint variant_adptv, double ftol) : this(pagmoPINVOKE.new_sade__SWIG_3(gen, variant, variant_adptv, ftol), true) {
  }

  public sade(uint gen, uint variant, uint variant_adptv) : this(pagmoPINVOKE.new_sade__SWIG_4(gen, variant, variant_adptv), true) {
  }

  public sade(uint gen, uint variant) : this(pagmoPINVOKE.new_sade__SWIG_5(gen, variant), true) {
  }

  public sade(uint gen) : this(pagmoPINVOKE.new_sade__SWIG_6(gen), true) {
  }

  public sade() : this(pagmoPINVOKE.new_sade__SWIG_7(), true) {
  }

  public population evolve(population arg0) {
    population ret = new population(pagmoPINVOKE.sade_evolve(swigCPtr, population.getCPtr(arg0)), true);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public string get_name() {
    string ret = pagmoPINVOKE.sade_get_name(swigCPtr);
    return ret;
  }

  public void set_seed(uint arg0) {
    pagmoPINVOKE.sade_set_seed(swigCPtr, arg0);
  }

  public uint get_seed() {
    uint ret = pagmoPINVOKE.sade_get_seed(swigCPtr);
    return ret;
  }

  public uint get_verbosity() {
    uint ret = pagmoPINVOKE.sade_get_verbosity(swigCPtr);
    return ret;
  }

  public void set_verbosity(uint arg0) {
    pagmoPINVOKE.sade_set_verbosity(swigCPtr, arg0);
  }

  public uint get_gen() {
    uint ret = pagmoPINVOKE.sade_get_gen(swigCPtr);
    return ret;
  }

  public string get_extra_info() {
    string ret = pagmoPINVOKE.sade_get_extra_info(swigCPtr);
    return ret;
  }

  public SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_int_unsigned_long_long_double_double_double_double_double_t_t get_log() {
    SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_int_unsigned_long_long_double_double_double_double_double_t_t ret = new SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_int_unsigned_long_long_double_double_double_double_double_t_t(pagmoPINVOKE.sade_get_log(swigCPtr), false);
    return ret;
  }

}

}
