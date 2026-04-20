using pagmo;

namespace Examples.Pagmo.NET.OrbitalManeuver;

/// <summary>
/// A pagmo user-defined problem (UDP) that minimises total delta-v for a
/// sequence of coast-and-burn maneuvers subject to equality constraints on
/// the desired final orbital state.
///
/// Decision vector layout (4 variables per burn, N burns total):
///   [ coastDuration_0, dvR_0, dvI_0, dvC_0,
///     coastDuration_1, dvR_1, dvI_1, dvC_1, ... ]
///
/// Fitness vector layout:
///   [ totalDeltaV, constraintResiduals... ]
/// where each constraint residual is (actual - desired) for each non-null
/// field in <see cref="ManeuverTarget"/>.  Equality constraints are satisfied
/// when all residuals are zero.
/// </summary>
internal sealed class ManeuverOptimizationProblem : ManagedProblemBase
{
    private readonly KeplerianElements _initial;
    private readonly double _t0;
    private readonly int _numBurns;
    private readonly ManeuverTarget _target;
    private readonly double[] _lower;
    private readonly double[] _upper;
    private readonly double _mu;

    /// <param name="initial">Initial orbital elements.</param>
    /// <param name="t0">Initial epoch (same units as coast durations).</param>
    /// <param name="numBurns">Number of coast-and-burn segments.</param>
    /// <param name="target">Desired final state; null fields are unconstrained.</param>
    /// <param name="maxCoastDuration">Upper bound on each coast duration.</param>
    /// <param name="maxDeltaV">Symmetric bound on each RIC delta-v component.</param>
    /// <param name="mu">Gravitational parameter (must match distance/time units).</param>
    public ManeuverOptimizationProblem(
        KeplerianElements initial,
        double t0,
        int numBurns,
        ManeuverTarget target,
        double maxCoastDuration,
        double maxDeltaV,
        double mu = OrbitalMechanics.EarthGm)
    {
        _initial = initial;
        _t0 = t0;
        _numBurns = numBurns;
        _target = target;
        _mu = mu;

        int n = numBurns * 4;
        _lower = new double[n];
        _upper = new double[n];
        for (int b = 0; b < numBurns; b++)
        {
            _lower[b * 4 + 0] = 0.0;            _upper[b * 4 + 0] = maxCoastDuration;
            _lower[b * 4 + 1] = -maxDeltaV;     _upper[b * 4 + 1] = maxDeltaV;
            _lower[b * 4 + 2] = -maxDeltaV;     _upper[b * 4 + 2] = maxDeltaV;
            _lower[b * 4 + 3] = -maxDeltaV;     _upper[b * 4 + 3] = maxDeltaV;
        }
    }

    public override string get_name() => "ManeuverOptimizationProblem";

    public override uint get_nec() => (uint)_target.ConstraintCount;

    public override PairOfDoubleVectors get_bounds() => Bounds(_lower, _upper);

    public override ThreadSafety get_thread_safety() => ThreadSafety.Basic;

    public override DoubleVector fitness(DoubleVector x)
    {
        var burns = DecodeDecisionVector(x);

        var history = OrbitalMechanics.Propagate(_initial, _t0, burns, _mu);
        var (final, finalTime) = history[history.Count - 1];

        double totalDv = 0.0;
        foreach (var b in burns)
            totalDv += b.TotalDeltaV;

        var f = new List<double> { totalDv };

        // Equality constraint residuals — each should be zero at the optimum.
        if (_target.SemiMajorAxis.HasValue)
            f.Add(final.SemiMajorAxis - _target.SemiMajorAxis.Value);
        if (_target.Eccentricity.HasValue)
            f.Add(final.Eccentricity - _target.Eccentricity.Value);
        if (_target.Inclination.HasValue)
            f.Add(final.Inclination - _target.Inclination.Value);
        if (_target.Raan.HasValue)
            f.Add(final.Raan - _target.Raan.Value);
        if (_target.ArgumentOfPeriapsis.HasValue)
            f.Add(final.ArgumentOfPeriapsis - _target.ArgumentOfPeriapsis.Value);
        if (_target.TrueAnomaly.HasValue)
            f.Add(NormalizeAngle(final.TrueAnomaly - _target.TrueAnomaly.Value));
        if (_target.TotalTime.HasValue)
            f.Add((finalTime - _t0) - _target.TotalTime.Value);

        return Vec(f.ToArray());
    }

    // -------------------------------------------------------------------------

    private CoastAndBurn[] DecodeDecisionVector(DoubleVector x)
    {
        var burns = new CoastAndBurn[_numBurns];
        for (int b = 0; b < _numBurns; b++)
        {
            burns[b] = new CoastAndBurn(
                CoastDuration: x[b * 4 + 0],
                DvRadial:      x[b * 4 + 1],
                DvInTrack:     x[b * 4 + 2],
                DvCrossTrack:  x[b * 4 + 3]);
        }
        return burns;
    }

    /// <summary>Wraps an angle difference into (-π, π].</summary>
    private static double NormalizeAngle(double delta)
    {
        while (delta > Math.PI)  delta -= 2.0 * Math.PI;
        while (delta < -Math.PI) delta += 2.0 * Math.PI;
        return delta;
    }
}
