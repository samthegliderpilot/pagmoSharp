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

public partial class de1220 : algorithm {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;

  internal de1220(global::System.IntPtr cPtr, bool cMemoryOwn) : base(pagmoPINVOKE.de1220_SWIGUpcast(cPtr), cMemoryOwn) {
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(de1220 obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  internal static global::System.Runtime.InteropServices.HandleRef swigRelease(de1220 obj) {
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
          pagmoPINVOKE.delete_de1220(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      base.Dispose(disposing);
    }
  }

  public de1220(uint gen, SWIGTYPE_p_std__vectorT_unsigned_int_t allowed_variants, uint variant_adptv, double ftol, double xtol, bool memory, uint seed) : this(pagmoPINVOKE.new_de1220__SWIG_0(gen, SWIGTYPE_p_std__vectorT_unsigned_int_t.getCPtr(allowed_variants), variant_adptv, ftol, xtol, memory, seed), true) {
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  public de1220(uint gen, SWIGTYPE_p_std__vectorT_unsigned_int_t allowed_variants, uint variant_adptv, double ftol, double xtol, bool memory) : this(pagmoPINVOKE.new_de1220__SWIG_1(gen, SWIGTYPE_p_std__vectorT_unsigned_int_t.getCPtr(allowed_variants), variant_adptv, ftol, xtol, memory), true) {
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  public de1220(uint gen, SWIGTYPE_p_std__vectorT_unsigned_int_t allowed_variants, uint variant_adptv, double ftol, double xtol) : this(pagmoPINVOKE.new_de1220__SWIG_2(gen, SWIGTYPE_p_std__vectorT_unsigned_int_t.getCPtr(allowed_variants), variant_adptv, ftol, xtol), true) {
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  public de1220(uint gen, SWIGTYPE_p_std__vectorT_unsigned_int_t allowed_variants, uint variant_adptv, double ftol) : this(pagmoPINVOKE.new_de1220__SWIG_3(gen, SWIGTYPE_p_std__vectorT_unsigned_int_t.getCPtr(allowed_variants), variant_adptv, ftol), true) {
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  public de1220(uint gen, SWIGTYPE_p_std__vectorT_unsigned_int_t allowed_variants, uint variant_adptv) : this(pagmoPINVOKE.new_de1220__SWIG_4(gen, SWIGTYPE_p_std__vectorT_unsigned_int_t.getCPtr(allowed_variants), variant_adptv), true) {
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  public de1220(uint gen, SWIGTYPE_p_std__vectorT_unsigned_int_t allowed_variants) : this(pagmoPINVOKE.new_de1220__SWIG_5(gen, SWIGTYPE_p_std__vectorT_unsigned_int_t.getCPtr(allowed_variants)), true) {
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  public de1220(uint gen) : this(pagmoPINVOKE.new_de1220__SWIG_6(gen), true) {
  }

  public de1220() : this(pagmoPINVOKE.new_de1220__SWIG_7(), true) {
  }

  public population evolve(population arg0) {
    population ret = new population(pagmoPINVOKE.de1220_evolve(swigCPtr, population.getCPtr(arg0)), true);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public new string get_name() {
    string ret = pagmoPINVOKE.de1220_get_name(swigCPtr);
    return ret;
  }

  public void set_seed(uint arg0) {
    pagmoPINVOKE.de1220_set_seed(swigCPtr, arg0);
  }

  public uint get_gen() {
    uint ret = pagmoPINVOKE.de1220_get_gen(swigCPtr);
    return ret;
  }

  public uint get_seed() {
    uint ret = pagmoPINVOKE.de1220_get_seed(swigCPtr);
    return ret;
  }

  public uint get_verbosity() {
    uint ret = pagmoPINVOKE.de1220_get_verbosity(swigCPtr);
    return ret;
  }

  public new void set_verbosity(uint arg0) {
    pagmoPINVOKE.de1220_set_verbosity(swigCPtr, arg0);
  }

  public new string get_extra_info() {
    string ret = pagmoPINVOKE.de1220_get_extra_info(swigCPtr);
    return ret;
  }

  public SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_int_unsigned_long_long_double_double_double_unsigned_int_double_double_t_t get_log() {
    SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_int_unsigned_long_long_double_double_double_unsigned_int_double_double_t_t ret = new SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_int_unsigned_long_long_double_double_double_unsigned_int_double_double_t_t(pagmoPINVOKE.de1220_get_log(swigCPtr), false);
    return ret;
  }

}

}
