using System;

namespace pagmo;

public sealed class s_policy : s_policyPagmoWrapper
{
    public s_policy()
    {
    }

    public s_policy(s_policyBase basePolicy)
        : base(TransferOwnership(basePolicy))
    {
    }

    private static s_policyBase TransferOwnership(s_policyBase basePolicy)
    {
        if (basePolicy == null)
        {
            throw new ArgumentNullException(nameof(basePolicy));
        }

        var released = s_policyBase.swigRelease(basePolicy);
        return new s_policyBase(released.Handle, false);
    }
}
