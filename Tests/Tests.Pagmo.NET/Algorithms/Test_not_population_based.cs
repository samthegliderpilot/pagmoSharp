using NUnit.Framework;
using pagmo;
using System;

namespace Tests.Pagmo.NET.Algorithms;

[TestFixture]
public class Test_not_population_based
{
    [Test]
    public void SelectionAndReplacementPoliciesAcceptValidInputs()
    {
        using var helper = new not_population_based();

        helper.set_random_sr_seed(11u);

        helper.set_selection("best");
        Assert.IsFalse(helper.selection_uses_count());
        Assert.AreEqual("best", helper.selection_policy());

        helper.set_replacement("worst");
        Assert.IsFalse(helper.replacement_uses_count());
        Assert.AreEqual("worst", helper.replacement_policy());

        helper.set_selection(0u);
        Assert.IsTrue(helper.selection_uses_count());
        Assert.AreEqual(0u, helper.selection_count());

        helper.set_replacement(1u);
        Assert.IsTrue(helper.replacement_uses_count());
        Assert.AreEqual(1u, helper.replacement_count());
    }

    [Test]
    public void SelectionAndReplacementPoliciesRejectInvalidInputs()
    {
        using var helper = new not_population_based();

        Assert.Throws<ApplicationException>(() => helper.set_selection("not-a-valid-selection"));
        Assert.Throws<ApplicationException>(() => helper.set_replacement("not-a-valid-replacement"));
    }
}
