namespace Examples.Pagmo.NET.OrbitalManeuver;

/// <summary>
/// A free-coast interval followed by an impulsive maneuver expressed in the
/// Radial / In-track / Cross-track (RIC) frame.
/// </summary>
internal sealed record CoastAndBurn(
    double CoastDuration,
    double DvRadial,
    double DvInTrack,
    double DvCrossTrack)
{
    public double TotalDeltaV =>
        Math.Sqrt(DvRadial * DvRadial + DvInTrack * DvInTrack + DvCrossTrack * DvCrossTrack);
}
