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
        if (handle == IntPtr.Zero)
        {
            throw new ArgumentException("Problem handle cannot be zero.", nameof(handle));
        }

        SetHandle(handle);
    }

    /// <summary>
    /// Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
    /// </summary>
    public override bool IsInvalid => handle == IntPtr.Zero;

    /// <summary>
    /// Releases the owned native `pagmo::problem` pointer.
    /// </summary>
    protected override bool ReleaseHandle()
    {
        NativeInterop.problem_delete(handle);
        return true;
    }
}

