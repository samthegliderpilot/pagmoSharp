namespace Examples.Pagmo.NET.OrbitalManeuver;

/// <summary>
/// Classical Keplerian orbital elements. Angles are in radians; distance and
/// gravitational-parameter units are caller-defined but must be consistent.
/// </summary>
internal sealed record KeplerianElements(
    double SemiMajorAxis,
    double Eccentricity,
    double Inclination,
    double Raan,
    double ArgumentOfPeriapsis,
    double TrueAnomaly);
