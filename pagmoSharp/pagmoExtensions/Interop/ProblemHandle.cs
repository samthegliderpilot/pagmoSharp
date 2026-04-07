using System;
using System.Runtime.InteropServices;

namespace pagmo;

/// <summary>
/// Owns a native pagmo::problem pointer allocated by the managed callback bridge.
/// Ensures deterministic and exception-safe deletion through SafeHandle semantics.
/// </summary>
internal sealed class ProblemHandle : SafeHandle
{
    internal ProblemHandle(IntPtr handle)
        : base(IntPtr.Zero, ownsHandle: true)
    {
        SetHandle(handle);
    }

    public override bool IsInvalid => handle == IntPtr.Zero;

    protected override bool ReleaseHandle()
    {
        NativeInterop.problem_delete(handle);
        return true;
    }
}
