using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using pagmo;

namespace Tests.Pagmo.NET.Utils;

[TestFixture]
public class Test_multi_objective
{
    [Test]
    public void FastNonDominatedSortingReturnsExpectedFrontShape()
    {
        var fitnessValues = new VectorOfVectorOfDoubles(new List<DoubleVector>
        {
            new DoubleVector(1.0, 3.0),
            new DoubleVector(2.0, 2.0),
            new DoubleVector(3.0, 1.0),
            new DoubleVector(4.0, 4.0)
        });

        using var sortingResult = pagmo.pagmo.FastNonDominatedSorting(fitnessValues);
        Assert.That(sortingResult.fronts.Count, Is.GreaterThanOrEqualTo(1));
        Assert.That(sortingResult.fronts[0].Count, Is.EqualTo(3));
    }

    [Test]
    public void RekSumProducesOutputRows()
    {
        var output = new VectorOfVectorOfDoubles(new List<DoubleVector>
        {
            new DoubleVector(0.0, 2.0, 1.0),
            new DoubleVector(5.0, 9.0, 10.0)
        });

        pagmo.RekSum.reksum(output, new ULongLongVector(new ulong[] { 1UL, 2UL, 3UL }), 1, 2);
        Assert.That(output.Count, Is.GreaterThan(0));
    }

    [Test]
    public void DecompositionWeightsValidatesMethodAndReturnsWeights()
    {
        var ex = Assert.Throws<ApplicationException>(
            () => pagmo.DecompositionWeights.decomposition_weights(2, 2, "huh"));
        Assert.That(ex!.Message, Does.Contain("unknown"));

        var weights = pagmo.DecompositionWeights.decomposition_weights(2, 4, "grid");
        Assert.That(weights.Count, Is.GreaterThan(0));
    }

