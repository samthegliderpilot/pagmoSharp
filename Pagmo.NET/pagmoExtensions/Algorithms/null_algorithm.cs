namespace pagmo;

/// <summary>
/// Represents null_algorithm. Uses pagmo-native semantics. See docs/api-reference.md for upstream links.
/// </summary>
public partial class null_algorithm : IAlgorithm
{
    private uint _seed;
    private uint _verbosity;

    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
    public void set_seed(uint seed)
    {
        _seed = seed;
    }

    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
    public uint get_seed()
    {
        return _seed;
    }

    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
    public uint get_verbosity()
    {
        return _verbosity;
    }

    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
    public void set_verbosity(uint level)
    {
        _verbosity = level;
    }

    /// <summary>
    /// Invokes the corresponding pagmo API. See docs/api-reference.md for upstream links.
    /// </summary>
    public string get_extra_info()
    {
        return "Null algorithm performs no evolution and returns the input population unchanged.";
    }
}

