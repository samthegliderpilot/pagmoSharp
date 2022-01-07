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

public class problemBase : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal problemBase(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(problemBase obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~problemBase() {
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
          pagmoPINVOKE.delete_problemBase(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public virtual DoubleVector fitness(DoubleVector arg0) {
    DoubleVector ret = new DoubleVector((SwigDerivedClassHasMethod("fitness", swigMethodTypes0) ? pagmoPINVOKE.problemBase_fitnessSwigExplicitproblemBase(swigCPtr, DoubleVector.getCPtr(arg0)) : pagmoPINVOKE.problemBase_fitness(swigCPtr, DoubleVector.getCPtr(arg0))), true);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public virtual PairOfDoubleVectors get_bounds() {
    PairOfDoubleVectors ret = new PairOfDoubleVectors((SwigDerivedClassHasMethod("get_bounds", swigMethodTypes1) ? pagmoPINVOKE.problemBase_get_boundsSwigExplicitproblemBase(swigCPtr) : pagmoPINVOKE.problemBase_get_bounds(swigCPtr)), true);
    return ret;
  }

  public virtual bool has_batch_fitness() {
    bool ret = (SwigDerivedClassHasMethod("has_batch_fitness", swigMethodTypes2) ? pagmoPINVOKE.problemBase_has_batch_fitnessSwigExplicitproblemBase(swigCPtr) : pagmoPINVOKE.problemBase_has_batch_fitness(swigCPtr));
    return ret;
  }

  public virtual string get_name() {
    string ret = (SwigDerivedClassHasMethod("get_name", swigMethodTypes3) ? pagmoPINVOKE.problemBase_get_nameSwigExplicitproblemBase(swigCPtr) : pagmoPINVOKE.problemBase_get_name(swigCPtr));
    return ret;
  }

  public problemBase() : this(pagmoPINVOKE.new_problemBase(), true) {
    SwigDirectorConnect();
  }

  private void SwigDirectorConnect() {
    if (SwigDerivedClassHasMethod("fitness", swigMethodTypes0))
      swigDelegate0 = new SwigDelegateproblemBase_0(SwigDirectorMethodfitness);
    if (SwigDerivedClassHasMethod("get_bounds", swigMethodTypes1))
      swigDelegate1 = new SwigDelegateproblemBase_1(SwigDirectorMethodget_bounds);
    if (SwigDerivedClassHasMethod("has_batch_fitness", swigMethodTypes2))
      swigDelegate2 = new SwigDelegateproblemBase_2(SwigDirectorMethodhas_batch_fitness);
    if (SwigDerivedClassHasMethod("get_name", swigMethodTypes3))
      swigDelegate3 = new SwigDelegateproblemBase_3(SwigDirectorMethodget_name);
    pagmoPINVOKE.problemBase_director_connect(swigCPtr, swigDelegate0, swigDelegate1, swigDelegate2, swigDelegate3);
  }

  private bool SwigDerivedClassHasMethod(string methodName, global::System.Type[] methodTypes) {
    global::System.Reflection.MethodInfo methodInfo = this.GetType().GetMethod(methodName, global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic | global::System.Reflection.BindingFlags.Instance, null, methodTypes, null);
    bool hasDerivedMethod = methodInfo.DeclaringType.IsSubclassOf(typeof(problemBase));
    return hasDerivedMethod;
  }

  private global::System.IntPtr SwigDirectorMethodfitness(global::System.IntPtr arg0) {
    return DoubleVector.getCPtr(fitness(new DoubleVector(arg0, false))).Handle;
  }

  private global::System.IntPtr SwigDirectorMethodget_bounds() {
    return PairOfDoubleVectors.getCPtr(get_bounds()).Handle;
  }

  private bool SwigDirectorMethodhas_batch_fitness() {
    return has_batch_fitness();
  }

  private string SwigDirectorMethodget_name() {
    return get_name();
  }

  public delegate global::System.IntPtr SwigDelegateproblemBase_0(global::System.IntPtr arg0);
  public delegate global::System.IntPtr SwigDelegateproblemBase_1();
  public delegate bool SwigDelegateproblemBase_2();
  public delegate string SwigDelegateproblemBase_3();

  private SwigDelegateproblemBase_0 swigDelegate0;
  private SwigDelegateproblemBase_1 swigDelegate1;
  private SwigDelegateproblemBase_2 swigDelegate2;
  private SwigDelegateproblemBase_3 swigDelegate3;

  private static global::System.Type[] swigMethodTypes0 = new global::System.Type[] { typeof(DoubleVector) };
  private static global::System.Type[] swigMethodTypes1 = new global::System.Type[] {  };
  private static global::System.Type[] swigMethodTypes2 = new global::System.Type[] {  };
  private static global::System.Type[] swigMethodTypes3 = new global::System.Type[] {  };
}

}
