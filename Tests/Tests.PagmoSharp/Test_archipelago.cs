using System;
using NUnit.Framework;
using pagmo;
using Tests.PagmoSharp.TestProblems;

namespace Tests.PagmoSharp
{
    [TestFixture]
    public class Test_archipelago
    {
        private sealed class UnsupportedAlgorithm : IAlgorithm
        {
            public population evolve(population pop) => pop;
            public void set_seed(uint seed) { }
            public uint get_seed() => 0;
            public uint get_verbosity() => 0;
            public void set_verbosity(uint level) { }
            public string get_name() => "UnsupportedAlgorithm";
            public string get_extra_info() => string.Empty;
            public void Dispose() { }
        }

        [Test]
        public void PushBackIslandRejectsManagedProblemWithThreadSafetyNone()
        {
            using var archi = new archipelago();
            using IAlgorithm algo = new bee_colony();
            using var problem = new OneDimensionalSimpleProblem();

            var ex = Assert.Throws<InvalidOperationException>(
                () => archi.push_back_island(algo, problem, 8, 2));
            Assert.That(ex!.Message, Does.Contain("thread_safety.basic or thread_safety.constant"));
        }

        [Test]
        public void ManagedThreadSafeProblemCanEvolveInArchipelago()
        {
            using var archi = new archipelago();
            using IAlgorithm algo = new bee_colony();
            using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();

            archi.push_back_island(algo, problem, 32, 2);
            Assert.AreEqual(1u, archi.size());

            archi.evolve(1);
            archi.wait_check();
            Assert.AreEqual(evolve_status.idle, archi.status());

            var db = archi.MigrantsDb;
            Assert.IsNotNull(db);
            var log = archi.MigrationLog;
            Assert.IsNotNull(log);
        }

        [Test]
        public void PushBackIslandRejectsUnsupportedManagedAlgorithmType()
        {
            using var archi = new archipelago();
            using IAlgorithm algo = new UnsupportedAlgorithm();
            using var problem = new TwoDimensionalSingleObjectiveProblemWrapper();

            var ex = Assert.Throws<NotSupportedException>(() => archi.push_back_island(algo, problem, 8, 2));
            Assert.That(ex!.Message, Does.Contain("UnsupportedAlgorithm"));
        }

        [Test]
        public void MigrationAndTopologyControlsCanBeSet()
        {
            using var archi = new archipelago();
            using var ringTopo = new ring(4, 0.7);

            archi.set_migration_type(migration_type.broadcast);
            archi.set_migrant_handling(migrant_handling.evict);
            archi.set_topology_ring(ringTopo);

            Assert.AreEqual(migration_type.broadcast, archi.get_migration_type());
            Assert.AreEqual(migrant_handling.evict, archi.get_migrant_handling());
            Assert.AreEqual("Ring", archi.get_topology_name());
        }
    }
}
