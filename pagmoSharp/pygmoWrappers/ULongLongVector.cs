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

public class ULongLongVector : global::System.IDisposable, global::System.Collections.IEnumerable, global::System.Collections.Generic.IList<ulong>
 {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal ULongLongVector(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(ULongLongVector obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~ULongLongVector() {
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
          pagmoPINVOKE.delete_ULongLongVector(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
    }
  }

  public ULongLongVector(global::System.Collections.IEnumerable c) : this() {
    if (c == null)
      throw new global::System.ArgumentNullException("c");
    foreach (ulong element in c) {
      this.Add(element);
    }
  }

  public ULongLongVector(global::System.Collections.Generic.IEnumerable<ulong> c) : this() {
    if (c == null)
      throw new global::System.ArgumentNullException("c");
    foreach (ulong element in c) {
      this.Add(element);
    }
  }

  public bool IsFixedSize {
    get {
      return false;
    }
  }

  public bool IsReadOnly {
    get {
      return false;
    }
  }

  public ulong this[int index]  {
    get {
      return getitem(index);
    }
    set {
      setitem(index, value);
    }
  }

  public int Capacity {
    get {
      return (int)capacity();
    }
    set {
      if (value < size())
        throw new global::System.ArgumentOutOfRangeException("Capacity");
      reserve((uint)value);
    }
  }

  public int Count {
    get {
      return (int)size();
    }
  }

  public bool IsSynchronized {
    get {
      return false;
    }
  }

  public void CopyTo(ulong[] array)
  {
    CopyTo(0, array, 0, this.Count);
  }

  public void CopyTo(ulong[] array, int arrayIndex)
  {
    CopyTo(0, array, arrayIndex, this.Count);
  }

  public void CopyTo(int index, ulong[] array, int arrayIndex, int count)
  {
    if (array == null)
      throw new global::System.ArgumentNullException("array");
    if (index < 0)
      throw new global::System.ArgumentOutOfRangeException("index", "Value is less than zero");
    if (arrayIndex < 0)
      throw new global::System.ArgumentOutOfRangeException("arrayIndex", "Value is less than zero");
    if (count < 0)
      throw new global::System.ArgumentOutOfRangeException("count", "Value is less than zero");
    if (array.Rank > 1)
      throw new global::System.ArgumentException("Multi dimensional array.", "array");
    if (index+count > this.Count || arrayIndex+count > array.Length)
      throw new global::System.ArgumentException("Number of elements to copy is too large.");
    for (int i=0; i<count; i++)
      array.SetValue(getitemcopy(index+i), arrayIndex+i);
  }

  public ulong[] ToArray() {
    ulong[] array = new ulong[this.Count];
    this.CopyTo(array);
    return array;
  }

  global::System.Collections.Generic.IEnumerator<ulong> global::System.Collections.Generic.IEnumerable<ulong>.GetEnumerator() {
    return new ULongLongVectorEnumerator(this);
  }

  global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator() {
    return new ULongLongVectorEnumerator(this);
  }

  public ULongLongVectorEnumerator GetEnumerator() {
    return new ULongLongVectorEnumerator(this);
  }

  // Type-safe enumerator
  /// Note that the IEnumerator documentation requires an InvalidOperationException to be thrown
  /// whenever the collection is modified. This has been done for changes in the size of the
  /// collection but not when one of the elements of the collection is modified as it is a bit
  /// tricky to detect unmanaged code that modifies the collection under our feet.
  public sealed class ULongLongVectorEnumerator : global::System.Collections.IEnumerator
    , global::System.Collections.Generic.IEnumerator<ulong>
  {
    private ULongLongVector collectionRef;
    private int currentIndex;
    private object currentObject;
    private int currentSize;

    public ULongLongVectorEnumerator(ULongLongVector collection) {
      collectionRef = collection;
      currentIndex = -1;
      currentObject = null;
      currentSize = collectionRef.Count;
    }

    // Type-safe iterator Current
    public ulong Current {
      get {
        if (currentIndex == -1)
          throw new global::System.InvalidOperationException("Enumeration not started.");
        if (currentIndex > currentSize - 1)
          throw new global::System.InvalidOperationException("Enumeration finished.");
        if (currentObject == null)
          throw new global::System.InvalidOperationException("Collection modified.");
        return (ulong)currentObject;
      }
    }

    // Type-unsafe IEnumerator.Current
    object global::System.Collections.IEnumerator.Current {
      get {
        return Current;
      }
    }

    public bool MoveNext() {
      int size = collectionRef.Count;
      bool moveOkay = (currentIndex+1 < size) && (size == currentSize);
      if (moveOkay) {
        currentIndex++;
        currentObject = collectionRef[currentIndex];
      } else {
        currentObject = null;
      }
      return moveOkay;
    }

    public void Reset() {
      currentIndex = -1;
      currentObject = null;
      if (collectionRef.Count != currentSize) {
        throw new global::System.InvalidOperationException("Collection modified.");
      }
    }

    public void Dispose() {
        currentIndex = -1;
        currentObject = null;
    }
  }

  public void Clear() {
    pagmoPINVOKE.ULongLongVector_Clear(swigCPtr);
  }

  public void Add(ulong x) {
    pagmoPINVOKE.ULongLongVector_Add(swigCPtr, x);
  }

  private uint size() {
    uint ret = pagmoPINVOKE.ULongLongVector_size(swigCPtr);
    return ret;
  }

  private uint capacity() {
    uint ret = pagmoPINVOKE.ULongLongVector_capacity(swigCPtr);
    return ret;
  }

  private void reserve(uint n) {
    pagmoPINVOKE.ULongLongVector_reserve(swigCPtr, n);
  }

  public ULongLongVector() : this(pagmoPINVOKE.new_ULongLongVector__SWIG_0(), true) {
  }

  public ULongLongVector(ULongLongVector other) : this(pagmoPINVOKE.new_ULongLongVector__SWIG_1(ULongLongVector.getCPtr(other)), true) {
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  public ULongLongVector(int capacity) : this(pagmoPINVOKE.new_ULongLongVector__SWIG_2(capacity), true) {
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  private ulong getitemcopy(int index) {
    ulong ret = pagmoPINVOKE.ULongLongVector_getitemcopy(swigCPtr, index);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  private ulong getitem(int index) {
    ulong ret = pagmoPINVOKE.ULongLongVector_getitem(swigCPtr, index);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  private void setitem(int index, ulong val) {
    pagmoPINVOKE.ULongLongVector_setitem(swigCPtr, index, val);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  public void AddRange(ULongLongVector values) {
    pagmoPINVOKE.ULongLongVector_AddRange(swigCPtr, ULongLongVector.getCPtr(values));
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  public ULongLongVector GetRange(int index, int count) {
    global::System.IntPtr cPtr = pagmoPINVOKE.ULongLongVector_GetRange(swigCPtr, index, count);
    ULongLongVector ret = (cPtr == global::System.IntPtr.Zero) ? null : new ULongLongVector(cPtr, true);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public void Insert(int index, ulong x) {
    pagmoPINVOKE.ULongLongVector_Insert(swigCPtr, index, x);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  public void InsertRange(int index, ULongLongVector values) {
    pagmoPINVOKE.ULongLongVector_InsertRange(swigCPtr, index, ULongLongVector.getCPtr(values));
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  public void RemoveAt(int index) {
    pagmoPINVOKE.ULongLongVector_RemoveAt(swigCPtr, index);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  public void RemoveRange(int index, int count) {
    pagmoPINVOKE.ULongLongVector_RemoveRange(swigCPtr, index, count);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  public static ULongLongVector Repeat(ulong value, int count) {
    global::System.IntPtr cPtr = pagmoPINVOKE.ULongLongVector_Repeat(value, count);
    ULongLongVector ret = (cPtr == global::System.IntPtr.Zero) ? null : new ULongLongVector(cPtr, true);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
    return ret;
  }

  public void Reverse() {
    pagmoPINVOKE.ULongLongVector_Reverse__SWIG_0(swigCPtr);
  }

  public void Reverse(int index, int count) {
    pagmoPINVOKE.ULongLongVector_Reverse__SWIG_1(swigCPtr, index, count);
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  public void SetRange(int index, ULongLongVector values) {
    pagmoPINVOKE.ULongLongVector_SetRange(swigCPtr, index, ULongLongVector.getCPtr(values));
    if (pagmoPINVOKE.SWIGPendingException.Pending) throw pagmoPINVOKE.SWIGPendingException.Retrieve();
  }

  public bool Contains(ulong value) {
    bool ret = pagmoPINVOKE.ULongLongVector_Contains(swigCPtr, value);
    return ret;
  }

  public int IndexOf(ulong value) {
    int ret = pagmoPINVOKE.ULongLongVector_IndexOf(swigCPtr, value);
    return ret;
  }

  public int LastIndexOf(ulong value) {
    int ret = pagmoPINVOKE.ULongLongVector_LastIndexOf(swigCPtr, value);
    return ret;
  }

  public bool Remove(ulong value) {
    bool ret = pagmoPINVOKE.ULongLongVector_Remove(swigCPtr, value);
    return ret;
  }

}

}
