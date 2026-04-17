using System;
using System.Runtime.InteropServices;

namespace pagmo;

/// <summary>
/// Managed wrapper that registers a C# <see cref="RPolicyCallback"/> subclass as a pagmo
/// replacement policy. Transfers ownership of the callback to the native side.
/// </summary>
public sealed class r_policy : ManagedRPolicy
{
    /// <summary>
    /// Creates an empty replacement-policy wrapper.
    /// </summary>
    public r_policy()
    {
    }

    /// <summary>
    /// Creates an <see cref="r_policy"/> backed by the given <paramref name="basePolicy"/> callback.
    /// Ownership of <paramref name="basePolicy"/> is transferred to the native side.
    /// </summary>
    public r_policy(RPolicyCallback basePolicy)
        : base()
    {
        var transferred = TransferOwnership(basePolicy);
        try
        {
            setBasePolicy(transferred);
        }
        catch
        {
            // If transfer succeeded but native wrapper rejected the policy, release to avoid leaking.
            DeleteTransferredPolicy(transferred);
            throw;
        }
    }

    private static RPolicyCallback TransferOwnership(RPolicyCallback basePolicy)
    {
        if (basePolicy == null)
        {
            throw new ArgumentNullException(nameof(basePolicy));
        }

        var currentPtr = RPolicyCallback.getCPtr(basePolicy);
        if (currentPtr.Handle == IntPtr.Zero)
        {
            throw new ObjectDisposedException(nameof(basePolicy), "The provided replacement policy has already been disposed.");
        }

        var released = RPolicyCallback.swigRelease(basePolicy);
        if (released.Handle == IntPtr.Zero)
        {
            throw new ObjectDisposedException(nameof(basePolicy), "The provided replacement policy could not transfer ownership.");
        }

        return new RPolicyCallback(released.Handle, false);
    }

    private static void DeleteTransferredPolicy(RPolicyCallback transferredPolicy)
    {
        var ptr = RPolicyCallback.getCPtr(transferredPolicy);
        if (ptr.Handle != IntPtr.Zero)
        {
            pagmoPINVOKE.delete_RPolicyCallback(new HandleRef(null, ptr.Handle));
        }
    }
}
