namespace Examples.Pagmo.NET.OrbitalManeuver;

/// <summary>
/// Desired final state for a maneuver sequence. Any element left null is
/// unconstrained. Angles are in radians.
/// </summary>
internal sealed record ManeuverTarget(
    double? SemiMajorAxis = null,
    double? Eccentricity = null,
    double? Inclination = null,
    double? Raan = null,
    double? ArgumentOfPeriapsis = null,
    double? TrueAnomaly = null,
    double? TotalTime = null)
{
    /// <summary>Number of equality constraints implied by this target.</summary>
    public int ConstraintCount =>
        (SemiMajorAxis.HasValue ? 1 : 0) +
        (Eccentricity.HasValue ? 1 : 0) +
        (Inclination.HasValue ? 1 : 0) +
        (Raan.HasValue ? 1 : 0) +
        (ArgumentOfPeriapsis.HasValue ? 1 : 0) +
        (TrueAnomaly.HasValue ? 1 : 0) +
        (TotalTime.HasValue ? 1 : 0);
}
