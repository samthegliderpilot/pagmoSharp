using System;

namespace pagmo;

public sealed class r_policy : r_policyPagmoWrapper
{
    public r_policy()
    {
    }

    public r_policy(r_policyBase basePolicy)
        : base(TransferOwnership(basePolicy))
    {
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
}
