using System;
using System.Runtime.InteropServices;

namespace pagmo;

public sealed class s_policy : s_policyPagmoWrapper
{
    public s_policy()
    {
    }

    public s_policy(s_policyBase basePolicy)
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

    private static s_policyBase TransferOwnership(s_policyBase basePolicy)
    {
        if (basePolicy == null)
        {
            throw new ArgumentNullException(nameof(basePolicy));
        }

        var currentPtr = s_policyBase.getCPtr(basePolicy);
        if (currentPtr.Handle == IntPtr.Zero)
        {
            throw new ObjectDisposedException(nameof(basePolicy), "The provided selection policy has already been disposed.");
        }

        var released = s_policyBase.swigRelease(basePolicy);
        if (released.Handle == IntPtr.Zero)
        {
            throw new ObjectDisposedException(nameof(basePolicy), "The provided selection policy could not transfer ownership.");
        }

        return new s_policyBase(released.Handle, false);
    }

    private static void DeleteTransferredPolicy(s_policyBase transferredPolicy)
    {
        var ptr = s_policyBase.getCPtr(transferredPolicy);
        if (ptr.Handle != IntPtr.Zero)
        {
            pagmoPINVOKE.delete_s_policyBase(new HandleRef(null, ptr.Handle));
        }
    }
}
