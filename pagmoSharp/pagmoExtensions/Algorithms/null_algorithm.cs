namespace pagmo;

public partial class null_algorithm : IAlgorithm
{
    private uint _seed;
    private uint _verbosity;

    public void set_seed(uint seed)
    {
        _seed = seed;
    }

    public uint get_seed()
    {
        return _seed;
    }

    public uint get_verbosity()
    {
        return _verbosity;
    }

    public void set_verbosity(uint level)
    {
        _verbosity = level;
    }

    public string get_extra_info()
    {
        return "Null algorithm performs no evolution and returns the input population unchanged.";
    }
}
