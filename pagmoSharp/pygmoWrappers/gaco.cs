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

public partial class gaco : algorithm {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;

  internal gaco(global::System.IntPtr cPtr, bool cMemoryOwn) : base(pagmoPINVOKE.gaco_SWIGUpcast(cPtr), cMemoryOwn) {
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(gaco obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  internal static global::System.Runtime.InteropServices.HandleRef swigRelease(gaco obj) {
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
          pagmoPINVOKE.delete_gaco(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      base.Dispose(disposing);
    }
  }

  public gaco(uint gen, uint ker, double q, double oracle, double acc, uint threshold, uint n_gen_mark, uint impstop, uint evalstop, double focus, bool memory, uint seed) : this(pagmoPINVOKE.new_gaco__SWIG_0(gen, ker, q, oracle, acc, threshold, n_gen_mark, impstop, evalstop, focus, memory, seed), true) {
  }

  public gaco(uint gen, uint ker, double q, double oracle, double acc, uint threshold, uint n_gen_mark, uint impstop, uint evalstop, double focus, bool memory) : this(pagmoPINVOKE.new_gaco__SWIG_1(gen, ker, q, oracle, acc, threshold, n_gen_mark, impstop, evalstop, focus, memory), true) {
  }

  public gaco(uint gen, uint ker, double q, double oracle, double acc, uint threshold, uint n_gen_mark, uint impstop, uint evalstop, double focus) : this(pagmoPINVOKE.new_gaco__SWIG_2(gen, ker, q, oracle, acc, threshold, n_gen_mark, impstop, evalstop, focus), true) {
  }

  public gaco(uint gen, uint ker, double q, double oracle, double acc, uint threshold, uint n_gen_mark, uint impstop, uint evalstop) : this(pagmoPINVOKE.new_gaco__SWIG_3(gen, ker, q, oracle, acc, threshold, n_gen_mark, impstop, evalstop), true) {
  }

  public gaco(uint gen, uint ker, double q, double oracle, double acc, uint threshold, uint n_gen_mark, uint impstop) : this(pagmoPINVOKE.new_gaco__SWIG_4(gen, ker, q, oracle, acc, threshold, n_gen_mark, impstop), true) {
  }

  public gaco(uint gen, uint ker, double q, double oracle, double acc, uint threshold, uint n_gen_mark) : this(pagmoPINVOKE.new_gaco__SWIG_5(gen, ker, q, oracle, acc, threshold, n_gen_mark), true) {
  }

  public gaco(uint gen, uint ker, double q, double oracle, double acc, uint threshold) : this(pagmoPINVOKE.new_gaco__SWIG_6(gen, ker, q, oracle, acc, threshold), true) {
  }

  public gaco(uint gen, uint ker, double q, double oracle, double acc) : this(pagmoPINVOKE.new_gaco__SWIG_7(gen, ker, q, oracle, acc), true) {
  }

  public gaco(uint gen, uint ker, double q, double oracle) : this(pagmoPINVOKE.new_gaco__SWIG_8(gen, ker, q, oracle), true) {
  }

  public gaco(uint gen, uint ker, double q) : this(pagmoPINVOKE.new_gaco__SWIG_9(gen, ker, q), true) {
  }

  public gaco(uint gen, uint ker) : this(pagmoPINVOKE.new_gaco__SWIG_10(gen, ker), true) {
  }

  public gaco(uint gen) : this(pagmoPINVOKE.new_gaco__SWIG_11(gen), true) {
  }

  public gaco() : this(pagmoPINVOKE.new_gaco__SWIG_12(), true) {
  }

  public uint get_gen() {
    uint ret = pagmoPINVOKE.gaco_get_gen(swigCPtr);
    return ret;
  }

  public virtual population evolve(population arg0) {
    population ret = new population(pagmoPINVOKE.gaco_evolve(swigCPtr, population.getCPtr(arg0)), true);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public virtual string get_name() {
    string ret = pagmoPINVOKE.gaco_get_name(swigCPtr);
    return ret;
  }

  public void set_seed(uint arg0) {
    pagmoPINVOKE.gaco_set_seed(swigCPtr, arg0);
  }

  public uint get_seed() {
    uint ret = pagmoPINVOKE.gaco_get_seed(swigCPtr);
    return ret;
  }

  public uint get_verbosity() {
    uint ret = pagmoPINVOKE.gaco_get_verbosity(swigCPtr);
    return ret;
  }

  public void set_verbosity(uint arg0) {
    pagmoPINVOKE.gaco_set_verbosity(swigCPtr, arg0);
  }

  public void set_bfe(bfe b) {
    pagmoPINVOKE.gaco_set_bfe(swigCPtr, bfe.getCPtr(b));
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  public string get_extra_info() {
    string ret = pagmoPINVOKE.gaco_get_extra_info(swigCPtr);
    return ret;
  }

  public SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_int_size_t_double_unsigned_int_double_double_double_t_t get_log() {
    SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_int_size_t_double_unsigned_int_double_double_double_t_t ret = new SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_int_size_t_double_unsigned_int_double_double_double_t_t(pagmoPINVOKE.gaco_get_log(swigCPtr), false);
    return ret;
  }

}

}
