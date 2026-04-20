namespace Examples.Pagmo.NET.OrbitalManeuver;

/// <summary>
/// Two-body orbital mechanics helpers: Kepler propagation and Gauss
/// variation-of-parameters for impulsive RIC burns.
/// </summary>
internal static class OrbitalMechanics
{
    /// <summary>Earth gravitational parameter (km³/s²).</summary>
    public const double EarthGm = 398600.4418;

    /// <summary>
    /// Returns the true anomaly after coasting for <paramref name="duration"/>
    /// time units using Kepler's equation.
    /// </summary>
    public static double AdvanceTrueAnomaly(
        double semiMajorAxis, double eccentricity, double trueAnomaly,
        double duration, double mu)
    {
        // True anomaly → eccentric anomaly
        double halfNu = trueAnomaly / 2.0;
        double E0 = 2.0 * Math.Atan2(
            Math.Sqrt(1.0 - eccentricity) * Math.Sin(halfNu),
            Math.Sqrt(1.0 + eccentricity) * Math.Cos(halfNu));

        // Eccentric anomaly → mean anomaly, then advance
        double M0 = E0 - eccentricity * Math.Sin(E0);
        double n = Math.Sqrt(mu / (semiMajorAxis * semiMajorAxis * semiMajorAxis));
        double M1 = M0 + n * duration;

        // Solve Kepler's equation M = E - e*sin(E) via Newton-Raphson
        double E1 = M1;
        for (int iter = 0; iter < 50; iter++)
        {
            double dE = (M1 - E1 + eccentricity * Math.Sin(E1)) /
                        (1.0 - eccentricity * Math.Cos(E1));
            E1 += dE;
            if (Math.Abs(dE) < 1e-12)
                break;
        }

        // Eccentric anomaly → true anomaly
        return 2.0 * Math.Atan2(
            Math.Sqrt(1.0 + eccentricity) * Math.Sin(E1 / 2.0),
            Math.Sqrt(1.0 - eccentricity) * Math.Cos(E1 / 2.0));
    }

    /// <summary>
    /// Applies an impulsive RIC burn using the Gauss variation-of-parameters
    /// equations and returns the resulting Keplerian elements.
    /// </summary>
    /// <remarks>
    /// The Gauss equations are linearised in Δv; they are accurate for small
    /// burns and provide a consistent first-order model suitable for
    /// optimisation problems.  Near-zero eccentricity causes the argument-of-
    /// perigee term to become singular — avoid e ≈ 0 or handle it externally.
    /// </remarks>
    public static KeplerianElements ApplyBurn(
        KeplerianElements el, CoastAndBurn burn, double mu)
    {
        double a = el.SemiMajorAxis;
        double e = el.Eccentricity;
        double i = el.Inclination;
        double nu = el.TrueAnomaly;
        double omega = el.ArgumentOfPeriapsis;
        double u = omega + nu;                        // argument of latitude

        double p = a * (1.0 - e * e);
        double h = Math.Sqrt(mu * p);
        double r = p / (1.0 + e * Math.Cos(nu));

        double dvR = burn.DvRadial;
        double dvS = burn.DvInTrack;
        double dvW = burn.DvCrossTrack;

        // Gauss variation-of-parameters (Bate, Mueller & White §9.5)
        double da = (2.0 * a * a / h) *
                    (e * Math.Sin(nu) * dvR + (p / r) * dvS);

        double de = (1.0 / h) *
                    (p * Math.Sin(nu) * dvR +
                     ((p + r) * Math.Cos(nu) + r * e) * dvS);

        double di = (r * Math.Cos(u) / h) * dvW;

        double dRaan = (r * Math.Sin(u)) / (h * Math.Sin(i)) * dvW;

        double dOmega = (1.0 / (h * e)) *
                        (-p * Math.Cos(nu) * dvR + (p + r) * Math.Sin(nu) * dvS) -
                        (r * Math.Sin(u) * Math.Cos(i)) / (h * Math.Sin(i)) * dvW;

        double aNew = a + da;
        double eNew = e + de;

        // Recover true anomaly in the new orbit from position + radial velocity.
        // Position is unchanged during an impulsive burn; radial velocity picks
        // up the radial component of Δv.
        double pNew = aNew * (1.0 - eNew * eNew);
        double hNew = Math.Sqrt(mu * pNew);

        double vrBefore = (mu / h) * e * Math.Sin(nu);
        double vrAfter = vrBefore + dvR;

        // r = p_new / (1 + e_new·cos ν_new)  →  cos ν_new
        // ṙ = (μ/h_new)·e_new·sin ν_new  →  sin ν_new
        double cosNuNew = (pNew / r - 1.0) / eNew;
        double sinNuNew = vrAfter * hNew / (mu * eNew);
        double nuNew = Math.Atan2(sinNuNew, cosNuNew);

        return new KeplerianElements(
            SemiMajorAxis: aNew,
            Eccentricity: eNew,
            Inclination: i + di,
            Raan: el.Raan + dRaan,
            ArgumentOfPeriapsis: el.ArgumentOfPeriapsis + dOmega,
            TrueAnomaly: nuNew);
    }

    /// <summary>
    /// Coasts for <paramref name="burn"/>.<see cref="CoastAndBurn.CoastDuration"/>,
    /// then applies the burn.  Returns the new elements and elapsed time
    /// (t0 + coast duration).
    /// </summary>
    public static (KeplerianElements Elements, double Time) Propagate(
        KeplerianElements initial, double t0, CoastAndBurn burn, double mu)
    {
        double nuAfterCoast = AdvanceTrueAnomaly(
            initial.SemiMajorAxis, initial.Eccentricity, initial.TrueAnomaly,
            burn.CoastDuration, mu);

        var afterCoast = initial with { TrueAnomaly = nuAfterCoast };
        var afterBurn = ApplyBurn(afterCoast, burn, mu);
        return (afterBurn, t0 + burn.CoastDuration);
    }

    /// <summary>
    /// Applies each coast-and-burn in sequence and returns the state after
    /// every maneuver.
    /// </summary>
    public static IReadOnlyList<(KeplerianElements Elements, double Time)> Propagate(
        KeplerianElements initial, double t0,
        IReadOnlyList<CoastAndBurn> maneuvers, double mu)
    {
        var results = new List<(KeplerianElements, double)>(maneuvers.Count);
        var current = initial;
        var currentTime = t0;

        foreach (var maneuver in maneuvers)
        {
            var (next, nextTime) = Propagate(current, currentTime, maneuver, mu);
            results.Add((next, nextTime));
            current = next;
            currentTime = nextTime;
        }

        return results;
    }
}
