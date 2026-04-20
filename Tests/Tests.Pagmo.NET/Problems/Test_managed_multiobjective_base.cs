using NUnit.Framework;
using pagmo;
using Tests.Pagmo.NET.TestProblems;

namespace Tests.Pagmo.NET.Problems;

// Shared MO managed-problem assertions so constrained/unconstrained variants
// can reuse one validation suite and only swap fixture + constraint expectation.
public abstract class Test_managed_multiobjective_base
{
    protected abstract TestProblemWrapper CreateManagedProblem();
    protected abstract bool ExpectConstraints { get; }

    [Test]
    public void FitnessShapeMatchesDeclaredObjectiveAndConstraintCounts()
    {
        using var managed = CreateManagedProblem();
        using var prob = new problem(managed);
        using var bounds = prob.get_bounds();
        using var probe = new DoubleVector(new[]
        {
            0.5 * (bounds.first[0] + bounds.second[0]),
            0.5 * (bounds.first[1] + bounds.second[1])
        });
        using var fitness = prob.fitness(probe);

        Assert.That(prob.get_nobj(), Is.GreaterThanOrEqualTo(2u), "multi-objective tests must expose >= 2 objectives");
        Assert.That(fitness.Count, Is.EqualTo((int)(prob.get_nobj() + prob.get_nec() + prob.get_nic())));
    }

    [Test]
    public void ObjectiveValuesRespondToDifferentInputs()
    {
        using var managed = CreateManagedProblem();
        using var prob = new problem(managed);
        using var x1 = new DoubleVector(new[] { -1.0, 2.0 });
        using var x2 = new DoubleVector(new[] { 2.0, -1.0 });
        using var f1 = prob.fitness(x1);
        using var f2 = prob.fitness(x2);

        Assert.That(f1.Count, Is.GreaterThanOrEqualTo(2));
        Assert.That(f2.Count, Is.GreaterThanOrEqualTo(2));
        var objectiveDelta = System.Math.Abs(f1[0] - f2[0]) + System.Math.Abs(f1[1] - f2[1]);
        Assert.That(objectiveDelta, Is.GreaterThan(1e-12), "at least one objective component should differ across inputs");
    }

    [Test]
    public void ConstraintMetadataMatchesFixtureMode()
    {
        using var managed = CreateManagedProblem();
        using var prob = new problem(managed);
        var totalConstraints = prob.get_nec() + prob.get_nic();

        if (ExpectConstraints)
        {
            Assert.That(totalConstraints, Is.GreaterThan(0u));
            return;
        }

        Assert.That(totalConstraints, Is.EqualTo(0u));
    }
}

[TestFixture]
public sealed class Test_managed_multiobjective_unconstrained : Test_managed_multiobjective_base
{
    protected override TestProblemWrapper CreateManagedProblem() => new TwoDimensionalMultiObjectiveProblemWrapper();
    protected override bool ExpectConstraints => false;
}

[TestFixture]
public sealed class Test_managed_multiobjective_constrained : Test_managed_multiobjective_base
{
    protected override TestProblemWrapper CreateManagedProblem() => new TwoDimensionalMultiObjectiveConstrainedProblemWrapper();
    protected override bool ExpectConstraints => true;

    [Test]
    public void InequalityConstraintValueIsProducedInFitnessOutput()
    {
        using var managed = CreateManagedProblem();
        using var prob = new problem(managed);
        using var x = new DoubleVector(new[] { 0.25, 0.25 });
        using var fitness = prob.fitness(x);

        Assert.That(fitness.Count, Is.EqualTo(3));
        Assert.That(fitness[2], Is.EqualTo(-0.5).Within(1e-12), "constraint should match x + y - 1");
    }
}
