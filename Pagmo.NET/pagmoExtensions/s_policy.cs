using System;
using System.Runtime.InteropServices;

namespace pagmo;

/// <summary>
/// Managed wrapper that registers a C# <see cref="SPolicyCallback"/> subclass as a pagmo
/// selection policy. Transfers ownership of the callback to the native side.
/// </summary>
public sealed class s_policy : ManagedSPolicy
{
    /// <summary>
    /// Creates an empty selection-policy wrapper.
    /// </summary>
    public s_policy()
    {
    }

    /// <summary>
    /// Creates an <see cref="s_policy"/> backed by the given <paramref name="basePolicy"/> callback.
    /// Ownership of <paramref name="basePolicy"/> is transferred to the native side.
    /// </summary>
    public s_policy(SPolicyCallback basePolicy)
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

    private static SPolicyCallback TransferOwnership(SPolicyCallback basePolicy)
    {
        if (basePolicy == null)
        {
            throw new ArgumentNullException(nameof(basePolicy));
        }

        var currentPtr = SPolicyCallback.getCPtr(basePolicy);
        if (currentPtr.Handle == IntPtr.Zero)
        {
            throw new ObjectDisposedException(nameof(basePolicy), "The provided selection policy has already been disposed.");
        }

        var released = SPolicyCallback.swigRelease(basePolicy);
        if (released.Handle == IntPtr.Zero)
        {
            throw new ObjectDisposedException(nameof(basePolicy), "The provided selection policy could not transfer ownership.");
        }

        return new SPolicyCallback(released.Handle, false);
    }

    private static void DeleteTransferredPolicy(SPolicyCallback transferredPolicy)
    {
        var ptr = SPolicyCallback.getCPtr(transferredPolicy);
        if (ptr.Handle != IntPtr.Zero)
        {
            pagmoPINVOKE.delete_SPolicyCallback(new HandleRef(null, ptr.Handle));
        }
    }
}
