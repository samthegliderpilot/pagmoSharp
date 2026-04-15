using System;
using System.Runtime.InteropServices;

namespace pagmo;

/// <summary>
/// Represents r_policy. Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
/// </summary>
public sealed class r_policy : r_policyPagmoWrapper
{
    public r_policy()
    {
    }

    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
    public r_policy(r_policyBase basePolicy)
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

    private static r_policyBase TransferOwnership(r_policyBase basePolicy)
    {
        if (basePolicy == null)
        {
            throw new ArgumentNullException(nameof(basePolicy));
        }

        var currentPtr = r_policyBase.getCPtr(basePolicy);
        if (currentPtr.Handle == IntPtr.Zero)
        {
            throw new ObjectDisposedException(nameof(basePolicy), "The provided replacement policy has already been disposed.");
        }

        var released = r_policyBase.swigRelease(basePolicy);
        if (released.Handle == IntPtr.Zero)
        {
            throw new ObjectDisposedException(nameof(basePolicy), "The provided replacement policy could not transfer ownership.");
        }

        return new r_policyBase(released.Handle, false);
    }

    private static void DeleteTransferredPolicy(r_policyBase transferredPolicy)
    {
        var ptr = r_policyBase.getCPtr(transferredPolicy);
        if (ptr.Handle != IntPtr.Zero)
        {
            pagmoPINVOKE.delete_r_policyBase(new HandleRef(null, ptr.Handle));
        }
    }
}