    [Test]
    public void MultiObjectiveUtilitiesExposeCoreStaticHelpers()
    {
        var lhs = new DoubleVector(1.0, 1.0);
        var rhs = new DoubleVector(2.0, 3.0);
        Assert.That(pagmo.pagmo.pareto_dominance(lhs, rhs), Is.True);
        Assert.That(pagmo.pagmo.pareto_dominance(rhs, lhs), Is.False);
        Assert.That(pagmo.pagmo.ParetoDominates(lhs, rhs), Is.True);
        Assert.That(pagmo.pagmo.ParetoDominates(rhs, lhs), Is.False);

        var points = new VectorOfVectorOfDoubles(new List<DoubleVector>
        {
            new DoubleVector(1.0, 3.0),
            new DoubleVector(2.0, 2.0),
            new DoubleVector(3.0, 1.0),
            new DoubleVector(4.0, 4.0)
        });

        var front = pagmo.pagmo.non_dominated_front_2d(points);
        Assert.That(front.Count, Is.EqualTo(3));
        CollectionAssert.AreEquivalent(new[] { 0UL, 1UL, 2UL }, front.ToArray());
        CollectionAssert.AreEquivalent(new[] { 0UL, 1UL, 2UL }, pagmo.pagmo.NonDominatedFront2DIndices(points));

        var sorted = pagmo.pagmo.sort_population_mo(points);
        Assert.That(sorted.Count, Is.EqualTo(points.Count));
        CollectionAssert.AreEquivalent(new[] { 0UL, 1UL, 2UL, 3UL }, sorted.ToArray());
        CollectionAssert.AreEquivalent(new[] { 0UL, 1UL, 2UL, 3UL }, pagmo.pagmo.SortPopulationMoIndices(points));

        var selected = pagmo.pagmo.select_best_N_mo(points, 2);
        Assert.That(selected.Count, Is.EqualTo(2));
        foreach (var idx in selected)
        {
            Assert.That(idx, Is.LessThan((ulong)points.Count));
        }
        var selectedProjected = pagmo.pagmo.SelectBestNMoIndices(points, 2UL);
        Assert.That(selectedProjected.Length, Is.EqualTo(2));
        foreach (var idx in selectedProjected)
        {
            Assert.That(idx, Is.LessThan((ulong)points.Count));
        }

        var ideal = pagmo.pagmo.ideal(points);
        Assert.That(ideal[0], Is.EqualTo(1.0).Within(1e-12));
        Assert.That(ideal[1], Is.EqualTo(1.0).Within(1e-12));
        var idealProjected = pagmo.pagmo.IdealValues(points);
        Assert.That(idealProjected[0], Is.EqualTo(1.0).Within(1e-12));
        Assert.That(idealProjected[1], Is.EqualTo(1.0).Within(1e-12));

        var nadir = pagmo.pagmo.nadir(points);
        Assert.That(nadir[0], Is.EqualTo(3.0).Within(1e-12));
        Assert.That(nadir[1], Is.EqualTo(3.0).Within(1e-12));
        var nadirProjected = pagmo.pagmo.NadirValues(points);
        Assert.That(nadirProjected[0], Is.EqualTo(3.0).Within(1e-12));
        Assert.That(nadirProjected[1], Is.EqualTo(3.0).Within(1e-12));

        var crowdingFront = new VectorOfVectorOfDoubles(new List<DoubleVector>
        {
            new DoubleVector(1.0, 3.0),
            new DoubleVector(2.0, 2.0),
            new DoubleVector(3.0, 1.0)
        });
        var crowding = pagmo.pagmo.crowding_distance(crowdingFront);
        Assert.That(crowding.Count, Is.EqualTo(3));
        Assert.That(crowding.Any(value => double.IsInfinity(value)), Is.True);

        var decomposed = pagmo.pagmo.decompose_objectives(
            new DoubleVector(2.0, 4.0),
            new DoubleVector(0.5, 0.5),
            new DoubleVector(0.0, 0.0),
            "weighted");
        Assert.That(decomposed.Count, Is.EqualTo(1));
        Assert.That(decomposed[0], Is.EqualTo(3.0).Within(1e-12));

        var decomposedProjected = pagmo.pagmo.DecomposeObjectiveValues(
            new DoubleVector(2.0, 4.0),
            new DoubleVector(0.5, 0.5),
            new DoubleVector(0.0, 0.0),
            "weighted");
        Assert.That(decomposedProjected.Length, Is.EqualTo(1));
        Assert.That(decomposedProjected[0], Is.EqualTo(3.0).Within(1e-12));
    }

    [Test]
    public void ProjectionHelpersValidateNullArguments()
    {
        Assert.Throws<ArgumentNullException>(() => pagmo.pagmo.ParetoDominates(null, new DoubleVector(1.0, 2.0)));
        Assert.Throws<ArgumentNullException>(() => pagmo.pagmo.ParetoDominates(new DoubleVector(1.0, 2.0), null));
        Assert.Throws<ArgumentNullException>(() => pagmo.pagmo.NonDominatedFront2DIndices(null));
        Assert.Throws<ArgumentNullException>(() => pagmo.pagmo.SortPopulationMoIndices(null));
        Assert.Throws<ArgumentNullException>(() => pagmo.pagmo.SelectBestNMoIndices(null, 1UL));
        Assert.Throws<ArgumentNullException>(() => pagmo.pagmo.IdealValues(null));
        Assert.Throws<ArgumentNullException>(() => pagmo.pagmo.NadirValues(null));
        Assert.Throws<ArgumentNullException>(() => pagmo.pagmo.DecomposeObjectiveValues(null, new DoubleVector(1.0), new DoubleVector(0.0), "weighted"));
        Assert.Throws<ArgumentNullException>(() => pagmo.pagmo.DecomposeObjectiveValues(new DoubleVector(1.0), null, new DoubleVector(0.0), "weighted"));
        Assert.Throws<ArgumentNullException>(() => pagmo.pagmo.DecomposeObjectiveValues(new DoubleVector(1.0), new DoubleVector(1.0), null, "weighted"));
        Assert.Throws<ArgumentNullException>(() => pagmo.pagmo.DecomposeObjectiveValues(new DoubleVector(1.0), new DoubleVector(1.0), new DoubleVector(0.0), null));
    }
}

