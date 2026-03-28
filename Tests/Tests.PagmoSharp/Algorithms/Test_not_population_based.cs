using NUnit.Framework;
using pagmo;
using System;

namespace Tests.PagmoSharp.Algorithms;

[TestFixture]
public class Test_not_population_based
{
    [Test]
    public void SelectionAndReplacementPoliciesAcceptValidInputs()
    {
        using var helper = new not_population_based();

        Assert.DoesNotThrow(() => helper.set_random_sr_seed(11u));
        Assert.DoesNotThrow(() => helper.set_selection("best"));
        Assert.DoesNotThrow(() => helper.set_replacement("worst"));
        Assert.DoesNotThrow(() => helper.set_selection(0u));
        Assert.DoesNotThrow(() => helper.set_replacement(1u));
    }

    [Test]
    public void SelectionAndReplacementPoliciesRejectInvalidInputs()
    {
        using var helper = new not_population_based();

        Assert.Throws<ApplicationException>(() => helper.set_selection("not-a-valid-selection"));
        Assert.Throws<ApplicationException>(() => helper.set_replacement("not-a-valid-replacement"));
    }
}
