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

public partial class xnes : algorithm {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;

  internal xnes(global::System.IntPtr cPtr, bool cMemoryOwn) : base(pagmoPINVOKE.xnes_SWIGUpcast(cPtr), cMemoryOwn) {
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(xnes obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  internal static global::System.Runtime.InteropServices.HandleRef swigRelease(xnes obj) {
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
          pagmoPINVOKE.delete_xnes(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      base.Dispose(disposing);
    }
  }

  public xnes(uint gen, double eta_mu, double eta_sigma, double eta_b, double sigma0, double ftol, double xtol, bool memory, bool force_bounds, uint seed) : this(pagmoPINVOKE.new_xnes__SWIG_0(gen, eta_mu, eta_sigma, eta_b, sigma0, ftol, xtol, memory, force_bounds, seed), true) {
  }

  public xnes(uint gen, double eta_mu, double eta_sigma, double eta_b, double sigma0, double ftol, double xtol, bool memory, bool force_bounds) : this(pagmoPINVOKE.new_xnes__SWIG_1(gen, eta_mu, eta_sigma, eta_b, sigma0, ftol, xtol, memory, force_bounds), true) {
  }

  public xnes(uint gen, double eta_mu, double eta_sigma, double eta_b, double sigma0, double ftol, double xtol, bool memory) : this(pagmoPINVOKE.new_xnes__SWIG_2(gen, eta_mu, eta_sigma, eta_b, sigma0, ftol, xtol, memory), true) {
  }

  public xnes(uint gen, double eta_mu, double eta_sigma, double eta_b, double sigma0, double ftol, double xtol) : this(pagmoPINVOKE.new_xnes__SWIG_3(gen, eta_mu, eta_sigma, eta_b, sigma0, ftol, xtol), true) {
  }

  public xnes(uint gen, double eta_mu, double eta_sigma, double eta_b, double sigma0, double ftol) : this(pagmoPINVOKE.new_xnes__SWIG_4(gen, eta_mu, eta_sigma, eta_b, sigma0, ftol), true) {
  }

  public xnes(uint gen, double eta_mu, double eta_sigma, double eta_b, double sigma0) : this(pagmoPINVOKE.new_xnes__SWIG_5(gen, eta_mu, eta_sigma, eta_b, sigma0), true) {
  }

  public xnes(uint gen, double eta_mu, double eta_sigma, double eta_b) : this(pagmoPINVOKE.new_xnes__SWIG_6(gen, eta_mu, eta_sigma, eta_b), true) {
  }

  public xnes(uint gen, double eta_mu, double eta_sigma) : this(pagmoPINVOKE.new_xnes__SWIG_7(gen, eta_mu, eta_sigma), true) {
  }

  public xnes(uint gen, double eta_mu) : this(pagmoPINVOKE.new_xnes__SWIG_8(gen, eta_mu), true) {
  }

  public xnes(uint gen) : this(pagmoPINVOKE.new_xnes__SWIG_9(gen), true) {
  }

  public xnes() : this(pagmoPINVOKE.new_xnes__SWIG_10(), true) {
  }

  public population evolve(population arg0) {
    population ret = new population(pagmoPINVOKE.xnes_evolve(swigCPtr, population.getCPtr(arg0)), true);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public void set_seed(uint arg0) {
    pagmoPINVOKE.xnes_set_seed(swigCPtr, arg0);
  }

  public uint get_seed() {
    uint ret = pagmoPINVOKE.xnes_get_seed(swigCPtr);
    return ret;
  }

  public new void set_verbosity(uint level) {
    pagmoPINVOKE.xnes_set_verbosity(swigCPtr, level);
  }

  public uint get_verbosity() {
    uint ret = pagmoPINVOKE.xnes_get_verbosity(swigCPtr);
    return ret;
  }

  public uint get_gen() {
    uint ret = pagmoPINVOKE.xnes_get_gen(swigCPtr);
    return ret;
  }

  public new string get_name() {
    string ret = pagmoPINVOKE.xnes_get_name(swigCPtr);
    return ret;
  }

  public new string get_extra_info() {
    string ret = pagmoPINVOKE.xnes_get_extra_info(swigCPtr);
    return ret;
  }

  public SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_int_unsigned_long_long_double_double_double_double_t_t get_log() {
    SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_int_unsigned_long_long_double_double_double_double_t_t ret = new SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_int_unsigned_long_long_double_double_double_double_t_t(pagmoPINVOKE.xnes_get_log(swigCPtr), false);
    return ret;
  }

}

}