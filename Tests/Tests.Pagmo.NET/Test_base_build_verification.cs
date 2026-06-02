using NUnit.Framework;
using pagmo;

namespace Tests.Pagmo.NET;

/// <summary>
/// Verifies that non-release (CI) builds do NOT include IPOPT.
/// Set PAGMONET_BASE_VERIFY=1 in CI to activate these assertions.
/// </summary>
[TestFixture]
public class Test_base_build_verification
{
    private static void SkipIfNotBaseVerify()
    {
        if (System.Environment.GetEnvironmentVariable("PAGMONET_BASE_VERIFY") != "1")
            Assert.Pass("Set PAGMONET_BASE_VERIFY=1 to run base build verification.");
    }

    [Test]
    public void BaseBuildExcludesIpopt()
    {
        SkipIfNotBaseVerify();
        Assert.That(OptionalSolverAvailability.IsIpoptAvailable, Is.False,
            "Base builds must not include IPOPT — consumers should reference the Pagmo.NET.Ipopt package.");
    }

    [Test]
    public void BaseBuildIncludesNlopt()
    {
        SkipIfNotBaseVerify();
        Assert.That(OptionalSolverAvailability.IsNloptAvailable, Is.True,
            "Base builds must include NLopt.");
    }
}
