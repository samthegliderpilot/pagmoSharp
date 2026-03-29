using System;
using System.Runtime.InteropServices;

namespace pagmo;

public sealed class r_policy : r_policyPagmoWrapper
{
    public r_policy()
    {
    }

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

        var released = r_policyBase.swigRelease(basePolicy);
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
