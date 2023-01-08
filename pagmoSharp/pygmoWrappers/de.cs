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

public partial class de : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal de(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(de obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  internal static global::System.Runtime.InteropServices.HandleRef swigRelease(de obj) {
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

  ~de() {
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
          pagmoPINVOKE.delete_de(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public de(uint gen, double F, double CR, uint variant, double ftol, double xtol, uint seed) : this(pagmoPINVOKE.new_de__SWIG_0(gen, F, CR, variant, ftol, xtol, seed), true) {
  }

  public de(uint gen, double F, double CR, uint variant, double ftol, double xtol) : this(pagmoPINVOKE.new_de__SWIG_1(gen, F, CR, variant, ftol, xtol), true) {
  }

  public de(uint gen, double F, double CR, uint variant, double ftol) : this(pagmoPINVOKE.new_de__SWIG_2(gen, F, CR, variant, ftol), true) {
  }

  public de(uint gen, double F, double CR, uint variant) : this(pagmoPINVOKE.new_de__SWIG_3(gen, F, CR, variant), true) {
  }

  public de(uint gen, double F, double CR) : this(pagmoPINVOKE.new_de__SWIG_4(gen, F, CR), true) {
  }

  public de(uint gen, double F) : this(pagmoPINVOKE.new_de__SWIG_5(gen, F), true) {
  }

  public de(uint gen) : this(pagmoPINVOKE.new_de__SWIG_6(gen), true) {
  }

  public de() : this(pagmoPINVOKE.new_de__SWIG_7(), true) {
  }

  public population evolve(population arg0) {
    population ret = new population(pagmoPINVOKE.de_evolve(swigCPtr, population.getCPtr(arg0)), true);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public void set_seed(uint arg0) {
    pagmoPINVOKE.de_set_seed(swigCPtr, arg0);
  }

  public uint get_seed() {
    uint ret = pagmoPINVOKE.de_get_seed(swigCPtr);
    return ret;
  }

  public void set_verbosity(uint level) {
    pagmoPINVOKE.de_set_verbosity(swigCPtr, level);
  }

  public uint get_verbosity() {
    uint ret = pagmoPINVOKE.de_get_verbosity(swigCPtr);
    return ret;
  }

  public uint get_gen() {
    uint ret = pagmoPINVOKE.de_get_gen(swigCPtr);
    return ret;
  }

  public string get_name() {
    string ret = pagmoPINVOKE.de_get_name(swigCPtr);
    return ret;
  }

  public string get_extra_info() {
    string ret = pagmoPINVOKE.de_get_extra_info(swigCPtr);
    return ret;
  }

  public SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_int_unsigned_long_long_double_double_double_t_t get_log() {
    SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_int_unsigned_long_long_double_double_double_t_t ret = new SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_int_unsigned_long_long_double_double_double_t_t(pagmoPINVOKE.de_get_log(swigCPtr), false);
    return ret;
  }

}

}
