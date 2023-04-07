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

public partial class simulated_annealing : algorithm {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;

  internal simulated_annealing(global::System.IntPtr cPtr, bool cMemoryOwn) : base(pagmoPINVOKE.simulated_annealing_SWIGUpcast(cPtr), cMemoryOwn) {
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(simulated_annealing obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  internal static global::System.Runtime.InteropServices.HandleRef swigRelease(simulated_annealing obj) {
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
          pagmoPINVOKE.delete_simulated_annealing(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      base.Dispose(disposing);
    }
  }

  public simulated_annealing(double Ts, double Tf, uint n_T_adj, uint n_range_adj, uint bin_size, double start_range, uint seed) : this(pagmoPINVOKE.new_simulated_annealing__SWIG_0(Ts, Tf, n_T_adj, n_range_adj, bin_size, start_range, seed), true) {
  }

  public simulated_annealing(double Ts, double Tf, uint n_T_adj, uint n_range_adj, uint bin_size, double start_range) : this(pagmoPINVOKE.new_simulated_annealing__SWIG_1(Ts, Tf, n_T_adj, n_range_adj, bin_size, start_range), true) {
  }

  public simulated_annealing(double Ts, double Tf, uint n_T_adj, uint n_range_adj, uint bin_size) : this(pagmoPINVOKE.new_simulated_annealing__SWIG_2(Ts, Tf, n_T_adj, n_range_adj, bin_size), true) {
  }

  public simulated_annealing(double Ts, double Tf, uint n_T_adj, uint n_range_adj) : this(pagmoPINVOKE.new_simulated_annealing__SWIG_3(Ts, Tf, n_T_adj, n_range_adj), true) {
  }

  public simulated_annealing(double Ts, double Tf, uint n_T_adj) : this(pagmoPINVOKE.new_simulated_annealing__SWIG_4(Ts, Tf, n_T_adj), true) {
  }

  public simulated_annealing(double Ts, double Tf) : this(pagmoPINVOKE.new_simulated_annealing__SWIG_5(Ts, Tf), true) {
  }

  public simulated_annealing(double Ts) : this(pagmoPINVOKE.new_simulated_annealing__SWIG_6(Ts), true) {
  }

  public simulated_annealing() : this(pagmoPINVOKE.new_simulated_annealing__SWIG_7(), true) {
  }

  public population evolve(population arg0) {
    population ret = new population(pagmoPINVOKE.simulated_annealing_evolve(swigCPtr, population.getCPtr(arg0)), true);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public new void set_verbosity(uint level) {
    pagmoPINVOKE.simulated_annealing_set_verbosity(swigCPtr, level);
  }

  public uint get_verbosity() {
    uint ret = pagmoPINVOKE.simulated_annealing_get_verbosity(swigCPtr);
    return ret;
  }

  public void set_seed(uint arg0) {
    pagmoPINVOKE.simulated_annealing_set_seed(swigCPtr, arg0);
  }

  public uint get_seed() {
    uint ret = pagmoPINVOKE.simulated_annealing_get_seed(swigCPtr);
    return ret;
  }

  public new string get_name() {
    string ret = pagmoPINVOKE.simulated_annealing_get_name(swigCPtr);
    return ret;
  }

  public new string get_extra_info() {
    string ret = pagmoPINVOKE.simulated_annealing_get_extra_info(swigCPtr);
    return ret;
  }

  public SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_long_long_double_double_double_double_t_t get_log() {
    SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_long_long_double_double_double_double_t_t ret = new SWIGTYPE_p_std__vectorT_std__tupleT_unsigned_long_long_double_double_double_double_t_t(pagmoPINVOKE.simulated_annealing_get_log(swigCPtr), false);
    return ret;
  }

}

}
