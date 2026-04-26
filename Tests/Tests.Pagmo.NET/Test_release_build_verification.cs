using System;
using NUnit.Framework;
using pagmo;

namespace Tests.Pagmo.NET;

/// <summary>
/// Verifies that release/NuGet builds include all expected optional solvers.
/// Skipped by default; set the PAGMONET_RELEASE_VERIFY=1 environment variable
/// to run these assertions (e.g. in release CI or when testing a NuGet install).
/// </summary>
[TestFixture]
public class Test_release_build_verification
{
    private static void SkipIfNotReleaseVerify()
    {
        if (Environment.GetEnvironmentVariable("PAGMONET_RELEASE_VERIFY") != "1")
            Assert.Pass("Set PAGMONET_RELEASE_VERIFY=1 to run release solver verification.");
    }

    [Test]
    public void ReleaseBuildIncludesNlopt()
    {
        SkipIfNotReleaseVerify();
        Assert.That(OptionalSolverAvailability.IsNloptAvailable, Is.True,
            "Release builds must include NLopt statically linked.");
    }

    [Test]
    public void ReleaseBuildIncludesIpopt()
    {
        SkipIfNotReleaseVerify();
        Assert.That(OptionalSolverAvailability.IsIpoptAvailable, Is.True,
            "Release builds must include IPOPT statically linked.");
    }
}
