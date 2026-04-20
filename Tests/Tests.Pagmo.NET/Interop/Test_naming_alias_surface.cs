using System.Linq;
using NUnit.Framework;
using pagmo;

namespace Tests.Pagmo.NET.Interop;

[TestFixture]
public class Test_naming_alias_surface
{
    [Test]
    public void ArchipelagoExposesPascalCaseCompatibilityAliases()
    {
        var methods = typeof(archipelago).GetMethods().Select(m => m.Name).ToArray();

        Assert.That(methods, Does.Contain("PushBackIsland"));
        Assert.That(methods, Does.Contain("GetIsland"));
        Assert.That(methods, Does.Contain("GetIslandCopy"));
    }

    [Test]
    public void ArchipelagoRetainsSnakeCaseRuntimeEntryPoints()
    {
        var methods = typeof(archipelago).GetMethods().Select(m => m.Name).ToArray();

        Assert.That(methods, Does.Contain("push_back_island"));
        Assert.That(methods, Does.Contain("get_island_copy"));
    }

    [Test]
    public void TopologyProjectionExtensionsUsePascalCaseNaming()
    {
        var methods = typeof(TopologyConnectionExtensions).GetMethods().Select(m => m.Name).Distinct().ToArray();
        Assert.That(methods, Does.Contain("GetConnectionsData"));
        Assert.That(methods, Does.Not.Contain("get_connections_data"));
    }
}

