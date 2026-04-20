using NUnit.Framework;
using pagmo;

namespace Tests.Pagmo.NET;

[TestFixture]
public class Test_migration_entry
{
    [Test]
    public void MigrationEntryExposesNumericIds()
    {
        using var decision = new DoubleVector(new[] { 1.0, 2.0 });
        using var fitness = new DoubleVector(new[] { 3.0 });
        using var entry = new MigrationEntry();
        entry.t = 0.5;
        entry.island_id = 7UL;
        entry.x = decision;
        entry.f = fitness;
        entry.migration_id = 11UL;
        entry.immigrant_id = 13UL;

        Assert.That(entry.migration_id, Is.EqualTo(11UL));
        Assert.That(entry.immigrant_id, Is.EqualTo(13UL));

        entry.migration_id = 17UL;
        entry.immigrant_id = 19UL;

        Assert.That(entry.migration_id, Is.EqualTo(17UL));
        Assert.That(entry.immigrant_id, Is.EqualTo(19UL));
    }
}
