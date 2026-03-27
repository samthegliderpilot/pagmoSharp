using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp.Algorithms;

[TestFixture]
public class Test_not_population_based
{
    [Test]
    public void SelectionAndReplacementPoliciesCanBeConfigured()
    {
        using var helper = new not_population_based();

        helper.set_random_sr_seed(11u);
        helper.set_selection("best");
        helper.set_replacement("worst");
        helper.set_selection(0u);
        helper.set_replacement(1u);

        Assert.Pass();
    }
}
