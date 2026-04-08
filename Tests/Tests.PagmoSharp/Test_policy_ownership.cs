using System;
using NUnit.Framework;
using pagmo;

namespace Tests.PagmoSharp;

[TestFixture]
public class Test_policy_ownership
{
    private sealed class ManagedReplacementPolicy : r_policyBase
    {
        public override IndividualsGroup replace(IndividualsGroup a, uint b, uint c, uint d, uint e, uint f, DoubleVector g, IndividualsGroup h)
        {
            return a;
        }

        public override string get_name() => "ManagedReplacementPolicy";
        public override string get_extra_info() => "managed r_policyBase test";
        public override bool is_valid() => true;
    }

    private sealed class ManagedSelectionPolicy : s_policyBase
    {
        public override IndividualsGroup select(IndividualsGroup a, uint b, uint c, uint d, uint e, uint f, DoubleVector g)
        {
            return a;
        }

        public override string get_name() => "ManagedSelectionPolicy";
        public override string get_extra_info() => "managed s_policyBase test";
        public override bool is_valid() => true;
    }

    [Test]
    public void ReplacementPolicyConstructorRejectsNull()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => new r_policy(null!));
        Assert.That(ex!.ParamName, Is.EqualTo("basePolicy"));
    }

    [Test]
    public void SelectionPolicyConstructorRejectsNull()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => new s_policy(null!));
        Assert.That(ex!.ParamName, Is.EqualTo("basePolicy"));
    }

    [Test]
    public void ReplacementPolicyTransferMakesWrapperUsable()
    {
        using var basePolicy = new ManagedReplacementPolicy();
        using var wrapped = new r_policy(basePolicy);
        Assert.That(wrapped.is_valid(), Is.True);
        Assert.That(wrapped.get_name(), Is.EqualTo("ManagedReplacementPolicy"));
    }

    [Test]
    public void SelectionPolicyTransferMakesWrapperUsable()
    {
        using var basePolicy = new ManagedSelectionPolicy();
        using var wrapped = new s_policy(basePolicy);
        Assert.That(wrapped.is_valid(), Is.True);
        Assert.That(wrapped.get_name(), Is.EqualTo("ManagedSelectionPolicy"));
    }

    [Test]
    public void ReplacementPolicyConstructorRejectsAlreadyDisposedBasePolicy()
    {
        var basePolicy = new ManagedReplacementPolicy();
        basePolicy.Dispose();
        var ex = Assert.Throws<ObjectDisposedException>(() => new r_policy(basePolicy));
        Assert.That(ex!.ObjectName, Is.EqualTo("basePolicy"));
    }

    [Test]
    public void SelectionPolicyConstructorRejectsAlreadyDisposedBasePolicy()
    {
        var basePolicy = new ManagedSelectionPolicy();
        basePolicy.Dispose();
        var ex = Assert.Throws<ObjectDisposedException>(() => new s_policy(basePolicy));
        Assert.That(ex!.ObjectName, Is.EqualTo("basePolicy"));
    }
}
